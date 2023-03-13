using Core.Common;
using Core.Configs;
using Core.Services;
using Core.Systems;
using Core.UI;
using Leopotam.EcsLite;
using UnityEngine;

namespace Core
{
    public class GameEntry : MonoBehaviour
    {
        [SerializeField] private GameConfig _config;
        [SerializeField] private GameResources _resources;
        [SerializeField] private SceneReferences _sceneReferences;
        
        private EcsWorld _world;
        private IEcsSystems _systems;

        private void Start()
        {
            Application.targetFrameRate = 60;
            
            var saveLoadService = new SaveLoadService(_config.SavePaths.Directory, _config.SavePaths.Extension);
            var playerMoneyService = new PlayerMoneyService();
            var businessFactory = new BusinessViewFactory(_resources.BusinessView);
            
            _world = new EcsWorld();
            _systems = new EcsSystems(_world);
            
            _systems
                .Add(new BusinessInitSystem(_config.Businesses))
                
                .Add(new BusinessSaveLoadSystem(saveLoadService, _config.SavePaths))
                .Add(new MoneySaveLoadSystem(saveLoadService, playerMoneyService, _config.SavePaths))
                
                .Add(new BusinessProgressUpdateSystem())
                .Add(new BusinessMoneyGatherer(playerMoneyService))
                
                .Add(new BusinessViewInitSystem(_sceneReferences.UI.BusinessListParent, businessFactory))
                .Add(new BusinessViewProgressUpdateSystem())
                .Add(new BusinessViewUpgradeRequestSystem())
                
                .Add(new BusinessUpgradeSystem(playerMoneyService))
                .Add(new BusinessViewUpgradesUpdateSystem())
                
                .Add(new MoneyViewUpdateSystem(playerMoneyService, _sceneReferences.UI.Money))
                .Add(new SaveCooldownSystem(_config.SaveInterval))
                .Init();
        }
    
        private void Update()
        {
            _systems?.Run();
        }

        private void OnDestroy() 
        {
            _systems?.Destroy();
            _systems = null;
            
            _world?.Destroy();
            _world = null;
        }
    }
}