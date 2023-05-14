using System.Collections.Generic;
using System.Collections.ObjectModel;
using EncounterTester.Data;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI;
using UniverseLib.UI.Widgets.ScrollView;

namespace EncounterTester.UI
{
    public class EncounterSelector : ICellPoolDataSource<EncounterCell>
    {
        public event EncounterSelectedDelegate OnEncounterSelected;
        
        public delegate void EncounterSelectedDelegate(EncounterCell data, int index);

        private List<EncounterData> _encounterList = new();

        public ReadOnlyCollection<EncounterData> EncounterList
            => _encounterList.AsReadOnly();

        public int ItemCount => _encounterList.Count;

        public float DefaultHeight => 30;

        private ScrollPool<EncounterCell> _pool;

        public void ConstructContent(GameObject contentRoot)
        {
            var root = UIFactory.CreateUIObject(GetType().Name, contentRoot);
            UIFactory.SetLayoutElement(root, flexibleWidth: 9999, flexibleHeight: 9999);
            UIFactory.SetLayoutGroup<VerticalLayoutGroup>(root, false, false, true, true);
            _pool = UIFactory.CreateScrollPool<EncounterCell>(root, "encounterScrollPool", out var scrollRoot, out var addContent);
            UIFactory.SetLayoutElement(scrollRoot, flexibleWidth:9999, flexibleHeight: 9999);
            _pool.Initialize(this);
        }

        public void UpdateCells(List<EncounterData> staticDataList)
        {
            _encounterList = staticDataList;
            _pool.Refresh(true, true);
        }

        public void OnCellBorrowed(EncounterCell cell) { }

        public void SetCell(EncounterCell cell, int index)
        {
            if (index >= ItemCount)
            {
                cell.Disable();
                return;
            }

            cell.Index = index;
            cell.Label = EncounterHelper.GetEncounterName(_encounterList, index);
            cell.OnEdit = (d, i) => OnEncounterSelected?.Invoke(d, i);
            cell.OnExecute = (d, i) => EncounterHelper.ExecuteEncounter(_encounterList[i]);
        }
    }
}