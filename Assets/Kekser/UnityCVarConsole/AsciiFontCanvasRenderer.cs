using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Kekser.UnityCVarConsole
{
    public class AsciiFontCanvasRenderer : MonoBehaviour
    {
        [SerializeField]
        private RawImage _rawImage;
        
        [SerializeField]
        private AsciiFontAsset _fontAsset;
        [SerializeField]
        private Vector2Int _size = new Vector2Int(80, 25);

        private Vector2Int _currentSize;
        private Texture2D _texture;
        private Color[] _frameBuffer;

        private bool[] _charSpriteCache;
        
        private byte[] _charBuffer;
        private Color[] _fontColorBuffer;
        private Color[] _backgroundColorBuffer;
        
        private List<int> _dirtyIndices = new List<int>();
        
        public Vector2Int Size
        {
            get
            {
                if (_currentSize == Vector2Int.zero)
                    SetSize(_size);
                return _currentSize;
            }
            set => SetSize(value);
        }

        private void Awake()
        {
            SetSize(_size);
            CreateFontBuffers();
        }

        private void OnDestroy()
        {
            if (_texture != null)
                Destroy(_texture);
        }
        
        private int ToPixelIndex(int pX, int pY)
        {
            return pY * (_currentSize.x * _fontAsset.FontSize.x) + pX;
        }
        
        private int ToPixelIndex(Vector2Int pixel)
        {
            return ToPixelIndex(pixel.x, pixel.y);
        }
        
        private Vector2Int FromPixelIndex(int pIndex)
        {
            return new Vector2Int(pIndex % (_currentSize.x * _fontAsset.FontSize.x), pIndex / (_currentSize.x * _fontAsset.FontSize.x));
        }

        private int ToIndex(int x, int y)
        {
            return y * _currentSize.x + x;
        }
        
        private int ToIndex(Vector2Int pixel)
        {
            return ToIndex(pixel.x, pixel.y);
        }
        
        private Vector2Int FromIndex(int index)
        {
            return new Vector2Int(index % _currentSize.x, index / _currentSize.x);
        }

        private void CreateBuffers()
        {
            _charBuffer = new byte[_currentSize.x * _currentSize.y];
            _fontColorBuffer = new Color[_currentSize.x * _currentSize.y];
            _backgroundColorBuffer = new Color[_currentSize.x * _currentSize.y];
        }

        private void CreateFontBuffers()
        {
            _charSpriteCache = new bool[_fontAsset.FontSprites.Length * _fontAsset.FontSize.x * _fontAsset.FontSize.y];
            for (int i = 0; i < _fontAsset.FontSprites.Length; i++)
            {
                Sprite sprite = _fontAsset.FontSprites[i];
                Color[] pixels = sprite.texture.GetPixels((int)sprite.textureRect.x, (int)sprite.textureRect.y, (int)sprite.textureRect.width, (int)sprite.textureRect.height);
                for (int j = 0; j < pixels.Length; j++)
                {
                    _charSpriteCache[i * pixels.Length + j] = pixels[j].r > 0.5f;
                }
            }
        }
        
        private void CreateTexture()
        {
            if (_texture != null)
                Destroy(_texture);
            
            Vector2Int textureSize = _fontAsset.FontSize * _currentSize;
            _texture = new Texture2D(textureSize.x, textureSize.y, TextureFormat.RGB24, true);
            _texture.filterMode = FilterMode.Point;
            _texture.wrapMode = TextureWrapMode.Clamp;
            _texture.anisoLevel = 0;
            _texture.Apply();
            
            _frameBuffer = new Color[textureSize.x * textureSize.y];
            
            if (_rawImage != null)
                _rawImage.texture = _texture;
        }
        
        public void SetSize(Vector2Int size)
        {
            if (_currentSize == size)
                return;
            _currentSize = size;
            CreateBuffers();
        }
        
        public void BufferChar(int x, int y, char c, Color fontColor, Color backgroundColor)
        {
            if (x < 0 || x >= _currentSize.x || y < 0 || y >= _currentSize.y)
                return;
            
            byte cByte = CharMapping.GetCP437Byte(c);
            
            var index = ToIndex(x, y);
            if (_charBuffer[index] == cByte && _fontColorBuffer[index] == fontColor && _backgroundColorBuffer[index] == backgroundColor)
                return;
            
            _charBuffer[index] = cByte;
            _fontColorBuffer[index] = fontColor;
            _backgroundColorBuffer[index] = backgroundColor;
            
            if (!_dirtyIndices.Contains(index))
                _dirtyIndices.Add(index);
        }
        
        private void BufferPixel(int pIndex, Color color)
        {
            if (pIndex < 0 || pIndex >= _frameBuffer.Length)
                return;
            _frameBuffer[pIndex] = color;
        }
        
        public void BufferPixel(int pX, int pY, Color color)
        {
            BufferPixel(ToPixelIndex(pX, pY), color);
        }
        
        public void BufferPixel(Vector2Int pixel, Color color)
        {
            BufferPixel(ToPixelIndex(pixel), color);
        }

        public void BufferTexture(int pX, int pY, Texture2D texture, Vector2 scale)
        {
            if (texture == null)
                return;
            
            int pixelCount = texture.width * texture.height;
            Color[] pixels = texture.GetPixels();
            for (int i = 0; i < pixelCount; i++)
            {
                int spriteX = i % texture.width;
                int spriteY = i / texture.width;
                BufferPixel(pX + Mathf.RoundToInt(spriteX * scale.x), pY + Mathf.RoundToInt(spriteY * scale.y), pixels[i]);
            }
        }
        
        public void BufferTexture(int pX, int pY, Texture2D texture)
        {
            BufferTexture(pX, pY, texture, Vector2.one);
        }
        
        public void BufferTexture(int pX, int pY, Texture2D texture, int pSizeX, int pSizeY)
        {
            if (texture == null)
                return;
            
            Vector2 scale = new Vector2((float)pSizeX / texture.width, (float)pSizeY / texture.height);
            BufferTexture(pX, pY, texture, scale);
        }

        public void ClearBuffer()
        {
            if (_frameBuffer == null)
                return;
            
            for (int i = 0; i < _frameBuffer.Length; i++)
                _frameBuffer[i] = Color.clear;
            
            _dirtyIndices.Clear();
        }

        public void Render()
        {
            Vector2Int textureSize = _fontAsset.FontSize * _currentSize;
            if (_texture == null || _texture.width != textureSize.x || _texture.height != textureSize.y)
                CreateTexture();
            
            int pixelCount = _fontAsset.FontSize.x * _fontAsset.FontSize.y;

            int index = 0;
            Vector2Int pos = Vector2Int.zero;
            for (int c = 0; c < _dirtyIndices.Count; c++)
            {
                index = _dirtyIndices[c];
                pos = FromIndex(index);
                for (int i = 0; i < pixelCount; i++)
                {
                    int spriteX = i % _fontAsset.FontSize.x;
                    int spriteY = i / _fontAsset.FontSize.x;
                    bool draw = _charSpriteCache[_charBuffer[index] * pixelCount + i];
                    _frameBuffer[ToPixelIndex(pos.x * _fontAsset.FontSize.x + spriteX, pos.y * _fontAsset.FontSize.y + spriteY)] = draw ? _fontColorBuffer[index] : _backgroundColorBuffer[index];
                }
            }
            
            _dirtyIndices.Clear();
            _texture.SetPixels(_frameBuffer);
            _texture.Apply();
        }
    }
}