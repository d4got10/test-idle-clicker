using Core.Components;
using Leopotam.EcsLite;

namespace Core.Systems
{
    public sealed class BusinessViewProgressUpdateSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var filter = world.Filter<BusinessViewComponent>().Inc<BusinessProgressComponent>().End();
            var progressPool = world.GetPool<BusinessProgressComponent>();
            var viewPool = world.GetPool<BusinessViewComponent>();
        
            foreach (int entity in filter)
            {
                ref var progressComponent = ref progressPool.Get(entity);
                ref var viewComponent = ref viewPool.Get(entity);

                viewComponent.View.SetProgress(progressComponent.Progress);
            }
        }
    }
}