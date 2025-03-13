using System.Collections;
using UnityEngine;

namespace Kekser.UnityCVarConsole
{
    public class CVarCoroutines : MonoBehaviour
    {
        private static bool _initialized;
        private static CVarCoroutines _instance;
        
        private static void Initialize()
        {
            if (_initialized)
                return;
            
            _instance = new GameObject("CVarCoroutines").AddComponent<CVarCoroutines>();
            DontDestroyOnLoad(_instance.gameObject);
            
            _initialized = true;
        }
        
        public static Coroutine Run(IEnumerator coroutine)
        {
            Initialize();
            return _instance.StartCoroutine(coroutine);
        }
        
        public static void Stop(Coroutine coroutine)
        {
            if (!_initialized)
                return;
            
            _instance.StopCoroutine(coroutine);
        }
    }
}