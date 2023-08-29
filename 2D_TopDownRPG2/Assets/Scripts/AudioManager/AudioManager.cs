using CongTDev.ObjectPooling;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

namespace CongTDev.AudioManagement
{
    public static class AudioManager
    {
        public const string MASTER_VOLUME = "MasterVolume";
        public const string MUSIC_VOLUME = "MusicVolume";
        public const string SFX_VOLUME = "SFXVolume";
        public const string UI_VOLUME = "UIVolume";
        private const float MIN_VALUE = 0.0001f;
        private const float MAX_VALUE = 1f;

        public static readonly AudioMixer Mixer;
        private static readonly Dictionary<string, AudioAsset> _audioAssets;
        private static readonly Dictionary<AudioAsset.MixerGroup, AudioMixerGroup> _audioGroups;
        private static readonly Prefab _audioSourcePrefab;
        public static float MasterVolume
        {
            get => Mathf.Clamp(PlayerPrefs.GetFloat(MASTER_VOLUME, MAX_VALUE), MIN_VALUE, MAX_VALUE);
            set
            {
                value = Mathf.Clamp(value, MIN_VALUE, MAX_VALUE);
                Mixer.SetFloat(MASTER_VOLUME, ValueToVolume(value));
                PlayerPrefs.SetFloat(MASTER_VOLUME, value);
            }
        }

        public static float MusicVolume
        {
            get => Mathf.Clamp(PlayerPrefs.GetFloat(MUSIC_VOLUME, MAX_VALUE), MIN_VALUE, MAX_VALUE);
            set
            {
                value = Mathf.Clamp(value, MIN_VALUE, MAX_VALUE);
                Mixer.SetFloat(MUSIC_VOLUME, ValueToVolume(value));
                PlayerPrefs.SetFloat(MUSIC_VOLUME, value);
            }
        }
        public static float SFXVolume
        {
            get => Mathf.Clamp(PlayerPrefs.GetFloat(SFX_VOLUME, MAX_VALUE), MIN_VALUE, MAX_VALUE);
            set
            {
                value = Mathf.Clamp(value, MIN_VALUE, MAX_VALUE);
                Mixer.SetFloat(SFX_VOLUME, ValueToVolume(value));
                PlayerPrefs.SetFloat(SFX_VOLUME, value);
            }
        }

        public static float UIVolume
        {
            get => Mathf.Clamp(PlayerPrefs.GetFloat(UI_VOLUME, MAX_VALUE), MIN_VALUE, MAX_VALUE);
            set
            {
                value = Mathf.Clamp(value, MIN_VALUE, MAX_VALUE);
                Mixer.SetFloat(UI_VOLUME, ValueToVolume(value));
                PlayerPrefs.SetFloat(UI_VOLUME, value);
            }
        }

        static AudioManager()
        {
            Mixer = Resources.Load<AudioMixer>("Audio/AudioMixer");
            _audioAssets = Resources.LoadAll<AudioAsset>("Audio/AudioAssets")
                                    .ToDictionary(audioAsset => audioAsset.name, audioAsset => audioAsset);
            _audioSourcePrefab = Resources.Load<GameObject>("Audio/AudioSourceHelper").GetComponent<Prefab>();

            var mixerGroups = Mixer.FindMatchingGroups("");
            _audioGroups = new()
            {
                { AudioAsset.MixerGroup.Master, mixerGroups.First((group) => group.name == "Master") },
                { AudioAsset.MixerGroup.Music, mixerGroups.First((group) => group.name == "Music") },
                { AudioAsset.MixerGroup.SFX, mixerGroups.First((group) => group.name == "SFX") },
                { AudioAsset.MixerGroup.UI, mixerGroups.First((group) => group.name == "UI") },
            };
            LoadVolume();
        }

        public static void LoadVolume()
        {
            MasterVolume = MasterVolume;
            MusicVolume = MusicVolume;
            SFXVolume = SFXVolume;
            UIVolume = UIVolume;
        }

        public static float ValueToVolume(float value)
        {
            return Mathf.Log10(value) * 20;
        }

        public static float VolumeToValue(float volume)
        {
            return Mathf.Pow(10f, volume / 20);
        }

        public static PoolingAudioSource Play(string soundName)
        {
            if (!_audioAssets.TryGetValue(soundName, out AudioAsset audioAsset))
            {
                Debug.LogWarning($"Sound \"{soundName}\" does not exits!");
                return NullSource.Instance;
            }

            if (!PoolManager.Get<PoolingAudioSource>(_audioSourcePrefab, out var audioSource))
            {
                Debug.LogError("Fail to create audio source. Please check your resource floder");
                return NullSource.Instance;
            }  

            return audioSource.Play(audioAsset.AudioClip, _audioGroups[audioAsset.Mixer]);
        }
    }
}
