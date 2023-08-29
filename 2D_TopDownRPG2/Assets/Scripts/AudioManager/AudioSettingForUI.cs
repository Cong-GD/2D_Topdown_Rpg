using UnityEngine;
using UnityEngine.UI;

namespace CongTDev.AudioManagement
{
    public class AudioSettingForUI : MonoBehaviour
    {
        [SerializeField] private Slider masterSlider;
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider sfxSlider;
        [SerializeField] private Slider uiSlider;

        private void OnEnable()
        {
            if(masterSlider != null)
            {
                masterSlider.value = AudioManager.MasterVolume;
                masterSlider.onValueChanged.AddListener(OnMasterSliderChange);
            }
            if(musicSlider != null)
            {
                musicSlider.value = AudioManager.MusicVolume;
                musicSlider.onValueChanged.AddListener(OnMusicSliderChange);
            }
            if (sfxSlider != null)
            {
                sfxSlider.value = AudioManager.SFXVolume;
                sfxSlider.onValueChanged.AddListener(OnSFXSliderChange);
            }
            if (uiSlider != null)
            {
                uiSlider.value = AudioManager.UIVolume;
                uiSlider.onValueChanged.AddListener(OnUIliderChange);
            }
        }

        private void OnDisable()
        {
            if (masterSlider != null)
            {
                masterSlider.onValueChanged.RemoveListener(OnMasterSliderChange);
            }
            if (musicSlider != null)
            {
                musicSlider.onValueChanged.RemoveListener(OnMusicSliderChange);
            }
            if (sfxSlider != null)
            {
                sfxSlider.onValueChanged.RemoveListener(OnSFXSliderChange);
            }
            if (uiSlider != null)
            {
                uiSlider.onValueChanged.RemoveListener(OnUIliderChange);
            }
        }

        public void OnMasterSliderChange(float value)
        {
            AudioManager.MasterVolume = value;
        }

        public void OnMusicSliderChange(float value)
        {
            AudioManager.MusicVolume = value;
        }

        public void OnSFXSliderChange(float value)
        {
            AudioManager.SFXVolume = value;
        }
        public void OnUIliderChange(float value)
        {
            AudioManager.UIVolume = value;
        }
    }
}