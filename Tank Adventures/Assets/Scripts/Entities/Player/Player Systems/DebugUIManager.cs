using UnityEngine;

namespace Entities.Player.Player_Systems
{
    public class DebugUIManager : EntitySystem
    {
        [SerializeField] private GameObject canvasUpgrade;
        [SerializeField] private GameObject canvasPortal;
        [SerializeField] private GameObject canvasPlayer;
        [SerializeField] private GameObject canvasEndScreen;
        [SerializeField] private GameObject canvasOthers;
        [SerializeField] private GameObject canvasStats;

        private void OnToggleUpgrade()
        {
            if (canvasUpgrade == null) return;
            Toggle(canvasUpgrade);
        }

        private void OnTogglePortal()
        {
            if (canvasPortal == null) return;
            Toggle(canvasPortal);
        }

        private void OnTogglePlayer()
        {
            if (canvasPlayer == null) return;
            Toggle(canvasPlayer);
        }

        private void OnToggleEndScreen()
        {
            if (canvasEndScreen == null) return;
            Toggle(canvasEndScreen);
        }

        private void OnToggleOthers()
        {
            if (canvasOthers == null) return;
            Toggle(canvasOthers);
        }

        private void OnToggleStats()
        {
            if (canvasStats == null) return;
            Toggle(canvasStats);
        }
        
        private static void Toggle(GameObject obj)
        {
            Debug.Log($"Toggle {obj.name}");
            obj.SetActive(!obj.activeSelf);
        }
    }
}