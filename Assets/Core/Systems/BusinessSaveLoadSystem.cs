using Core.Components;
using Core.Configs;
using Core.Services;
using Leopotam.EcsLite;

namespace Core.Systems
{
    public class BusinessSaveLoadSystem : IEcsInitSystem, IEcsPostRunSystem, IEcsDestroySystem
    {
        public BusinessSaveLoadSystem(SaveLoadService saveLoadService, SavePathsConfig paths)
        {
            _saveLoadService = saveLoadService;
            _paths = paths;
        }

        private readonly SaveLoadService _saveLoadService;
        private readonly SavePathsConfig _paths;
        private readonly float _saveInterval;

        public void Init(IEcsSystems systems)
        {
            if (!_saveLoadService.Exists(_paths.Business)) return;

            var world = systems.GetWorld();
            var filter = world
                .Filter<BusinessProgressComponent>()
                .Inc<BusinessUpgradesComponent>()
                .End();
            var progressPool = world.GetPool<BusinessProgressComponent>();
            var upgradesPool = world.GetPool<BusinessUpgradesComponent>();

            if (!_saveLoadService.TryLoad<SaveData>(_paths.Business, out var data)) return;

            int index = 0;
            foreach (int entity in filter)
            {
                ref var progress = ref progressPool.Get(entity);
                ref var upgrades = ref upgradesPool.Get(entity);

                var businessData = data.Businesses[index++];
                progress.Progress = businessData.ProgressComponent.Progress;
                upgrades.Level = businessData.UpgradesComponent.Level;
                upgrades.FirstUpgradeBought = businessData.UpgradesComponent.FirstUpgradeBought;
                upgrades.SecondUpgradeBought = businessData.UpgradesComponent.SecondUpgradeBought;

                if (index >= data.Businesses.Length) return;
            }
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
                {
                    SaveBusinessStates(systems);
                    return;
                }
            }
        }

        public void Destroy(IEcsSystems systems)
        {
            SaveBusinessStates(systems);
        }

        private void SaveBusinessStates(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var filter = world
                .Filter<BusinessProgressComponent>()
                .Inc<BusinessUpgradesComponent>()
                .End();
            var progressPool = world.GetPool<BusinessProgressComponent>();
            var upgradesPool = world.GetPool<BusinessUpgradesComponent>();

            var data = new SaveData
            {
                Businesses = new BusinessData[filter.GetEntitiesCount()]
            };

            int index = 0;
            foreach (int entity in filter)
            {
                ref var progress = ref progressPool.Get(entity);
                ref var upgrades = ref upgradesPool.Get(entity);

                var businessData = new BusinessData
                {
                    ProgressComponent = progress,
                    UpgradesComponent = upgrades
                };
                data.Businesses[index++] = businessData;
            }

            _saveLoadService.Save(_paths.Business, data);
        }

        private struct SaveData
        {
            public BusinessData[] Businesses;
        }

        private struct BusinessData
        {
            public BusinessProgressComponent ProgressComponent;
            public BusinessUpgradesComponent UpgradesComponent;
        }
    }
}