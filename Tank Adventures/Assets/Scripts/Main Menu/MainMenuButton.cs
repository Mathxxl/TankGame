using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Main_Menu
{
    //DEPRECATED
    public class MainMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private Button _button;
        //private static EventSystem _eventSystem;

        private void Awake()
        {
            _button = GetComponent<Button>();
            //_eventSystem ??= FindObjectOfType<EventSystem>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_button == null) return;
            
            Debug.Log("Over");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_button == null) return;

            Debug.Log("Not Over");
        }
    }
}