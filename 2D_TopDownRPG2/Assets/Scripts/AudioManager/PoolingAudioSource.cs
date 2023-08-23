using CongTDev.ObjectPooling;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace CongTDev.AudioManagement
{
    public class PoolingAudioSource : PoolObject
    {
        [SerializeField] private AudioSource audioSource;

        private Func<bool> _predicate;

        private Action _onEndPlaying;

        private void FixedUpdate()
        {
            if (!audioSource.isPlaying)
            {
                Stop();
            }
        }

        public PoolingAudioSource Play(AudioClip audioClip, AudioMixerGroup mixerGroup = null)
        {
            _predicate = null;
            _onEndPlaying = null;
            audioSource.outputAudioMixerGroup = mixerGroup;
            audioSource.clip = audioClip;
            audioSource.loop = false;
            audioSource.Play();
            return this;
        }

        public PoolingAudioSource While(Func<bool> predicate)
        {
            if (predicate == null)
                return this;

            audioSource.loop = true;
            _predicate = predicate;
            StopAllCoroutines();
            StartCoroutine(StopLoopCoroutine());
            return this;
        }

        public PoolingAudioSource WhileTrue()
        {
            audioSource.loop = true;
            StopAllCoroutines();
            return this;
        }

        public PoolingAudioSource OnEndPlay(Action onEndPlaying)
        {
            _onEndPlaying = onEndPlaying;
            return this;
        }

        public void Stop()
        {
            ReturnToPool();
            _onEndPlaying?.Invoke();
            _onEndPlaying = null;
        }

        private IEnumerator StopLoopCoroutine()
        {
            while (_predicate != null && _predicate())
            {
                yield return null;
            }
            Stop();
        }
    }
}