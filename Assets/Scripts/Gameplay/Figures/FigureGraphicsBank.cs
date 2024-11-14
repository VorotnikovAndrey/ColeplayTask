using System;
using System.Collections.Generic;
using System.Linq;
using PlayVibe;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Gameplay.Figure
{
    [CreateAssetMenu(fileName = nameof(FigureGraphicsBank), menuName = "SO/FigureGraphicsBank")]
    public class FigureGraphicsBank : ScriptableObject
    {
        [SerializeField] private List<FigureGraphicsBankData> data;

        private Dictionary<FigureGraphicType, AssetReference> dataDictionary;

        public AssetReference GetAssetReference(FigureGraphicType type)
        {
            dataDictionary ??= data.ToDictionary(d => d.Type, d => d.Asset);
            
            if (dataDictionary.TryGetValue(type, out var asset))
            {
                return asset;
            }

            Debug.LogWarning($"FigureGraphicType '{type}' not found in FigureGraphicsBank.".AddColorTag(Color.red));
            
            return null;
        }

        [Serializable]
        public class FigureGraphicsBankData
        {
            public FigureGraphicType Type;
            public AssetReference Asset;
        }
    }
}