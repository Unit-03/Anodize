namespace Drone
{
	public struct HSVColour
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
		
		public float Value {
			get => _value;
			set => _value = AMath.Clamp(value, 0, 1);
		}

		public float Alpha {
			get => _alpha;
			set => _alpha = AMath.Clamp(value, 0, 1);
		}

		#endregion

		#region Fields

		private float _hue;
		private float _saturation;
		private float _value;
		private float _alpha;

		#endregion

		#region Constructors

		public HSVColour(float hue, float saturation, float value, float alpha = 1)
		{
			_hue        = AMath.Mod(hue, 360);
			_saturation = AMath.Clamp(saturation, 0, 1);
			_value      = AMath.Clamp(value,      0, 1);
			_alpha      = AMath.Clamp(alpha,      0, 1);
		}

		#endregion

		#region Methods

		public override bool Equals(object obj) => obj is HSVColour colour && this == colour;
		public override int GetHashCode() => HashCode.Combine(Hue, Saturation, Value, Alpha);

		public override string ToString()
		{
			return $"{Hue},{Saturation},{Value},{Alpha}";
		}

		#endregion

		#region Static Methods

		public static HSVColour Lerp(HSVColour start, HSVColour end, float amount)
		{
			return LerpUnclamped(start, end, AMath.Clamp(amount, 0, 1));
		}

		public static HSVColour LerpUnclamped(HSVColour start, HSVColour end, float amount)
		{
			float hue        = (end.Hue        - start.Hue)        * amount;
			float saturation = (end.Saturation - start.Saturation) * amount;
			float value      = (end.Value      - start.Value)      * amount;
			float alpha      = (end.Alpha      - start.Alpha)      * amount;

			return new HSVColour(start.Hue + hue, start.Saturation + saturation, start.Value + value, start.Alpha + alpha);
		}

		public static HSVColour FromRGB(byte red, byte green, byte blue, byte alpha = 255)
		{
			return InternalFromRGB(red   / 255f, 
								   green / 255f, 
								   blue  / 255f, 
								   alpha / 255f);
		}

		public static HSVColour FromRGB(int red, int green, int blue, int alpha = 255)
		{
			return InternalFromRGB(AMath.Clamp(red   / 255f, 0, 1),
								   AMath.Clamp(green / 255f, 0, 1),
								   AMath.Clamp(blue  / 255f, 0, 1),
								   AMath.Clamp(alpha / 255f, 0, 1));
		}

		public static HSVColour FromRGB(float red, float green, float blue, float alpha = 1)
		{
			return InternalFromRGB(AMath.Clamp(red,   0, 1), 
								   AMath.Clamp(green, 0, 1), 
								   AMath.Clamp(blue,  0, 1), 
								   AMath.Clamp(alpha, 0, 1));
		}

		private static HSVColour InternalFromRGB(float r, float g, float b, float a)
		{
			float max = AMath.Max(r, g, b);
			float min = AMath.Min(r, g, b);

			float chroma = max - min;

			float hue = -1;

			if (max == min)
				hue = 0;
			else if (max == r)
				hue = (((60 * (g - b)) / chroma) + 360) % 360;
			else if (max == g)
				hue = (((60 * (b - r)) / chroma) + 120) % 360;
			else if (max == b)
				hue = (((60 * (r - g)) / chroma) + 240) % 360;

			float saturation = max == 0 ? 0 : chroma / max;
			float value = max;

			return new HSVColour() {
				_hue        = hue,
				_saturation = saturation,
				_value      = value,
				_alpha      = a
			};
		}

		public static HSVColour FromHSL(float hue, float saturation, float lightness, float alpha = 1)
		{
			return InternalFromHSL(AMath.Clamp(hue,        0, 1),
								   AMath.Clamp(saturation, 0, 1),
								   AMath.Clamp(lightness,  0, 1),
								   AMath.Clamp(alpha,      0, 1));
		}

		private static HSVColour InternalFromHSL(float h, float s, float l, float a)
		{
			float v = l + (s * AMath.Min(l, 1 - l));
			s = AMath.Approx(v, 0) ? 0 : 2 * (1 - (l / v));

			return new HSVColour() {
				_hue        = h,
				_saturation = s,
				_value      = v,
				_alpha      = a
			};
		}

		#endregion

		#region Operators

		// <||| Comparison |||>
		public static bool operator ==(HSVColour left, HSVColour right)
		{
			return AMath.Approx(left.Hue,        right.Hue)        &&
				   AMath.Approx(left.Saturation, right.Saturation) &&
				   AMath.Approx(left.Value,      right.Value)      &&
				   AMath.Approx(left.Alpha,      right.Alpha);
		}

		public static bool operator !=(HSVColour left, HSVColour right)
		{
			return !AMath.Approx(left.Hue,        right.Hue)        ||
				   !AMath.Approx(left.Saturation, right.Saturation) ||
				   !AMath.Approx(left.Value,      right.Value)      ||
				   !AMath.Approx(left.Alpha,      right.Alpha);
		}

		// <||| Casts |||>
		public static explicit operator HSVColour(RGBColour colour)
		{
			return InternalFromRGB(colour.RF,
								   colour.GF,
								   colour.BF,
								   colour.AF);
		}

		public static explicit operator HSVColour(HSLColour colour)
		{
			return InternalFromHSL(colour.Hue,
								   colour.Saturation,
								   colour.Lightness,
								   colour.Alpha);
		}

		#endregion
	}
}
