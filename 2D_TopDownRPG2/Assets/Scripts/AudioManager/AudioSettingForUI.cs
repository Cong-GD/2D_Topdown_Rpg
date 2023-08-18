using UnityEngine;
using UnityEngine.UI;

namespace CongTDev.AudioManagement
{
    public class AudioSettingForUI : MonoBehaviour
    {
        [SerializeField] private Slider masterSlider;
        [SerializeField] private Slider musicSlider;

        private void OnEnable()
        {
            if(masterSlider != null)
            {
                masterSlider.value = AudioManager.Instance.MasterVolume;
                masterSlider.onValueChanged.AddListener(OnMasterSliderChange);
            }
            if(musicSlider != null)
            {
                musicSlider.value = AudioManager.Instance.MusicVolume;
                musicSlider.onValueChanged.AddListener(OnMusicSliderChange);
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
        }

        public void OnMasterSliderChange(float value)
        {
            AudioManager.Instance.MasterVolume = value;
        }

        public void OnMusicSliderChange(float value)
        {
            AudioManager.Instance.MusicVolume = value;
        }
    }
}