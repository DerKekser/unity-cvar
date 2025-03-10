using System;
using System.Collections;
using System.Threading;
using UnityEngine;

namespace Kekser.PowerCVarConsole
{
    public class CVarDisplay : MonoBehaviour
    {
        private struct CharInfo
        {
            public char Char { get; set; }
            public Color Color { get; set; }
            public Color BackgroundColor { get; set; }
            
            public CharInfo(char c, Color color, Color backgroundColor)
            {
                Char = c;
                Color = color;
                BackgroundColor = backgroundColor;
            }
        }
        
        [SerializeField]
        private AsciiFontCanvasRenderer _fontRenderer;
        [SerializeField]
        private int _refreshRate = 15;

        private CancellationTokenSource _cts;

        private Vector2Int _lastSize;
        
        private int _lines;
        private int _charsPerLine;

        private Vector2Int _cursorPosition;
        private Color _currentColor = Color.white;
        private Color _currentBackgroundColor = Color.black;
        private CharInfo[] _charBuffer;

        private float _cursorBlinkTime = 1f;
        private bool _cursorActive = false;
        private bool _cursorVisible = false;
        private bool _lastCursorVisible = false;
        
        private bool _isDirty;
        
        private void Awake()
        {
            RecalculateSize();
        }

        private void Start()
        {
            _cts = new CancellationTokenSource();
            StartCoroutine(RefreshLoop());
        }

        private void OnApplicationQuit()
        {
            _cts?.Cancel();
        }
        
        private IEnumerator RefreshLoop()
        {
            while (!_cts.IsCancellationRequested)
            {
                yield return new WaitForSeconds(1f / _refreshRate);
                
                RecalculateSize();
                if (_isDirty)
                    RenderText();

                if (_cursorActive)
                {
                    _cursorVisible = (Time.time % _cursorBlinkTime) < _cursorBlinkTime / 2f;
                    if (_cursorVisible != _lastCursorVisible || _isDirty)
                        RenderCursor();
                }
                
                if (_cursorVisible != _lastCursorVisible || _isDirty)
                    _fontRenderer.Render();
                _lastCursorVisible = _cursorVisible;
                _isDirty = false;
            }
        }
        
        private Vector2Int ToPosition(int index)
        {
            return new Vector2Int(index % _charsPerLine, index / _charsPerLine);
        }
        
        private int ToIndex(Vector2Int position)
        {
            return position.y * _charsPerLine + position.x;
        }
        
        public void DisplayCursor()
        {
            _cursorActive = true;
        }
        
        public void HideCursor()
        {
            _cursorActive = false;
        }
        
        public void CursorNextLine()
        {
            if (_cursorPosition.y >= _lines - 1)
            {
                CharInfo[] newBuffer = new CharInfo[_charBuffer.Length];
                Array.Copy(_charBuffer, _charsPerLine, newBuffer, 0, _charBuffer.Length - _charsPerLine);
                _charBuffer = newBuffer;
            }
            else
                _cursorPosition.y++;
        }

        public void CursorPrevLine()
        {
            if (_cursorPosition.y > 0)
                _cursorPosition.y--;
        }

        public void CursorForward()
        {
            _cursorPosition.x++;
            if (_cursorPosition.x >= _charsPerLine)
            {
                _cursorPosition.x = 0;
                CursorNextLine();
            }
        }
        
        public void CursorBackward()
        {
            _cursorPosition.x--;
            if (_cursorPosition.x < 0)
            {
                _cursorPosition.x = _charsPerLine - 1;
                CursorPrevLine();
            }
        }
        
        private void RecalculateSize()
        {
            if (_lastSize == _fontRenderer.Size)
                return;
            _lastSize = _fontRenderer.Size;
            _lines = _lastSize.y;
            _charsPerLine = _lastSize.x;
            _charBuffer = new CharInfo[_lines * _charsPerLine];
            Clear();
        }
        
        private void RenderCursor()
        {
            int index = ToIndex(_cursorPosition);
            CharInfo charInfo = _charBuffer[index];
            
            if (charInfo.Char != '\0' && charInfo.Char != ' ')
                _fontRenderer.BufferChar(
                    _cursorPosition.x, 
                    _lines - _cursorPosition.y - 1,
                    charInfo.Char, 
                    charInfo.Color, 
                     _cursorVisible ? Color.gray : charInfo.BackgroundColor
                );
            else
                _fontRenderer.BufferChar(
                    _cursorPosition.x, 
                    _lines - _cursorPosition.y - 1,
                    _cursorVisible ? '_' : ' ',
                    _cursorVisible ? Color.gray : charInfo.Color, 
                    charInfo.BackgroundColor
                    );
        }
        
        private void RenderText()
        {
            for (int i = 0; i < _charBuffer.Length; i++)
            {
                Vector2Int position = ToPosition(i);
                CharInfo charInfo = _charBuffer[i];

                if (charInfo.Char != '\0' && charInfo.Char != ' ')
                {
                    _fontRenderer.BufferChar(
                        position.x, 
                        _lines - position.y - 1,
                        charInfo.Char, 
                        charInfo.Color, 
                        charInfo.BackgroundColor
                        );
                }
                else
                    _fontRenderer.BufferChar(
                        position.x, 
                        _lines - position.y - 1,
                        ' ',
                        charInfo.Color,
                        charInfo.BackgroundColor
                        );
            }
        }
        
        private void RequestRender()
        {
            _isDirty = true;
        }

        public Vector2Int GetSize()
        {
            return new Vector2Int(_charsPerLine, _lines);
        }
        
        public Vector2Int GetPixelSize()
        {
            return GetSize() * _fontRenderer.Size;
        }
        
        public Vector2Int GetCursor()
        {
            return _cursorPosition;
        }

        public void SetCursor(int x, int y)
        {
            x = Mathf.Clamp(x, 0, _charsPerLine - 1);
            y = Mathf.Clamp(y, 0, _lines - 1);
            _cursorPosition.x = x;
            _cursorPosition.y = y;
            RequestRender();
        }
        
        public Color GetColor(Vector2Int position)
        {
            int index = ToIndex(position);
            return _charBuffer[index].Color;
        }

        public Color GetColor()
        {
            return _currentColor;
        }

        public void SetColor(Color color)
        {
            _currentColor = color;
        }
        
        public Color GetBackgroundColor(Vector2Int position)
        {
            int index = ToIndex(position);
            return _charBuffer[index].Color;
        }
        
        public Color GetBackgroundColor()
        {
            return _currentBackgroundColor;
        }
        
        public void SetBackgroundColor(Color color)
        {
            _currentBackgroundColor = color;
        }

        public void Clear()
        {
            if (_charBuffer == null)
                RecalculateSize();
            
            _charBuffer = new CharInfo[_lines * _charsPerLine];
            _cursorPosition = Vector2Int.zero;
            _currentColor = Color.white;
            _currentBackgroundColor = Color.black;
            _fontRenderer.ClearBuffer();
            RequestRender();
        }
        
        public char GetChar(Vector2Int position)
        {
            int index = ToIndex(position);
            return _charBuffer[index].Char;
        }

        public void SetChar(char c)
        {
            int index = ToIndex(_cursorPosition);
            _charBuffer[index].Char = c;
            _charBuffer[index].Color = _currentColor;
            _charBuffer[index].BackgroundColor = _currentBackgroundColor;
        }

        public void Write(char c)
        {
            if (_charBuffer == null)
                Clear();
            
            switch (c)
            {
                case '\n':
                    CursorNextLine();
                    _cursorPosition.x = 0;
                    return;
                case '\r':
                    _cursorPosition.x = 0;
                    return;
                case '\t':
                    _cursorPosition.x += 4;
                    return;
                case '\b':
                    CursorBackward();
                    return;
            }
            
            SetChar(c);
            _cursorPosition.x++;
            if (_cursorPosition.x >= _charsPerLine)
                Write('\n');
            
            RequestRender();
        }

        public void DrawTexture(Texture2D texture, int x, int y, int width, int height)
        {
            //Workaround for the fact that the display cant buffer textures
            //We need to render the char buffer first, then render the texture
            //This breaks the render loop and we render a frame more than we need to but it works
            //Multiple textures drawn in the same frame can cause lag
            //TODO: Fix this
            RenderText();       
            _fontRenderer.Render();
            
            _fontRenderer.BufferTexture(x, y, texture, width, height);
            RequestRender();
        }
    }
}