using System;
using UnityEngine;
using UnityEngine.UI;

namespace Main_Menu
{
    public class SettingsManager : MonoBehaviour
    {
        [SerializeField] private GameObject settingsHolder;
        [SerializeField] private Slider volumeSlider;

        private void Awake()
        {
            if (!PlayerPrefs.HasKey("musicVolume"))
            {
                PlayerPrefs.SetFloat("musicVolume", 1);
            }
            Load();
        }

        public void ChangeVolume()
        {
            AudioListener.volume = volumeSlider.value;
            Save();
        }

        public void ChangeHolderState()
        {
            settingsHolder.SetActive(!settingsHolder.activeSelf);
        }

        private void Load()
        {
            volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
        }

        private void Save()
        {
            PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
        }
    }
}