namespace Drone
{
	/// <summary>An axis-aligned rectangle of any size with support for repositioning and resizing around a pivot<br/>
	/// The pivot is normalised to the top-left corner of the rect</summary>
	public struct IntRect
	{
		#region Properties

		// Position

		public int X { get; set; }
		public int Y { get; set; }

		public IntVector2 Position {
			get => new IntVector2(X, Y);
			set {
				X = value.X; 
				Y = value.Y;
			}
		}

		public IntVector2 Centre {
			get => new IntVector2(XMin + (Width / 2), YMin + (Height / 2));
			set {
				X = Margin.Left  + (value.X - (Width  / 2));
				Y = Margin.Right + (value.Y - (Height / 2));
			}
		}

		// Size

		public int Width { get; set; }
		public int Height { get; set; }

		public IntVector2 Size {
			get => new IntVector2(Width, Height);
			set {
				Width  = value.Width;
				Height = value.Height;
			}
		}

		public IntVector2 Extents {
			get => new IntVector2(Width / 2, Height / 2);
			set {
				Width  = value.Width  * 2;
				Height = value.Height * 2;
			}
		}

		// Pivot

		public int PivotX { get; set; }
		public int PivotY { get; set; }

		public IntVector2 Pivot {
			get => new IntVector2(PivotX, PivotY);
			set {
				PivotX = value.X;
				PivotY = value.Y;
			}
		}

		// Offsets

		public IntMargin Margin {
			get => new IntMargin(X - XMin, XMax - X, 
							  Y - YMin, YMax - Y);
			set {
				Width  = value.TotalX;
				Height = value.TotalY;

				PivotX = value.Left / Width;
				PivotY = value.Top  / Height;
			}
		}

		public IntMargin Border {
			get => new IntMargin(XMin, XMax, YMin, YMax);
			set {
				Width  = value.Right  - value.Left;
				Height = value.Bottom - value.Top;

				X = value.Left + (PivotX * Width);
				Y = value.Top  + (PivotY * Height);
			}
		}

		public int XMin => X - (Width * PivotX);
		public int XMax => XMin + Width;
		public int YMin => Y - (Height * PivotY);
		public int YMax => YMin + Height;

		#endregion

		#region Constructor

		public IntRect(int x, int y, int width, int height) : this(x, y, width, height, 0, 0)
		{
		}

		public IntRect(int x, int y, int width, int height, int pivotX, int pivotY)
		{
			X = x;
			Y = y;

			Width  = width;
			Height = height;

			PivotX = pivotX;
			PivotY = pivotY;
		}

		public IntRect(IntVector2 position, IntVector2 size) : this(position.X, position.Y, size.Width, size.Height)
		{
		}

		public IntRect(IntVector2 position, IntVector2 pivot, IntVector2 size) : this(position.X, position.Y, size.Width, size.Height, pivot.X, pivot.Y)
		{
		}

		#endregion

		#region Methods
		
		/// <summary>Sets the position of the rect directly without adjusting the size or pivot</summary>
		public void Set(int x, int y)
		{
			X = x;
			Y = y;
		}

		/// <summary>Sets the position and size of the rect directly without adjusting the pivot</summary>
		public void Set(int x, int y, int width, int height)
		{
			Width  = width;
			Height = height;

			Set(x, y);
		}

		/// <summary>Sets the position, size, and pivot of the rect directly</summary>
		public void Set(int x, int y, int width, int height, int pivotX, int pivotY)
		{
			Set(x, y, width, height);

			PivotX = pivotX;
			PivotY = pivotY;
		}

		/// <summary>Sets the position of the rect directly without adjusting the size or pivot</summary>
		public void Set(IntVector2 position)
		{
			Set(position.X, position.Y);
		}

		/// <summary>Sets the position and size of the rect directly without adjusting the pivot</summary>
		public void Set(IntVector2 position, IntVector2 size)
		{
			Set(position.X, position.Y, size.Width, size.Height);
		}

		/// <summary>Sets the position, size, and pivot of the rect directly</summary>
		public void Set(IntVector2 position, IntVector2 size, IntVector2 pivot)
		{
			Set(position.X, position.Y, size.Width, size.Height, pivot.X, pivot.Y);
		}

		/// <summary>Returns true if a set of co-ordinates lies within this rect</summary>
		public bool Contains(int x, int y)
		{
			return x >= XMin && x <= XMax && 
				   y >= YMin && y <= YMax;
		}

		/// <summary>Returns true if a set of co-ordinates lies within this rect</summary>
		public bool Contains(IntVector2 point)
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
		public Vector2 AbsoluteToRelative(int absoluteX, int absoluteY)
		{
			return new Vector2((absoluteX - XMin) / (float)Width, 
							   (absoluteY - YMin) / (float)Height);
		}

		/// <summary>Converts a set of absolute world coordinates to coordinates relative to the size of the rect</summary>
		public Vector2 AbsoluteToRelative(IntVector2 absolutePoint)
		{
			return AbsoluteToRelative(absolutePoint.X, absolutePoint.Y);
		}

		/// <summary>Converts a set of coordinates relative to the size of the rect to absolute world coordinates</summary>
		public IntVector2 RelativeToAbsolute(float relativeX, float relativeY)
		{
			return new IntVector2((int)((relativeX * Width)  + 0.5f), 
								  (int)((relativeY * Height) + 0.5f));
		}

		/// <summary>Converts a set of coordinates relative to the size of the rect to absolute world coordinates</summary>
		public IntVector2 RelativeToAbsolute(Vector2 relativePoint)
		{
			return RelativeToAbsolute(relativePoint.X, relativePoint.Y);
		}

		/// <summary>Converts the rect to a string in the format "X, Y, PivotX, PivotY, WidthxHeight" (e.g. "20, 45, 0.5, 1, 100x50")</summary>
		public override string ToString()
		{
			return $"{X}, {Y}, {PivotX}, {PivotY}, {Width}x{Height}";
		}

		public override bool Equals(object obj) => obj is IntRect rect && rect == this;
		public override int GetHashCode() => base.GetHashCode();

		#endregion

		#region Operators

		public static bool operator ==(IntRect left, IntRect right)
		{
			return left.X      == right.X      && left.Y      == right.Y      &&
				   left.Width  == right.Width  && left.Height == right.Height &&
				   left.PivotX == right.PivotX && left.PivotY == right.PivotY;
		}

		public static bool operator !=(IntRect left, IntRect right)
		{
			return !(left == right);
		}

		#endregion
	}
}
