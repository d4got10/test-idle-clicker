using UnityEngine;

namespace Core.Configs
{
    [CreateAssetMenu(menuName = "Core/Save Paths Config")]
    public class SavePathsConfig : ScriptableObject
    {
        public string Directory;
        public string Extension;
        public string Business;
        public string Money;
    }
}