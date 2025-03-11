using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Kekser.UnityCVar;
using Kekser.UnityCVarConsole;
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
        
        public bool AnyKeyDown => InputEnabled && Input.anyKeyDown;
        public string InputString => InputEnabled ? Input.inputString : string.Empty;
        
        public bool GetKeyDown(KeyCode keyCode) => InputEnabled && Input.GetKeyDown(keyCode);
        public bool GetKeyUp(KeyCode keyCode) => InputEnabled && Input.GetKeyUp(keyCode);
        public bool GetKey(KeyCode keyCode) => InputEnabled && Input.GetKey(keyCode);
        
        [CVar("cmd_clear")]
        public void ClearConsole()
        {
            _display.Clear();
        }

        public bool InputEnabled
        {
            get { return _inputEnabled && gameObject.activeInHierarchy; }
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
        
        public IEnumerator PressKeyCoroutine(Action<char> onKeyPress)
        {
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
                
                    onKeyPress(InputString[0]);
                    yield break;
                }
            }
            onKeyPress('\0');
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
            Vector2Int startPosition = Display.GetCursor();
            int historyIndex = -1;
            int cursorPosition = 0;
            Display.DisplayCursor();
            StringBuilder inputBuffer = new StringBuilder();

            while (!_cts.IsCancellationRequested)
            {
                yield return null; // Wait for next frame

                if (!AnyKeyDown)
                    continue;
                
                int oldInputLength = inputBuffer.Length;
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
                        case KeyCode.Backspace when cursorPosition > 0 && inputBuffer.Length > 0:
                            inputBuffer.Remove(cursorPosition - 1, 1);
                            cursorPosition = Mathf.Max(0, cursorPosition - 1);
                            break;
                        case KeyCode.Delete when cursorPosition < inputBuffer.Length && inputBuffer.Length > 0:
                            inputBuffer.Remove(cursorPosition, 1);
                            break;
                        case KeyCode.UpArrow:
                            if (!useHistory)
                                continue;
                            int oldIndex1 = historyIndex;
                            historyIndex++;
                            historyIndex = Mathf.Clamp(historyIndex, -1, _history.Count - 1);
                            if (historyIndex == oldIndex1)
                                break;
                            inputBuffer.Clear();
                            cursorPosition = 0;
                            if (historyIndex == -1)
                                break;
                            inputBuffer.Insert(cursorPosition, _history[historyIndex]);
                            cursorPosition = _history[historyIndex].Length;
                            break;
                        case KeyCode.DownArrow:
                            if (!useHistory)
                                continue;
                            int oldIndex2 = historyIndex;
                            historyIndex--;
                            historyIndex = Mathf.Clamp(historyIndex, -1, _history.Count - 1);
                            if (historyIndex == oldIndex2)
                                break;
                            inputBuffer.Clear();
                            cursorPosition = 0;
                            if (historyIndex == -1)
                                break;
                            inputBuffer.Append(_history[historyIndex]);
                            cursorPosition = _history[historyIndex].Length;
                            break;
                        case KeyCode.LeftArrow when cursorPosition > 0:
                            cursorPosition = Mathf.Max(0, cursorPosition - 1);
                            break;
                        case KeyCode.RightArrow when cursorPosition < inputBuffer.Length:
                            cursorPosition = Mathf.Min(inputBuffer.Length, cursorPosition + 1);
                            break;
                        case KeyCode.Home:
                            cursorPosition = 0;
                            break;
                        case KeyCode.End:
                            cursorPosition = inputBuffer.Length;
                            break;
                        case KeyCode.Backspace:
                        case KeyCode.Delete:
                        case KeyCode.LeftArrow:
                        case KeyCode.RightArrow:
                            break;
                        default:
                            if (consumedInputString)
                                break;
                            historyIndex = -1;
                            consumedInputString = true;
                            foreach (char c in InputString)
                            {
                                inputBuffer.Insert(cursorPosition, c);
                                cursorPosition++;
                            }
                            break;
                    }
                }
                
                Display.SetColor(Color.white);
                Display.SetCursor(startPosition.x, startPosition.y);
                Write(inputBuffer.ToString());
                if (oldInputLength > inputBuffer.Length)
                    for (int i = 0; i < oldInputLength - inputBuffer.Length; i++)
                        Display.Write(' ');
                Display.SetCursor(startPosition.x + cursorPosition, startPosition.y);
            }
            
            Display.HideCursor();
            onLineRead(null);
        }

        private IEnumerator SplitMessage(string message)
        {
            Color originalColor = Display.GetColor();

            int maxLines = Display.GetSize().y - 2;
            List<string> lines = new List<string>();
            foreach (string line in message.Split('\n'))
            {
                if (line.Length <= Display.GetSize().x)
                {
                    lines.Add(line);
                    continue;
                }
                
                int lc = Mathf.CeilToInt((float)line.Length / (Display.GetSize().x - 1));
                for (int i = 0; i < lc; i++)
                {
                    int startIndex = i * (Display.GetSize().x - 1);
                    int length = Mathf.Min((Display.GetSize().x - 1), line.Length - startIndex);
                    lines.Add(line.Substring(startIndex, length));
                }
            }
            
            int lineCount = 0;
            int targetLine = maxLines;

            while (!_cts.IsCancellationRequested)
            {
                if (lineCount >= lines.Count)
                    break;
                
                if (lineCount < targetLine)
                {
                    Display.SetColor(originalColor);
                    WriteLine(lines[lineCount]);
                    lineCount++;
                }
                else
                {
                    Display.SetColor(Color.gray);
                    Write("Press any key to continue...");
                    yield return PressKeyCoroutine(c => targetLine += maxLines);
                    Display.SetCursor(0, Display.GetCursor().y);
                    for (int i = 0; i < Display.GetSize().x - 1; i++)
                        Display.Write(' ');
                    Display.SetCursor(0, Display.GetCursor().y);
                }
                
                yield return null;
            }
        }
        
        private IEnumerator RunCmd(string cmd)
        {
            if (string.IsNullOrWhiteSpace(cmd))
                yield break;
            cmd = cmd.Trim();
            
            CVarResult cVarResult = _cVarManager.ExecuteCommand(cmd);
            
            if (string.IsNullOrWhiteSpace(cVarResult.Message))
                yield break;
            
            Display.SetColor(cVarResult.Success ? Color.white : Color.red);
            yield return SplitMessage(cVarResult.Message);
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
                    Write(GetTargetString(target));
                }
                Display.SetColor(Color.gray);
                Write("> ");
                yield return ReadLineCoroutine(result =>
                {
                    input = result;
                }, true);
                
                yield return RunCmd(input);
                
                yield return null;
                input = string.Empty;
            }
        }
        
        private string GetTargetString(CVarTarget target)
        {
            if (target.TargetType == CVarTargetType.None)
                return "None";
            if (target.TargetType == CVarTargetType.GameObjectList)
                return $"GameObject: {target.TargetName}";
            if (target.TargetType == CVarTargetType.ClassList)
                return $"Class: {target.TargetName}";
            return "Unknown";
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