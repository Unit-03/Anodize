using System;

namespace Drone
{
	/// <summary>Stores two values of the samme type as "Left" and "Right". The order of the values is irrelevant, this is just a container.</summary>
	public class Pair<T> : Pair<T, T>
	{
		#region Properties
		
		// This override exists to skip the boxing/unboxing of values that the base Pair has to do due to its support of different types
		/// <summary>Shortcut for Left and Right according to the parity of the index; even accesses Left, odd accesses Right</summary>
		public new T this[int index] {
			get {
				return (index & 1) == 0 ? Left : Right;
			}
			set {
				if ((index & 1) == 0)
					Left = value;
				else
					Right = value;
			}
		}

		#endregion
	}

	/// <summary>Stores two values of independent type as "Left" and "Right". The order of the values is irrelevant, this is just a container.</summary>
	public class Pair<TLeft, TRight>
    {
		#region Properties

		/// <summary>The first value provided to the Pair</summary>
		public virtual TLeft  Left  { get; set; }
		/// <summary>The second value provided to the Pair</summary>
		public virtual TRight Right { get; set; }

		/// <summary>Shortcut for Left and Right according to the parity of the index; even accesses Left, odd accesses Right</summary>
		public virtual object this[int index] {
			get {
				return (index & 1) == 0 ? (object)Left : (object)Right;
			}
			set {
				if ((index & 1) == 0)
				{
					Left = value is TLeft leftVal
						? leftVal
						: throw new ArgumentException($"Value '{value}' could not be assigned to index {index}. Value should be of type {typeof(TLeft)} but was {value.GetType()}");
				}
				else
				{
					Right = value is TRight rightVal
						? rightVal
						: throw new ArgumentException($"Value '{value}' could not be assigned to index {index}. Value should be of type {typeof(TRight)} but was {value.GetType()}");
				}
			}
		}

		#endregion

		#region Constructor

		public Pair()
		{
			Left  = default;
			Right = default;
		}

		public Pair(TLeft left, TRight right)
		{
			Left  = left;
			Right = right;
		}

		#endregion

		#region Methods

		/// <summary>Checks if Left and Right are equal</summary>
		public virtual bool Match()
		{
			return Left.Equals(Right);
		}

		/// <summary>Converts the Pair to a string in the format "{Left, Right}" (e.g. "{87, 92}")</summary>
		public override string ToString()
		{
			return $"{{{Left}, {Right}}}";
		}

		public override bool Equals(object obj)
		{
			return obj is Pair<TLeft, TRight> pair && this == pair;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		#endregion

		#region Operators

		public static bool operator ==(Pair<TLeft, TRight> left, Pair<TLeft, TRight> right)
		{
			return left.Left.Equals(right.Left) && left.Right.Equals(right.Right);
		}

		public static bool operator !=(Pair<TLeft, TRight> left, Pair<TLeft, TRight> right)
		{
			return !(left == right);
		}

		public static implicit operator (TLeft left, TRight right)(Pair<TLeft, TRight> pair)
		{
			return (left: pair.Left, right: pair.Right);
		}

		public static implicit operator Pair<TLeft, TRight>((TLeft left, TRight right) tuple)
		{
			return new Pair<TLeft, TRight>(tuple.left, tuple.right);
		}

		#endregion
	}
}
