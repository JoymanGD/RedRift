using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Waldem.Unity.Events;
using WaldemGame.Abstract;

namespace WaldemGame.Managers
{    
    public class InputManager : AManager, IUpdatable
    {
    #pragma warning disable 0649
        [SerializeField]
        private InputKeyData[] keyBindings;
        [SerializeField]
        private InputMouseData[] mouseBindings;
    #pragma warning restore 0649

        private bool inputEnabled = true;
        private bool mouseEnabled = true;
        private bool keyboardEnabled = true;

        [HideInInspector]
        public Vector2Event OnAxisInputUpdate = new Vector2Event();
        [HideInInspector]
        public FloatEvent OnMouseWheelScrolled = new FloatEvent();
        [HideInInspector]
        public CustomEvent<InputKeyData> OnKeyPressed = new CustomEvent<InputKeyData>();
        [HideInInspector]
        public CustomEvent<InputMouseData> OnMouseButtonPressed = new CustomEvent<InputMouseData>();

        public override void Init()
        {
            if(!Inited)
            {
                Inited = true;
            }
        }

        public void DoUpdate(float deltaTime)
        {
            if(inputEnabled)
            {
                if(keyboardEnabled)
                {
                    var horizontal = Input.GetAxisRaw("Horizontal");
                    var vertical = Input.GetAxisRaw("Vertical");
                    var axisInputVector = new Vector2(horizontal, vertical);

                    OnAxisInputUpdate?.Invoke(axisInputVector);
                }

                if(mouseEnabled)
                {
                    var mouseScroll = Input.mouseScrollDelta.y;

                    if(mouseScroll != 0)
                    {
                        OnMouseWheelScrolled?.Invoke(mouseScroll);
                    }
                }
            }

            foreach (var item in keyBindings)
            {
                if(item.enabled && Input.GetKeyDown(item.keyCode))
                {
                    OnKeyPressed?.Invoke(item);
                }
            }

            foreach (var item in mouseBindings)
            {
                if(item.enabled && Input.GetMouseButtonDown(item.Button))
                {
                    OnMouseButtonPressed?.Invoke(item);
                }
            }
        }

        public void EnableAllInputButtons(InputTypes inputType)
        {
            switch(inputType)
            {
                case InputTypes.All:
                    EnableAllInputButtons(InputTypes.Keyboard);
                    EnableAllInputButtons(InputTypes.Mouse);
                    break;
                case InputTypes.Keyboard:
                    foreach (ref InputKeyData item in keyBindings.AsSpan())
                    {
                        item.SetActive(true);
                    }
                    break;
                case InputTypes.Mouse:
                    foreach (ref InputMouseData item in mouseBindings.AsSpan())
                    {
                        item.SetActive(true);
                    }
                    break;
            }
        }

        public void DisableAllInputButtons(InputTypes inputType)
        {
            switch(inputType)
            {
                case InputTypes.All:
                    DisableAllInputButtons(InputTypes.Keyboard);
                    DisableAllInputButtons(InputTypes.Mouse);
                    break;
                case InputTypes.Keyboard:
                    foreach (ref InputKeyData item in keyBindings.AsSpan())
                    {
                        item.SetActive(false);
                    }
                    break;
                case InputTypes.Mouse:
                    foreach (ref InputMouseData item in mouseBindings.AsSpan())
                    {
                        item.SetActive(false);
                    }
                    break;
            }
        }

        public void DisableInputButton(InputTypes inputType, string name)
        {
            switch(inputType)
            {
                case InputTypes.All:
                    DisableInputButton(InputTypes.Keyboard, name);
                    DisableInputButton(InputTypes.Mouse, name);
                    break;
                case InputTypes.Keyboard:
                    foreach (ref InputKeyData item in keyBindings.AsSpan())
                    {
                        if(item.Name == name)
                        {
                            item.SetActive(false);
                        }
                    }
                    break;
                case InputTypes.Mouse:
                    foreach (ref InputMouseData item in mouseBindings.AsSpan())
                    {
                        if(item.Name == name)
                        {
                            item.SetActive(false);
                        }
                    }
                    break;
            }
        }

        public void DisableAllExceptOneInputButton(InputTypes inputType, string name)
        {
            switch(inputType)
            {
                case InputTypes.All:
                    DisableAllExceptOneInputButton(InputTypes.Keyboard, name);
                    DisableAllExceptOneInputButton(InputTypes.Mouse, name);
                    break;
                case InputTypes.Keyboard:
                    foreach (ref InputKeyData item in keyBindings.AsSpan())
                    {
                        item.SetActive(item.Name == name);
                    }
                    break;
                case InputTypes.Mouse:
                    foreach (ref InputMouseData item in mouseBindings.AsSpan())
                    {
                        item.SetActive(item.Name == name);
                    }
                    break;
            }
        }

        public KeyCode GetKeyByInputName(string inputName)
        {
            InputKeyData binding = keyBindings.FirstOrDefault(b=> b.Name == inputName);

            return binding.keyCode;
        }

        public int GetMouseButtonByInputName(string inputName)
        {
            InputMouseData binding = mouseBindings.FirstOrDefault(b=> b.Name == inputName);

            return binding.Button;
        }

        public void DisableInput(InputTypes inputType)
        {
            switch (inputType)
            {
                case InputTypes.Keyboard:
                    OnAxisInputUpdate?.Invoke(Vector2.zero);
                    keyboardEnabled = false;
                    break;
                case InputTypes.Mouse:
                    mouseEnabled = false;
                    break;
                case InputTypes.All:
                    OnAxisInputUpdate?.Invoke(Vector2.zero);
                    inputEnabled = false;
                    break;
            }

        }

        public void EnableInput(InputTypes inputType)
        {
            switch (inputType)
            {
                case InputTypes.Keyboard:
                    keyboardEnabled = true;
                    break;
                case InputTypes.Mouse:
                    mouseEnabled = true;
                    break;
                case InputTypes.All:
                    inputEnabled = true;
                    break;
            }

        }
    }

    public enum SwipeTypes
    {
        Right, Up, Left, Down, Null
    }

    public enum InputTypes
    {
        All, Keyboard, Mouse
    }

    [Serializable]
    public struct InputKeyData
    {
        public string Name;
        public KeyCode keyCode;
        public bool enabled;

        public static bool operator ==(InputKeyData a, InputKeyData b)
        {
            return a.Name == b.Name && a.keyCode == b.keyCode;
        }

        public static bool operator !=(InputKeyData a, InputKeyData b)
        {
            return a.Name != b.Name || a.keyCode != b.keyCode;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public void SetActive(bool value)
        {
            enabled = value;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    [Serializable]
    public struct InputMouseData
    {
        public string Name;
        public int Button;
        public bool enabled;

        public void SetActive(bool value)
        {
            enabled = value;
        }
    }
}