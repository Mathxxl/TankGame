using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LoadingScreen
{
    public class SceneLoader : MonoBehaviour
    {
        #region Struct
        
        [Serializable]
        private struct ButtonAndScene
        {
            public Button button;
            public string sceneName;
            public void Deconstruct(out string scenename, out Button o)
            {
                scenename = sceneName;
                o = button;
            }
        }
        
        #endregion
        
        #region Attributes

        [SerializeField] private List<ButtonAndScene> buttons;
        [SerializeField] private FillBar fillBar;
        [SerializeField] private Fading fadingSystem;
        [Tooltip("Object holder for everything that should be set inactive when loading")][SerializeField] private GameObject frontHolder;

        #endregion
    
        #region Methods

        private void Start()
        {
            foreach (var (sceneName, button) in buttons)
            {
                button.onClick.AddListener(() => ToLoadScene(sceneName));
                //button.onClick.AddListener(() => button.gameObject.SetActive(false)); //Should not be needed anymore
            }
            fillBar.SetActive(false);
        }

        public void ToLoadScene(string sceneName)
        {
            frontHolder.gameObject.SetActive(false);
            StartCoroutine(LoadScene(sceneName));
        }

        /// <summary>
        /// Asynchronous Loading of the Scene in parameter
        /// </summary>
        /// <returns></returns>
        private IEnumerator LoadScene(string sceneName)
        {
            yield return null;
        
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            asyncOperation.allowSceneActivation = false;
            fadingSystem.Fade();
            fillBar.SetActive(true);
            fillBar.ChangeBarValue(0);

            while (!asyncOperation.isDone)
            {
                fillBar.ChangeBarValue(asyncOperation.progress / 0.9f);
                // Check if the load has finished
                if (asyncOperation.progress >= 0.9f)
                {
                    //On attend que le fade in soit terminé
                    yield return new WaitUntil(() => !fadingSystem.IsFading);
                    //On prépare le fade vers écran noir
                    fadingSystem.Set(fadingSystem.SplashScreenToGo, null);
                    fadingSystem.Fade();
                    fillBar.Fade();
                    //On attend que celui-ci soit terminé
                    yield return new WaitUntil(() => !fadingSystem.IsFading);
                    //On lance finalement la scène
                    OnSceneLoaded?.Invoke(sceneName);
                    asyncOperation.allowSceneActivation = true;
                }

                yield return null;
            }
        }

        #endregion

        #region Events

        public delegate void OnSceneLoadedEventHandler(string sceneName);

        public OnSceneLoadedEventHandler OnSceneLoaded;

        #endregion
    }
}
