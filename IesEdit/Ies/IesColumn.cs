using System;

namespace IesEdit.Ies
{
	/// <summary>
	/// Represents a column in an IES file.
	/// </summary>
	public class IesColumn : IComparable<IesColumn>
	{
		/// <summary>
		/// Gets or sets the column's name.
		/// </summary>
		public string Column { get; set; }

		/// <summary>
		/// Gets or sets the column's name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the type of the data in this column.
		/// </summary>
		public ColumnType Type { get; set; }

		/// <summary>
		/// ?
		/// </summary>
		// Default: SP
		// Name_XX: XX
		public PropertyAccess Access { get; set; } = PropertyAccess.SP;

		/// <summary>
		/// ?
		/// </summary>
		// true if Name ends in "_NT"?
		public int Sync { get; set; }

		/// <summary>
		/// Gets or sets order in which columns were declared (?).
		/// </summary>
		public int DeclarationIndex { get; set; }

		/// <summary>
		/// Returns true if this column's type is Number.
		/// </summary>
		public bool IsNumber { get { return (this.Type == ColumnType.Number); } }

		/// <summary>
		/// Compares this column with the given one, returning a difference
		/// based on the type and declaration index.
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public int CompareTo(IesColumn other)
		{
			// Compare declaration if the two columns are the same type,
			// i.e. both numbers or both not numbers.
			if (this.Type == other.Type || (!this.IsNumber && !other.IsNumber))
				return this.DeclarationIndex.CompareTo(other.DeclarationIndex);

			// Otherwise compare the type
			if (this.Type < other.Type)
				return -1;

			return 1;
		}

		/// <summary>
		/// Returns a string representation of this column.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return this.Column + "/" + this.Name + " : " + this.Type;
		}
	}
}
