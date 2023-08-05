using System;
using System.Collections.Generic;
using System.IO;

namespace IesEdit.Ies
{
	/// <summary>
	/// Represents a list of values, making up a row of data in an IES file.
	/// </summary>
	public class IesRow : Dictionary<string, object>
	{
		/// <summary>
		/// Gets or sets this row's class id.
		/// </summary>
		public int ClassId { get; set; }

		/// <summary>
		/// Gets or sets this row's class name.
		/// </summary>
		public string ClassName { get; set; }

		/// <summary>
		/// Returns a information about the various string values,
		/// and whether they use SCR (?).
		/// </summary>
		public Dictionary<string, bool> UseScr { get; } = new Dictionary<string, bool>();

		/// <summary>
		/// Returns the value with the given name as an int.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException">
		/// Thrown if the field doesn't exist.
		/// </exception>
		/// <exception cref="InvalidDataException">
		/// Thrown if the type of the field is not numeric.
		/// </exception>
		public int GetInt32(string name)
			=> (int)this.GetFloat(name);

		/// <summary>
		/// Returns the value with the given name as a uint.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException">
		/// Thrown if the field doesn't exist.
		/// </exception>
		/// <exception cref="InvalidDataException">
		/// Thrown if the type of the field is not numeric.
		/// </exception>
		public uint GetUInt32(string name)
			=> (uint)this.GetFloat(name);

		/// <summary>
		/// Returns the value with the given name as a float.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException">
		/// Thrown if the field doesn't exist.
		/// </exception>
		/// <exception cref="InvalidDataException">
		/// Thrown if the type of the field is not numeric.
		/// </exception>
		public float GetFloat(string name)
		{
			if (!this.TryGetValue(name, out var value))
				throw new ArgumentException($"Field '{name}' not found.");

			if (!(value is float floatValue))
				throw new InvalidDataException($"Field '{name}' is not a numeric value.");

			return floatValue;
		}

		/// <summary>
		/// Returns the value with the given name as a string.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException">
		/// Thrown if the field doesn't exist.
		/// </exception>
		/// <exception cref="InvalidDataException">
		/// Thrown if the type of the field is not numeric.
		/// </exception>
		public string GetString(string name)
		{
			if (!this.TryGetValue(name, out var value))
				throw new ArgumentException($"Field '{name}' not found.");

			if (!(value is string stringValue))
				throw new InvalidDataException($"Field '{name}' is not a string value.");

			return stringValue;
		}
	}
}
