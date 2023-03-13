using System;
using Core.Configs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class OneTimeUpgradeButton : MonoBehaviour
    {
        public event Action Clicked;
        
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _price;
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _profit;
        [SerializeField] private ContentToggle _purchasedToggle;

        private void Awake() => _button.onClick.AddListener(OnClick);
        private void OnDestroy() => _button.onClick.RemoveListener(OnClick);

        public void SetConfig(BusinessUpgradeConfig config)
        {
            _name.text = config.Name;
            _price.text = $"{config.Cost}$";
            
            int multiplierPercents = (int)(100 * config.Multiplier);
            _profit.text = $"+ {multiplierPercents}%";
        }
        
        public void ChangeToPurchasedState() => _purchasedToggle.SetOn();
        public void ChangeToAvailableState() => _purchasedToggle.SetOff();

        private void OnClick() => Clicked?.Invoke();
    }
}