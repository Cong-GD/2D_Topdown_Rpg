using System.Collections;
using UnityEngine;

namespace CongTDev.AudioManagement
{
    /// <summary>
    /// Use for play audio throught unity event on scene
    /// </summary>
    public class AudioPlayerHelper : MonoBehaviour
    {
        [SerializeField] private string defaultBackgroundMusic;

        private PoolingAudioSource _musicSource;
        private Coroutine _musicDelayCoroutine;

        private void Start()
        {
            PlayDefaultMusic();
        }

        public void PlayDefaultMusic()
        {
            if (!string.IsNullOrEmpty(defaultBackgroundMusic))
            {
                PlayeMusic(defaultBackgroundMusic);
            }
        }

        public void PlayeMusic(string musicName)
        {
            if(_musicDelayCoroutine != null)
            {
                StopCoroutine(_musicDelayCoroutine);
            }
            StopMusic();
            _musicDelayCoroutine = StartCoroutine(PlayMusicDelay(musicName));
        }
        private IEnumerator PlayMusicDelay(string musicName)
        {
            yield return 0.5f.Wait();
            StopMusic();
            _musicSource = AudioManager.Play(musicName).WhileTrue();
        }

        public void PlaySound(string soundName)
        {
            AudioManager.Play(soundName);
        }

        public void StopMusic()
        {
            if (_musicSource != null && _musicSource.IsPlaying)
            {
                _musicSource.Stop();
                _musicSource = null;
            }
        }
    }
}
