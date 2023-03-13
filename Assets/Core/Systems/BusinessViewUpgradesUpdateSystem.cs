using Core.Common;
using Core.Components;
using Core.Extensions;
using Leopotam.EcsLite;

namespace Core.Systems
{
    public sealed class BusinessViewUpgradesUpdateSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var filter = world
                .Filter<BusinessViewComponent>()
                .Inc<BusinessUpgradesComponent>()
                .Inc<BusinessConfigComponent>()
                .End();
            var upgradesPool = world.GetPool<BusinessUpgradesComponent>();
            var viewPool = world.GetPool<BusinessViewComponent>();
            var configPool = world.GetPool<BusinessConfigComponent>();
        
            foreach (int entity in filter)
            {
                ref var upgradesComponent = ref upgradesPool.Get(entity);
                ref var viewComponent = ref viewPool.Get(entity);
                ref var config = ref configPool.Get(entity);

                var view = viewComponent.View;
                view.SetLevel(upgradesComponent.Level);
                view.SetIncome(upgradesComponent.GetTotalIncome(config));
                view.SetUpgradePrice(upgradesComponent.GetUpgradePrice(BusinessUpgradeType.Level, config));
                view.SetUpgradePurchaseStatus(view.FirstOption, upgradesComponent.FirstUpgradeBought);
                view.SetUpgradePurchaseStatus(view.SecondOption, upgradesComponent.SecondUpgradeBought);
            }
        }
    }
}