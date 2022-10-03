using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Waldem.Unity.Events;
using WaldemGame.Abstract;

namespace WaldemGame.Managers
{
    public class AudioManager : AManager
    {
    #pragma warning disable 0649
        [SerializeField]
        private List<Toggle> MusicToggles;
        [SerializeField]
        private List<Toggle> SoundsToggles;
        [SerializeField]
        private AudioSource MusicSource;
        [SerializeField]
        private AudioSource SoundsSource;
    #pragma warning restore 0649

        private bool musicOn;
        private bool soundsOn;
        private Dictionary<string, AudioClip> Musics = new Dictionary<string, AudioClip>();
        private Dictionary<string, AudioClip> sounds = new Dictionary<string, AudioClip>();

        public override void Init()
        {
            if(!Inited)
            {
                SubscribeToggles();
                LoadPrefs();
                
                MusicSource.gameObject.SetActive(true);
                SoundsSource.gameObject.SetActive(true);

                LoadSounds();
                LoadMusics();

                var gameManager = App.GetManager<GameManager>();
                // gameManager.OnAppEnter.AddListener(()=> PlayMusic("MainMenuTheme"));
                // gameManager.OnMenu.AddListener(()=> PlayMusic("MainMenuTheme"));
                // gameManager.OnGameStart.AddListener((arg0, arg1)=> PlayMusic("GameTheme"));

                var systemManager = App.GetManager<SystemManager>();
                systemManager.OnSave.AddListener(SavePrefs);
                Inited = true;
            }
        }

        private void LoadPrefs()
        {
            var musicValue = PlayerPrefs.GetInt("MusicOn", 1);
            
            if(musicValue == 1 || musicValue == 0)
            {
                SetMusic(Convert.ToBoolean(musicValue));
            }

            var soundsValue = PlayerPrefs.GetInt("SoundsOn", 1);

            if(soundsValue == 1 || soundsValue == 0)
            {
                SetSounds(Convert.ToBoolean(soundsValue));
            }
        }

        private void SubscribeToggles()
        {
            foreach (var item in SoundsToggles)
            {
                item.onValueChanged.AddListener(SetSounds);
            }

            foreach (var item in MusicToggles)
            {
                item.onValueChanged.AddListener(SetMusic);
            }
        }

        private void SetSoundsToggles(bool value)
        {
            foreach (var item in SoundsToggles)
            {
                item.isOn = value;
            }
        }

        private void SetMusicToggles(bool value)
        {
            foreach (var item in MusicToggles)
            {
                item.isOn = value;
            }
        }

        private void SavePrefs(SystemManager systemManager)
        {
            systemManager.AddDataToSave("MusicOn", Convert.ToInt32(musicOn));
            systemManager.AddDataToSave("SoundsOn", Convert.ToInt32(soundsOn));
        }

        private void LoadSounds()
        {
            AudioClip[] sounds = Resources.LoadAll<AudioClip>("Audio/Sounds");
            foreach (var item in sounds)
            {
                this.sounds.Add(item.name, item);
            }
        }

        private void LoadMusics()
        {
            AudioClip[] musics = Resources.LoadAll<AudioClip>("Audio/Music");
            foreach (var item in musics)
            {
                Musics.Add(item.name, item);
            }
        }

        public void PlayMusic(string musicName, bool looped = true)
        {
            if(Musics.ContainsKey(musicName))
            {
                var clip = Musics[musicName];
                MusicSource.clip = clip;
                MusicSource.loop = looped;

                if(MusicSource.enabled)
                {
                    MusicSource.Play();
                }
            }
        }

        public void PlaySound(string soundName, float volume = 1)
        {
            if(SoundsSource.enabled)
            {
                if(sounds.ContainsKey(soundName))
                {
                    var clip = sounds[soundName];
                    SoundsSource.PlayOneShot(clip, volume);
                }
            }
        }

        public void SetMusic(bool value)
        {
            MusicSource.enabled = value;
            musicOn = value;
            SetMusicToggles(musicOn);
        }

        public void SetSounds(bool value)
        {
            SoundsSource.enabled = value;
            soundsOn = value;
            SetSoundsToggles(soundsOn);

            if(value)
            {
                PlaySound("ButtonClick_01");
            }
        }
    }
}