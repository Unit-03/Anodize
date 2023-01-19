namespace Drone
{
	public struct HSLColour
	{
		#region Properties

		public float Hue {
			get => _hue;
			set => _hue = AMath.Mod(value, 360);
		}
		
		public float Saturation {
			get => _saturation;
			set => _saturation = AMath.Clamp(value, 0, 1);
		}
		
		public float Lightness {
			get => _lightness;
			set => _lightness = AMath.Clamp(value, 0, 1);
		}

		public float Alpha {
			get => _alpha;
			set => _alpha = AMath.Clamp(value, 0, 1);
		}

		#endregion

		#region Fields

		private float _hue;
		private float _saturation;
		private float _lightness;
		private float _alpha;

		#endregion

		#region Constructors

		public HSLColour(float hue, float saturation, float lightness, float alpha = 1)
		{
			_hue        = AMath.Mod(hue, 360);
			_saturation = AMath.Clamp(saturation, 0, 1);
			_lightness  = AMath.Clamp(lightness,  0, 1);
			_alpha      = AMath.Clamp(alpha,      0, 1);
		}

		#endregion

		#region Methods

		public override bool Equals(object obj) => obj is HSLColour colour && this == colour;
		public override int GetHashCode() => HashCode.Combine(Hue, Saturation, Lightness, Alpha);

		public override string ToString()
		{
			return $"{Hue},{Saturation},{Lightness},{Alpha}";
		}

		#endregion

		#region Static Methods

		public static HSLColour Lerp(HSLColour start, HSLColour end, float amount)
		{
			return LerpUnclamped(start, end, AMath.Clamp(amount, 0, 1));
		}

		public static HSLColour LerpUnclamped(HSLColour start, HSLColour end, float amount)
		{
			float hue        = (end.Hue        - start.Hue)        * amount;
			float saturation = (end.Saturation - start.Saturation) * amount;
			float lightness  = (end.Lightness  - start.Lightness)  * amount;
			float alpha      = (end.Alpha      - start.Alpha)      * amount;

			return new HSLColour(start.Hue + hue, start.Saturation + saturation, start.Lightness + lightness, start.Alpha + alpha);
		}

		public static HSLColour FromRGB(byte red, byte green, byte blue, byte alpha = 255)
		{
			return FromRGB(red   / 255f,
				           green / 255f,
						   blue  / 255f,
						   alpha / 255f);
		}

		public static HSLColour FromRGB(int red, int green, int blue, int alpha = 255)
		{
			return FromRGB(AMath.Clamp(red   / 255f, 0, 1),
						   AMath.Clamp(green / 255f, 0, 1),
						   AMath.Clamp(blue  / 255f, 0, 1),
						   AMath.Clamp(alpha / 255f, 0, 1));
		}

		public static HSLColour FromRGB(float red, float green, float blue, float alpha = 1)
		{
			return InternalFromRGB(AMath.Clamp(red,   0, 1),
								   AMath.Clamp(green, 0, 1),
								   AMath.Clamp(blue,  0, 1),
								   AMath.Clamp(alpha, 0, 1));
		}

		private static HSLColour InternalFromRGB(float r, float g, float b, float a)
		{
			float max = AMath.Max(r, g, b);
			float min = AMath.Min(r, g, b);

			float chroma = max - min;

			float hue = -1;

			if (max == min)
				hue = 0;
			else if (max == r)
				hue = ((60 * (g - b)) / chroma) + 360;
			else if (max == g)
				hue = ((60 * (b - r)) / chroma) + 120;
			else if (max == b)
				hue = ((60 * (r - g)) / chroma) + 240;

			hue %= 360;

			float lightness = (max + min) / 2;
			float saturation = chroma == 0 ? 0 : 1 - AMath.Abs((2 * lightness) - 1);

			return new HSLColour() {
				_hue        = hue,
				_saturation = saturation,
				_lightness  = lightness,
				_alpha      = a
			};
		}

		public static HSLColour FromHSV(float hue, float saturation, float value, float alpha = 1)
		{
			return InternalFromHSV(AMath.Mod(hue, 360), 
								   AMath.Clamp(saturation, 0, 1), 
								   AMath.Clamp(value,      0, 1), 
								   AMath.Clamp(alpha,      0, 1));
		}

		private static HSLColour InternalFromHSV(float h, float s, float v, float a)
		{
			float l = v * (1 - (s / 2));
			s = AMath.Approx(l, 0) || AMath.Approx(l, 1) ? 0 : (v - l) / AMath.Min(l, 1 - l);

			return new HSLColour() {
				_hue        = h,
				_saturation = s,
				_lightness  = l,
				_alpha      = a
			};
		}

		#endregion

		#region Operators

		// <||| Comparison |||>
		public static bool operator ==(HSLColour left, HSLColour right)
		{
			return AMath.Approx(left.Hue,        right.Hue)        &&
				   AMath.Approx(left.Saturation, right.Saturation) &&
				   AMath.Approx(left.Lightness,  right.Lightness)  &&
				   AMath.Approx(left.Alpha,      right.Alpha);
		}

		public static bool operator !=(HSLColour left, HSLColour right)
		{
			return !AMath.Approx(left.Hue,        right.Hue)        ||
				   !AMath.Approx(left.Saturation, right.Saturation) ||
				   !AMath.Approx(left.Lightness,  right.Lightness)  ||
				   !AMath.Approx(left.Alpha,      right.Alpha);
		}

		// <||| Casts |||>
		public static explicit operator HSLColour(RGBColour colour)
		{
			return InternalFromRGB(colour.RF,
								   colour.GF,
								   colour.BF,
								   colour.AF);
		}

		public static explicit operator HSLColour(HSVColour colour)
		{
			return InternalFromHSV(colour.Hue,
								   colour.Saturation,
								   colour.Value,
								   colour.Alpha);
		}

		#endregion
	}
}
