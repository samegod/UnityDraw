using System;
using UnityEngine;
using UnityEngine.UI;

namespace TexturePaintTool.UI
{
    public class BrushUIElement : MonoBehaviour
    {
        public event Action<Brush> OnBrushSelected; 
        
        [SerializeField] private RawImage icon;
        [SerializeField] private Image background;
        [SerializeField] private Text name;
        [SerializeField] private Color defaultColor;
        [SerializeField] private Color selectedColor;

        private Brush _storedBrush;

        public void StoreNewBrush(Brush newBrush)
        {
            _storedBrush = newBrush;
            icon.texture = _storedBrush.Texture;
            name.text = _storedBrush.DisplayedName;
        }

        public void SetSelected(Brush selectedBrush)
        {
            background.color = _storedBrush == selectedBrush ? selectedColor : defaultColor;
        }

        public void SelectBrush()
        {
            OnBrushSelected?.Invoke(_storedBrush);
        }
    }
}