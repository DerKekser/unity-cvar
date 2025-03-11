using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Kekser.PowerCVar;
using Kekser.PowerCVarConsole;
using UnityEngine;

namespace Game.Scripts.Gameplay.ComputerSystem
{
    public class CVarConsole : MonoBehaviour
    {
        [SerializeField]
        private CVarDisplay _display;
        [SerializeField]
        private bool _inputEnabled = false;

        private CancellationTokenSource _cts;
        
        private CVarManager _cVarManager = new CVarManager();
        
        private List<string> _history = new List<string>();
        
        public CVarDisplay Display
        {
            get { return _display; }
            set => _display = value;
        }
        
        public bool AnyKeyDown => _inputEnabled && Input.anyKeyDown;
        public string InputString => _inputEnabled ? Input.inputString : string.Empty;
        
        public bool GetKeyDown(KeyCode keyCode) => _inputEnabled && Input.GetKeyDown(keyCode);
        public bool GetKeyUp(KeyCode keyCode) => _inputEnabled && Input.GetKeyUp(keyCode);
        public bool GetKey(KeyCode keyCode) => _inputEnabled && Input.GetKey(keyCode);
        
        [CVar("cmd_clear")]
        public void ClearConsole()
        {
            _display.Clear();
        }

        public bool InputEnabled
        {
            get { return _inputEnabled; }
            set => _inputEnabled = value;
        }
        
        public void NewLine()
        {
            _display.Write('\n');
        }

        public void Write(string text)
        {
            if (text == null || text.Length <= 0)
                return;
            
            foreach (char c in text)
                _display.Write(c);
        }
        
        public void Write(int number)
        {
            Write(number.ToString());
        }
        
        public void Write(float number)
        {
            Write(number.ToString());
        }
        
        public void Write(double number)
        {
            Write(number.ToString());
        }
        
        public void Write(bool boolean)
        {
            Write(boolean.ToString());
        }
        
        public void WriteLine(string text)
        {
            Write(text);
            NewLine();
        }
        
        public void WriteLine(int number)
        {
            Write(number);
            NewLine();
        }
        
        public void WriteLine(float number)
        {
            Write(number);
            NewLine();
        }
        
        public void WriteLine(double number)
        {
            Write(number);
            NewLine();
        }

        public IEnumerator ReadKeyCoroutine(Action<char> onKeyRead)
        {
            Display.DisplayCursor();
    
            while (!_cts.IsCancellationRequested)
            {
                yield return null;
        
                if (!AnyKeyDown)
                    continue;
            
                foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
                {
                    if (!GetKeyDown(keyCode))
                        continue;
                
                    if (InputString.Length == 0)
                        continue;
                
                    Display.Write(InputString[0]);
                    NewLine();
                    Display.HideCursor();
            
                    onKeyRead(InputString[0]);
                    yield break;
                }
            }
            
            Display.HideCursor();
            onKeyRead('\0');
        }

        public IEnumerator ReadLineCoroutine(Action<string> onLineRead, bool useHistory = false)
        {
            int historyIndex = -1;
            Display.DisplayCursor();
            StringBuilder inputBuffer = new StringBuilder();

            while (!_cts.IsCancellationRequested)
            {
                yield return null; // Wait for next frame

                if (!AnyKeyDown)
                    continue;

                bool consumedInputString = false;

                foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
                {
                    if (!GetKeyDown(keyCode))
                        continue;

                    switch (keyCode)
                    {
                        case KeyCode.Return:
                        {
                            string input = inputBuffer.ToString();
                            NewLine();
                            Display.HideCursor();
                            if (useHistory)
                                _history.Insert(0, input);
                            onLineRead(input);
                            yield break;
                        }
                        case KeyCode.Escape:
                            NewLine();
                            Display.HideCursor();
                            onLineRead(null);
                            yield break;
                        case KeyCode.Space:
                            inputBuffer.Append(' ');
                            _display.Write(' ');
                            break;
                        case KeyCode.Backspace when inputBuffer.Length <= 0:
                            continue;
                        case KeyCode.Backspace:
                            inputBuffer.Remove(inputBuffer.Length - 1, 1);
                            _display.CursorBackward();
                            _display.Write('\0');
                            _display.CursorBackward();
                            break;
                        case KeyCode.UpArrow:
                            if (!useHistory)
                                continue;
                            int oldIndex1 = historyIndex;
                            historyIndex++;
                            historyIndex = Mathf.Clamp(historyIndex, -1, _history.Count - 1);
                            if (historyIndex == oldIndex1)
                                break;

                            for (int i = 0; i < inputBuffer.Length; i++)
                            {
                                _display.CursorBackward();
                                _display.Write('\0');
                                _display.CursorBackward();
                            }

                            inputBuffer.Clear();
                            if (historyIndex == -1)
                                break;
                            inputBuffer.Append(_history[historyIndex]);
                            Write(_history[historyIndex]);
                            break;
                        case KeyCode.DownArrow:
                            if (!useHistory)
                                continue;
                            int oldIndex2 = historyIndex;
                            historyIndex--;
                            historyIndex = Mathf.Clamp(historyIndex, -1, _history.Count - 1);
                            if (historyIndex == oldIndex2)
                                break;

                            for (int i = 0; i < inputBuffer.Length; i++)
                            {
                                _display.CursorBackward();
                                _display.Write('\0');
                                _display.CursorBackward();
                            }

                            inputBuffer.Clear();
                            if (historyIndex == -1)
                                break;
                            inputBuffer.Append(_history[historyIndex]);
                            Write(_history[historyIndex]);
                            break;
                        default:
                            if (consumedInputString)
                                break;
                            historyIndex = -1;
                            consumedInputString = true;
                            inputBuffer.Append(InputString);
                            Write(InputString);
                            break;
                    }
                }
            }
            
            Display.HideCursor();
            onLineRead(null);
        }
        
        public void RunCmd(string cmd)
        {
            if (string.IsNullOrEmpty(cmd))
                return;
            cmd = cmd.Trim();
            
            CVarResult cVarResult = _cVarManager.ExecuteCommand(cmd);
            
            Display.SetColor(cVarResult.Success ? Color.white : Color.red);
            if (string.IsNullOrWhiteSpace(cVarResult.Message))
                return;
            WriteLine(cVarResult.Message);
        }
        
        private IEnumerator UpdateRunner()
        {
            string input = string.Empty;
            while (!_cts.IsCancellationRequested)
            {
                Display.SetBackgroundColor(Color.black);
                CVarTarget target = _cVarManager.GetTarget();
                if (target.TargetType != CVarTargetType.None)
                {
                    Display.SetColor(Color.green);
                    Write(_cVarManager.Target);
                }
                Display.SetColor(Color.gray);
                Write("> ");
                yield return ReadLineCoroutine(result =>
                {
                    input = result;
                }, true);
                
                RunCmd(input);
                
                yield return null;
                input = string.Empty;
            }
        }
        
        private void OnLogMessageReceived(string condition, string stackTrace, LogType type)
        {
            switch (type)
            {
                case LogType.Error:
                case LogType.Exception:
                    Display.SetColor(Color.red);
                    break;
                case LogType.Warning:
                    Display.SetColor(Color.yellow);
                    break;
                case LogType.Log:
                    Display.SetColor(Color.white);
                    break;
                default:
                    Display.SetColor(Color.gray);
                    break;
            }
            WriteLine(condition);
        }

        
        private void Start()
        {
            _cts = new CancellationTokenSource();
            //Application.logMessageReceived -= OnLogMessageReceived; // TODO: improve input handling and re-enable
            StartCoroutine(UpdateRunner());
        }
    }
}