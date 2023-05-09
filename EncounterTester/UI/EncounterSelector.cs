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

        public int ItemCount => EncounterHelper.RailwayEncounters.Count;

        public float DefaultHeight => 30;

        public void ConstructContent(GameObject contentRoot)
        {
            var root = UIFactory.CreateUIObject(GetType().Name, contentRoot);
            UIFactory.SetLayoutElement(root, flexibleWidth: 9999, flexibleHeight: 9999);
            UIFactory.SetLayoutGroup<VerticalLayoutGroup>(root, false, false, true, true);
            var pool = UIFactory.CreateScrollPool<EncounterCell>(root, "encounterScrollPool", out var scrollRoot, out var addContent);
            UIFactory.SetLayoutElement(scrollRoot, flexibleWidth:9999, flexibleHeight: 9999);
            pool.Initialize(this);
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
            cell.Label = EncounterHelper.GetEncounterName(index);
            cell.OnEdit += (d, i) => OnEncounterSelected?.Invoke(d, i);
            cell.OnExecute += (d, i) => EncounterHelper.ExecuteEncounter(new EncounterData()
            {
                StageData = EncounterHelper.RailwayEncounters[i],
                StageType = STAGE_TYPE.RAILWAY_DUNGEON,
                ProgressType = STAGE_PROGRESS_TYPE.RAILWAY_DUNGEON_AB_BATTLE
            });
        }
    }
}