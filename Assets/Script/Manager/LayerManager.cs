using System;
using System.Collections.Generic;
using Script;
using UnityEngine;

namespace Manager
{
    [Serializable]
    public class LayerGroup
    {
        public LayerManager.LayerType layerType;
        public int index;
    }
    public class LayerManager : Singleton<LayerManager>
    {
        public enum LayerType
        {
            Player,
            Enemy,
            Obstacle
        }

        [SerializeField] private List<LayerType> layerGroups;
        private Dictionary<LayerType, LayerGroup> _layerDic;

        protected override void Awake()
        {
            base.Awake();
            Init();
        }

        private void Init()
        {
            foreach (var group in layerGroups)
            {
                //_layerDic.Add(group.layerType, group);
            }
        }

        private int GetLayerMaskIndex(string str)
        {
            return LayerMask.NameToLayer(str);
        }
        
        public int GetLayerIndex(LayerType layerType)
        {
            return _layerDic[layerType].index;
        }
    }
}
