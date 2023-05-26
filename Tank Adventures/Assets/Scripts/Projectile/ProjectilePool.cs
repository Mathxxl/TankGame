using UnityEngine;
using UnityEngine.Pool;

namespace Projectile
{
    public class ProjectilePool
    {
        private IObjectPool<global::Projectile.Projectile> _pool;
        private GameObject _projectilePrefab;
        private Transform _parent;
        private int _maxPoolSize;

        public IObjectPool<global::Projectile.Projectile> Pool => _pool;

        public ProjectilePool(GameObject prefab, Transform spawn, int size)
        {
            _projectilePrefab = prefab;
            _parent = spawn;
            _maxPoolSize = size;

            _pool = new ObjectPool<global::Projectile.Projectile>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, 10, _maxPoolSize);

        }

        private global::Projectile.Projectile CreatePooledItem()
        {
            var go = Object.Instantiate(_projectilePrefab, _parent.position, _parent.rotation);
            var p = go.GetComponent<global::Projectile.Projectile>();
            p.LinkedPool = this;

            return p;
        }

        private void OnReturnedToPool(global::Projectile.Projectile p)
        {
            p.gameObject.SetActive(false);
        }

        private void OnTakeFromPool(global::Projectile.Projectile p)
        {
            var pObj = p.gameObject;
            pObj.transform.position = _parent.position;
            pObj.transform.rotation = _parent.rotation;
            pObj.SetActive(true);
        }

        private void OnDestroyPoolObject(global::Projectile.Projectile p)
        {
            Object.Destroy(p.gameObject);
        }
    }
}
