namespace IesEdit.Ies
{
	/// <summary>
	/// Describes a column's type.
	/// </summary>
	public enum ColumnType : ushort
	{
		/// <summary>
		/// A numeric value.
		/// </summary>
		Number,

		/// <summary>
		/// A string value.
		/// </summary>
		String,

		/// <summary>
		/// A string value containing the name of a function to call to
		/// retrieve the value.
		/// </summary>
		Calculated,
	}
}
