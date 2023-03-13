using System.Collections.Generic;
using Core.Components;
using Core.Configs.Businesses;
using Leopotam.EcsLite;

namespace Core.Systems
{
    public sealed class BusinessInitSystem : IEcsInitSystem
    {
        public BusinessInitSystem(IEnumerable<BusinessConfig> businesses)
        {
            _businesses = businesses;
        }
        
        private readonly IEnumerable<BusinessConfig> _businesses;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var progressPool = world.GetPool<BusinessProgressComponent>();
            var configPool = world.GetPool<BusinessConfigComponent>();
            var upgradesPool = world.GetPool<BusinessUpgradesComponent>();

            bool first = true;
            foreach (BusinessConfig businessConfig in _businesses)
            {
                int entity = world.NewEntity();
                progressPool.Add(entity);
                ref var configComponent = ref configPool.Add(entity);
                configComponent.Config = businessConfig;

                ref var upgradesComponent = ref upgradesPool.Add(entity);
                upgradesComponent.Level = first ? 1 : 0;
                first = false;
            }
        }
    }
}