using UnityEngine;

namespace Core.Configs.Businesses
{
    [CreateAssetMenu(menuName = "Core/Business/Config")]
    public class BusinessConfig : ScriptableObject
    {
        public string Name;
        public float Delay;
        public int BasePrice;
        public int BaseIncome;
        public BusinessUpgradeConfig FirstUpgrade;
        public BusinessUpgradeConfig SecondUpgrade;
    }
}