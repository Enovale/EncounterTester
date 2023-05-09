using System.Linq;
using Il2CppSystem.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI;
using UniverseLib.UI.Panels;

namespace EncounterTester.UI
{
    public class EncounterPanel : PanelBase
    {
        public static EncounterPanel Instance;
        
        public static bool IsShown
        {
            get => Plugin.UiBase is { Enabled: true };
            set
            {
                if (Plugin.UiBase == null || !Plugin.UiBase?.RootObject || Plugin.UiBase.Enabled == value)
                    return;

                UniversalUI.SetUIActive(MyPluginInfo.PLUGIN_GUID, value);
            }
        }
        
        public override string Name { get; } = "Encounter Tester";

        public override int MinWidth { get; } = 800;

        public override int MinHeight { get; } = 400;
        
        public override Vector2 DefaultAnchorMin { get; }
        
        public override Vector2 DefaultAnchorMax { get; }

        private EncounterSelector _encounterSelector;
        private EncounterDetails _encounterDetails;
        
        public EncounterPanel(UIBase owner) : base(owner)
        {
            Instance = this;
        }
        
        protected override void OnClosePanelClicked()
        {
            IsShown = false;
        }

        protected override void ConstructPanelContent()
        {
            var group = UIFactory.CreateHorizontalGroup(ContentRoot, GetType().Name, false, false, true, true);
            UIFactory.SetLayoutElement(group, flexibleWidth: 9999, flexibleHeight: 9999);
            _encounterSelector = new();
            _encounterSelector.ConstructContent(group);
            _encounterDetails = new();
            _encounterDetails.ConstructUI(group);
            _encounterSelector.OnEncounterSelected += (data, i) => _encounterDetails.UpdateUI(EncounterHelper.RailwayEncounters[i]);
        }
    }
}