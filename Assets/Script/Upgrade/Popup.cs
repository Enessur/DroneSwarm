using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Upgrade
{
    public class Popup : MonoBehaviour
    {
        [SerializeField] private Transform content;
        [SerializeField] private float duration = 0.5f;
        [SerializeField] private Ease ease;
        private bool _isOpen;
        private Vector3 _startScale;

        protected virtual void Start()
        {
            Init();
        }

        protected virtual void Init()
        {
            _startScale = content.lossyScale;
        }

        [Button]
        protected virtual void Open()
        {
            if (_isOpen)
            {
                return;
            }

            content.DOScale(_startScale, duration).SetEase(ease);
            _isOpen = true;
        }
        [Button]
        protected virtual void Close()
        {
            if (!_isOpen)
            {
                return;
            }

            content.DOScale(0, duration).SetEase(ease);
            _isOpen = false;
        }
    }
}