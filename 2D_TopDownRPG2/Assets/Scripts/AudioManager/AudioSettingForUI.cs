﻿using UnityEngine;
using UnityEngine.UI;

namespace CongTDev.AudioManagement
{
    public class AudioSettingForUI : MonoBehaviour
    {
        [SerializeField] private Slider masterSlider;
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider sfxSlider;

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
    }
}