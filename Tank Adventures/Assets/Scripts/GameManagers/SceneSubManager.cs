using LoadingScreen;
using UnityEngine;

namespace GameManagers
{
    public class SceneSubManager : Manager
    {
        [SerializeField] private SceneLoader sceneLoader;

        private void OnEnable()
        {
            gameManager.Events.OnSelectedScene += ManagerLoadScene;
        }

        private void OnDisable()
        {
            gameManager.Events.OnSelectedScene -= ManagerLoadScene;
        }

        public void ManagerLoadScene(string sceneName)
        {
            sceneLoader.ToLoadScene(sceneName);
        }
    }
}