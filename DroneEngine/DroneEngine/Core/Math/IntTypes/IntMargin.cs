using System;

namespace Drone
{
	/// <summary>Defines offsets from each side of a rect</summary>
	public struct IntMargin
	{
		#region Properties

		public int Left { get; set; }
		public int Right { get; set; }
		public int Top { get; set; }
		public int Bottom { get; set; }

		public int TotalX => Math.Abs(Left) + Math.Abs(Right);
		public int TotalY => Math.Abs(Top)  + Math.Abs(Bottom);

		#endregion

		#region Constructors

		public IntMargin(int left, int right, int top, int bottom)
		{
			Left   = left;
			Right  = right;
			Top    = top;
			Bottom = bottom;
		}

		public IntMargin(IntVector2 topLeft, IntVector2 botRight) : this (topLeft.X, botRight.X, topLeft.Y, botRight.Y)
		{
		}

		#endregion

		#region Static Methods

		/// <summary>Creates a IntMargin of offsets to the pivot point of a rect with dimensions <paramref name="width"/> by <paramref name="height"/></summary>
		public static IntMargin Calculate(int pivotX, int pivotY, int width, int height)
		{
			int left = pivotX * width;
			int top  = pivotY * height;

			return new IntMargin(left, width  - left,
								 top,  height - top);
		}

		/// <summary>Creates a IntMargin of offsets to the pivot point of a rect with dimensions of <paramref name="size"/></summary>
		public static IntMargin Calculate(IntVector2 pivot, IntVector2 size)
		{
			int left = pivot.X * size.X;
			int top  = pivot.Y * size.Y;

			return new IntMargin(left, size.X - left,
								 top,  size.Y - top);
		}

		#endregion

		#region Methods
		
		/// <summary>Sets the left, right, top, and bottom values directly</summary>
		public void Set(int left, int right, int top, int bottom)
		{
			Left   = left;
			Right  = right;
			Top    = top;
			Bottom = bottom;
		}

		/// <summary>Sets the top left and bottom right values directly</summary>
		public void Set(IntVector2 topLeft, IntVector2 botRight)
		{
			Set(topLeft.X, botRight.X, topLeft.Y, botRight.Y);
		}

		/// <summary>Converts the vector to a string in the format "Left, Right, Top, Bottom" (e.g. "10, 10, 20, 20")</summary>
		public override string ToString()
		{
			return $"{Left}, {Right}, {Top}, {Bottom}";
		}

		public override bool Equals(object obj) => obj is IntMargin IntMargin && IntMargin == this;
		public override int GetHashCode() => base.GetHashCode();

		#endregion

		#region Operators

		public static IntMargin operator ++(IntMargin margin) => new IntMargin(margin.Left + 1, margin.Right + 1, margin.Top + 1, margin.Bottom + 1);
		public static IntMargin operator --(IntMargin margin) => new IntMargin(margin.Left - 1, margin.Right - 1, margin.Top - 1, margin.Bottom - 1);

		public static IntMargin operator -(IntMargin margin) => new IntMargin(-margin.Left, -margin.Right, -margin.Top, -margin.Bottom);

		public static bool operator ==(IntMargin left, IntMargin right)
		{
			return left.Left == right.Left && left.Right  == right.Right &&
				   left.Top  == right.Top  && left.Bottom == right.Bottom;
		}

		public static bool operator !=(IntMargin left, IntMargin right)
		{
			return !(left == right);
		}

		public static IntMargin operator +(IntMargin margin, int amount) => new IntMargin(margin.Left + amount, margin.Right + amount, margin.Top + amount, margin.Bottom + amount);
		public static IntMargin operator -(IntMargin margin, int amount) => new IntMargin(margin.Left - amount, margin.Right - amount, margin.Top - amount, margin.Bottom - amount);
		public static IntMargin operator *(IntMargin margin, int amount) => new IntMargin(margin.Left * amount, margin.Right * amount, margin.Top * amount, margin.Bottom * amount);
		public static IntMargin operator /(IntMargin margin, int amount) => new IntMargin(margin.Left / amount, margin.Right / amount, margin.Top / amount, margin.Bottom / amount);

		public static IntMargin operator +(IntMargin margin, IntMargin amount) => new IntMargin(margin.Left + amount.Left, margin.Right + amount.Right, margin.Top + amount.Top, margin.Bottom + amount.Bottom);
		public static IntMargin operator -(IntMargin margin, IntMargin amount) => new IntMargin(margin.Left - amount.Left, margin.Right - amount.Right, margin.Top - amount.Top, margin.Bottom - amount.Bottom);
		public static IntMargin operator *(IntMargin margin, IntMargin amount) => new IntMargin(margin.Left * amount.Left, margin.Right * amount.Right, margin.Top * amount.Top, margin.Bottom * amount.Bottom);
		public static IntMargin operator /(IntMargin margin, IntMargin amount) => new IntMargin(margin.Left / amount.Left, margin.Right / amount.Right, margin.Top / amount.Top, margin.Bottom / amount.Bottom);

		#endregion
	}
}
