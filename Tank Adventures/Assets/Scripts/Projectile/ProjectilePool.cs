using UnityEngine;
using UnityEngine.Pool;

namespace Projectile
{
    public class ProjectilePool
    {
        private IObjectPool<Projectile> _pool;
        private GameObject _projectilePrefab;
        private Transform _parent;
        private int _maxPoolSize;

        public IObjectPool<Projectile> Pool => _pool;

        public ProjectilePool(GameObject prefab, Transform spawn, int size)
        {
            _projectilePrefab = prefab;
            _parent = spawn;
            _maxPoolSize = size;

            _pool = new ObjectPool<Projectile>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, 10, _maxPoolSize);

        }

        private Projectile CreatePooledItem()
        {
            var go = Object.Instantiate(_projectilePrefab, _parent.position, _parent.rotation);
            var p = go.GetComponent<Projectile>();
            p.LinkedPool = this;

            return p;
        }

        private void OnReturnedToPool(Projectile p)
        {
            p.gameObject.SetActive(false);
        }

        private void OnTakeFromPool(Projectile p)
        {
            if (p == null) return;
            var pObj = p.gameObject;
            pObj.transform.position = _parent.position;
            pObj.transform.rotation = _parent.rotation;
            pObj.SetActive(true);
        }

        private void OnDestroyPoolObject(Projectile p)
        {
            Object.Destroy(p.gameObject);
        }
    }
}
