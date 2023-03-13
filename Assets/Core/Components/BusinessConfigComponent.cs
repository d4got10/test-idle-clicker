using Core.Configs;
using Core.Configs.Businesses;

namespace Core.Components
{
    public struct BusinessConfigComponent
    {
        public BusinessConfig Config;
        
        public string Name => Config.Name;
        public float Delay => Config.Delay;
        public int BaseCost => Config.BasePrice;
        public int BaseIncome => Config.BaseIncome;
        public BusinessUpgradeConfig FirstUpgrade => Config.FirstUpgrade;
        public BusinessUpgradeConfig SecondUpgrade => Config.SecondUpgrade;
    }
}