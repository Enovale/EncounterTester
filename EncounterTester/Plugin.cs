using System;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using EncounterTester.Patches;
using EncounterTester.UI;
using HarmonyLib;
using UnityEngine;
using UniverseLib;
using UniverseLib.Config;
using UniverseLib.UI;

namespace EncounterTester
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BasePlugin
    {
        public static ManualLogSource PluginLog;

        public static UIBase UiBase { get; private set; }

        public Plugin()
        {
            PluginLog = Log;
        }

        public override void Load()
        {
            // Plugin startup logic
            Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

            Initialize();

            var harmony = new Harmony($"com.enovale.{MyPluginInfo.PLUGIN_GUID}");
            harmony.PatchAll(typeof(BanPrevention));
            harmony.PatchAll(typeof(Cheats));

            Universe.Init(3f, OnInitialized, UniverseLog, new UniverseLibConfig()
            {
                Disable_EventSystem_Override = true,
                Force_Unlock_Mouse = true,
                Unhollowed_Modules_Folder = "interop"
            });
        }

        private void UniverseLog(string arg1, LogType arg2)
        {
            switch (arg2)
            {
                case LogType.Error:
                    PluginLog.LogError(arg1);
                    break;
                case LogType.Assert:
                    PluginLog.LogFatal(arg1);
                    break;
                case LogType.Warning:
                    PluginLog.LogWarning(arg1);
                    break;
                case LogType.Log:
                    PluginLog.LogInfo(arg1);
                    break;
                case LogType.Exception:
                    PluginLog.LogFatal(arg1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(arg2), arg2, null);
            }
        }

        private void OnInitialized()
        {
            UiBase = UniversalUI.RegisterUI(MyPluginInfo.PLUGIN_GUID, UiUpdate);
            UiBase.SetOnTop();
            EncounterPanel.IsShown = false;
        }

        private void UiUpdate()
        {
        }

        private void Initialize()
        {
            PluginBootstrap.Setup();
        }
    }
}