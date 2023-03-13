using System.Collections.Generic;
using Core.Configs.Businesses;
using UnityEngine;

namespace Core.Configs
{
    [CreateAssetMenu(menuName = "Core/Game Config")]
    public class GameConfig : ScriptableObject
    {
        public List<BusinessConfig> Businesses;
        public SavePathsConfig SavePaths;
        public float SaveInterval;
    }
}