using System;

namespace Drone
{
	/// <summary>Defines offsets from each side of a rect</summary>
	public struct Margin
	{
		#region Properties

		public float Left { get; set; }
		public float Right { get; set; }
		public float Top { get; set; }
		public float Bottom { get; set; }

		public float TotalX => Math.Abs(Left) + Math.Abs(Right);
		public float TotalY => Math.Abs(Top)  + Math.Abs(Bottom);

		#endregion

		#region Constructors

		public Margin(float left, float right, float top, float bottom)
		{
			Left   = left;
			Right  = right;
			Top    = top;
			Bottom = bottom;
		}

		public Margin(Vector2 topLeft, Vector2 botRight) : this (topLeft.X, botRight.X, topLeft.Y, botRight.Y)
		{
		}

		#endregion

		#region Static Methods

		/// <summary>Creates a margin of offsets to the pivot point of a rect with dimensions <paramref name="width"/> by <paramref name="height"/></summary>
		public static Margin Calculate(float pivotX, float pivotY, float width, float height)
		{
			float left = pivotX * width;
			float top  = pivotY * height;

			return new Margin(left, width  - left,
							  top,  height - top);
		}

		/// <summary>Creates a margin of offsets to the pivot point of a rect with dimensions of <paramref name="size"/></summary>
		public static Margin Calculate(Vector2 pivot, Vector2 size)
		{
			float left = pivot.X * size.X;
			float top  = pivot.Y * size.Y;

			return new Margin(left, size.X - left,
							  top,  size.Y - top);

		}

		#endregion

		#region Methods
		
		/// <summary>Sets the left, right, top, and bottom values directly</summary>
		public void Set(float left, float right, float top, float bottom)
		{
			Left   = left;
			Right  = right;
			Top    = top;
			Bottom = bottom;
		}

		/// <summary>Sets the top left and bottom right values directly</summary>
		public void Set(Vector2 topLeft, Vector2 botRight)
		{
			Set(topLeft.X, botRight.X, topLeft.Y, botRight.Y);
		}

		/// <summary>Converts the vector to a string in the format "Left, Right, Top, Bottom" (e.g. "10, 10, 20, 20")</summary>
		public override string ToString()
		{
			return $"{Left}, {Right}, {Top}, {Bottom}";
		}

		public override bool Equals(object obj) => obj is Margin margin && margin == this;
		public override int GetHashCode() => base.GetHashCode();

		#endregion

		#region Operators

		public static Margin operator ++(Margin margin) => new Margin(margin.Left + 1, margin.Right + 1, margin.Top + 1, margin.Bottom + 1);
		public static Margin operator --(Margin margin) => new Margin(margin.Left - 1, margin.Right - 1, margin.Top - 1, margin.Bottom - 1);

		public static Margin operator -(Margin margin) => new Margin(-margin.Left, -margin.Right, -margin.Top, -margin.Bottom);

		public static bool operator ==(Margin left, Margin right)
		{
			return left.Left == right.Left && left.Right  == right.Right &&
				   left.Top  == right.Top  && left.Bottom == right.Bottom;
		}

		public static bool operator !=(Margin left, Margin right)
		{
			return !(left == right);
		}

		public static Margin operator +(Margin margin, float amount) => new Margin(margin.Left + amount, margin.Right + amount, margin.Top + amount, margin.Bottom + amount);
		public static Margin operator -(Margin margin, float amount) => new Margin(margin.Left - amount, margin.Right - amount, margin.Top - amount, margin.Bottom - amount);
		public static Margin operator *(Margin margin, float amount) => new Margin(margin.Left * amount, margin.Right * amount, margin.Top * amount, margin.Bottom * amount);
		public static Margin operator /(Margin margin, float amount) => new Margin(margin.Left / amount, margin.Right / amount, margin.Top / amount, margin.Bottom / amount);

		public static Margin operator +(Margin margin, Margin amount) => new Margin(margin.Left + amount.Left, margin.Right + amount.Right, margin.Top + amount.Top, margin.Bottom + amount.Bottom);
		public static Margin operator -(Margin margin, Margin amount) => new Margin(margin.Left - amount.Left, margin.Right - amount.Right, margin.Top - amount.Top, margin.Bottom - amount.Bottom);
		public static Margin operator *(Margin margin, Margin amount) => new Margin(margin.Left * amount.Left, margin.Right * amount.Right, margin.Top * amount.Top, margin.Bottom * amount.Bottom);
		public static Margin operator /(Margin margin, Margin amount) => new Margin(margin.Left / amount.Left, margin.Right / amount.Right, margin.Top / amount.Top, margin.Bottom / amount.Bottom);

		#endregion
	}
}
