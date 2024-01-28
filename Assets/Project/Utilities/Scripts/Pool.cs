using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Utilities
{
    public interface IPoolBase {}
    public class Pool<T> : IPoolBase where T : Behaviour
    {
        private ObjectPool<T> _pool;
        private T _prototype;
        private readonly LinkedList<T> _activeInstances = new();

        public Pool(T prototype)
        {
            _prototype = prototype;
            _pool = new ObjectPool<T>(OnCreate, OnGet, OnRelease, OnDestroy);
        }

        private T OnCreate()
        {
            T talentViewInstance = Object.Instantiate(_prototype);
            return talentViewInstance;
        }

        private void OnGet(T obj)
        {
            obj.gameObject.SetActive(true);
            _activeInstances.AddLast(obj);
        }

        private void OnRelease(T obj)
        {
            if (obj == null || obj.gameObject == null || !_activeInstances.Contains(obj)) return;
            _activeInstances.Remove(obj);
            obj.gameObject.SetActive(false);
        }

        private void OnDestroy(T obj)
        {
        }

        public T Get()
        {
            return _pool.Get();
        }
        
        public T Get(Transform parent)
        {
            T result = _pool.Get();
            result.transform.SetParent(parent);
            return result;
        }

        public void Release(T view)
        {
            _pool.Release(view);
            _activeInstances.Remove(view);
        }
        
        public void Release(params T[] views)
        {
            foreach (T view in views)
            {
                Release(view);
            }
        }

        public void ReleaseAll()
        {
            while (_activeInstances.Any())
            {
                Release(_activeInstances.First.Value);
            }
        }
    }
}