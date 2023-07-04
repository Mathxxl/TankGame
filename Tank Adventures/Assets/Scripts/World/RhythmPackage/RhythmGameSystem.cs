using System;
using System.Collections;
using UnityEngine;

namespace World.RhythmPackage
{
    public class RhythmGameSystem : MonoBehaviour
    {
        [SerializeField] private RhythmMap map;
        [SerializeField] private Transform startPoint;
        [SerializeField] private Transform endPoint;
        [SerializeField] private float maxTickLifetime = 3600;
        [SerializeField] private float maxX = 1;
        [SerializeField] private float maxY = 1;
        
        private float _currentSpeed;
        private float _currentTime;
        private RhythmReferencesSystem _referencesSystem;

        private void Awake()
        {
            _currentSpeed = map.speed;
            _currentTime = map.totalTime;
            _referencesSystem = FindObjectOfType<RhythmReferencesSystem>();
        }

        private void Start()
        {
            if (_referencesSystem == null)
            {
                Debug.LogWarning("No references system found");
                return;
            }
            
            StartMap();
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private void StartMap()
        {
            foreach (var obj in map.objects)
            {
                StartCoroutine(SpawnObjectRoutine(obj));
            }
        }

        private IEnumerator SpawnObjectRoutine(RhythmMapObject mapObject)
        {
            yield return new WaitForSeconds(mapObject.timing * _currentTime);
            var toSpawnObj = _referencesSystem._dicObj[mapObject.type];
            SpawnObject(toSpawnObj, mapObject.x, mapObject.y);
        }

        private IEnumerator ObjectLifetime(Transform objTransform)
        {
            if(endPoint == null || !objTransform.TryGetComponent(out Rigidbody rb)) yield break;

            var i = 0;
            while (objTransform.position.z > endPoint.position.z && i < maxTickLifetime)
            {
                yield return null;
                i++;
                rb.velocity = -Vector3.forward * _currentSpeed;
            }
            
            Destroy(objTransform.gameObject);
        }

        private void SpawnObject(GameObject obj, float moveX = 0, float moveY = 0)
        {
            var sPos = startPoint.position;
            var newPos = new Vector3(sPos.x + moveX * maxX, sPos.y + moveY * maxY, sPos.z);

            var spawnedObject = Instantiate(obj, newPos, obj.transform.rotation);
            StartCoroutine(ObjectLifetime(spawnedObject.transform));
        }
    }
}