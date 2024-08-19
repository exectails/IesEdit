using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace IesEdit.Ies
{
	/// <summary>
	/// Represents an IES file.
	/// </summary>
	public class IesFile
	{
		private const int HeaderNameLengths = 0x40;
		private const int ColumnSize = 136;
		private const int SizesPos = (2 * HeaderNameLengths + 2 * sizeof(short));
		private const string DefaultString = "None";
		private const float DefaultNumber = 0f;

		private const string RootName = "idspace";
		private const string ElementsName = "Category";
		private const string ElementName = "Class";
		private const string IdSpaceName = "id";
		private const string KeySpaceName = "keyid";

		/// <summary>
		/// Returns the file's header.
		/// </summary>
		public IesHeader Header { get; } = new IesHeader();

		/// <summary>
		/// Returns the file's columns.
		/// </summary>
		public List<IesColumn> Columns { get; } = new List<IesColumn>();

		/// <summary>
		/// Returns the file's rows, which contain the actual data.
		/// </summary>
		public List<IesRow> Rows { get; } = new List<IesRow>();

		/// <summary>
		/// Creates new, empty instance.
		/// </summary>
		public IesFile()
		{
		}

		/// <summary>
		/// Loads data from given IES or XML file and returns
		/// the new instance.
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public static IesFile LoadFile(string filePath)
		{
			if (Path.GetExtension(filePath) == ".ies")
				return LoadIesFile(filePath);
			else if (Path.GetExtension(filePath) == ".xml")
				return LoadXmlFile(filePath);
			else
				throw new ArgumentException("Invalid file type.");
		}

		/// <summary>
		/// Loads data from given IES file and returns the new instance.
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public static IesFile LoadIesFile(string filePath)
		{
			var result = new IesFile();
			result.LoadFromIes(filePath);

			return result;
		}

		/// <summary>
		/// Loads data from given XML file and returns the new instance.
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public static IesFile LoadXmlFile(string filePath)
		{
			var result = new IesFile();
			result.LoadFromXml(filePath);

			return result;
		}

		/// <summary>
		/// Loads data from given XML string and returns the new instance.
		/// </summary>
		/// <param name="filePath"></param>
		/// <param name="xml"></param>
		/// <returns></returns>
		public static IesFile LoadXmlString(string filePath, string xml)
		{
			var doc = XDocument.Parse(xml);

			var result = new IesFile();

			result.LoadInfoFromDataXml(doc);
			result.LoadXmlDoc(filePath, doc);

			return result;
		}

		/// <summary>
		/// Loads data from IES file into this instance.
		/// </summary>
		/// <param name="filePath"></param>
		private void LoadFromIes(string filePath)
		{
			using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
			using (var br = new BinaryReader(fs))
			{
				this.ReadHeader(br);
				this.ReadColumns(br);
				this.ReadRows(br);
			}
		}

		/// <summary>
		/// Loads data from IES file into this instance.
		/// </summary>
		/// <param name="filePath"></param>
		private void LoadFromXml(string filePath)
		{
			var dataFilePath = filePath;
			var infoFilePath = Path.ChangeExtension(filePath, ".info.xml");

			if (!File.Exists(dataFilePath))
				throw new FileNotFoundException($"XML file '{Path.GetFileName(dataFilePath)}' not found.");

			var dataXml = XDocument.Load(dataFilePath, LoadOptions.SetLineInfo);

			if (File.Exists(infoFilePath))
			{
				this.LoadInfoFromInfoXml(infoFilePath);
			}
			else
			{
				this.LoadInfoFromDataXml(dataXml);
			}

			this.LoadXmlDoc(filePath, dataXml);
		}

		/// <summary>
		/// Loads data from XML doc.
		/// </summary>
		/// <param name="filePath"></param>
		/// <param name="dataXml"></param>
		/// <exception cref="InvalidDataException"></exception>
		private void LoadXmlDoc(string filePath, XDocument dataXml)
		{
			this.Rows.Clear();
			foreach (var element in dataXml.Root.Descendants(ElementName))
			{
				var row = new IesRow();

				foreach (var column in this.Columns)
				{
					var key = column.Name;

					var attr = element.Attribute(key);
					if (attr == null)
					{
						if (column.IsNumber)
						{
							row.Add(key, 0f);
						}
						else
						{
							row.Add(key, "");
							row.UseScr.Add(key, false);
						}
					}
					else
					{
						if (column.IsNumber)
						{
							if (!IsValueNumeric(attr.Value))
							{
								var fileName = Path.GetFileName(filePath);
								var lineInfo = (IXmlLineInfo)element;
								throw new InvalidDataException(string.Format("Expected numeric value for attribute '{0}' in '{1}@{2}:{3}'.", key, fileName, lineInfo.LineNumber, lineInfo.LinePosition));
							}

							row.Add(key, float.Parse(attr.Value));
						}
						else
						{
							row.Add(key, attr.Value != DefaultString ? attr.Value : "");
							row.UseScr.Add(key, attr.Value.Contains("SCR_") || attr.Value.Contains("SCP"));
						}
					}

					if (key == "ClassID")
						row.ClassId = int.Parse(attr.Value);
					else if (key == "ClassName")
						row.ClassName = attr.Value;
				}

				//var classNameAttr = element.Attribute("__ClassNameAttr");
				//if (classNameAttr != null)
				//	row["ClassName"] = classNameAttr.Value;

				this.Rows.Add(row);
			}

			this.Header.ColumnCount = this.Columns.Count;
			this.Header.NumberColumnCount = this.Columns.Count(a => a.IsNumber);
			this.Header.StringColumnCount = this.Header.ColumnCount - this.Header.NumberColumnCount;
		}

		/// <summary>
		/// Loads IES information from info XML file.
		/// </summary>
		/// <param name="filePath"></param>
		private void LoadInfoFromInfoXml(string filePath)
		{
			if (!File.Exists(filePath))
				throw new FileNotFoundException($"Info XML file '{Path.GetFileName(filePath)}' not found.");

			var infoXml = XDocument.Load(filePath);

			this.Header.IdSpace = infoXml.Root.Attribute("IdSpace").Value;
			this.Header.KeySpace = infoXml.Root.Attribute("KeySpace").Value;
			this.Header.Version = int.Parse(infoXml.Root.Attribute("Version").Value);
			this.Header.UseClassId = infoXml.Root.Attribute("UseClassId").Value.ToUpper() == "TRUE";

			this.Columns.Clear();
			foreach (var element in infoXml.Root.Element("Columns").Elements("Column"))
			{
				var column = new IesColumn();

				column.Column = element.Attribute("Column").Value;
				column.Name = element.Attribute("Name").Value;
				column.Type = (ColumnType)Enum.Parse(typeof(ColumnType), element.Attribute("Type").Value);
				column.Access = (PropertyAccess)Enum.Parse(typeof(PropertyAccess), element.Attribute("Access").Value);
				column.Sync = int.Parse(element.Attribute("Sync").Value);
				column.DeclarationIndex = int.Parse(element.Attribute("DeclarationIndex").Value);

				this.Columns.Add(column);
			}
		}

		/// <summary>
		/// Loads IES information from data file.
		/// </summary>
		/// <param name="dataXml"></param>
		private void LoadInfoFromDataXml(XDocument dataXml)
		{
			var idSpaceElement = dataXml.Root;
			var idSpace = idSpaceElement.Attribute("id");

			if (idSpaceElement.Name != RootName || idSpace == null)
				throw new ArgumentException("Invalid IES base XML file.");

			this.Header.IdSpace = idSpace.Value;
			this.Header.KeySpace = idSpaceElement.Attribute("keyspace")?.Value ?? "";
			this.Header.ColumnCount = 0;
			this.Header.RowCount = 0;
			this.Header.NumberColumnCount = 0;
			this.Header.StringColumnCount = 0;

			// We used to get the column types from the first row read, but this
			// turned out to be unreliable, as some property values might look
			// like numbers initially, but are actually strings. For example,
			// AniTime and HitTime in the skill data are sometimes lists of
			// numbers stored as strings, where the numbers are separated by
			// semicolons. Instead, we now go through the data and figure out
			// the types first.
			var columnTypes = new Dictionary<string, ColumnType>();
			foreach (var element in idSpaceElement.Descendants(ElementName))
			{
				foreach (var attr in element.Attributes())
				{
					var propertyName = attr.Name.LocalName;
					var type = ColumnType.String;

					if (propertyName.StartsWith("CP_"))
					{
						type = ColumnType.Calculated;
					}
					else if (IsValueNumeric(attr.Value))
					{
						type = ColumnType.Number;
					}

					if (columnTypes.TryGetValue(propertyName, out var existingType))
					{
						// Switch types that vary between rows to strings,
						// as the data is not consistent and must support
						// arbitrary values.
						if (existingType != type)
							columnTypes[propertyName] = ColumnType.String;
					}
					else
					{
						columnTypes[propertyName] = type;
					}
				}
			}

			this.Columns.Clear();
			foreach (var element in idSpaceElement.Descendants(ElementName))
			{
				this.Header.RowCount++;

				foreach (var attr in element.Attributes())
				{
					var propertyName = attr.Name.LocalName;
					if (this.Columns.Any(a => a.Name == propertyName))
						continue;

					var nameLength = propertyName.Length;
					var simpleName = propertyName;
					var access = PropertyAccess.SP;
					var sync = false;

					if (simpleName.StartsWith("EP_"))
					{
						access = PropertyAccess.EP;
						simpleName = simpleName.Substring(3);
					}
					else if (simpleName.StartsWith("CP_"))
					{
						access = PropertyAccess.CP;
						simpleName = simpleName.Substring(3);
					}
					else if (simpleName.StartsWith("VP_"))
					{
						access = PropertyAccess.VP;
						simpleName = simpleName.Substring(3);
					}
					else if (simpleName.StartsWith("CT_"))
					{
						access = PropertyAccess.CT;
						simpleName = simpleName.Substring(3);
					}

					var ntIndex = simpleName.IndexOf("_NT");
					if (ntIndex != -1)
					{
						sync = true;
						simpleName = simpleName.Substring(0, ntIndex);
					}

					//var type = ColumnType.String;
					//if (propertyName.StartsWith("CP_"))
					//{
					//	type = ColumnType.Calculated;
					//}
					//else if (IsValueNumeric(attr.Value))
					//{
					//	type = ColumnType.Number;
					//}

					if (!columnTypes.TryGetValue(propertyName, out var type))
						type = ColumnType.String;

					var column = new IesColumn();
					column.Name = propertyName;
					column.Column = simpleName;
					column.Type = type;
					column.Access = access;
					column.Sync = sync ? 1 : 0;

					if (column.IsNumber)
						column.DeclarationIndex = this.Columns.Count(a => a.IsNumber);
					else
						column.DeclarationIndex = this.Columns.Count(a => !a.IsNumber);

					if (propertyName == "ClassID")
						this.Header.UseClassId = true;

					this.Columns.Add(column);
				}

				this.Header.ColumnCount = this.Columns.Count;
				this.Header.NumberColumnCount = this.Columns.Count(a => a.IsNumber);
				this.Header.StringColumnCount = this.Columns.Count(a => !a.IsNumber);
			}
		}

		/// <summary>
		/// Returns true if the given string is a numeric value.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		private static bool IsValueNumeric(string value)
		{
			if (value.Length == 0)
				return false;

			if (value.StartsWith("-"))
				return true;

			return value.All(a => a == ' ' || a == '.' || (a >= '0' && a <= '9'));
		}

		/// <summary>
		/// Reads header from binary reader into this instance.
		/// </summary>
		/// <param name="reader"></param>
		private void ReadHeader(BinaryReader reader)
		{
			this.Header.IdSpace = reader.ReadFixedString(HeaderNameLengths);
			this.Header.KeySpace = reader.ReadFixedString(HeaderNameLengths);
			this.Header.Version = reader.ReadUInt16();
			reader.ReadUInt16(); // padding
			this.Header.InfoSize = reader.ReadUInt32();
			this.Header.DataSize = reader.ReadUInt32();
			this.Header.TotalSize = reader.ReadUInt32();
			this.Header.UseClassId = (reader.ReadByte() != 0);
			reader.ReadByte(); // padding
			this.Header.RowCount = reader.ReadUInt16();
			this.Header.ColumnCount = reader.ReadUInt16();
			this.Header.NumberColumnCount = reader.ReadUInt16();
			this.Header.StringColumnCount = reader.ReadUInt16();
			reader.ReadUInt16(); // padding
		}

		/// <summary>
		/// Reads columns from binary reader into this instance.
		/// </summary>
		/// <param name="reader"></param>
		private void ReadColumns(BinaryReader reader)
		{
			for (var i = 0; i < this.Header.ColumnCount; i++)
			{
				var item = new IesColumn();

				item.Column = reader.ReadXoredFixedString(HeaderNameLengths);
				item.Name = reader.ReadXoredFixedString(HeaderNameLengths);
				item.Type = (ColumnType)reader.ReadUInt16();
				item.Access = (PropertyAccess)reader.ReadUInt16();
				item.Sync = reader.ReadUInt16();
				item.DeclarationIndex = reader.ReadUInt16();

				this.Columns.Add(item);
			}
		}

		/// <summary>
		/// Reads data from binary reader into this instance.
		/// </summary>
		/// <param name="reader"></param>
		private void ReadRows(BinaryReader reader)
		{
			for (var i = 0; i < this.Header.RowCount; ++i)
			{
				var row = new IesRow();

				row.ClassId = reader.ReadInt32();
				row.ClassName = reader.ReadXoredLpString();

				var numbers = new List<float>();
				var strings = new List<string>();
				var bools = new List<bool>();

				for (var j = 0; j < this.Header.NumberColumnCount; ++j)
				{
					var value = reader.ReadSingle();
					numbers.Add(value);
				}

				for (var j = 0; j < this.Header.StringColumnCount; ++j)
				{
					var value = reader.ReadXoredLpString();
					strings.Add(value);
				}

				for (var j = 0; j < this.Header.StringColumnCount; ++j)
				{
					var value = reader.ReadByte() != 0;
					bools.Add(value);
				}

				for (var j = 0; j < this.Header.ColumnCount; ++j)
				{
					var column = this.Columns[j];
					var key = column.Name;

					//if (column.Name.Contains("AniTime") && row.ClassId == 1531)
					//	Console.WriteLine();

					//if (column.Name.Contains("HitTime") && row.ClassId == 51107)
					//	Console.WriteLine();

					if (column.IsNumber)
					{
						row.Add(key, numbers[column.DeclarationIndex]);
					}
					else
					{
						row.Add(key, strings[column.DeclarationIndex]);
						row.UseScr.Add(key, bools[column.DeclarationIndex]);
					}
				}

				this.Rows.Add(row);
			}
		}

		/// <summary>
		/// Saves this instance's data to IES file.
		/// </summary>
		/// <param name="filePath"></param>
		public void SaveIes(string filePath)
		{
			var columns = this.Columns.ToList();
			var sortedColumns = columns.OrderBy(a => a.IsNumber ? 0 : 1).ThenBy(a => a.DeclarationIndex);
			var rows = this.Rows;

			var rowCount = rows.Count;
			var colCount = columns.Count;
			var numberColCount = columns.Count(a => a.IsNumber);
			var stringColCount = colCount - numberColCount;

			using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
			using (var bw = new BinaryWriter(fs))
			{
				bw.WriteFixedString(this.Header.IdSpace, HeaderNameLengths);
				bw.WriteFixedString(this.Header.KeySpace ?? "", HeaderNameLengths);
				bw.Write((ushort)this.Header.Version);
				bw.Write((ushort)0); // padding
				bw.Write((uint)this.Header.InfoSize);
				bw.Write((uint)this.Header.DataSize);
				bw.Write((uint)this.Header.TotalSize);
				bw.Write(this.Header.UseClassId ? (byte)1 : (byte)0);
				bw.Write((byte)0); // padding
				bw.Write((ushort)rowCount);
				bw.Write((ushort)colCount);
				bw.Write((ushort)numberColCount);
				bw.Write((ushort)stringColCount);
				bw.Write((ushort)0); // padding

				foreach (var column in columns)
				{
					bw.WriteXoredFixedString(column.Column, HeaderNameLengths);
					bw.WriteXoredFixedString(column.Name, HeaderNameLengths);
					bw.Write((ushort)column.Type);
					bw.Write((ushort)column.Access);
					bw.Write((ushort)column.Sync);
					bw.Write((ushort)column.DeclarationIndex);
				}

				var rowsStart = bw.BaseStream.Position;
				foreach (var row in rows)
				{
					bw.Write(row.ClassId);
					bw.WriteXoredLpString(row.ClassName ?? "");

					foreach (var column in sortedColumns)
					{
						if (!row.TryGetValue(column.Name, out var value))
						{
							if (column.IsNumber)
								bw.Write(0f);
							else
								bw.Write((ushort)0);
						}
						else
						{
							if (column.IsNumber)
								bw.Write((float)value);
							else
								bw.WriteXoredLpString((string)value);
						}
					}

					foreach (var column in sortedColumns.Where(a => !a.IsNumber))
					{
						if (row.UseScr.TryGetValue(column.Name, out var value))
							bw.Write(value ? (byte)1 : (byte)0);
						else
							bw.Write((byte)0);
					}
				}

				this.Header.InfoSize = columns.Count * ColumnSize;
				this.Header.DataSize = bw.BaseStream.Position - rowsStart;
				this.Header.TotalSize = bw.BaseStream.Position;

				bw.BaseStream.Seek(SizesPos, SeekOrigin.Begin);
				bw.Write((uint)this.Header.InfoSize);
				bw.Write((uint)this.Header.DataSize);
				bw.Write((uint)this.Header.TotalSize);
				bw.BaseStream.Seek(0, SeekOrigin.End);
			}
		}

		/// <summary>
		/// Saves data and information as XML files.
		/// </summary>
		/// <param name="filePath"></param>
		public void SaveXml(string filePath)
		{
			var dataFilePath = filePath;
			var headerFilePath = Path.ChangeExtension(dataFilePath, ".info.xml");

			using (var fs = new FileStream(dataFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
				this.WriteXmlData(fs);

			using (var fs = new FileStream(headerFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
				this.WriteXmlInfo(fs);
		}

		/// <summary>
		/// Returns the IES file's data as XML.
		/// </summary>
		public string GetXml()
		{
			using (var ms = new MemoryStream())
			{
				this.WriteXmlData(ms);
				ms.Seek(0, SeekOrigin.Begin);

				using (var sr = new StreamReader(ms))
					return sr.ReadToEnd();
			}
		}

		/// <summary>
		/// Writes data to XML stream.
		/// </summary>
		/// <param name="stream"></param>
		private void WriteXmlData(Stream stream)
		{
			var settings = new XmlWriterSettings();
			settings.Encoding = new UTF8Encoding(false);
			settings.Indent = true;
			settings.IndentChars = "\t";

			using (var sw = new StreamWriter(stream, Encoding.ASCII, 1024, true))
			using (var writer = XmlWriter.Create(stream, settings))
			{
				writer.WriteStartDocument();
				writer.WriteComment(" Generated by IesEdit. ");
				{
					writer.WriteStartElement(RootName);
					writer.WriteAttributeString(IdSpaceName, this.Header.IdSpace);
					if (!string.IsNullOrWhiteSpace(this.Header.KeySpace))
						writer.WriteAttributeString(KeySpaceName, this.Header.KeySpace);
					//writer.WriteAttributeString("Version", this.Header.Version.ToString());
					//writer.WriteAttributeString("UseClassId", this.Header.UseClassId ? "TRUE" : "FALSE");
					{
						writer.WriteStartElement(ElementsName);
						foreach (var row in this.Rows)
						{
							writer.WriteStartElement(ElementName);
							//writer.WriteAttributeString("ClassID", row.ClassId.ToString());
							//writer.WriteAttributeString("ClassName", row.ClassName);

							//if (row.TryGetValue("ClassName", out var value) && value is string stringValue && stringValue != row.ClassName)
							//	writer.WriteAttributeString("__ClassNameAttr", stringValue);

							var fields = row.OrderBy(a => this.Columns.FirstOrDefault(b => b.Name == a.Key)?.DeclarationIndex ?? 10000)
											.ThenBy(a => this.Columns.FirstOrDefault(b => b.Name == a.Key)?.Type ?? (ColumnType)10000);

							foreach (var field in fields)
							{
								//if (field.Key == "ClassID" || field.Key == "ClassName")
								//	continue;

								if (field.Value is float floatValue)
								{
									if (floatValue != 0)
										writer.WriteAttributeString(field.Key, floatValue.ToString("G17", CultureInfo.InvariantCulture));
									else
										writer.WriteAttributeString(field.Key, DefaultNumber.ToString("G17", CultureInfo.InvariantCulture));
								}
								else
								{
									var stringValue = field.Value.ToString();
									if (stringValue.Length > 0)
										writer.WriteAttributeString(field.Key, field.Value.ToString());
									else
										writer.WriteAttributeString(field.Key, DefaultString);
								}
							}
							writer.WriteEndElement();
						}
						writer.WriteEndElement();
					}
					writer.WriteEndElement();
				}

				sw.WriteLine();
			}
		}

		/// <summary>
		/// Writes information about the file to XML stream.
		/// </summary>
		/// <param name="stream"></param>
		private void WriteXmlInfo(Stream stream)
		{
			var settings = new XmlWriterSettings();
			settings.Encoding = Encoding.UTF8;
			settings.Indent = true;
			settings.IndentChars = "\t";

			using (var sw = new StreamWriter(stream, Encoding.ASCII))
			using (var writer = XmlWriter.Create(stream, settings))
			{
				writer.WriteStartDocument();
				writer.WriteComment(" Generated by IesEdit. ");
				{
					writer.WriteStartElement(RootName);
					writer.WriteAttributeString("IdSpace", this.Header.IdSpace);
					writer.WriteAttributeString("KeySpace", this.Header.KeySpace);
					writer.WriteAttributeString("Version", this.Header.Version.ToString());
					writer.WriteAttributeString("UseClassId", this.Header.UseClassId ? "TRUE" : "FALSE");
					{
						var columns = this.Columns.OrderBy(a => a.DeclarationIndex).ThenBy(a => a.Type);

						writer.WriteStartElement("Columns");
						foreach (var column in columns)
						{
							writer.WriteStartElement("Column");
							writer.WriteAttributeString("Column", column.Column);
							writer.WriteAttributeString("Name", column.Name);
							writer.WriteAttributeString("Type", column.Type.ToString());
							writer.WriteAttributeString("Access", column.Access.ToString());
							writer.WriteAttributeString("Sync", column.Sync.ToString());
							writer.WriteAttributeString("DeclarationIndex", column.DeclarationIndex.ToString());
							writer.WriteEndElement();
						}
						writer.WriteEndElement();
					}
					writer.WriteEndElement();
				}

				sw.WriteLine();
			}
		}
	}
}
