using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Characteristics
{
    /// <summary>
    /// Encapsulate objets that should not be destroyed on load
    /// </summary>
    public sealed class DontDestroy : MonoBehaviour
    {
        [SerializeField] private string[] excludeScenesNames;
        
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += SceneManagerOnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= SceneManagerOnSceneLoaded;
        }

        private void SceneManagerOnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (excludeScenesNames.Any((eName) => scene.name == eName))
            {
                Destroy(gameObject);
            }
        }
    }
}
