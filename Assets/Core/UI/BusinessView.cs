using System.Collections.Generic;
using Core.Common;
using Core.Configs.Businesses;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class BusinessView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _level;
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _income;
        [SerializeField] private Slider _progressBar;
        [SerializeField] private Button _levelUpgrade;
        [SerializeField] private TextMeshProUGUI _levelUpgradePrice;
        [SerializeField] private OneTimeUpgradeButton _firstOption;
        [SerializeField] private OneTimeUpgradeButton _secondOption;

        public OneTimeUpgradeButton FirstOption => _firstOption;
        public OneTimeUpgradeButton SecondOption => _secondOption;

        public BusinessUpgradeType UpgradeRequestedType { get; private set; }
        public bool UpgradeRequested { get; private set; }

        private readonly Dictionary<object, int> _cachedInts = new();

        private void Awake()
        {
            _levelUpgrade.onClick.AddListener(OnLevelUpgradeClick);
            _firstOption.Clicked += OnFirstOptionClick;
            _secondOption.Clicked += OnSecondOptionClick;
        }

        private void OnDestroy()
        {
            _levelUpgrade.onClick.RemoveListener(OnLevelUpgradeClick);
            _firstOption.Clicked -= OnFirstOptionClick;
            _secondOption.Clicked -= OnSecondOptionClick;
        }

        public void SetConfig(BusinessConfig config)
        {
            _name.text = config.Name;
            _firstOption.SetConfig(config.FirstUpgrade);
            _secondOption.SetConfig(config.SecondUpgrade);
        }

        public void SetProgress(float progress) => _progressBar.value = progress;
        public void SetUpgradePrice(int value)
        {
            if (IsCachedAndEqual(_levelUpgradePrice, value)) return;
            
            SetPrice(_levelUpgradePrice, value);
            _cachedInts[_levelUpgradePrice] = value;
        }

        public void SetIncome(int value)
        {
            if (IsCachedAndEqual(_income, value)) return;
            
            SetPrice(_income, value);
            _cachedInts[_income] = value;
        }

        public void SetLevel(int value)
        {
            if (IsCachedAndEqual(_level, value)) return;
            
            _level.text = value.ToString();
            _cachedInts[_level] = value;
        }

        public void ResetUpgradeRequest() => UpgradeRequested = false;

        public void SetUpgradePurchaseStatus(OneTimeUpgradeButton upgradeButton, bool status)
        {
            if(status)
                upgradeButton.ChangeToPurchasedState();
            else
                upgradeButton.ChangeToAvailableState();
        }

        private bool IsCachedAndEqual(object source, int value)
        {
            return _cachedInts.TryGetValue(source, out int cached) && cached == value;
        }

        private void SetPrice(TextMeshProUGUI target, int cost)
        {
            target.text = $"{cost}$";
        }
        
        private void OnLevelUpgradeClick()
        {
            UpgradeRequestedType = BusinessUpgradeType.Level;
            UpgradeRequested = true;
        }

        private void OnFirstOptionClick()
        {
            UpgradeRequestedType = BusinessUpgradeType.Option1;
            UpgradeRequested = true;
        }

        private void OnSecondOptionClick()
        {
            UpgradeRequestedType = BusinessUpgradeType.Option2;
            UpgradeRequested = true;
        }
    }
}