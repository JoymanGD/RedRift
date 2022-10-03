using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Waldem.Unity.Events;
using WaldemGame.Abstract;

namespace WaldemGame.Managers
{    
    public class SystemManager : AManager
    {
    #pragma warning disable 0649
        [SerializeField]
        private EventSystem EventSystem;
    #pragma warning restore 0649

        public CustomEvent<SystemManager> OnSave = new CustomEvent<SystemManager>();

        private Dictionary<string, object> DataToSave;

        public override void Init()
        {
            if(!Inited)
            {
                EventSystem.gameObject.SetActive(true);

                DataToSave = new Dictionary<string, object>();

                var gameManager = App.GetManager<GameManager>();
                gameManager.OnAppPause.AddListener(SaveAllData);
                gameManager.OnAppQuit.AddListener(SaveAllData);
                Inited = true;
            }
        }

        public void AddDataToSave(string name, object value)
        {
            if(DataToSave.ContainsKey(name))
            {
                DataToSave[name] = value;
            }
            else
            {
                DataToSave.Add(name, value);
            }
        }

        public T GetData<T>(string name)
        {
            var type = typeof(T);
            object result = null;

            if(DataToSave.ContainsKey(name))
            {
                result = DataToSave[name];
            }
            else
            {
                switch (type.Name)
                {
                    case "Int32":
                        result = PlayerPrefs.GetInt(name);
                        break;
                    case "Single":
                        result = PlayerPrefs.GetFloat(name);
                        break;
                    case "String":
                        result = PlayerPrefs.GetString(name);
                        break;
                    default:
                        Debug.LogWarning($"Data was not found: unknown name. Name: {name}");
                        result = (T)default;
                        break;
                }
            }

            return (T)result;
        }

        private void SaveAllData()
        {
            OnSave?.Invoke(this);
            foreach (var item in DataToSave)
            {
                var name = item.Key;
                var value = item.Value;
                SaveData(name, value);
            }
        }

        private void SaveData(string name, object value)
        {
            var type = value.GetType();

            switch (type.Name)
            {
                case "Int32":
                    PlayerPrefs.SetInt(name, (int)value);
                    break;
                case "Single":
                    PlayerPrefs.SetFloat(name, (float)value);
                    break;
                case "String":
                    PlayerPrefs.SetString(name, (string)value);
                    break;
                default:
                    Debug.LogWarning($"Data was not saved: unknown data type. Name: {name}, Object: {value}, Type name: {type.Name}");
                    break;
            }
        }
    }
}