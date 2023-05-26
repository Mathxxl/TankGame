using UnityEngine;

namespace Characteristics
{
    /// <summary>
    /// Encapsulate objets that should not be destroyed on load
    /// </summary>
    public sealed class DontDestroy : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
