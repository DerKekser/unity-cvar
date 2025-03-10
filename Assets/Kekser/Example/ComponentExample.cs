using Kekser.PowerCVar;
using UnityEngine;

namespace Kekser.Example
{
    public class ComponentExample : MonoBehaviour
    {
        [SerializeField]
        [CVar("exp_player_field")]
        private int _playerField = 0;
    }
}