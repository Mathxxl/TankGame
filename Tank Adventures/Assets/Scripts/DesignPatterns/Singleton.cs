using UnityEngine;

namespace DesignPatterns
{
    /// <summary>
    /// Class to create singletons
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Singleton<T> : MonoBehaviour
        where T : Singleton<T>
    {
        #region Fields

        private static T _instance;

        #endregion

        #region Properties
    
        public static bool HasInstance => _instance != null
#if UNITY_EDITOR
                                          || !Application.isPlaying && FindObjectOfType<T>()
#endif
        ;

        public static T Instance
        {
            get
            {
                if (_instance != null) return _instance;

                var asset = FindObjectOfType<T>();
                if (asset != null)
                {
                    if (Application.isPlaying)
                    {
                        asset.Awake();
                    }
                    else
                    {
                        _instance = asset;
                    }
                
                    return _instance;
                }
                return _instance;
            }
        }

        public virtual bool UseDontDestroyOnLoad => false;

        public virtual bool DontInstantiate => false;

        #endregion

        #region Protected Methods

        protected virtual void OnAwake() { }

        #endregion

        #region Unity Event Functions

        protected void Awake()
        {
            // For [ExecuteInEditMode] objects
            if (!Application.isPlaying) return;

            if (_instance != null)
            {
                if (_instance != this)
                {
                    DestroyImmediate(gameObject);
                }
            
                return;
            }

            _instance = (T)this;
        

            if (UseDontDestroyOnLoad && !DontInstantiate) DontDestroyOnLoad(gameObject);

            OnAwake();
        }

        protected virtual void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }

        #endregion
    }
}