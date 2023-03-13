using System;
using UnityEngine.Serialization;

namespace Core.Configs
{
    [Serializable]
    public struct BusinessUpgradeConfig
    {
        public string Name;
        [FormerlySerializedAs("Price")] public int Cost;
        public float Multiplier;
    }
}