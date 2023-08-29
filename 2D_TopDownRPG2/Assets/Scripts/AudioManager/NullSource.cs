using System;
using UnityEngine;
using UnityEngine.Audio;

namespace CongTDev.AudioManagement
{
    public class NullSource : PoolingAudioSource
    {
        private static PoolingAudioSource _instance;

        public static PoolingAudioSource Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GameObject().AddComponent<NullSource>();

                return _instance;
            }
        }

        public override bool IsPlaying => false;

        protected override void FixedUpdate() { }
        public override PoolingAudioSource Play(AudioClip audioClip, AudioMixerGroup mixerGroup = null)
        {
            return this;
        }

        public override PoolingAudioSource SetVolume(float volume)
        {
            return this;
        }

        public override PoolingAudioSource While(Func<bool> predicate)
        {
            return this;
        }

        public override PoolingAudioSource WhileTrue()
        {
            return this;
        }

        public override PoolingAudioSource OnComplete(Action onComplete)
        {
            return this;
        }

        public override void Stop()
        {
        }
    }
}