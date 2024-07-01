using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace TexturePaintTool.UI
{
    public class BrushesPicker : MonoBehaviour
    {
        public event Action<Brush> OnBrushSelected;
        public event Action<float> OnScaleChanged; 
        
        [SerializeField] private List<Brush> brushes;
        [SerializeField] private RectTransform brushesContainer;
        [SerializeField] private Slider scale;
        [SerializeField] private BrushUIElement brushPrefab;

        private List<BrushUIElement> _brushUIElements = new();
        private float _selectedScale;

        private void Awake()
        {
            AddBrushes();
            
            scale.onValueChanged.AddListener(ScaleChanged);
        }

        private void Start()
        {
            if (_brushUIElements.Count > 0)
            {
                _brushUIElements[0].SelectBrush();
            }
            ScaleChanged(scale.value);
        }

        private void AddBrushes()
        {
            foreach (var brush in brushes)
            {
                BrushUIElement newUIElement = Instantiate(brushPrefab, brushesContainer, true);
                newUIElement.StoreNewBrush(brush);
                newUIElement.transform.localScale = Vector3.one;
                newUIElement.OnBrushSelected += BrushSelected;
                
                _brushUIElements.Add(newUIElement);
            }
        }

        private void ScaleChanged(float newScale)
        {
            OnScaleChanged?.Invoke(newScale);
        }
        
        private void BrushSelected(Brush brush)
        {
            foreach (var brushUIElement in _brushUIElements)
            {
                brushUIElement.SetSelected(brush);
            }
            
            OnBrushSelected?.Invoke(brush);
        }
    }
}
