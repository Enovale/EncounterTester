using System;
using System.IO;
using System.Text.Json;
using Dungeon;
using EncounterTester.Data;
using EncounterTester.UI;
using Il2CppInterop.Runtime.Injection;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace EncounterTester
{
    public class PluginBootstrap : MonoBehaviour
    {
        public static PluginBootstrap Instance;

        internal static void Setup()
        {
            ClassInjector.RegisterTypeInIl2Cpp<PluginBootstrap>();

            GameObject obj = new(MyPluginInfo.PLUGIN_GUID + "bootstrap");
            DontDestroyOnLoad(obj);
            obj.hideFlags |= HideFlags.HideAndDontSave;
            Instance = obj.AddComponent<PluginBootstrap>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F10) && StaticDataManager.Instance is {IsDataLoaded:true})
            {
                EncounterPanel.Instance ??= new EncounterPanel(Plugin.UiBase);
                EncounterPanel.IsShown = !EncounterPanel.IsShown;
            }

            if (Input.GetKeyDown(KeyCode.F11))
            {
                //var sd = Singleton<StaticDataManager>.Instance.GetDungeonStage(99999, true, DUNGEON_TYPES.STORY_DUNGEON);

                var sd = new StageStaticData()
                {
                    id = 99999,
                    _stageType = STAGE_BATTLE_TYPE.Abnormality,
                    stageLevel = 25,
                    recommendedLevel = 25,
                    dangerLevel = 1,
                    staminaCost = 0,
                    participantInfo = new ParticipantsInfo()
                    {
                        min = 5,
                        max = 5
                    },
                    waveList = new List<Wave>(),
                    turnLimit = 99,
                    rewardList = new List<Reward>(),
                    questlist = new List<QuestCondition>(),
                    stageScriptNameAfterClear = "Eunbong_Strong",
                    abnormalityEventList = new List<int>(),
                };
                var quest = new QuestCondition()
                {
                    localizeId = 1,
                    args = new List<string>()
                };
                quest.args.Add("10");
                sd.questlist.Add(quest);

                var firstWave = new Wave()
                {
                    battleMapInfo = new BattleMapInfo()
                    {
                        mapName = "LCorpElevatorBlue"
                    },
                    unitList = new List<EnemyData>(),
                    enemyPositionID = 28,
                    bgmList = new List<string>()
                };
                firstWave.bgmList.Add("Battle_Cp2_Ally_3");
                firstWave.bgmList.Add("Battle_Cp3_5_Enemy_2");
                firstWave.unitList.Add(new EnemyData()
                {
                    unitID = 8035,
                    unitCount = 1,
                    unitLevel = 25
                });
                firstWave.unitList.Add(new EnemyData()
                {
                    unitID = 8039,
                    unitCount = 1,
                    unitLevel = 25
                });
                sd.waveList.Add(firstWave);
                for (var i = 0; i < 13; i++)
                {
                    var wave = new Wave()
                    {
                        unitList = new List<EnemyData>(),
                        enemyPositionID = 28,
                    };
                    wave.unitList.Add(new EnemyData()
                    {
                        unitID = 8040,
                        unitCount = 1,
                        unitLevel = 25
                    });
                    wave.unitList.Add(new EnemyData()
                    {
                        unitID = 8035,
                        unitCount = 1,
                        unitLevel = 25
                    });
                    sd.waveList.Add(wave);
                }
                sd.SetEnemyData();
                
                EncounterHelper.ExecuteEncounter(new EncounterData()
                {
                    StageData = sd,
                    StageType = STAGE_TYPE.HELLS_CHICKEN_DUNGEON,
                    ProgressType = STAGE_PROGRESS_TYPE.STORY_DUNGEON_BATTLE
                });
            }
        }
    }
}