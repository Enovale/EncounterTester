using System.Linq;
using UnityEngine;
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
            var vertGroup = UIFactory.CreateVerticalGroup(ContentRoot, GetType().Name, false, false, true, true);
            UIFactory.SetLayoutElement(vertGroup, flexibleWidth: 9999, flexibleHeight: 9999);

            var selectGroup = UIFactory.CreateHorizontalGroup(vertGroup, "selectGroup", false, true, true, false);
            UIFactory.SetLayoutElement(selectGroup, flexibleWidth: 9999, minHeight: 28, flexibleHeight: 0);
            var dropdownObj = UIFactory.CreateDropdown(selectGroup, "encounterListDropdown", out var dropdown, "ERROR",
                12,
                i =>
                {
                    _encounterSelector.UpdateCells(EncounterHelper.EncounterLists[i].Data);
                }, EncounterHelper.EncounterLists.Select(e => e.Name).ToArray());
            UIFactory.SetLayoutElement(dropdownObj, flexibleWidth: 9999, flexibleHeight: 9999);
            
            var group = UIFactory.CreateHorizontalGroup(vertGroup, "horizontalGroup", false, false, true, true);
            UIFactory.SetLayoutElement(group, flexibleWidth: 9999, flexibleHeight: 9999);
            _encounterSelector = new();
            _encounterSelector.ConstructContent(group);
            _encounterDetails = new();
            _encounterDetails.ConstructUI(group);
            _encounterSelector.UpdateCells(EncounterHelper.EncounterLists.First().Data);
            _encounterSelector.OnEncounterSelected += (data, i) => _encounterDetails.UpdateUI(_encounterSelector.EncounterList[i]);
        }
    }
}