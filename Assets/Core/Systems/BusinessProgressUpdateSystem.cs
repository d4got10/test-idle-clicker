using Core.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Core.Systems
{
    public sealed class BusinessProgressUpdateSystem : IEcsRunSystem
    {   
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var filter = world
                .Filter<BusinessProgressComponent>()
                .Inc<BusinessConfigComponent>()
                .Inc<BusinessUpgradesComponent>()
                .End();

            var progressPool = world.GetPool<BusinessProgressComponent>();
            var configPool = world.GetPool<BusinessConfigComponent>();
            var upgradesPool = world.GetPool<BusinessUpgradesComponent>();
            
            foreach (int entity in filter)
            {
                ref var data = ref progressPool.Get(entity);
                ref var config = ref configPool.Get(entity);
                ref var upgrades = ref upgradesPool.Get(entity);
                
                if(upgrades.Level == 0) continue;
                
                data.Progress += Time.deltaTime / config.Delay;
            }
        }
    }
}