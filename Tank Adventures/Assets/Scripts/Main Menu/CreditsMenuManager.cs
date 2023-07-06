using System;
using LoadingScreen;
using UnityEngine;

namespace Main_Menu
{
    public class CreditsMenuManager : MonoBehaviour
    {
        [SerializeField] private SceneLoader sceneLoader;
        
        private void OnEnable()
        {
            OnCreditsEnd += CreditsEnd;
        }

        private void OnDisable()
        {
            OnCreditsEnd -= CreditsEnd;
        }

        public void CreditsEnd()
        {
            sceneLoader.ToLoadScene("MainMenu");
        }

        public Action OnCreditsEnd;
        public Action OnCreditsStart;
    }
}