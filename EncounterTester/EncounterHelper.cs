using System;
using System.Collections.Generic;
using System.Linq;
using EncounterTester.Data;
using EncounterTester.Patches;

namespace EncounterTester
{
    public static class EncounterHelper
    {
        public static List<StageStaticData> RailwayEncounters { get; }

        static EncounterHelper()
        {
            RailwayEncounters = StaticDataManager.Instance.RailwayDungeonBattleList.GetList().ToArray().ToList();
        }

        public static string GetEncounterName(int index)
            => GetEncounterName(RailwayEncounters[index]);

        public static string GetEncounterName(StageStaticData stage)
        {
            var enemyList = StaticDataManager.Instance.EnemyUnitList;
            var abList = StaticDataManager.Instance.AbnormalityUnitList;
            var name = stage.EventScriptName;
            if (name == null && stage.stageScriptNameAfterClear is not "AbnormalityEvent" and not "Normal")
                name = stage.stageScriptNameAfterClear;
            if (name == null)
            {
                var unit = stage.GetAllEnemyDataByWave(stage.GetMaxWaveCount() - 1).ToArray().MaxBy(e =>
                {
                    var id = e.GetID();
                    var data = stage.StageType == STAGE_BATTLE_TYPE.Abnormality
                        ? abList.GetData(id).Cast<UnitStaticData>()
                        : enemyList.GetData(id).Cast<UnitStaticData>();
                    return data.GetMaxHp(e.GetLevel());
                });
                name = TextDataManager.Instance.EnemyList.GetData(unit.GetID()).GetName();
            }

            return name;
        }

        public static void ExecuteEncounter(EncounterData encounter)
        {
            BanPrevention.SafeMode = true;
            var gm = GlobalGameManager.Instance;
            gm.CurrentStage.SetNodeIDs(-1, -1, -1, -1, encounter.ProgressType);
            gm.CurrentStage.SetCurrentStageType(encounter.StageType);
            Singleton<StageController>.Instance.InitStageModel(encounter.StageData, encounter.StageType);
            gm.LoadScene(SCENE_STATE.Battle, (Action)(() => gm.StartStage()));
        }
    }
}