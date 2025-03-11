using Kekser.UnityCVar;
using UnityEngine;

namespace Kekser.Example
{
    public class ComponentExample : MonoBehaviour
    {
        [CVar("exp_player_static_field")]
        private static int _playerStaticField = 0;
        
        [SerializeField]
        [CVar("exp_player_field")]
        private int _playerField = 0;
    }
}