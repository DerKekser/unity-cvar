using System.Linq;
using System.Text;
using UnityEngine;

namespace Kekser.UnityCVar
{
    public static class GlobalCVars
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            CVarAttributeCache.RegisterCVar(
                "go_setactive",
                "Enables or disables the current target GameObject (true/false)",
                typeof(GameObject),
                "SetActive"
            );
            
            CVarAttributeCache.RegisterCVar(
                "go_pos",
                "Changes position of the targeted GameObject",
                typeof(Transform),
                "position"
            );
            
            CVarAttributeCache.RegisterCVar(
                "go_rot",
                "Changes rotation of the targeted GameObject",
                typeof(Transform),
                "rotation"
            );

            CVarAttributeCache.RegisterCVar(
                "cam_fov",
                "Changes FOV of the targeted camera",
                typeof(Camera),
                "fieldOfView"
            );
        }
        
        [CVar("go_list", "Lists all active GameObjects in the current scene")]
        public static string ListGameObjects(string filter = "")
        {
            StringBuilder builder = new StringBuilder();
            foreach (GameObject obj in Object.FindObjectsOfType<GameObject>().OrderBy(go => go.name))
            {
                if (!string.IsNullOrWhiteSpace(filter) && !obj.name.Contains(filter)) 
                    continue;
                
                builder.AppendLine($"- {obj.name}: {obj.GetInstanceID()}");
            }
            return builder.ToString();
        }

        [CVar("dbg_log", "Prints a standard debug message")]
        public static void Print(string message)
        {
            Debug.Log(message);
        }
        
        [CVar("dbg_error", "Prints an error message with red highlighting")]
        public static void PrintError(string message)
        {
            Debug.LogError(message);
        }
        
        [CVar("dbg_warning", "Prints a warning message with yellow highlighting")]
        public static void PrintWarning(string message)
        {
            Debug.LogWarning(message);
        }
        
        [CVar("scn_load", "Unloads current scene and loads specified scene")]
        public static void LoadScene(string sceneName)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
        
        [CVar("scn_unload", "Unloads specified scene")]
        public static void UnloadScene(string sceneName)
        {
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneName);
        }
        
        [CVar("scn_reload", "Restarts the active scene")]
        public static void ReloadScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
        
        [CVar("scn_list", "Lists all scenes in the build settings")]
        public static void ListScenes()
        {
            for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings; i++)
            {
                string scenePath = UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i);
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
                Debug.Log(sceneName);
            }
        }
        
        [CVar("scn_additive", "Loads scene without unloading current scene")]
        public static void LoadSceneAdditive(string sceneName)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
        }
        
        [CVar("sys_version", "Prints the current Project version")]
        public static string Version
        {
            get => Application.version;
        }
        
        [CVar("sys_timescale", "Changes Time.timeScale (0.5 = half speed, 2.0 = double speed)")]
        public static float TimeScale
        {
            get => Time.timeScale;
            set => Time.timeScale = value;
        }
        
        [CVar("sys_quit", "Closes the game immediately")]
        public static void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBGL
#else
            Application.Quit();
#endif
        }

        [CVar("sys_vsync", "Changes the VSync count (0 = off, 1 = every vblank, 2 = every second vblank)")]
        public static int VSync
        {
            get => QualitySettings.vSyncCount;
            set => QualitySettings.vSyncCount = value;
        }
        
        [CVar("sys_fps", "Shows real-time FPS information")]
        public static int FPS
        {
            get => (int)(1f / Time.deltaTime);
        }
        
        [CVar("sys_targetfps", "Limits framerate to specified value, -1 = no limit")]
        public static int TargetFPS
        {
            get => Application.targetFrameRate;
            set => Application.targetFrameRate = value;
        }
        
        [CVar("sys_gfxlevel", " Changes the global quality setting (0-5)")]
        public static int GraphicsLevel
        {
            get => QualitySettings.GetQualityLevel();
            set => QualitySettings.SetQualityLevel(value);
        }
        
        [CVar("phy_gravity", "Changes Physics.gravity vector")]
        public static Vector3 Gravity
        {
            get => Physics.gravity;
            set => Physics.gravity = value;
        }
        
        [CVar("phy_timestep", "Adjusts Time.fixedDeltaTime value")]
        public static float FixedDeltaTime
        {
            get => Time.fixedDeltaTime;
            set => Time.fixedDeltaTime = value;
        }
        
        [CVar("aud_volume", "Adjusts AudioListener.volume (0.0-1.0)")]
        public static float AudioVolume
        {
            get => AudioListener.volume;
            set => AudioListener.volume = value;
        }
        
        [CVar("aud_mute", "Enables/disables all game audio")]
        public static bool AudioMute
        {
            get => AudioListener.pause;
            set => AudioListener.pause = value;
        }
        
        [CVar("rnd_quality", "Adjusts render texture quality")]
        public static int RenderTextureQuality
        {
            get => QualitySettings.masterTextureLimit;
            set => QualitySettings.masterTextureLimit = value;
        }
        
        [CVar("rnd_aa", "Changes anti-aliasing method and sample count")]
        public static int AntiAliasingMethod
        {
            get => QualitySettings.antiAliasing;
            set => QualitySettings.antiAliasing = value;
        }
        
        [CVar("rnd_shadows", "Enables/disables shadows or sets quality")]
        public static ShadowQuality Shadows
        {
            get => QualitySettings.shadows;
            set => QualitySettings.shadows = value;
        }
    }
}