using System.IO;

namespace IesEdit.Ies
{
	/// <summary>
	/// Represents an IES file's header.
	/// </summary>
	public class IesHeader
	{
		/// <summary>
		/// Gets or sets the header's id space.
		/// </summary>
		public string IdSpace { get; set; }

		/// <summary>
		/// Gets or sets the header's key space.
		/// </summary>
		public string KeySpace { get; set; } = "";

		/// <summary>
		/// Gets or sets the version.
		/// </summary>
		public int Version { get; set; } = 1;

		/// <summary>
		/// Gets or sets the offset of the data.
		/// </summary>
		public long InfoSize { get; set; }

		/// <summary>
		/// Gets or sets the offset of the resources.
		/// </summary>
		public long DataSize { get; set; }

		/// <summary>
		/// Gets or sets the total file size.
		/// </summary>
		public long TotalSize { get; set; }

		/// <summary>
		/// Gets or sets whether to use a class id.
		/// </summary>
		public bool UseClassId { get; set; }

		/// <summary>
		/// Gets or sets the number of columns.
		/// </summary>
		public int ColumnCount { get; set; }

		/// <summary>
		/// Gets or sets the number of rows.
		/// </summary>
		public int RowCount { get; set; }

		/// <summary>
		/// Gets or sets the number of Number column.
		/// </summary>
		public int NumberColumnCount { get; set; }

		/// <summary>
		/// Gets or sets the number of String column.
		/// </summary>
		public int StringColumnCount { get; set; }
	}
}
