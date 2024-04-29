using System;
using UnityEngine;

namespace AstroMath
{
    [Serializable]
    public struct Audio
    {
        public string name;
        public bool loop;
        public AudioClip audioClip;
    }

    public class AudioContainer : MonoBehaviour
    {
        public static AudioContainer instance;

        public Audio[] soundEffects;
        public Audio[] dialogues;
        public Audio[] music;

        void Awake()
        {
            instance = this;
        }
    }
}