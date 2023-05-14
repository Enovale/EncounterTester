using System;
using HarmonyLib;

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
                GlobalGameManager.Instance.LoadScene(SCENE_STATE.Main, (Action)(() => SafeMode = false));
            }
            return !SafeMode;
        }
    }
}