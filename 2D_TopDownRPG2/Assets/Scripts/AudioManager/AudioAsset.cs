using UnityEngine;

namespace CongTDev.AudioManagement
{
    [CreateAssetMenu(menuName = "AudioAsset")]
    public class AudioAsset : ScriptableObject
    {
        public enum MixerGroup
        {
            Master,
            Music,
            SFX,
            UI
        }

        [field: SerializeField] public AudioClip AudioClip { get; private set; }
        [field: SerializeField] public MixerGroup Mixer { get; private set; }
    }
}