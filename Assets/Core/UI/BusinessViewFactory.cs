using UnityEngine;

namespace Core.UI
{
    public class BusinessViewFactory
    {
        public BusinessViewFactory(BusinessView prefab)
        {
            _prefab = prefab;
        }

        private readonly BusinessView _prefab;

        public BusinessView Create(Transform parent) => Object.Instantiate(_prefab, parent);
    }
}