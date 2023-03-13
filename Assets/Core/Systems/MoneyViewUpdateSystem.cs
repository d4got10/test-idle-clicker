using Core.Services;
using Leopotam.EcsLite;
using TMPro;

namespace Core.Systems
{
    public class MoneyViewUpdateSystem : IEcsRunSystem
    {
        public MoneyViewUpdateSystem(PlayerMoneyService playerMoneyService, TextMeshProUGUI view)
        {
            _playerMoneyService = playerMoneyService;
            _view = view;
        }

        private readonly PlayerMoneyService _playerMoneyService;
        private readonly TextMeshProUGUI _view;

        public void Run(IEcsSystems systems)
        {
            _view.text = $"{_playerMoneyService.Amount}$";
        }
    }
}