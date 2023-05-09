using System;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI;
using UniverseLib.UI.Models;
using UniverseLib.UI.Panels;

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
            
            var inputField = CreateInputFieldWithLabel(UIRoot, "idField", "ID", "ID");
            inputField.Component.contentType = InputField.ContentType.IntegerNumber;
            inputField.OnValueChanged += (val) => _data.id = int.Parse(val);
            _onUiUpdated += () => inputField.Text = _data.id.ToString();
        }

        public void UpdateUI(StageStaticData data)
        {
            _data = data;
            _onUiUpdated?.Invoke();
        }

        private InputFieldRef CreateInputFieldWithLabel(GameObject parent, string name, string placeholderText, string label)
        {
            var group = UIFactory.CreateHorizontalGroup(parent, name + "Group", false, false, true, true);
            var labelObj = UIFactory.CreateLabel(group, name + "Label", label);
            var inputField = UIFactory.CreateInputField(group, name, placeholderText);
            UIFactory.SetLayoutElement(inputField.GameObject, flexibleWidth: 9999, minHeight: 25, flexibleHeight: 0);
            return inputField;
        }
    }
}