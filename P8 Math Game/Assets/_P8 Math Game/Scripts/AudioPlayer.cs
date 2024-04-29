using System;
using UnityEngine;

namespace AstroMath
{
    public class AudioPlayer : MonoBehaviour
    {
        AudioSource audioSource;

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

            audioSource.playOnAwake = false;
        }

        public void PlaySoundEffect(string soundName)
        {
            var arrayToSearch = AudioContainer.instance.soundEffects;
            PlayAudio(arrayToSearch, soundName);
        }

        public void PlayDialogue(string dialogueName)
        {
            var arrayToSearch = AudioContainer.instance.dialogues;
            PlayAudio(arrayToSearch, dialogueName);
        }

        public void PlayMusic(string musicName, bool loop = false)
        {
            var arrayToSearch = AudioContainer.instance.music;
            PlayAudio(arrayToSearch, musicName, loop);
        }

        public void PlayMusicWithLoop(string musicName)
        {
            PlayMusic(musicName, true);
        }
        public void PlayMusicNoLoop(string musicName)
        {
            PlayMusic(musicName);
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