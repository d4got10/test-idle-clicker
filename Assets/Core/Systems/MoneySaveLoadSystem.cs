using Core.Components;
using Core.Configs;
using Core.Services;
using Leopotam.EcsLite;

namespace Core.Systems
{
    public class MoneySaveLoadSystem : IEcsInitSystem, IEcsPostRunSystem, IEcsDestroySystem
    {
        public MoneySaveLoadSystem(SaveLoadService saveLoadService, PlayerMoneyService moneyService, SavePathsConfig paths)
        {
            _saveLoadService = saveLoadService;
            _moneyService = moneyService;
            _paths = paths;
        }

        private readonly SaveLoadService _saveLoadService;
        private readonly PlayerMoneyService _moneyService;
        private readonly SavePathsConfig _paths;

        public void Init(IEcsSystems systems)
        {
            if (!_saveLoadService.Exists(_paths.Money)) return;

            if(_saveLoadService.TryLoad<SaveData>(_paths.Money, out var data))
                _moneyService.Load(data.Money);
            
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
                    SaveMoneyData();
                    return;
                }
            }
        }

        public void Destroy(IEcsSystems systems)
        {
            SaveMoneyData();
        }

        private void SaveMoneyData()
        {
            _saveLoadService.Save(_paths.Money, new SaveData
            {
                Money = _moneyService.Amount
            });
        }

        private struct SaveData
        {
            public int Money;
        }
    }
}