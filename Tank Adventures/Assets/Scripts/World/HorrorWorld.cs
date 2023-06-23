using UnityEngine;

namespace World
{
    /// <summary>
    /// Describes the specific behaviours of the Horror World<br/>
    /// In this world there is no light but a low source that follows the player
    /// </summary>
    public class HorrorWorld : World
    {
        [SerializeField] private GameObject lightObjectPrefab;
        private GameObject _lightObject;
        
        protected override void OnEnter()
        {
            SetLight();
        }

        protected override void OnExit()
        {
            UnSetLight();
        }

        protected override void OnUpdate()
        {
            
        }

        private void SetLight()
        {
            _lightObject = Instantiate(lightObjectPrefab, manager.GManager.Player.transform);
            _lightObject.transform.rotation = lightObjectPrefab.transform.rotation;
        }

        private void UnSetLight()
        {
            Destroy(_lightObject);
        }
    }
}