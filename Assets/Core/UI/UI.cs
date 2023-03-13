using TMPro;
using UnityEngine;

namespace Core.UI
{
    public class UI : MonoBehaviour
    {
        [field: SerializeField] public Transform BusinessListParent { get; private set; }
        [field: SerializeField] public TextMeshProUGUI Money { get; private set; }
    }
}