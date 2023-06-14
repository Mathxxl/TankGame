using LoadingScreen;
using UnityEngine;

namespace Main_Menu
{
    /// <summary>
    /// Manages the main game menu
    /// </summary>
    public class MainMenuManager : MonoBehaviour //-> Manager ?
    {
        //[SerializedField] private SaveManager saveManager;
        [SerializeField] private SceneLoader sceneLoader;
        
        [Header("Scenes")]
        [SerializeField] private string newGameScene;

        public void NewGame()
        {
            //animation or smth
            
            sceneLoader.ToLoadScene(newGameScene);
        }

        public void LoadGame()
        {
            //TODO with save system
        }
        
        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
