using System;
using Core.Common;
using Core.Components;

namespace Core.Extensions
{
    public static class BusinessUpgradesExtensions
    {
        public static void Upgrade(ref this BusinessUpgradesComponent component, BusinessUpgradeType type)
        {
            switch (type)
            {
                case BusinessUpgradeType.Level:
                    component.Level++;
                    break;
                case BusinessUpgradeType.Option1:
                    component.FirstUpgradeBought = true;
                    break;
                case BusinessUpgradeType.Option2:
                    component.SecondUpgradeBought = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, "Unhandled upgrade type");
            }
        }

        public static bool UpgradeIsNotPurchased(ref this BusinessUpgradesComponent component, BusinessUpgradeType type)
        {
            return type switch
            {
                BusinessUpgradeType.Level => true,
                BusinessUpgradeType.Option1 => !component.FirstUpgradeBought,
                BusinessUpgradeType.Option2 => !component.SecondUpgradeBought,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unhandled upgrade type")
            };
        }
        
        public static int GetUpgradePrice(ref this BusinessUpgradesComponent component, 
            BusinessUpgradeType type,
            BusinessConfigComponent configComponent)
        {
            return type switch
            {
                BusinessUpgradeType.Level => (component.Level + 1) * configComponent.BaseCost,
                BusinessUpgradeType.Option1 => configComponent.FirstUpgrade.Cost,
                BusinessUpgradeType.Option2 => configComponent.SecondUpgrade.Cost,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unhandled upgrade type")
            };
        }
        
        public static int GetTotalIncome(ref this BusinessUpgradesComponent upgrades, BusinessConfigComponent config)
        {
            var multiplier = 1f
                             + (upgrades.FirstUpgradeBought ? config.FirstUpgrade.Multiplier : 0f)
                             + (upgrades.SecondUpgradeBought ? config.SecondUpgrade.Multiplier : 0f);
            
            return (int)(upgrades.Level * config.BaseIncome * multiplier);
        }
    }
}