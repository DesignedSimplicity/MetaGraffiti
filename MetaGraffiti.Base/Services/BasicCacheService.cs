using System;
using System.Collections.Generic;
using System.Linq;

namespace MetaGraffiti.Base.Services
{
	public interface ICacheService<T>
	{
		void Reset();

		int Count { get; }

		List<T> All { get; }

		T this[string ID] { get; }

		bool Contains(string ID);

		T Get(string ID);

		T Add(string ID, T data);
		T AddOrIgnore(string ID, T data);
		T AddOrUpdate(string ID, T data);

		T Update(string ID, T data);

		void Remove(string ID);
		void RemoveOrIgnore(string ID);
	}

	public class NullCacheService<T> : ICacheService<T> 
	{
		public int Count { get { return 0; } }

		public List<T> All { get { return new List<T>(); } }

		public T this[string ID] { get { return default(T); } }

		public void Reset() { }

		public bool Contains(string ID) { return false; }

		public T Get(string ID) { return default(T); }

		public T Add(string ID, T data) { return data; }
		public T AddOrIgnore(string ID, T data) { return data; }
		public T AddOrUpdate(string ID, T data) { return data; }

		public T Update(string ID, T data) { return data; }

		public void Remove(string ID) { }
		public void RemoveOrIgnore(string ID) { }
	}

	public class BasicCacheService<T> : ICacheService<T>
	{
		private Dictionary<string, T> _cache = new Dictionary<string, T>();

		public int Count { get { return _cache.Count; } }

		public List<T> All { get { return _cache.Values.ToList(); } }

		public T this[string ID] { get { return Get(ID); } }

		public void Reset()
		{
			lock (_cache)
			{
				_cache = new Dictionary<string, T>();
			}
		}
	
		/// <summary>
		/// Checks if the cache contains the item
		/// </summary>
		public bool Contains(string ID)
		{
			lock (_cache)
			{
				return _cache.ContainsKey(ID);
			}
		}

		/// <summary>
		/// Returns an existing item in cache or null if not exists
		/// </summary>
		public T Get(string ID)
		{
			lock (_cache)
			{
				if (_cache.ContainsKey(ID)) return _cache[ID];

				return default(T);
			}
		}

		/// <summary>
		/// Adds item to the cache, throws exception if already exists
		/// </summary>
		public T Add(string ID, T data)
		{
			lock (_cache)
			{
				if (_cache.ContainsKey(ID)) throw new Exception($"{ID} already exists in cache");

				_cache.Add(ID, data);
				return data;
			}
		}

		/// <summary>
		/// Adds item to the cache, ignores if already exists, returns cached item
		/// </summary>
		public T AddOrIgnore(string ID, T data)
		{
			lock (_cache)
			{
				if (_cache.ContainsKey(ID)) return _cache[ID];

				_cache.Add(ID, data);
				return data;
			}
		}

		/// <summary>
		/// Adds item to the cache, updated if already exists, returns cached item
		/// </summary>
		public T AddOrUpdate(string ID, T data)
		{
			lock (_cache)
			{
				if (_cache.ContainsKey(ID))
					_cache[ID] = data;
				else
					_cache.Add(ID, data);

				return data;
			}
		}

		/// <summary>
		/// Updates an existing item in the cache, throw exception if not present
		/// </summary>
		public T Update(string ID, T data)
		{
			lock (_cache)
			{
				if (!_cache.ContainsKey(ID)) throw new Exception($"{ID} does not exists in cache");

				_cache[ID] = data;
				return data;
			}
		}

		/// <summary>
		/// Remove existing item from cache, throws exception if not present
		/// </summary>
		public void Remove(string ID)
		{
			lock (_cache)
			{
				if (!_cache.ContainsKey(ID)) throw new Exception($"{ID} does not exists in cache");

				_cache.Remove(ID);
			}
		}

		/// <summary>
		/// Remove existing item from cache if present
		/// </summary>
		public void RemoveOrIgnore(string ID)
		{
			lock (_cache)
			{
				if (_cache.ContainsKey(ID)) _cache.Remove(ID);
			}
		}
	}
}
