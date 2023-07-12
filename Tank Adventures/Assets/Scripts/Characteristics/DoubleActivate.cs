using UnityEngine;

namespace Characteristics
{
    public class DoubleActivate : MonoBehaviour
    {
        private void Awake()
        {
            gameObject.SetActive(false);
            gameObject.SetActive(true);
        }
    }
}