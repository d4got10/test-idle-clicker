using Core.Components;
using Core.Extensions;
using Core.Services;
using Leopotam.EcsLite;

namespace Core.Systems
{
    public sealed class BusinessMoneyGatherer : IEcsRunSystem
    {
        public BusinessMoneyGatherer(PlayerMoneyService playerMoneyService)
        {
            _playerMoneyService = playerMoneyService;
        }

        private readonly PlayerMoneyService _playerMoneyService;

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var filter = world
                .Filter<BusinessProgressComponent>()
                .Inc<BusinessConfigComponent>()
                .Inc<BusinessUpgradesComponent>()
                .End();

            var progressPool = world.GetPool<BusinessProgressComponent>();
            var upgradesPool = world.GetPool<BusinessUpgradesComponent>();
            var configPool = world.GetPool<BusinessConfigComponent>();

            foreach (int entity in filter)
            {
                ref var data = ref progressPool.Get(entity);
                if (data.Progress >= 1)
                {
                    data.Progress = 0;
                    var upgrades = upgradesPool.Get(entity);
                    var config = configPool.Get(entity);
                    _playerMoneyService.Add(upgrades.GetTotalIncome(config));
                }
            }
        }
    }
}