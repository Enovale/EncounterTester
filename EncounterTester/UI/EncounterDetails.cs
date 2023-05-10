using System;
using Il2CppSystem.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI;
using UniverseLib.UI.Models;

namespace EncounterTester.UI
{
    public class EncounterDetails : UIModel
    {
        private GameObject _uiRoot;

        public override GameObject UIRoot => _uiRoot;

        private StageStaticData _data;

        private UiUpdatedDelegate _onUiUpdated;

        private delegate void UiUpdatedDelegate();

        public override void ConstructUI(GameObject parent)
        {
            UIFactory.CreateScrollView(parent, GetType().Name + "scrollView", out _uiRoot, out var scrollBar);
            UIFactory.SetLayoutElement(UIRoot, flexibleWidth: 9999, flexibleHeight: 9999);

            UIFactory.CreateLabel(UIRoot, "titleLabel", "Encounter Details:");
            
            var inputField = CreateInputFieldWithLabel(UIRoot, "idField", "ID", "ID", InputField.ContentType.IntegerNumber);
            inputField.OnValueChanged += (val) => _data.id = int.Parse(val);
            _onUiUpdated += () => inputField.Text = _data.id.ToString();
            
            var inputField1 = CreateInputFieldWithLabel(UIRoot, "stageScriptAfterClearField", "Stage Script After Clear", "Stage Script After Clear", InputField.ContentType.Alphanumeric);
            inputField1.OnValueChanged += (val) => _data.stageScriptNameAfterClear = val;
            _onUiUpdated += () => inputField1.Text = _data.stageScriptNameAfterClear;

            var stageType = UIFactory.CreateDropdown(UIRoot, "stageTypeDropdown", out var dropdown, "Stage Type", 14,
                i => _data._stageType = (STAGE_BATTLE_TYPE)i);
            var battleTypes = new List<string>();
            foreach (var type in Enum.GetValues<STAGE_BATTLE_TYPE>())
            {
                battleTypes.Add(type.ToString());
            }
            dropdown.AddOptions(battleTypes);
            UIFactory.SetLayoutElement(stageType.gameObject, flexibleWidth: 50, minHeight: 25, flexibleHeight: 0);
            _onUiUpdated += () => dropdown.value = (int)_data._stageType;
        }

        public void UpdateUI(StageStaticData data)
        {
            _data = data;
            _onUiUpdated?.Invoke();
        }

        private InputFieldRef CreateInputFieldWithLabel(GameObject parent, string name, string placeholderText, string label, InputField.ContentType contentType = InputField.ContentType.Standard)
        {
            var group = UIFactory.CreateHorizontalGroup(parent, name + "Group", false, false, true, true);
            UIFactory.SetLayoutElement(group.gameObject, flexibleWidth: 80, minHeight: 25, flexibleHeight: 0);
            var labelObj = UIFactory.CreateLabel(group, name + "Label", label);
            labelObj.alignment = TextAnchor.MiddleCenter;
            UIFactory.SetLayoutElement(labelObj.gameObject, flexibleWidth: 30, minHeight: 25, flexibleHeight: 0);
            var inputField = UIFactory.CreateInputField(group, name, placeholderText);
            UIFactory.SetLayoutElement(inputField.GameObject, flexibleWidth: 50, minHeight: 25, flexibleHeight: 0);

            inputField.Component.contentType = contentType;
            return inputField;
        }
    }
}