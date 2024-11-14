using System;
using System.Collections.Generic;
using System.Linq;
using PlayVibe;
using UnityEngine;

namespace Gameplay.Figure
{
    [CreateAssetMenu(fileName = nameof(FigureColorBank), menuName = "SO/FigureColorBank")]
    public class FigureColorBank : ScriptableObject
    {
        [SerializeField] private List<FigureColorData> data;
        
        private Dictionary<FigureColorType, Material> dataDictionary;

        public Material GetMaterial(FigureColorType type)
        {
            dataDictionary ??= data.ToDictionary(d => d.Type, d => d.Material);
            
            if (dataDictionary.TryGetValue(type, out var asset))
            {
                return asset;
            }

            Debug.LogWarning($"FigureColorType '{type}' not found in FigureColorBank.".AddColorTag(Color.red));
            
            return null;
        }
        
        [Serializable]
        public class FigureColorData
        {
            public FigureColorType Type;
            public Material Material;
        }
    }
}