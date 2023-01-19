namespace Drone
{
	/// <summary>Supports the storing and validation of a value. If the cache is invalid then the value should be considered invalid too</summary>
	public interface ICache
	{
		bool Valid { get; }

		/// <summary>Invalidates the cache and resets the value to its default</summary>
		void Clear();
		/// <summary>Invalidates the cache, indicates that the value stored in this cache is out-of-date and shouldn't be used</summary>
		void Invalidate();
		/// <summary>Stores the value provided and flags it as valid</summary>
		void Store(object val);
	}

	/// <summary>Supports the storing and validation of a value. If the cache is invalid then the value should be considered invalid too</summary>
	public interface ICache<T> : ICache
	{
		/// <summary>Stores the value provided and flags it as valid</summary>
		void Store(T val);
	}
}
