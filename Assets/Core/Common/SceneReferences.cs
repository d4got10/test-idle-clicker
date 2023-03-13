using UnityEngine;

namespace Core.Common
{
    public class SceneReferences : MonoBehaviour
    {
        [field: SerializeField] public UI.UI UI { get; private set; }
    }
}