using System.Collections.Generic;

namespace Drone
{
	/// <summary>Automatically invalidates attached caches when the value of the hook is changed</summary>
	public class CacheHook<T>
	{
		#region Properties

		public T Value {
			get => _value;
			set {
				if (!_value.Equals(value))
				{
					if (hook == null)
						hooks.Invalidate();
					else
						hook.Invalidate();
				}

				_value = value;
			}
		}

		#endregion

		#region Methods

		private T _value;

		private ICache hook;
		private CacheSet hooks;

		#endregion

		#region Constructor

		public CacheHook(ICache cache)
		{
			hook = cache;
		}

		public CacheHook(params ICache[] caches)
		{
			hooks = new CacheSet(caches);
		}

		public CacheHook(IEnumerable<ICache> caches)
		{
			hooks = new CacheSet(caches);
		}

		public CacheHook(CacheSet caches)
		{
			hooks = new CacheSet(caches);
		}

		public CacheHook(CacheHook<T> copyHook)
		{
			if (copyHook.hook == null)
				hooks = new CacheSet(copyHook.hooks);
			else
				hook = copyHook.hook;
		}

		public CacheHook(T value, ICache cache) : this(cache)
		{
			_value = value;
		}

		public CacheHook(T value, params ICache[] caches) : this(caches)
		{
			_value = value;
		}

		public CacheHook(T value, IEnumerable<ICache> caches) : this(caches)
		{
			_value = value;
		}

		public CacheHook(T value, CacheSet caches) : this(caches)
		{
			_value = value;
		}

		public CacheHook(T value, CacheHook<T> copyHook) : this(copyHook)
		{
			_value = value;
		}

		#endregion

		#region Methods

		/// <summary>Converts the CacheHook's value to a string using its implementation</summary>
		public override string ToString()
		{
			return Value.ToString();
		}

		public override bool Equals(object obj) => obj is CacheHook<T> cache && this == cache;
		public override int GetHashCode() => base.GetHashCode();

		#endregion

		#region Operators

		public static bool operator ==(CacheHook<T> left, CacheHook<T> right) => left.Value.Equals(right.Value);
		public static bool operator !=(CacheHook<T> left, CacheHook<T> right) => !(left == right);

		public static implicit operator T(CacheHook<T> cache) => cache.Value;

		#endregion
	}
}
