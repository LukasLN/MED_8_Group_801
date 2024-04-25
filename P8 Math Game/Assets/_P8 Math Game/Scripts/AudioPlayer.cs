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

    public class AudioPlayer : MonoBehaviour
    {
        AudioSource audioSource;
        [SerializeField] Audio[] soundEffects;
        [SerializeField] Audio[] music;

        private void Awake()
        {
            if (GetComponent<AudioSource>() == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
            else
            {
                audioSource = GetComponent<AudioSource>();
            }
        }

        public void PlaySoundEffect(string soundName, bool loop = false)
        {
            var arrayToSearch = soundEffects;
            PlayAudio(soundEffects, soundName, loop);
        }

        public void PlaySoundEffect(string soundName)
        {
            var arrayToSearch = soundEffects;
            PlayAudio(soundEffects, soundName, false);
        }

        public void PlayMusic(string musicName, bool loop = false)
        {
            var arrayToSearch = music;
            PlayAudio(music, musicName, loop);
        }

        void PlayAudio(Audio[] array, string audioName, bool loop = false)
        {
            audioSource.clip = Array.Find(array, audio => audio.name == audioName).audioClip;
            audioSource.loop = loop;
            audioSource.Play();
        }

        public void StopAudio()
        {
            audioSource.Stop();
        }
    }
}