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
        
        [CVar("con_clear", "Erases all text in the console window")]
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
                if (!AnyKeyDown)
                {
                    yield return null;
                    continue;
                }
            
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
                
                yield return null;
            }
        }

        public IEnumerator ReadLineCoroutine(Action<string> onLineRead, bool useHistory = false)
        {
            Vector2Int startPosition = Display.GetCursor();
            int historyIndex = -1;
            int cursorPosition = 0;
            Display.DisplayCursor();
            StringBuilder inputBuffer = new StringBuilder();

            int oldInputLength = 0;
            
            while (!_cts.IsCancellationRequested)
            {
                if (!AnyKeyDown)
                {
                    yield return null;
                    continue;
                }
                
                bool consumedInputString = false;

                foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
                {
                    if (!GetKeyDown(keyCode))
                        continue;

                    switch (keyCode)
                    {
                        case KeyCode.Return:
                        {
                            string i = inputBuffer.ToString();
                            NewLine();
                            Display.HideCursor();
                            if (useHistory)
                                _history.Insert(0, i);
                            onLineRead(i);
                            yield break;
                        }
                        case KeyCode.Tab:
                        {
                            string i = inputBuffer.ToString();
                            string c = TryToCompleteCmd(i);
                            if (c.Length <= i.Length)
                                break;
                            inputBuffer.Clear();
                            inputBuffer.Append(c);
                            cursorPosition = c.Length;
                            break;
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
                                break;
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
                                break;
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
                
                string input = inputBuffer.ToString();
                string completion = TryToCompleteCmd(input);
                completion = completion.Substring(Mathf.Min(input.Length, completion.Length));
                
                Display.SetColor(Color.white);
                Display.SetCursor(startPosition.x, startPosition.y);
                Write(input);
                Display.SetColor(Color.gray);
                Write(completion);
                int newInputLength = input.Length + completion.Length;
                if (oldInputLength > newInputLength)
                    for (int i = 0; i < oldInputLength - newInputLength; i++)
                        Display.Write(' ');
                Display.SetCursor(startPosition.x + cursorPosition, startPosition.y);
                oldInputLength = newInputLength;
                
                yield return null; // Wait for next frame
            }
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
                
                string rest = line;
                while (rest.Length > 0)
                {
                    int length = Mathf.Min(Display.GetSize().x - 1, rest.Length);
                    string temp = rest.Substring(0, length);
                    int spaceIndex = temp.LastIndexOf(' ');
                    if (spaceIndex == -1 || length == rest.Length)
                    {
                        lines.Add(temp);
                        rest = rest.Substring(length);
                    }
                    else
                    {
                        lines.Add(temp.Substring(0, spaceIndex));
                        rest = rest.Substring(spaceIndex + 1);
                    }
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

        private string[] GetCompletions(string cmd)
        {
            List<string> completions = new List<string>();
            foreach (KeyValuePair<string, ICVar> cVar in CVarAttributeCache.Cache.OrderBy(c => c.Key))
            {
                if (cVar.Key.StartsWith(cmd))
                    completions.Add(cVar.Key);
            }
            return completions.ToArray();
        }
        
        private string TryToCompleteCmd(string input)
        {
            string[] parts = input.Split(' ');
            if (parts.Length == 0)
                return input;
            string cmd = parts[0];
            if (string.IsNullOrWhiteSpace(cmd))
                return input;
            string[] completions = GetCompletions(cmd);
            if (completions.Length == 0)
                return input;
            return completions[0];
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
        
        private void RenderIntro()
        {
            NewLine();
            Display.SetColor(Color.green);
            WriteLine(@"   __  __      _ __           _______    __          ");
            WriteLine(@"  / / / /___  (_) /___  __   / ____/ |  / /___ ______");
            WriteLine(@" / / / / __ \/ / __/ / / /  / /    | | / / __ `/ ___/");
            WriteLine(@"/ /_/ / / / / / /_/ /_/ /  / /___  | |/ / /_/ / /    ");
            WriteLine(@"\____/_/ /_/_/\__/\__, /   \____/  |___/\__,_/_/     ");
            WriteLine(@"                 /____/                              ");
            NewLine();
            WriteLine("Welcome to the Unity CVar Console!");
            WriteLine("Type 'con_list' to list all available commands.");
            WriteLine("To specify the scope of the list, use 'con_list <filter>'.");
            NewLine();
        }

        
        private void Start()
        {
            _cts = new CancellationTokenSource();
            //Application.logMessageReceived -= OnLogMessageReceived; // TODO: improve input handling and re-enable
            RenderIntro();
            CVarCoroutines.Run(UpdateRunner());
        }
        
        private void OnDestroy()
        {
            _cts?.Cancel();
        }

        private void OnApplicationQuit()
        {
            _cts?.Cancel();
        }
    }
}