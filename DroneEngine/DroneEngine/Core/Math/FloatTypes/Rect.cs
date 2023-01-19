namespace Drone
{
	/// <summary>An axis-aligned rectangle of any size with support for repositioning and resizing around a pivot<br/>
	/// The pivot is normalised to the top-left corner of the rect</summary>
	public struct Rect
	{
		#region Properties

		// Position

		public float X { get; set; }
		public float Y { get; set; }

		public Vector2 Position {
			get => new Vector2(X, Y);
			set {
				X = value.X; 
				Y = value.Y;
			}
		}

		public Vector2 Centre {
			get => new Vector2(XMin + (Width / 2), YMin + (Height / 2));
			set {
				X = Margin.Left  + (value.X - (Width  / 2));
				Y = Margin.Right + (value.Y - (Height / 2));
			}
		}

		// Size

		public float Width { get; set; }
		public float Height { get; set; }

		public Vector2 Size {
			get => new Vector2(Width, Height);
			set {
				Width  = value.Width;
				Height = value.Height;
			}
		}

		public Vector2 Extents {
			get => new Vector2(Width / 2, Height / 2);
			set {
				Width  = value.Width  * 2;
				Height = value.Height * 2;
			}
		}

		// Pivot

		public float PivotX { get; set; }
		public float PivotY { get; set; }

		public Vector2 Pivot {
			get => new Vector2(PivotX, PivotY);
			set {
				PivotX = value.X;
				PivotY = value.Y;
			}
		}

		// Offsets

		public Margin Margin {
			get => new Margin(X - XMin, XMax - X, 
							  Y - YMin, YMax - Y);
			set {
				Width  = value.TotalX;
				Height = value.TotalY;

				PivotX = value.Left / Width;
				PivotY = value.Top  / Height;
			}
		}

		public Margin Border {
			get => new Margin(XMin, XMax, YMin, YMax);
			set {
				Width  = value.Right  - value.Left;
				Height = value.Bottom - value.Top;

				X = value.Left + (PivotX * Width);
				Y = value.Top  + (PivotY * Height);
			}
		}

		public float XMin => X - (Width * PivotX);
		public float XMax => XMin + Width;
		public float YMin => Y - (Height * PivotY);
		public float YMax => YMin + Height;

		#endregion

		#region Constructor

		public Rect(float x, float y, float width, float height) : this(x, y, width, height, 0, 0)
		{
		}

		public Rect(float x, float y, float width, float height, float pivotX, float pivotY)
		{
			X = x;
			Y = y;

			Width  = width;
			Height = height;

			PivotX = pivotX;
			PivotY = pivotY;
		}

		public Rect(Vector2 position, Vector2 size) : this(position.X, position.Y, size.Width, size.Height)
		{
		}

		public Rect(Vector2 position, Vector2 pivot, Vector2 size) : this(position.X, position.Y, size.Width, size.Height, pivot.X, pivot.Y)
		{
		}

		#endregion

		#region Methods
		
		/// <summary>Sets the position of the rect directly without adjusting the size or pivot</summary>
		public void Set(float x, float y)
		{
			X = x;
			Y = y;
		}

		/// <summary>Sets the position and size of the rect directly without adjusting the pivot</summary>
		public void Set(float x, float y, float width, float height)
		{
			Width  = width;
			Height = height;

			Set(x, y);
		}

		/// <summary>Sets the position, size, and pivot of the rect directly</summary>
		public void Set(float x, float y, float width, float height, float pivotX, float pivotY)
		{
			Set(x, y, width, height);

			PivotX = pivotX;
			PivotY = pivotY;
		}

		/// <summary>Sets the position of the rect directly without adjusting the size or pivot</summary>
		public void Set(Vector2 position)
		{
			Set(position.X, position.Y);
		}

		/// <summary>Sets the position and size of the rect directly without adjusting the pivot</summary>
		public void Set(Vector2 position, Vector2 size)
		{
			Set(position.X, position.Y, size.Width, size.Height);
		}

		/// <summary>Sets the position, size, and pivot of the rect directly</summary>
		public void Set(Vector2 position, Vector2 size, Vector2 pivot)
		{
			Set(position.X, position.Y, size.Width, size.Height, pivot.X, pivot.Y);
		}

		/// <summary>Returns true if a set of co-ordinates lies within this rect</summary>
		public bool Contains(float x, float y)
		{
			return x >= XMin && x <= XMax && 
				   y >= YMin && y <= YMax;
		}

		/// <summary>Returns true if a set of co-ordinates lies within this rect</summary>
		public bool Contains(Vector2 point)
		{
			return Contains(point.X, point.Y);
		}

		/// <summary>Returns true if this rect overlaps with <paramref name="rect"/></summary>
		public bool Overlaps(Rect rect)
		{
			return XMin <= rect.XMax && XMax >= rect.XMax &&
				   YMin <= rect.YMax && YMax >= rect.YMin;
		}

		/// <summary>Converts a set of absolute world coordinates to coordinates relative to the size of the rect</summary>
		public Vector2 AbsoluteToRelative(float absoluteX, float absoluteY)
		{
			return new Vector2((absoluteX - XMin) / Width, (absoluteY - YMin) / Height);
		}

		/// <summary>Converts a set of absolute world coordinates to coordinates relative to the size of the rect</summary>
		public Vector2 AbsoluteToRelative(Vector2 absolutePoint)
		{
			return AbsoluteToRelative(absolutePoint.X, absolutePoint.Y);
		}

		/// <summary>Converts a set of coordinates relative to the size of the rect to absolute world coordinates</summary>
		public Vector2 RelativeToAbsolute(float relativeX, float relativeY)
		{
			return new Vector2(relativeX * Width, relativeY * Height);
		}

		/// <summary>Converts a set of coordinates relative to the size of the rect to absolute world coordinates</summary>
		public Vector2 RelativeToAbsolute(Vector2 relativePoint)
		{
			return RelativeToAbsolute(relativePoint.X, relativePoint.Y);
		}

		/// <summary>Converts the rect to a string in the format "X, Y, PivotX, PivotY, WidthxHeight" (e.g. "20, 45, 0.5, 1, 100x50")</summary>
		public override string ToString()
		{
			return $"{X}, {Y}, {PivotX}, {PivotY}, {Width}x{Height}";
		}

		public override bool Equals(object obj) => obj is Rect rect && rect == this;
		public override int GetHashCode() => base.GetHashCode();

		#endregion

		#region Operators

		public static bool operator ==(Rect left, Rect right)
		{
			return left.X      == right.X      && left.Y      == right.Y      &&
				   left.Width  == right.Width  && left.Height == right.Height &&
				   left.PivotX == right.PivotX && left.PivotY == right.PivotY;
		}

		public static bool operator !=(Rect left, Rect right)
		{
			return !(left == right);
		}

		#endregion
	}
}
