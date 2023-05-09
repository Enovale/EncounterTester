using HarmonyLib;

namespace EncounterTester.Patches
{
    public static class Cheats
    {
        private static bool _cheatsEnabled;

        public static bool CheatsEnabled
        {
            get => _cheatsEnabled && BanPrevention.SafeMode;
            set => _cheatsEnabled = value;
        }

        [HarmonyPatch(typeof(BattleUnitModel), nameof(BattleUnitModel.TakeHpDamage))]
        [HarmonyPrefix]
        public static void PreventDamage(BattleUnitModel __instance, ref int value)
        {
            if (CheatsEnabled && __instance.Faction == UNIT_FACTION.PLAYER)
            {
                value = 1;
            }
        }
    }
}