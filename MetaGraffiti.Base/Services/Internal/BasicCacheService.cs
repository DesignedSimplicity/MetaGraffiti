using System;
using System.Collections.Generic;
using System.Linq;

namespace MetaGraffiti.Base.Services.Internal
{
	public interface ICacheEntity
	{
		string Key { get; }
	}

	// TODO: move cache locking/init toggle into interface
	public interface ICacheService<T>
	{
		void Reset();

		int Count { get; }

		List<T> All { get; }

		T this[string key] { get; }

		bool Contains(string key);
		bool Contains(ICacheEntity entity);

		T Get(string key);

		T Add(string key, T data);
		T Add(ICacheEntity entity);
		T AddOrIgnore(string key, T data);
		T AddOrUpdate(string key, T data);

		T Update(string key, T data);

		void Remove(string key);
		void RemoveOrIgnore(string key);
	}

	public class NullCacheService<T> : ICacheService<T> 
	{
		public int Count { get { return 0; } }

		public List<T> All { get { return new List<T>(); } }

		public T this[string key] { get { return default(T); } }

		public void Reset() { }

		public bool Contains(string key) { return false; }
		public bool Contains(ICacheEntity entity) { return false; }

		public T Get(string key) { return default(T); }

		public T Add(string key, T data) { return data; }
		public T Add(ICacheEntity entity) { return (T)entity; }
		public T AddOrIgnore(string key, T data) { return data; }
		public T AddOrUpdate(string key, T data) { return data; }

		public T Update(string key, T data) { return data; }

		public void Remove(string key) { }
		public void RemoveOrIgnore(string key) { }
	}

	public class BasicCacheService<T> : ICacheService<T>
	{
		private Dictionary<string, T> _cache = new Dictionary<string, T>();

		public int Count { get { return _cache.Count; } }

		public List<T> All { get { return _cache.Values.ToList(); } }

		public T this[string key] { get { return Get(key); } }

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
		public bool Contains(string key)
		{
			lock (_cache)
			{
				return _cache.ContainsKey(key);
			}
		}
		public bool Contains(ICacheEntity entity)
		{
			lock (_cache)
			{
				return _cache.ContainsKey(entity.Key);
			}
		}

		/// <summary>
		/// Returns an existing item in cache or null if not exists
		/// </summary>
		public T Get(string key)
		{
			lock (_cache)
			{
				if (_cache.ContainsKey(key)) return _cache[key];

				return default(T);
			}
		}

		/// <summary>
		/// Adds item to the cache, throws exception if already exists
		/// </summary>
		public T Add(string key, T data)
		{
			lock (_cache)
			{
				if (_cache.ContainsKey(key)) throw new Exception($"{key} already exists in cache");

				_cache.Add(key, data);
				return data;
			}
		}
		public T Add(ICacheEntity entity)
		{
			lock (_cache)
			{
				var data = (T)entity;
				return Add(entity.Key, data);
			}
		}

		/// <summary>
		/// Adds item to the cache, ignores if already exists, returns cached item
		/// </summary>
		public T AddOrIgnore(string key, T data)
		{
			lock (_cache)
			{
				if (_cache.ContainsKey(key)) return _cache[key];

				_cache.Add(key, data);
				return data;
			}
		}

		/// <summary>
		/// Adds item to the cache, updated if already exists, returns cached item
		/// </summary>
		public T AddOrUpdate(string key, T data)
		{
			lock (_cache)
			{
				if (_cache.ContainsKey(key))
					_cache[key] = data;
				else
					_cache.Add(key, data);

				return data;
			}
		}

		/// <summary>
		/// Updates an existing item in the cache, throw exception if not present
		/// </summary>
		public T Update(string key, T data)
		{
			lock (_cache)
			{
				if (!_cache.ContainsKey(key)) throw new Exception($"{key} does not exists in cache");

				_cache[key] = data;
				return data;
			}
		}

		/// <summary>
		/// Remove existing item from cache, throws exception if not present
		/// </summary>
		public void Remove(string key)
		{
			lock (_cache)
			{
				if (!_cache.ContainsKey(key)) throw new Exception($"{key} does not exists in cache");

				_cache.Remove(key);
			}
		}

		/// <summary>
		/// Remove existing item from cache if present
		/// </summary>
		public void RemoveOrIgnore(string key)
		{
			lock (_cache)
			{
				if (_cache.ContainsKey(key)) _cache.Remove(key);
			}
		}
	}
}
