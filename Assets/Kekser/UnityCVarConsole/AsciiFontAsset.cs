using UnityEngine;

namespace Kekser.PowerCVarConsole
{
    [CreateAssetMenu(fileName = "AsciiFontAsset", menuName = "ASCII/Font Asset", order = 0)]
    public class AsciiFontAsset : ScriptableObject
    {
        [SerializeField]
        private Vector2Int _fontSize = new Vector2Int(8, 16);
        [SerializeField] 
        private Sprite[] _fontSprites;
        [SerializeField]
        private int _codePage = 437; // IBM PC https://de.wikipedia.org/wiki/Codepage_437

        public Sprite[] FontSprites => _fontSprites;
        public Vector2Int FontSize => _fontSize;
        public int CodePage => _codePage;
    }
}