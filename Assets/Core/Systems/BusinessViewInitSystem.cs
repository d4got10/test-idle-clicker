using System.Collections.Generic;
using Core.Components;
using Core.UI;
using Leopotam.EcsLite;
using UnityEngine;

namespace Core.Systems
{
    public sealed class BusinessViewInitSystem : IEcsInitSystem
    {
        public BusinessViewInitSystem(Transform businessListParent, BusinessViewFactory factory)
        {
            _businessListParent = businessListParent;
            _factory = factory;
        }

        private readonly Transform _businessListParent;
        private readonly BusinessViewFactory _factory;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var filter = world.Filter<BusinessConfigComponent>().Exc<BusinessViewComponent>().End();
            var businessDataPool = world.GetPool<BusinessConfigComponent>();
            var businessViewPool = world.GetPool<BusinessViewComponent>();
        
            foreach (int entity in filter)
            {
                ref var configComponent = ref businessDataPool.Get(entity);
                ref var viewComponent = ref businessViewPool.Add(entity);
                
                var view = _factory.Create(_businessListParent);
                viewComponent.View = view;
                
                view.SetConfig(configComponent.Config);
            }
        }
    }
}