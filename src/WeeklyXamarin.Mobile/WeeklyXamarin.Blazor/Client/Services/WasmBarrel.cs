using Blazored.LocalStorage;
using MonkeyCache;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeeklyXamarin.Blazor.Client.Services
{
    public class WasmBarrel : IBarrel
    {
        private ISyncLocalStorageService localStorage;
        public WasmBarrel(ISyncLocalStorageService localStorage)
        {
            this.localStorage = localStorage;
        }

        public void Add<T>(string key, T data, TimeSpan expireIn, string? eTag = null, JsonSerializerSettings? jsonSerializationSettings = null)
        {
            localStorage.SetItem(key, data);
        }

        public void Empty(params string[] key)
        {
            throw new NotImplementedException();
        }

        public void EmptyAll()
        {
            throw new NotImplementedException();
        }

        public void EmptyExpired()
        {
            throw new NotImplementedException();
        }

        public bool Exists(string key)
        {
            throw new NotImplementedException();
        }

        public T Get<T>(string key, JsonSerializerSettings? jsonSettings = null)
        {
            return localStorage.GetItem<T>(key);
        }

        public string GetETag(string key)
        {
            throw new NotImplementedException();
        }

        public DateTime? GetExpiration(string key)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetKeys(CacheState state = CacheState.Active)
        {
            throw new NotImplementedException();
        }

        public bool IsExpired(string key)
        {
            throw new NotImplementedException();
        }
    }
}
