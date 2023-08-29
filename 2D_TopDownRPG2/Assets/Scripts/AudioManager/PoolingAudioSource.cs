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
        private Action _onComplete;
        private Coroutine _checkingCoroutine;

        public virtual bool IsPlaying => audioSource.isPlaying;

        protected virtual void FixedUpdate()
        {
            if (!IsPlaying)
            {
                Stop();
            }
        }

        public virtual PoolingAudioSource Play(AudioClip audioClip, AudioMixerGroup mixerGroup = null)
        {
            _predicate = null;
            _onComplete = null;
            audioSource.outputAudioMixerGroup = mixerGroup;
            audioSource.clip = audioClip;
            audioSource.Play();
            return this;
        }

        public virtual PoolingAudioSource SetVolume(float volume)
        {
            volume = Mathf.Clamp01(volume);
            audioSource.volume = volume;
            _onComplete += () => audioSource.volume = 1f;
            return this;
        }

        public virtual PoolingAudioSource While(Func<bool> predicate)
        {
            if (predicate == null)
                return this;

            StopCoroutine();
            audioSource.loop = true;
            _onComplete += () => audioSource.loop = false;
            _predicate = predicate;
            _checkingCoroutine = StartCoroutine(CheckingCoroutine());
            return this;
        }

        public virtual PoolingAudioSource WhileTrue()
        {
            audioSource.loop = true;
            _onComplete += () => audioSource.loop = false;
            StopCoroutine();
            return this;
        }

        public virtual PoolingAudioSource OnComplete(Action onComplete)
        {
            _onComplete += onComplete;
            return this;
        }

        public virtual void Stop()
        {
            ReturnToPool();
            _onComplete?.Invoke();
            _onComplete = null;
        }

        private IEnumerator CheckingCoroutine()
        {
            while (_predicate != null && _predicate())
            {
                yield return null;
            }
            Stop();
        }

        private void StopCoroutine()
        {
            if (_checkingCoroutine != null)
            {
                StopCoroutine(_checkingCoroutine);
                _checkingCoroutine = null;
            }
        }
    }
}