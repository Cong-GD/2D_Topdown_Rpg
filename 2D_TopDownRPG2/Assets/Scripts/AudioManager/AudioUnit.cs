using System;
using UnityEngine;

namespace CongTDev.AudioManagement
{
    [Serializable]
    public class AudioUnit
    {
        [field: SerializeField] public string SoundName { get; private set; }
        [field: SerializeField] public AudioClip AudioClip { get; private set; }
        [field: SerializeField] public AudioSource AudioSource { get; private set; }
    }
}