using HarmonyLib;
using Server;
using UnityEngine.SceneManagement;

namespace EncounterTester.Patches
{
    public static class BanPrevention
    {
        public static bool SafeMode = false;
    
        [HarmonyPatch(typeof(GlobalGameManager), nameof(GlobalGameManager.LeaveStage))]
        [HarmonyPrefix]
        public static bool PreventLeaveStage()
        {
            if (SafeMode)
            {
                GlobalGameManager.Instance.LoadScene(SCENE_STATE.Main);
            }
            return !SafeMode;
        }
        
        public static void SceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (GlobalGameManager.Instance is {sceneState: SCENE_STATE.Main})
            {
                SafeMode = false;
            }
        }
    }
}