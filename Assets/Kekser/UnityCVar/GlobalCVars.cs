using UnityEngine;

namespace Kekser.UnityCVar
{
    public static class GlobalCVars
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)] 
        private static void Initialize()
        {
            CVarAttributeCache.RegisterCVar(
                "gameobject_active",
                "Sets the active state of the GameObject.",
                typeof(GameObject),
                "SetActive"
            );
        }
        
        [CVar("unity_quit")]
        public static void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBGL
#else
            UnityEngine.Application.Quit();
#endif
        }
        
        [CVar("unity_print")]
        public static void Print(string message)
        {
            UnityEngine.Debug.Log(message);
        }
        
        [CVar("unity_print.error")]
        public static void PrintError(string message)
        {
            UnityEngine.Debug.LogError(message);
        }
        
        [CVar("unity_print.warning")]
        public static void PrintWarning(string message)
        {
            UnityEngine.Debug.LogWarning(message);
        }
        
        [CVar("unity_scene.load")]
        public static void LoadScene(string sceneName)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
        
        [CVar("unity_timescale")]
        public static float TimeScale
        {
            get => UnityEngine.Time.timeScale;
            set => UnityEngine.Time.timeScale = value;
        }
        
        [CVar("unity_vsync")]
        public static int VSync
        {
            get => UnityEngine.QualitySettings.vSyncCount;
            set => UnityEngine.QualitySettings.vSyncCount = value;
        }
        
        [CVar("unity_fps")]
        public static int FPS
        {
            get => (int)(1f / UnityEngine.Time.deltaTime);
        }
        
        [CVar("unity_fps.target")]
        public static int TargetFPS
        {
            get => UnityEngine.Application.targetFrameRate;
            set => UnityEngine.Application.targetFrameRate = value;
        }
    }
}