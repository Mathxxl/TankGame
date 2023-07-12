using System.Collections;
using System.Linq;
using Projectile;
using UnityEngine;

namespace Entities.Player.Upgrades
{
    [RequireComponent(typeof(Collider))]
    public class ReflectZone : MonoBehaviour
    {
        [SerializeField] private float reflectProbability = 0.1f;
        [SerializeField] private Renderer bubbleRenderer;
        [SerializeField] private float bubbleTime = 0.3f;
        [SerializeField] [TagSelector] private string[] excludeTags;
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Projectile.Projectile projectile)) return;
            if (excludeTags.Any((t) => projectile.weapon.ThisEntity.CompareTag(t))) return;

            var roll = UnityEngine.Random.Range(0f, 1f);
            if(roll > reflectProbability) return;
            
            //Debug.Break();
            if (projectile is PlasmaProjectile plasmaProjectile)
            {
                plasmaProjectile.Plasma.transform.parent = plasmaProjectile.transform;
            }
            projectile.transform.Rotate(-90,0,0);
            projectile.Move();


            
            StartCoroutine(Bubble());
        }

        private IEnumerator Bubble()
        {
            if(bubbleRenderer == null) yield break;
            
            bubbleRenderer.gameObject.SetActive(true);

            yield return new WaitForSeconds(bubbleTime);
            
            bubbleRenderer.gameObject.SetActive(false);
        }
        
    }
}