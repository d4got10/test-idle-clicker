using Core.UI;
using UnityEngine;

namespace Core.Common
{
    [CreateAssetMenu(menuName = "Core/New Game Resources")]
    public class GameResources : ScriptableObject
    {
        [field: SerializeField] public BusinessView BusinessView { get; private set; }
    }
}