using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

namespace CongTDev.AudioManagement
{
    public class AudioManager : GlobalReference<AudioManager>
    {
        public const string MASTER_VOLUME = "MasterVolume";
        public const string MUSIC_VOLUME = "MusicVolume";
        private const float MIN_VALUE = 0.0001f;
        private const float MAX_VALUE = 1f;

        public static float ValueToVolume(float value)
        {
            return Mathf.Log10(value) * 20;
        }

        public static float VolumeToValue(float volume)
        {
            return Mathf.Pow(10f, volume / 20);
        }

        public static void PlaySound(string soundName)
        {
            Instance.Play(soundName);
        }

        [SerializeField] private AudioMixer mixer;
        [SerializeField] private List<AudioUnit> audioUnits;

        private Dictionary<string, AudioUnit> _unitMap = new();

        public float MasterVolume
        {
            get => Mathf.Clamp(PlayerPrefs.GetFloat(MASTER_VOLUME, MAX_VALUE), MIN_VALUE, MAX_VALUE);
            set
            {
                value = Mathf.Clamp(value, MIN_VALUE, MAX_VALUE);
                mixer.SetFloat(MASTER_VOLUME, ValueToVolume(value));
                PlayerPrefs.SetFloat(MASTER_VOLUME, value);
            }
        }

        public float MusicVolume
        {
            get => Mathf.Clamp(PlayerPrefs.GetFloat(MUSIC_VOLUME, MAX_VALUE), MIN_VALUE, MAX_VALUE);
            set
            {
                value = Mathf.Clamp(value, MIN_VALUE, MAX_VALUE);
                mixer.SetFloat(MUSIC_VOLUME, ValueToVolume(value));
                PlayerPrefs.SetFloat(MUSIC_VOLUME, value);
            }
        }

        protected override void Awake()
        {
            base.Awake();
            _unitMap = audioUnits.ToDictionary(unit => unit.SoundName, unit => unit);
        }

        private void Start()
        {
            MasterVolume = MasterVolume;
            MusicVolume = MusicVolume;
        }

        public void Play(string soundName)
        {
            if (_unitMap.TryGetValue(soundName, out var unit))
            {
                unit.AudioSource.clip = unit.AudioClip;
                unit.AudioSource.Play();
            }
        }
    }
}