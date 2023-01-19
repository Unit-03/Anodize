using System;

namespace Drone
{
	/// <summary>Stores a value alongside a flag to indicate if it's currently considered valid</summary>
	public class Cache<T> : ICache<T>
	{
		#region Properties

		public T Value {
			get {
				if (!Valid && autoCache != null)
					Store(autoCache.Invoke());

				return _value;
			}
			set {
				_value = value;
			}
		}

		public bool Valid { get; private set; }

		#endregion

		#region Fields

		protected Func<T> autoCache;
		protected T _value;

		#endregion

		#region Constructor

		public Cache()
		{
			Clear();
		}

		public Cache(Func<T> automaticCache) : this()
		{
			autoCache = automaticCache;
		}

		public Cache(T val, Func<T> automaticCache = null)
		{
			autoCache = automaticCache;
			Store(val);
		}

		#endregion

		#region Methods

		public void Clear()
		{
			Value = default;
			Valid = false;
		}

		public void Invalidate()
		{
			Valid = false;
		}

		public void Store(T val)
		{
			Value = val;
			Valid = true;
		}

		void ICache.Store(object val)
		{
			if (val is T tVal)
				Store(tVal);
		}

		/// <summary>Converts the Cache to a string in the format "Value:Valid" (e.g. "172:true")</summary>
		public override string ToString()
		{
			return $"{Value}:{Valid}";
		}

		public override bool Equals(object obj) => obj is Cache<T> cache && this == cache;
		public override int GetHashCode() => base.GetHashCode();

		#endregion

		#region Operators

		public static bool operator ==(Cache<T> left, Cache<T> right) => left.Value.Equals(right.Value);
		public static bool operator !=(Cache<T> left, Cache<T> right) => !(left == right);

		public static implicit operator T(Cache<T> cache) => cache.Value;
		public static explicit operator Cache<T>(T value) => new Cache<T>(value);

		#endregion
	}
}
