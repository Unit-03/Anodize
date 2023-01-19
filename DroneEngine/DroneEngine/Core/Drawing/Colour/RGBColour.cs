namespace Drone
{
	public struct RGBColour
	{
		#region Properties

		public byte R { get; set; }
		public byte G { get; set; }
		public byte B { get; set; }
		public byte A { get; set; }

		public byte this[int index] {
			get {
				switch (AMath.Mod(index, 4))
				{
					case 0: return R;
					case 1: return G;
					case 2: return B;
					case 3: return A;
					default: return 0;
				}
			}
			set { 
				switch (AMath.Mod(index, 4))
				{
					case 0: R = value; break;
					case 1: G = value; break;
					case 2: B = value; break;
					case 3: A = value; break;
				};
			}
		}

		public float RF {
			get => R / 255f;
			set => R = (byte)(AMath.Round(value * 255) & 0xFF);
		}

		public float GF {
			get => G / 255f;
			set => G = (byte)(AMath.Round(value * 255) & 0xFF);
		}

		public float BF {
			get => B / 255f;
			set => B = (byte)(AMath.Round(value * 255) & 0xFF);
		}

		public float AF {
			get => A / 255f;
			set => A = (byte)(AMath.Round(value * 255) & 0xFF);
		}

		#endregion

		#region Constructors

		public RGBColour(byte r, byte g, byte b, byte a = 255)
		{
			R = r;
			G = g;
			B = b;
			A = a;
		}

		public RGBColour(int r, int g, int b, int a = 255)
		{
			R = (byte)AMath.Clamp(r, 0, 255);
			G = (byte)AMath.Clamp(g, 0, 255);
			B = (byte)AMath.Clamp(b, 0, 255);
			A = (byte)AMath.Clamp(a, 0, 255);
		}

		public RGBColour(float r, float g, float b, float a = 1)
		{
			R = (byte)(AMath.Round(r * 255) & 0xFF);
			G = (byte)(AMath.Round(g * 255) & 0xFF);
			B = (byte)(AMath.Round(b * 255) & 0xFF);
			A = (byte)(AMath.Round(a * 255) & 0xFF);
		}

		public RGBColour(byte rgb, byte a)
		{
			R = rgb;
			G = rgb;
			B = rgb;
			A = a;
		}

		public RGBColour(int rgb, int a)
		{
			byte rgbB = (byte)AMath.Clamp(rgb, 0, 255);

			R = rgbB;
			G = rgbB;
			B = rgbB;
			A = (byte)AMath.Clamp(a, 0, 255);
		}

		public RGBColour(float rgb, float a)
		{
			R = (byte)(AMath.Round(rgb * 255) & 0xFF);
			G = (byte)(AMath.Round(rgb * 255) & 0xFF);
			B = (byte)(AMath.Round(rgb * 255) & 0xFF);
			A = (byte)(AMath.Round(a   * 255) & 0xFF);
		}

		public RGBColour(RGBColour rgb, byte a)
		{
			R = rgb.R;
			G = rgb.G;
			B = rgb.B;
			A = a;
		}

		public RGBColour(RGBColour rgb, int a)
		{
			R = rgb.R;
			G = rgb.G;
			B = rgb.B;
			A = (byte)AMath.Clamp(a, 0, 255);
		}

		public RGBColour(RGBColour rgb, float a)
		{
			R = rgb.R;
			G = rgb.G;
			B = rgb.B;
			A = (byte)(AMath.Round(a * 255) & 0xFF);
		}

		public RGBColour(int rgba)
		{
			R = (byte)((rgba >>  0) & 0xFF);
			G = (byte)((rgba >>  8) & 0xFF);
			B = (byte)((rgba >> 16) & 0xFF);
			A = (byte)((rgba >> 24) & 0xFF);
		}

		public RGBColour(uint rgba)
		{
			R = (byte)((rgba >>  0) & 0xFF);
			G = (byte)((rgba >>  8) & 0xFF);
			B = (byte)((rgba >> 16) & 0xFF);
			A = (byte)((rgba >> 24) & 0xFF);
		}

		#endregion

		#region Methods

		public override bool Equals(object obj) => obj is RGBColour colour && this == colour;
		public override int GetHashCode() => (int)this;

		public override string ToString()
		{
			return $"{R},{G},{B},{A}";
		}

		#endregion

		#region Static Methods

		public static RGBColour Lerp(RGBColour start, RGBColour end, float amount)
		{
			return LerpUnclamped(start, end, AMath.Clamp(amount, 0, 1));
		}

		public static RGBColour LerpUnclamped(RGBColour start, RGBColour end, float amount)
		{
			float r = (end.RF - start.RF) * amount;
			float g = (end.GF - start.GF) * amount;
			float b = (end.BF - start.BF) * amount;
			float a = (end.AF - start.AF) * amount;

			return new RGBColour(start.RF + r, start.GF + g, start.BF + b, start.AF + a);
		}

		public static RGBColour FromHSV(float hue, float saturation, float value, float alpha = 1)
		{
			return InternalFromHSV(AMath.Mod(hue, 360),
								   AMath.Clamp(saturation, 0, 1),
								   AMath.Clamp(value,      0, 1),
								   AMath.Clamp(alpha,      0, 1));
		}
		
		private static RGBColour InternalFromHSV(float h, float s, float v, float a)
		{
			float normalHue = h / 60;
			int indexHue = (int)normalHue;

			float hueEpsilon = normalHue - indexHue;

			float p = v * (1 -  s);
			float q = v * (1 - (s *      hueEpsilon));
			float t = v * (1 - (s * (1 - hueEpsilon)));

			float r, g, b;
			r = g = b = 0;

			switch (indexHue)
			{
				case 0:
					r = v;
					g = t;
					b = p;
					break;
				case 1:
					r = q;
					g = v;
					b = p;
					break;
				case 2:
					r = p;
					g = v;
					b = t;
					break;
				case 3:
					r = p;
					g = q;
					b = v;
					break;
				case 4:
					r = t;
					g = p;
					b = v;
					break;
				case 5:
					r = v;
					g = p;
					b = q;
					break;
			}

			return new RGBColour((byte)AMath.Round(r * 255),
				                 (byte)AMath.Round(g * 255),
								 (byte)AMath.Round(b * 255),
								 (byte)AMath.Round(a * 255));
		}

		public static RGBColour FromHSL(float hue, float saturation, float lightness, float alpha = 1)
		{
			return InternalFromHSL(AMath.Mod(hue, 360), 
								   AMath.Clamp(saturation, 0, 1), 
								   AMath.Clamp(lightness,  0, 1), 
								   AMath.Clamp(alpha,      0, 1));
		}

		private static RGBColour InternalFromHSL(float h, float s, float l, float a)
		{
			float r, g, b;

			if (AMath.Approx(s, 0))
			{
				r = g = b = l;
			}	
			else
			{
				static float HueToRGB(float p, float q, float t)
				{
					const float ONE_SIXTH  = 1f / 6;
					const float ONE_HALF   = 1f / 2;
					const float TWO_THIRDS = 2f / 3;

					t = AMath.Mod(t, 1);

					return t < ONE_SIXTH  ? p + ((q - p) * 6 * t) :
						   t < ONE_HALF   ? q:
						   t < TWO_THIRDS ? p + ((q - p) * 6 * (TWO_THIRDS - t)) :
											p;
				}

				const float ONE_THIRD = 1f / 3;

				float normalHue = h / 360;

				float q = l < 0.5f ? l * (1 + s) : l + s - (l * s);
				float p = (2 * l) - q;

				r = HueToRGB(p, q, normalHue + ONE_THIRD);
				g = HueToRGB(p, q, normalHue);
				b = HueToRGB(p, q, normalHue - ONE_THIRD);
			}

			return new RGBColour((byte)AMath.Round(r * 255),
								 (byte)AMath.Round(g * 255),
								 (byte)AMath.Round(b * 255),
								 (byte)AMath.Round(g * 255));
		}

		#endregion

		#region Operators

		// <||| Comparison |||>
		public static bool operator ==(RGBColour left, RGBColour right) => left.R == right.R && left.G == right.G && left.B == right.B && left.A == right.A;
		public static bool operator !=(RGBColour left, RGBColour right) => left.R != right.R || left.G != right.G || left.B != right.B || left.A != right.A;

		// <||| Arithmetic |||>
		public static RGBColour operator +(RGBColour left, RGBColour right) => new(left.R  + right.R,  left.G  + right.G,  left.B  + right.B,  left.A  + right.A);
		public static RGBColour operator -(RGBColour left, RGBColour right) => new(left.R  - right.R,  left.G  - right.G,  left.B  - right.B,  left.A  - right.A);
		public static RGBColour operator *(RGBColour left, RGBColour right) => new(left.RF * right.RF, left.GF * right.GF, left.BF * right.BF, left.AF * right.AF);
		public static RGBColour operator /(RGBColour left, RGBColour right) => new(left.RF / right.RF, left.GF / right.GF, left.BF / right.BF, left.AF / right.AF);

		public static RGBColour operator +(RGBColour left, int right) => new(left.R + right, left.G + right, left.B + right, left.A);
		public static RGBColour operator -(RGBColour left, int right) => new(left.R - right, left.G - right, left.B - right, left.A);
		public static RGBColour operator *(RGBColour left, int right) => new(left.R * right, left.G * right, left.B * right, left.A);
		public static RGBColour operator /(RGBColour left, int right) => new(left.R / right, left.G / right, left.B / right, left.A);

		public static RGBColour operator +(RGBColour left, float right) => new(left.RF + right, left.GF + right, left.BF + right, left.AF);
		public static RGBColour operator -(RGBColour left, float right) => new(left.RF - right, left.GF - right, left.BF - right, left.AF);
		public static RGBColour operator *(RGBColour left, float right) => new(left.RF * right, left.GF * right, left.BF * right, left.AF);
		public static RGBColour operator /(RGBColour left, float right) => new(left.RF / right, left.GF / right, left.BF / right, left.AF);

		// <||| Casts |||>
		public static explicit operator int(RGBColour colour)
		{
			return (colour.R <<  0) &
				   (colour.G <<  8) &
				   (colour.B << 16) &
				   (colour.A << 24);
		}

		public static explicit operator uint(RGBColour colour)
		{
			return ((uint)colour.R <<  0) &
				   ((uint)colour.G <<  8) &
				   ((uint)colour.B << 16) &
				   ((uint)colour.A << 24);
		}

		public static explicit operator RGBColour(HSVColour colour)
		{
			return InternalFromHSV(colour.Hue, colour.Saturation, colour.Value, colour.Alpha);
		}

		public static explicit operator RGBColour(HSLColour colour)
		{
			return InternalFromHSL(colour.Hue, colour.Saturation, colour.Lightness, colour.Alpha);
		}

		#endregion
	}
}
