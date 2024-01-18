using System;
using UnityEngine;

namespace KillerDoors.StaticDataSpace
{
    [CreateAssetMenu(fileName = "GameStaticData", menuName = "Static Data/Game")]
    public class GameStaticData : ScriptableObject
    {
        public Prices prices;
        public UpgradeDoor upgradeDoor;
        public SpawnPerson spawnPerson;
        public SpotAppearanceView spotAppearanceViewData;
        public Ads ads;
        public FingerView fingerViewAnimationData;

        public float[] coinUpgradeChancesOnLevels = new float[] { 50f, 50f, 40f, 40f, 30f, 30f, 25f, 25f, 20f, 20f, 20f, 15f };

        public float[] moveSpeedsPersons = new float[3] { 20f, 30f, 40f };

        public float doorGameDuration = 45f;

        [Serializable]
        public class Prices
        {
            public int[] protectionPrices = new int[5] { 6, 25, 100, 400, 1600 };
            public int[] dinamitPrices = new int[5] { 4, 16, 64, 256, 1024 };
        }
        [Serializable]
        public class UpgradeDoor
        {
            public float openTimeDoorUpgradeStep = 0.02f;
            public int maxDoorUpgrade = 24;
        }
        [Serializable]
        public class SpawnPerson
        {
            public float[] floorOpenOnSecond = new float[] { 5f, 10f, 30f };
            public int[] spawnChancesSinceLowSpeedPerson = new int[] { 10, 60, 30 };
            public float minTimeBetweenSwapn = 1.5f;
            public float maxTimeBetweenSwapn = 4f;
        }
        [Serializable]
        public class SpotAppearanceView
        {
            public Vector3 offsetEaningPointsWorld = new Vector3(-7f, 15f, 0f);
            public Vector3 offsetMergeResult = new Vector3(-40f, 100f, 0f);
            public float retentionView = 0.6f;
        }
        [Serializable]
        public class Ads
        {
            public int protectionCountAdsReward = 2;
            public int dinamitCountAdsReward = 2;
            public float timeValueAdsReward = 0.7f;
            public int multiplierAdsReward = 3;
            public bool AdsEnabled = true;
        }
        [Serializable]
        public class FingerView
        {
            public float dragSpeed = 120f;
            public float scaleSpeed = 2f;
            public float onDownScaleCoefficient = 0.8f;
            public float delayBetweenAnimationParts = 0.5f;
        }
    }
}