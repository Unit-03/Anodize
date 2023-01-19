using System;

namespace Drone
{
	public struct Vector2
	{
		#region Static Properties

		/// <summary>Shortcut for a Vector2 with both components set to 0</summary>
		public static Vector2 Zero => new Vector2(0, 0);
		/// <summary>Shortcut for a Vector2 with both components set to 1</summary>
		public static Vector2 One  => new Vector2(1, 1);


		/// <summary>Shortcut for a normalised Vector2 pointing right (0 degrees)</summary>
		public static Vector2 Right => new Vector2( 1,  0);
		/// <summary>Shortcut for a normalised Vector2 pointing down (90 degrees)</summary>
		public static Vector2 Down  => new Vector2( 0, -1);
		/// <summary>Shortcut for a normalised Vector2 pointing left (180 degrees)</summary>
		public static Vector2 Left  => new Vector2(-1,  0);
		/// <summary>Shortcut for a normalised Vector2 pointing up (270 degrees)</summary>
		public static Vector2 Up    => new Vector2( 0,  1);

		#endregion

		#region Properties

		public float X { get; set; }
		public float Y { get; set; }

		/// <summary>Shortcut for X and Y according to the parity of the index; even accesses X, odd accesses Y</summary>
		public float this[int index] {
			get => (index & 1) == 0 ? X : Y;
			set {
				if ((index & 1) == 0)
					X = value;
				else
					Y = value;
			}
		}

		/// <summary>Shortcut for X, use for readability when treating a Vector2 as dimensions instead of coordinates</summary>
		public float Width  => X;
		/// <summary>Shortcut for Y, use for readability when treating a Vector2 as dimensions instead of coordinates</summary>
		public float Height => Y;

		#endregion

		#region Constructor

		public Vector2(float x, float y)
		{
			X = x;
			Y = y;
		}

		public Vector2(float xy) : this(xy, xy) 
		{
		}

		#endregion

		#region Static Methods

		/// <summary>Returns a copy of <paramref name="vec"/> with its components scaled to a length of 1</summary>
		public static Vector2 Normalise(Vector2 vec)
		{
			return vec / vec.Length();
		}

		/// <summary>Returns a copy of <paramref name="vec"/> with its components scaled to a length of <paramref name="magnitude"/></summary>
		public static Vector2 Resize(Vector2 vec, float magnitude)
		{
			return vec * (magnitude / vec.Length());
		}

		/// <summary>Returns a normalised Vector2 that represents a clockwise rotation around the origin</summary>
		public static Vector2 FromAngle(float degrees)
		{
			float rads = -degrees * AMath.TO_RADIANS;
			return new Vector2(AMath.Cos(rads), AMath.Sin(rads));
		}

		/// <summary>Returns a copy of <paramref name="vec"/> that has been rotated clockwise by <paramref name="degrees"/></summary>
		public static Vector2 Rotate(Vector2 vec, float degrees)
		{
			return FromAngle(vec.Angle() + degrees);
		}

		/// <summary>Returns a Vector2 that lies perpendicular to <paramref name="vec"/>. It's always rotated 90 degrees clockwise</summary>
		public static Vector2 Perpendicular(Vector2 vec)
		{
			return new Vector2(vec.Y, -vec.X);
		}

		/// <summary>Returns a Vector2 with the components of <paramref name="vec"/> clamped to a minimum value of <paramref name="min"/></summary>
		public static Vector2 Min(Vector2 vec, float min)
		{
			Vector2 copy = vec;
			copy.Min(min);
			return copy;
		}

		/// <summary>Returns a Vector2 with the components of <paramref name="vec"/> clamped to a minimum value of their respective components from <paramref name="min"/></summary>
		public static Vector2 Min(Vector2 vec, Vector2 min)
		{
			Vector2 copy = vec;
			copy.Min(min);
			return copy;
		}

		/// <summary>Returns a Vector2 with the components of <paramref name="vec"/> clamped to a maximum value of <paramref name="max"/></summary>
		public static Vector2 Max(Vector2 vec, float max)
		{
			Vector2 copy = vec;
			copy.Max(max);
			return copy;
		}

		/// <summary>Returns a Vector2 with the components of <paramref name="vec"/> clamped to a maximum value of their respective components from <paramref name="max"/></summary>
		public static Vector2 Max(Vector2 vec, Vector2 max)
		{
			Vector2 copy = vec;
			copy.Max(max);
			return copy;
		}

		/// <summary>Returns a Vector2 with the components of <paramref name="vec"/> clamped to a minimum value of 
		/// <paramref name="min"/> and a maximum value of <paramref name="max"/></summary>
		public static Vector2 Clamp(Vector2 vec, float min, float max)
		{
			Vector2 copy = vec;
			copy.Clamp(min, max);
			return copy;
		}

		/// <summary>Returns a Vector2 with the components of <paramref name="vec"/> clamped to a minimum value of 
		/// their respective components from <paramref name="min"/> and a maximum value of <paramref name="max"/></summary>
		public static Vector2 Clamp(Vector2 vec, Vector2 min, Vector2 max)
		{
			Vector2 copy = vec;
			copy.Clamp(min, max);
			return copy;
		}

		/// <summary>Returns a Vector2 that represents a point along the line from <paramref name="start"/> to 
		/// <paramref name="end"/> at the normalised position of <paramref name="amount"/></summary>
		/// <param name="amount">Typically a value between 0 and 1 where 0 returns <paramref name="start"/> and 1 returns <paramref name="end"/></br>
		/// This value can be outside the range of 0 to 1 to get a point along the same line but beyond the start and end</param>
		public static Vector2 Lerp(Vector2 start, Vector2 end, float amount)
		{
			return start + ((end - start) * amount);
		}

		/// <summary>Returns a Vector2 that represents a point along the line from <paramref name="start"/> to 
		/// <paramref name="end"/> at the normalised position of <paramref name="amount"/></summary>
		/// <param name="amount">A value between 0 and 1 where 0 returns <paramref name="start"/> and 1 returns <paramref name="end"/></br>
		/// This value is clamped to the range 0 to 1 so that a point outside the line cannot be returned</param>
		public static Vector2 LerpClamped(Vector2 start, Vector2 end, float amount)
		{
			if		(amount < 0) amount = 0;
			else if (amount > 1) amount = 1;

			return Lerp(start, end, amount);
		}

		#endregion

		#region Methods

		/// <summary>Sets the components of this vector to <paramref name="xy"/></summary>
		public void Set(float xy)
		{
			X = xy;
			Y = xy;
		}

		/// <summary>Sets the components of this vector to <paramref name="x"/> and <paramref name="y"/> respectively</summary>
		public void Set(float x, float y)
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
		public float LengthSquared()
		{
			return (X * X) + (Y * Y);
		}

		/// <summary>Returns the distance between this vector and <paramref name="vec"/></summary>
		public float Distance(Vector2 vec)
		{
			float x = vec.X - X;
			float y = vec.Y - Y;
			return (float)Math.Sqrt((x * x) + (y * y));
		}

		/// <summary>Returns the distance squared between this vector and <paramref name="vec"/><br/>
		/// Should be used when the actual distance isn't required as it skips a square root operation</summary>
		public float DistanceSquared(Vector2 vec)
		{
			float x = vec.X - X;
			float y = vec.Y - Y;
			return (x * x) + (y * y);
		}

		/// <summary>Scales the components of this vector so that it has a length of 1</summary>
		public void Normalise()
		{
			float length = Length();
			X /= length;
			Y /= length;
		}

		/// <summary>Scales the components of this vector so that is has a length of <paramref name="magnitude"/></summary>
		public void Resize(float magnitude)
		{
			float scalar = magnitude / Length();
			X *= scalar;
			Y *= scalar;
		}
		
		/// <summary>Returns the angle of this vector around the origin, starting at 0 pointing right and going clockwise</summary>
		/// <param name="radians">Whether the returned angle should be in radians or degrees</param>
		public float Angle(bool radians = false)
		{
			float rads = (float)Math.Atan2(Y, X);
			rads = rads < 0 ? -rads : (AMath.PI * 2) - rads;

			return radians ? rads : rads * AMath.TO_DEGREES;
		}

		/// <summary>Rotates this vector clockwise by <paramref name="degrees"/> around the origin</summary>
		public void Rotate(float degrees)
		{
			float rads = Angle(true) + (degrees * AMath.TO_RADIANS);
			X = AMath.Cos(rads);
			Y = AMath.Sin(rads);
		}

		/// <summary>Returns the scalar dot product of this vector and <paramref name="vec"/></summary>
		public float Dot(Vector2 vec)
		{
			return (X * vec.X) + (Y * vec.Y);
		}

		/// <summary>Returns the scalar cross product of this vector and <paramref name="vec"/><br/>
		/// The sign of the cross product tells us if this vector is counter-clockwise or clockwise to <paramref name="vec"/></summary>
		public float Cross(Vector2 vec)
		{
			return (X * vec.Y) - (Y * vec.X);
		}

		/// <summary>Clamps the components of this vector to a minimum value of <paramref name="min"/></summary>
		public void Min(float min)
		{
			if (X < min) X = min;
			if (Y < min) Y = min;
		}

		/// <summary>Clamps the components of this vector to a minimum value of their respective components from <paramref name="min"/></summary>
		public void Min(Vector2 min)
		{
			if (X < min.X) X = min.X;
			if (Y < min.Y) Y = min.Y;
		}

		/// <summary>Clamps the components of this vector to a maximum value of <paramref name="max"/></summary>
		public void Max(float max)
		{
			if (X > max) X = max;
			if (Y > max) Y = max;
		}

		/// <summary>Clamps the components of this vector to a maximum value of their respective components from <paramref name="max"/></summary>
		public void Max(Vector2 max)
		{
			if (X > max.X) X = max.X;
			if (Y > max.Y) Y = max.Y;
		}

		/// <summary>Clamps the components of this vector to a minimum value of <paramref name="min"/> and a maximum value of <paramref name="max"/></summary>
		public void Clamp(float min, float max)
		{
			if		(X < min) X = min;
			else if (X > max) X = max;
			if		(Y < min) Y = min;
			else if (Y > max) Y = max;
		}

		/// <summary>Clamps the components of this vector to a minimum value of their respective components from <paramref name="min"/> and a maximum of <paramref name="max"/></summary>
		public void Clamp(Vector2 min, Vector2 max)
		{
			if		(X < min.X) X = min.X;
			else if (X > max.X) X = max.X;
			if		(Y < min.Y) Y = min.Y;
			else if (Y > max.Y) Y = max.Y;
		}
		
		/// <summary>Converts the vector to a string in the format "X, Y" (e.g. "2, 3")</summary>
		public override string ToString()
		{
			return $"{X}, {Y}";
		}

		public override bool Equals(object obj) => obj is Vector2 vec && this == vec;
		public override int GetHashCode() => base.GetHashCode();

		#endregion

		#region Operators

		public static Vector2 operator -(Vector2 vec) => new Vector2(-vec.X, -vec.Y);

		public static bool operator ==(Vector2 left, Vector2 right) => AMath.LenientApprox(left.X, right.X) && AMath.LenientApprox(left.Y, right.Y);
		public static bool operator !=(Vector2 left, Vector2 right) => !(left == right);

		public static Vector2 operator +(Vector2 vec, float scalar) => new Vector2(vec.X + scalar, vec.Y + scalar);
		public static Vector2 operator -(Vector2 vec, float scalar) => new Vector2(vec.X - scalar, vec.Y - scalar);
		public static Vector2 operator *(Vector2 vec, float scalar) => new Vector2(vec.X * scalar, vec.Y * scalar);
		public static Vector2 operator /(Vector2 vec, float scalar) => new Vector2(vec.X / scalar, vec.Y / scalar);

		public static Vector2 operator +(Vector2 left, Vector2 right) => new Vector2(left.X + right.X, left.Y + right.Y);
		public static Vector2 operator -(Vector2 left, Vector2 right) => new Vector2(left.X - right.X, left.Y - right.Y);
		public static Vector2 operator *(Vector2 left, Vector2 right) => new Vector2(left.X * right.X, left.Y * right.Y);
		public static Vector2 operator /(Vector2 left, Vector2 right) => new Vector2(left.X / right.X, left.Y / right.Y);

		public static explicit operator Vector2((float x, float y) tuple) => new Vector2(tuple.x, tuple.y);
		public static explicit operator (float x, float y)(Vector2 vec) => (x: vec.X, y: vec.Y);

		public static explicit operator IntVector2(Vector2 vec) => new IntVector2((int)vec.X, (int)vec.Y);

		#endregion
	}
}
