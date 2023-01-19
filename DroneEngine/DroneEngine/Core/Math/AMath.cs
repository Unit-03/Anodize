using System.Runtime.CompilerServices;

namespace Drone
{
	public static class AMath
	{
		#region Constants

		public const float     EPSILON = float.Epsilon;
		public const float BIG_EPSILON = 1.401298E-7f;

		public const float PI = 3.14159265f;
		public const float PI_2 = PI / 2;
		public const float PI_4 = PI / 4;

		public const float TO_RADIANS = PI / 180;
		public const float TO_DEGREES = 180 / PI;

		public const double     EPSILON_D = double.Epsilon;
		public const double BIG_EPSILON_D = 4.9406564584124654E-36;
		
		public const double PI_D = 3.1415926535897931d;
		public const double PI_2_D = PI_D / 2;
		public const double PI_4_D = PI_D / 4;

		public const double TO_RADIANS_D = PI_D / 180;
		public const double TO_DEGREES_D = 180 / PI_D;

		#endregion

		#region Trigonometry

		public static float Sin(float radians)
		{
			radians = Wrap(radians, -PI, PI);

			const float A0 =  1.0000000000000000e-0f;
			const float A1 = -1.6666666666401691e-1f;
			const float A2 =  8.3333333164901135e-3f;
			const float A3 = -1.9841266006591713e-4f;
			const float A4 =  2.7556901149173748e-6f;
			const float A5 = -2.5028452272926929e-8f;
			const float A6 =  1.5387306359264175e-10f;

			float rad2 = radians * radians;

			return radians * (A0 + (rad2 * 
							 (A1 + (rad2 * 
							 (A2 + (rad2 * 
							 (A3 + (rad2 * 
							 (A4 + (rad2 * 
							 (A5 + (rad2 * 
							  A6))))))))))));
		}

		public static float Cos(float radians)
		{
			return Sin(PI_2 - radians);
		}

		public static double Sin(double radians)
		{
			radians = Wrap(radians, -PI_D, PI_D);

			const double A0 =  1.0000000000000000e-0d;
			const double A1 = -1.6666666666401691e-1d;
			const double A2 =  8.3333333164901135e-3d;
			const double A3 = -1.9841266006591713e-4d;
			const double A4 =  2.7556901149173748e-6d;
			const double A5 = -2.5028452272926929e-8d;
			const double A6 =  1.5387306359264175e-10d;

			double rad2 = radians * radians;

			return radians * (A0 + (rad2 * 
							 (A1 + (rad2 * 
							 (A2 + (rad2 * 
							 (A3 + (rad2 * 
							 (A4 + (rad2 * 
							 (A5 + (rad2 * 
							  A6))))))))))));
		}

		public static double Cos(double radians)
		{
			return Sin(PI_2_D - radians);
		}

		#endregion

		#region Approximately

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool		 Approx(float  value, float  target) => Abs(target - value) <= EPSILON;
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool        Approx(double value, double target) => Abs(target - value) <= EPSILON_D;

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool LenientApprox(float  value, float  target) => Abs(target - value) <= BIG_EPSILON;
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool LenientApprox(double value, double target) => Abs(target - value) <= BIG_EPSILON_D;

		#endregion

		#region Absolute

		// Retains original type
		public static sbyte  Abs(sbyte  value) => value < 0 ? (sbyte)-value : value;
		public static short  Abs(short  value) => value < 0 ? (short)-value : value;
		public static int    Abs(int    value) => value < 0 ?        -value : value;
		public static long   Abs(long   value) => value < 0 ?        -value : value;
		public static float  Abs(float  value) => value < 0 ?        -value : value;
		public static double Abs(double value) => value < 0 ?        -value : value;

		// Converts to an unsigned integral
		public static byte   AbsU(sbyte value) => (byte)  (value < 0 ? -value : value);
		public static ushort AbsU(short value) => (ushort)(value < 0 ? -value : value);
		public static uint   AbsU(int   value) => (uint)  (value < 0 ? -value : value);
		public static ulong  AbsU(long  value) => (ulong) (value < 0 ? -value : value);

		#endregion

		#region Rounding

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static int  Round(float  value) => (int) (value + 0.5f + EPSILON);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static long Round(double value) => (long)(value + 0.5d + EPSILON_D);

		public static float Round(float value, int points)
		{
			double scalar = Math.Pow(10, points);
			return (float)((int)((value * scalar) + 0.5 + EPSILON) / scalar);
		}

		public static double Round(double value, long points)
		{
			double scalar = Math.Pow(10, points);
			return (double)((long)((value * scalar) + 0.5 + EPSILON_D) / scalar);
		}

		#endregion

		#region Modulus

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static sbyte  Mod(sbyte  value, sbyte  modulus) => (sbyte) (value < 0 ? modulus + (value % modulus) : value % modulus);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static byte   Mod(byte   value, byte   modulus) => (byte)  (                                          value % modulus);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static short  Mod(short  value, short  modulus) => (short) (value < 0 ? modulus + (value % modulus) : value % modulus);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static ushort Mod(ushort value, ushort modulus) => (ushort)(                                          value % modulus);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static int    Mod(int    value, int    modulus) =>          value < 0 ? modulus + (value % modulus) : value % modulus;
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static uint   Mod(uint   value, uint   modulus) =>												    value % modulus;
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static long   Mod(long   value, long   modulus) =>          value < 0 ? modulus + (value % modulus) : value % modulus;
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static ulong  Mod(ulong  value, ulong  modulus) =>												    value % modulus;
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static float  Mod(float  value, float  modulus) =>		  value < 0 ? modulus + (value % modulus) : value % modulus;
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static double Mod(double value, double modulus) =>		  value < 0 ? modulus + (value % modulus) : value % modulus;

		#endregion

		#region Ceiling
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static int  Ceil(float  value) =>  (int)(value + 1 + EPSILON);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static long Ceil(double value) => (long)(value + 1 + EPSILON_D);

		#endregion

		#region Floor

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static int  Floor(float  value) =>  (int)(value + EPSILON);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static long Floor(double value) => (long)(value + EPSILON_D);

		#endregion

		#region Minimum

		// Built-in Type Implementations
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static sbyte   Min(sbyte   left, sbyte   right) => left < right ? left : right;
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static byte    Min(byte    left, byte    right) => left < right ? left : right;
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static short   Min(short   left, short   right) => left < right ? left : right;
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static ushort  Min(ushort  left, ushort  right) => left < right ? left : right;
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static int     Min(int     left, int     right) => left < right ? left : right;
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static uint    Min(uint    left, uint    right) => left < right ? left : right;
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static long    Min(long    left, long    right) => left < right ? left : right;
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static ulong   Min(ulong   left, ulong   right) => left < right ? left : right;
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static float   Min(float   left, float   right) => left < right ? left : right;
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static double  Min(double  left, double  right) => left < right ? left : right;

		public static sbyte Min(params sbyte[] values)
		{
			sbyte min = values[0];

			for (int i = values.Length - 1; i > 1; --i) 
				if (values[i] < min) min = values[i];

			return min;
		}

		public static byte Min(params byte[] values)
		{
			byte min = values[0];

			for (int i = values.Length - 1; i > 1; --i) 
				if (values[i] < min) min = values[i];

			return min;
		}

		public static short Min(params short[] values)
		{
			short min = values[0];

			for (int i = values.Length - 1; i > 1; --i) 
				if (values[i] < min) min = values[i];

			return min;
		}

		public static ushort Min(params ushort[] values)
		{
			ushort min = values[0];

			for (int i = values.Length - 1; i > 1; --i) 
				if (values[i] < min) min = values[i];

			return min;
		}

		public static int Min(params int[] values)
		{
			int min = values[0];

			for (int i = values.Length - 1; i > 1; --i) 
				if (values[i] < min) min = values[i];

			return min;
		}

		public static uint Min(params uint[] values)
		{
			uint min = values[0];

			for (int i = values.Length - 1; i > 1; --i) 
				if (values[i] < min) min = values[i];

			return min;
		}

		public static long Min(params long[] values)
		{
			long min = values[0];

			for (int i = values.Length - 1; i > 1; --i) 
				if (values[i] < min) min = values[i];

			return min;
		}

		public static ulong Min(params ulong[] values)
		{
			ulong min = values[0];

			for (int i = values.Length - 1; i > 1; --i) 
				if (values[i] < min) min = values[i];

			return min;
		}

		public static float Min(params float[] values)
		{
			float min = values[0];

			for (int i = values.Length - 1; i > 1; --i) 
				if (values[i] < min) min = values[i];

			return min;
		}

		public static double Min(params double[] values)
		{
			double min = values[0];

			for (int i = values.Length - 1; i > 1; --i) 
				if (values[i] < min) min = values[i];

			return min;
		}

		public static decimal Min(params decimal[] values)
		{
			decimal min = values[0];

			for (int i = values.Length - 1; i > 1; --i) 
				if (values[i] < min) min = values[i];

			return min;
		}

		// Generic Implementation
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T Min<T>(T left, T right) where T : IComparable<T> => left.CompareTo(right) < 0 ? left : right;

		public static T Min<T>(params T[] values) where T : IComparable<T>
		{
			T min = values[0];

			for (int i = values.Length - 1; i > 1; --i)
			{
				if (values[i].CompareTo(min) < 0)
					min = values[i];
			}

			return min;
		}

		#endregion

		#region Maximum

		// Built-in Type Implementations
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static sbyte   Max(sbyte   left, sbyte   right) => left > right ? left : right;
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static byte    Max(byte    left, byte    right) => left > right ? left : right;
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static short   Max(short   left, short   right) => left > right ? left : right;
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static ushort  Max(ushort  left, ushort  right) => left > right ? left : right;
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static int     Max(int     left, int     right) => left > right ? left : right;
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static uint    Max(uint    left, uint    right) => left > right ? left : right;
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static long    Max(long    left, long    right) => left > right ? left : right;
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static ulong   Max(ulong   left, ulong   right) => left > right ? left : right;
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static float   Max(float   left, float   right) => left > right ? left : right;
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static double  Max(double  left, double  right) => left > right ? left : right;

		public static sbyte Max(params sbyte[] values)
		{
			sbyte max = values[0];

			for (int i = values.Length - 1; i > 1; --i) 
				if (values[i] > max) max = values[i];

			return max;
		}

		public static byte Max(params byte[] values)
		{
			byte max = values[0];

			for (int i = values.Length - 1; i > 1; --i) 
				if (values[i] > max) max = values[i];

			return max;
		}

		public static short Max(params short[] values)
		{
			short max = values[0];

			for (int i = values.Length - 1; i > 1; --i) 
				if (values[i] > max) max = values[i];

			return max;
		}

		public static ushort Max(params ushort[] values)
		{
			ushort max = values[0];

			for (int i = values.Length - 1; i > 1; --i) 
				if (values[i] > max) max = values[i];

			return max;
		}

		public static int Max(params int[] values)
		{
			int max = values[0];

			for (int i = values.Length - 1; i > 1; --i) 
				if (values[i] > max) max = values[i];

			return max;
		}

		public static uint Max(params uint[] values)
		{
			uint max = values[0];

			for (int i = values.Length - 1; i > 1; --i) 
				if (values[i] > max) max = values[i];

			return max;
		}

		public static long Max(params long[] values)
		{
			long max = values[0];

			for (int i = values.Length - 1; i > 1; --i) 
				if (values[i] > max) max = values[i];

			return max;
		}

		public static ulong Max(params ulong[] values)
		{
			ulong max = values[0];

			for (int i = values.Length - 1; i > 1; --i) 
				if (values[i] > max) max = values[i];

			return max;
		}

		public static float Max(params float[] values)
		{
			float max = values[0];

			for (int i = values.Length - 1; i > 1; --i) 
				if (values[i] > max) max = values[i];

			return max;
		}

		public static double Max(params double[] values)
		{
			double max = values[0];

			for (int i = values.Length - 1; i > 1; --i) 
				if (values[i] > max) max = values[i];

			return max;
		}

		public static decimal Max(params decimal[] values)
		{
			decimal max = values[0];

			for (int i = values.Length - 1; i > 1; --i) 
				if (values[i] > max) max = values[i];

			return max;
		}

		// Generic Implementation
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T Max<T>(T left, T right) where T : IComparable<T> => left.CompareTo(right) > 0 ? left : right;

		public static T Max<T>(params T[] values) where T : IComparable<T>
		{
			T max = values[0];

			for (int i = values.Length - 1; i > 1; --i)
			{
				if (values[i].CompareTo(max) > 0)
					max = values[i];
			}

			return max;
		}

		#endregion

		#region Clamp

		// Built-in Type Implementations
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static sbyte   Clamp(sbyte   value, sbyte   min, sbyte   max) => value < min ? min : value > max ? max : value;
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static byte    Clamp(byte    value, byte    min, byte    max) => value < min ? min : value > max ? max : value;
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static short   Clamp(short   value, short   min, short   max) => value < min ? min : value > max ? max : value;
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static ushort  Clamp(ushort  value, ushort  min, ushort  max) => value < min ? min : value > max ? max : value;
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static int     Clamp(int     value, int     min, int     max) => value < min ? min : value > max ? max : value;
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static uint    Clamp(uint    value, uint    min, uint    max) => value < min ? min : value > max ? max : value;
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static long    Clamp(long    value, long    min, long    max) => value < min ? min : value > max ? max : value;
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static ulong   Clamp(ulong   value, ulong   min, ulong   max) => value < min ? min : value > max ? max : value;
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static float   Clamp(float   value, float   min, float   max) => value < min ? min : value > max ? max : value;
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static double  Clamp(double  value, double  min, double  max) => value < min ? min : value > max ? max : value;

		// Generic Implementation
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T Clamp<T>(T value, T min, T max) where T : IComparable<T> => value.CompareTo(min) < 0 ? min : 
																					value.CompareTo(max) > 0 ? max : 
																					value;

		#endregion

		#region Wrap

		public static int Wrap(int value, int min, int max)
		{
			if (value >= min && value <= max)
				return value;

			return (value < min ? max : min) + ((value - min) % (max - min));
		}

		public static long Wrap(long value, long min, long max)
		{
			if (value >= min && value <= max)
				return value;

			return (value < min ? max : min) + ((value - min) % (max - min));
		}

		public static float Wrap(float value, float min, float max)
		{
			if (value >= min && value <= max)
				return value;

			return (value < min ? max : min) + ((value - min) % (max - min));
		}

		public static double Wrap(double value, double min, double max)
		{
			if (value >= min && value <= max)
				return value;

			return (value < min ? max : min) + ((value - min) % (max - min));
		}

		#endregion

		#region Lerp

		public static float Lerp         (float start, float end, float amount) => start + ((end - start) * Clamp(amount, 0, 1));
		public static float LerpUnclamped(float start, float end, float amount) => start + ((end - start) * amount);
		public static float InverseLerp  (float start, float end, float value)  => (value - start) / (end - start);

		public static double Lerp         (double start, double end, double amount) => start + ((end - start) * Clamp(amount, 0, 1));
		public static double LerpUnclamped(double start, double end, double amount) => start + ((end - start) * amount);
		public static double InverseLerp  (double start, double end, double value)  => (value - start) / (end - start);

		#endregion
	}
}
