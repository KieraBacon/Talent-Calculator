using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public class PoolManager
    {
        private struct TypeStringPair : IEqualityComparer<TypeStringPair>
        {
            public Type Type;
            public string Key;

            public TypeStringPair(Type type, string key)
            {
                Type = type;
                Key = key;
            }

            public bool Equals(TypeStringPair x, TypeStringPair y)
            {
                return x.Type == y.Type && x.Key == y.Key;
            }

            public int GetHashCode(TypeStringPair obj)
            {
                return HashCode.Combine(obj.Type, obj.Key);
            }
        }
        
        private static readonly Lazy<PoolManager> _lazy = new Lazy<PoolManager>(() => new PoolManager());
        public static PoolManager Instance =>
            _lazy.Value;

        private Dictionary<TypeStringPair, IPoolBase> _pools = new();

        public Pool<T> Get<T>(string key) where T : Behaviour
        {
            TypeStringPair typeStringPair = new TypeStringPair(typeof(T), key);
            if (_pools.TryGetValue(typeStringPair, out IPoolBase result)) return result as Pool<T>;
            
            T prototype = Resources.Load<T>(key);
            if (prototype == null) throw new Exception($"Prototype resource with key {key} not found.");
            result = new Pool<T>(prototype);
            _pools[typeStringPair] = result;
            return (Pool<T>)result;
        }
    }
}