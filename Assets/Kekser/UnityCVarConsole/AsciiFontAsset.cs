using UnityEngine;

namespace Kekser.UnityCVarConsole
{
    [CreateAssetMenu(fileName = "AsciiFontAsset", menuName = "ASCII/Font Asset", order = 0)]
    public class AsciiFontAsset : ScriptableObject
    {
        [SerializeField]
        private Vector2Int _fontSize = new Vector2Int(8, 16);
        [SerializeField] 
        private Sprite[] _fontSprites;

        public Sprite[] FontSprites => _fontSprites;
        public Vector2Int FontSize => _fontSize;
    }
}