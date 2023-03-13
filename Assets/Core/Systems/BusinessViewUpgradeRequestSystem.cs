using Core.Components;
using Leopotam.EcsLite;

namespace Core.Systems
{
    public sealed class BusinessViewUpgradeRequestSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var filter = world.Filter<BusinessViewComponent>().End();
            var viewPool = world.GetPool<BusinessViewComponent>();
            var upgradeRequestPool = world.GetPool<BusinessUpgradeRequest>();
            
            foreach (int entity in filter)
            {
                ref var viewComponent = ref viewPool.Get(entity);
                var view = viewComponent.View;
                if (view.UpgradeRequested)
                {
                    ref var request = ref upgradeRequestPool.Add(entity);
                    request.Type = view.UpgradeRequestedType;
                    view.ResetUpgradeRequest();
                }
            }
        }
    }
}