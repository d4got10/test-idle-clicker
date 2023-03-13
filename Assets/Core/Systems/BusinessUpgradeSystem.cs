using System;
using Core.Components;
using Core.Extensions;
using Core.Services;
using Leopotam.EcsLite;

namespace Core.Systems
{
    public class BusinessUpgradeSystem : IEcsRunSystem
    {
        public BusinessUpgradeSystem(PlayerMoneyService moneyService)
        {
            _moneyService = moneyService;
        }

        private readonly PlayerMoneyService _moneyService;

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var filter = world
                .Filter<BusinessUpgradeRequest>()
                .Inc<BusinessUpgradesComponent>()
                .Inc<BusinessConfigComponent>()
                .End();
            var requestPool = world.GetPool<BusinessUpgradeRequest>();
            var upgradesPool = world.GetPool<BusinessUpgradesComponent>();
            var configsPool = world.GetPool<BusinessConfigComponent>();

            foreach (int entity in filter)
            {
                ref var request = ref requestPool.Get(entity);
                ref var upgrades = ref upgradesPool.Get(entity);

                var upgradeType = request.Type;
                requestPool.Del(entity);

                if (!upgrades.UpgradeIsNotPurchased(upgradeType)) continue;
                
                ref var configComponent = ref configsPool.Get(entity);
                int price = upgrades.GetUpgradePrice(upgradeType, configComponent);
                
                if (!_moneyService.HasEnough(price)) continue;
                
                _moneyService.Spend(price);
                upgrades.Upgrade(upgradeType);
            }
        }
    }
}