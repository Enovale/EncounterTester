using System;

namespace EncounterTester.Data
{
    [Serializable]
    public class EncounterData
    {
        public StageStaticData StageData { get; set; }
        public STAGE_TYPE StageType { get; set; }
        public STAGE_PROGRESS_TYPE ProgressType { get; set; }
    }
}