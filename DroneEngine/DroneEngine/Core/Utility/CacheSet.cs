using System;
using System.Collections.Generic;

namespace Drone
{
	/// <summary>Allows the storage and updating of multiple caches at once. Useful for defining groups of values that share a cache state</summary>
	public class CacheSet : ICache
	{
		#region Properties

		public bool Valid {
			get {
				for (int i = 0; i < cacheStore.Length; ++i)
				{
					if (!cacheStore[i].Valid)
						return false;
				}

				return true;
			}
		}

		#endregion

		#region Fields

		private readonly ICache[] cacheStore;

		#endregion

		#region Constructors

		public CacheSet(ICache cache)
		{
			cacheStore = new ICache[1] { cache };
		}

		public CacheSet(params ICache[] caches)
		{
			cacheStore = new ICache[caches.Length];
			Array.Copy(caches, cacheStore, caches.Length);
		}

		public CacheSet(IEnumerable<ICache> caches)
		{
			int count = 0;

			foreach (ICache cache in caches)
				++count;

			int index = 0;
			cacheStore = new ICache[count];

			foreach (ICache cache in caches)
				cacheStore[index++] = cache;
		}

		public CacheSet(CacheSet caches) : this(caches.cacheStore)
		{
		}

		#endregion

		#region Methods

		public void Clear()
		{
			for (int i = 0; i < cacheStore.Length; ++i)
				cacheStore[i].Clear();
		}

		public void Invalidate()
		{
			for (int i = 0; i < cacheStore.Length; ++i)
				cacheStore[i].Invalidate();
		}

		public void Store(object val)
		{
			for (int i = 0; i < cacheStore.Length; ++i)
				cacheStore[i].Store(val);
		}

		#endregion
	}
}
