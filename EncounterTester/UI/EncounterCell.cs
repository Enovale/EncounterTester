using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI;
using UniverseLib.UI.Widgets.ScrollView;

namespace EncounterTester.UI
{
    public class EncounterCell : ICell
    {
        public bool Enabled => UIRoot.activeSelf;
        
        public RectTransform Rect { get; set; }

        public GameObject UIRoot { get; set; }
        
        public int Index { get; set; }
        
        public string Label
        {
            get => _label.text;
            set => _label.text = value;
        }

        public EncounterSelector.EncounterSelectedDelegate OnEdit;
        
        public EncounterSelector.EncounterSelectedDelegate OnExecute;

        public float DefaultHeight => 30;

        private Text _label;

        public GameObject CreateContent(GameObject parent)
        {
            UIRoot = UIFactory.CreateUIObject(GetType().Name, parent, new Vector2(100, DefaultHeight));
            Rect = UIRoot.GetComponent<RectTransform>();
            UIFactory.SetLayoutGroup<HorizontalLayoutGroup>(UIRoot, false, false, true, true, 5, childAlignment: TextAnchor.UpperLeft);
            UIFactory.SetLayoutElement(UIRoot, minWidth: 100, flexibleWidth: 9999, minHeight: 30, flexibleHeight: 600);
            UIRoot.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            _label = UIFactory.CreateLabel(UIRoot, "CellLabel", "NOT SET");
            UIFactory.SetLayoutElement(_label.gameObject, minHeight: 25, flexibleWidth: 9999);
            
            var runBtn = UIFactory.CreateButton(UIRoot, "runButton", "Run", new Color(0.33f, 0.32f, 0.31f));
            UIFactory.SetLayoutElement(runBtn.Component.gameObject, minHeight: 25, minWidth: 45);
            runBtn.OnClick += () => OnExecute?.Invoke(this, Index);

            var editBtn = UIFactory.CreateButton(UIRoot, "editButton", "Edit", new Color(0.2f, 0.25f, 0.2f));
            UIFactory.SetLayoutElement(editBtn.Component.gameObject, minHeight: 25, minWidth: 60);
            editBtn.OnClick += () => OnEdit?.Invoke(this, Index);

            return UIRoot;
        }

        public void Enable()
        {
            this.UIRoot.SetActive(true);
        }

        public void Disable()
        {
            this.UIRoot.SetActive(false);
        }
    }
}