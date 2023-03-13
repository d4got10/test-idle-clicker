using Core.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Core.Systems
{
    public class SaveCooldownSystem : IEcsInitSystem, IEcsPostRunSystem
    {
        public SaveCooldownSystem(float saveInterval)
        {
            _saveInterval = saveInterval;
        }

        private readonly float _saveInterval;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var cooldownPool = world.GetPool<BusinessSaveCooldownComponent>();

            int entity = world.NewEntity();
            ref var cooldownComponent = ref cooldownPool.Add(entity);
            cooldownComponent.RemainingTime = _saveInterval;
        }

        public void PostRun(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var filter = world.Filter<BusinessSaveCooldownComponent>().End();
            var cooldownPool = world.GetPool<BusinessSaveCooldownComponent>();

            foreach (int entity in filter)
            {
                ref var cooldownComponent = ref cooldownPool.Get(entity);

                if (cooldownComponent.RemainingTime <= 0) 
                    cooldownComponent.RemainingTime = _saveInterval;
                
                cooldownComponent.RemainingTime -= Time.deltaTime;
            }
        }
    }
}