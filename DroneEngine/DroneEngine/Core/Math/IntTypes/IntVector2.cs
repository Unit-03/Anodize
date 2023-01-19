using System;

namespace Drone
{
	public struct IntVector2
	{
		#region Constants

		private const double TO_RADIANS = 180 / Math.PI;
		private const double TO_DEGREES = Math.PI / 180;

		#endregion

		#region Static Properties

		/// <summary>Shortcut for a IntVector2 with both components set to 0</summary>
		public static IntVector2 Zero => new IntVector2(0, 0);
		/// <summary>Shortcut for a IntVector2 with both components set to 1</summary>
		public static IntVector2 One  => new IntVector2(1, 1);


		/// <summary>Shortcut for a normalised IntVector2 pointing right (0 degrees)</summary>
		public static IntVector2 Right => new IntVector2( 1,  0);
		/// <summary>Shortcut for a normalised IntVector2 pointing down (90 degrees)</summary>
		public static IntVector2 Down  => new IntVector2( 0, -1);
		/// <summary>Shortcut for a normalised IntVector2 pointing left (180 degrees)</summary>
		public static IntVector2 Left  => new IntVector2(-1,  0);
		/// <summary>Shortcut for a normalised IntVector2 pointing up (270 degrees)</summary>
		public static IntVector2 Up    => new IntVector2( 0,  1);

		#endregion

		#region Properties

		public int X { get; set; }
		public int Y { get; set; }

		/// <summary>Shortcut for X and Y according to the parity of the index; even accesses X, odd accesses Y</summary>
		public int this[int index] {
			get => (index & 1) == 0 ? X : Y;
			set {
				if ((index & 1) == 0)
					X = value;
				else
					Y = value;
			}
		}

		/// <summary>Shortcut for X, use for readability when treating a IntVector2 as dimensions instead of coordinates</summary>
		public int Width  => X;
		/// <summary>Shortcut for Y, use for readability when treating a IntVector2 as dimensions instead of coordinates</summary>
		public int Height => Y;

		#endregion

		#region Constructor

		public IntVector2(int x, int y)
		{
			X = x;
			Y = y;
		}

		public IntVector2(int xy) : this(xy, xy) 
		{
		}

		#endregion

		#region Static Methods

		/// <summary>Returns a IntVector2 that lies perpendicular to <paramref name="vec"/>. It's always rotated 90 degrees clockwise</summary>
		public IntVector2 Perpendicular(IntVector2 vec)
		{
			return new IntVector2(vec.Y, -vec.X);
		}

		/// <summary>Returns a IntVector2 with the components of <paramref name="vec"/> clamped to a minimum value of <paramref name="min"/></summary>
		public static IntVector2 Min(IntVector2 vec, int min)
		{
			return new IntVector2(vec.X < min ? min : vec.X, 
								  vec.Y < min ? min : vec.Y);
		}

		/// <summary>Returns a IntVector2 with the components of <paramref name="vec"/> clamped to a minimum value of their respective components from <paramref name="min"/></summary>
		public static IntVector2 Min(IntVector2 vec, IntVector2 min)
		{
			return new IntVector2(vec.X < min.X ? min.X : vec.X, 
								  vec.Y < min.Y ? min.Y : vec.Y);
		}

		/// <summary>Returns a IntVector2 with the components of <paramref name="vec"/> clamped to a maximum value of <paramref name="max"/></summary>
		public static IntVector2 Max(IntVector2 vec, int max)
		{
			return new IntVector2(vec.X > max ? max : vec.X, 
								  vec.Y > max ? max : vec.Y);
		}

		/// <summary>Returns a IntVector2 with the components of <paramref name="vec"/> clamped to a maximum value of their respective components from <paramref name="max"/></summary>
		public static IntVector2 Max(IntVector2 vec, IntVector2 max)
		{
			return new IntVector2(vec.X > max.X ? max.X : vec.X, 
							   vec.Y > max.Y ? max.Y : vec.Y);
		}

		/// <summary>Returns a IntVector2 with the components of <paramref name="vec"/> clamped to a minimum value of 
		/// <paramref name="min"/> and a maximum value of <paramref name="max"/></summary>
		public static IntVector2 Clamp(IntVector2 vec, int min, int max)
		{
			return new IntVector2(vec.X < min ? min : vec.X > max ? max : vec.X,
								  vec.Y < min ? min : vec.Y > max ? max : vec.Y);
		}

		/// <summary>Returns a IntVector2 with the components of <paramref name="vec"/> clamped to a minimum value of 
		/// their respective components from <paramref name="min"/> and a maximum value of <paramref name="max"/></summary>
		public static IntVector2 Clamp(IntVector2 vec, IntVector2 min, IntVector2 max)
		{
			return new IntVector2(vec.X < min.X ? min.X : vec.X > max.X ? max.X : vec.X,
								  vec.Y < min.Y ? min.Y : vec.Y > max.Y ? max.Y : vec.Y);
		}

		#endregion

		#region Methods

		/// <summary>Sets the components of this vector to <paramref name="xy"/></summary>
		public void Set(int xy)
		{
			X = xy;
			Y = xy;
		}

		/// <summary>Sets the components of this vector to <paramref name="x"/> and <paramref name="y"/> respectively</summary>
		public void Set(int x, int y)
		{
			X = x;
			Y = y;
		}

		/// <summary>Returns the length of this vector squared</summary>
		public float Length()
		{
			return (float)Math.Sqrt((X * X) + (Y * Y));
		}

		/// <summary>Returns the length of this vector squared<br/>
		/// Should be used when the actual length isn't required as it skips a square root operation</summary>
		public int LengthSquared()
		{
			return (X * X) + (Y * Y);
		}

		/// <summary>Returns the distance between this vector and <paramref name="vec"/></summary>
		public float Distance(IntVector2 vec)
		{
			int x = vec.X - X;
			int y = vec.Y - Y;
			return (float)Math.Sqrt((x * x) + (y * y));
		}

		/// <summary>Returns the distance squared between this vector and <paramref name="vec"/><br/>
		/// Should be used when the actual distance isn't required as it skips a square root operation</summary>
		public int DistanceSquared(IntVector2 vec)
		{
			int x = vec.X - X;
			int y = vec.Y - Y;
			return (x * x) + (y * y);
		}
		
		/// <summary>Returns the angle of this vector around the origin, starting at 0 pointing right and going clockwise</summary>
		/// <param name="radians">Whether the returned angle should be in radians or degrees</param>
		public float Angle(bool radians = false)
		{
			double rads = Math.Atan2(Y, X);
			return (float)(radians ? rads : rads * TO_DEGREES);
		}

		/// <summary>Returns the scalar dot product of this vector and <paramref name="vec"/></summary>
		public int Dot(IntVector2 vec)
		{
			return (X * vec.X) + (Y * vec.Y);
		}

		/// <summary>Returns the scalar cross product of this vector and <paramref name="vec"/><br/>
		/// The sign of the cross product tells us if this vector is counter-clockwise or clockwise to <paramref name="vec"/></summary>
		public int Cross(IntVector2 vec)
		{
			return (X * vec.Y) - (Y * vec.X);
		}

		/// <summary>Clamps the components of this vector to a minimum value of <paramref name="min"/></summary>
		public void Min(int min)
		{
			if (X < min) X = min;
			if (Y < min) Y = min;
		}

		/// <summary>Clamps the components of this vector to a minimum value of their respective components from <paramref name="min"/></summary>
		public void Min(IntVector2 min)
		{
			if (X < min.X) X = min.X;
			if (Y < min.Y) Y = min.Y;
		}

		/// <summary>Clamps the components of this vector to a maximum value of <paramref name="max"/></summary>
		public void Max(int max)
		{
			if (X > max) X = max;
			if (Y > max) Y = max;
		}

		/// <summary>Clamps the components of this vector to a maximum value of their respective components from <paramref name="max"/></summary>
		public void Max(IntVector2 max)
		{
			if (X > max.X) X = max.X;
			if (Y > max.Y) Y = max.Y;
		}

		/// <summary>Clamps the components of this vector to a minimum value of <paramref name="min"/> and a maximum value of <paramref name="max"/></summary>
		public void Clamp(int min, int max)
		{
			if		(X < min) X = min;
			else if (X > max) X = max;
			if		(Y < min) Y = min;
			else if (Y > max) Y = max;
		}

		/// <summary>Clamps the components of this vector to a minimum value of their respective components from <paramref name="min"/> and a maximum of <paramref name="max"/></summary>
		public void Clamp(IntVector2 min, IntVector2 max)
		{
			if		(X < min.X) X = min.X;
			else if (X > max.X) X = max.X;
			if		(Y < min.Y) Y = min.Y;
			else if (Y > max.Y) Y = max.Y;
		}

		public override string ToString()
		{
			return $"{X}, {Y}";
		}

		public override bool Equals(object obj) => obj is IntVector2 vec && this == vec;
		public override int GetHashCode() => base.GetHashCode();

		#endregion

		#region Operators

		public static IntVector2 operator ++(IntVector2 vec) => new IntVector2(vec.X + 1, vec.Y + 1);
		public static IntVector2 operator --(IntVector2 vec) => new IntVector2(vec.X - 1, vec.Y - 1);

		public static IntVector2 operator -(IntVector2 vec) => new IntVector2(-vec.X, -vec.Y);

		public static bool operator ==(IntVector2 left, IntVector2 right) => left.X == right.X && left.Y == right.Y;
		public static bool operator !=(IntVector2 left, IntVector2 right) => !(left == right);

		public static IntVector2 operator +(IntVector2 vec, int scalar) => new IntVector2(vec.X + scalar, vec.Y + scalar);
		public static IntVector2 operator -(IntVector2 vec, int scalar) => new IntVector2(vec.X - scalar, vec.Y - scalar);
		public static IntVector2 operator *(IntVector2 vec, int scalar) => new IntVector2(vec.X * scalar, vec.Y * scalar);
		public static IntVector2 operator /(IntVector2 vec, int scalar) => new IntVector2(vec.X / scalar, vec.Y / scalar);
		public static IntVector2 operator %(IntVector2 vec, int scalar) => new IntVector2(vec.X % scalar, vec.Y % scalar);

		public static IntVector2 operator +(IntVector2 left, IntVector2 right) => new IntVector2(left.X + right.X, left.Y + right.Y);
		public static IntVector2 operator -(IntVector2 left, IntVector2 right) => new IntVector2(left.X - right.X, left.Y - right.Y);
		public static IntVector2 operator *(IntVector2 left, IntVector2 right) => new IntVector2(left.X * right.X, left.Y * right.Y);
		public static IntVector2 operator /(IntVector2 left, IntVector2 right) => new IntVector2(left.X / right.X, left.Y / right.Y);
		public static IntVector2 operator %(IntVector2 left, IntVector2 right) => new IntVector2(left.X % right.X, left.Y % right.Y);

		public static explicit operator IntVector2((int x, int y) tuple) => new IntVector2(tuple.x, tuple.y);
		public static explicit operator (int x, int y)(IntVector2 vec) => (x: vec.X, y: vec.Y);

		public static implicit operator Vector2(IntVector2 intVec) => new Vector2(intVec.X, intVec.Y);

		#endregion
	}
}
