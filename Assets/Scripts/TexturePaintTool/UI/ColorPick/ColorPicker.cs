using System;
using UnityEngine;
using UnityEngine.UI;

namespace TexturePaintTool.UI.ColorPick
{
    public class ColorPicker : MonoBehaviour
    {
        public event Action<Color> OnColorChanged;

        [SerializeField] private Image colorDisplayer;
        [SerializeField] private Slider redParameter;
        [SerializeField] private Slider greenParameter;
        [SerializeField] private Slider blueParameter;

        private void Start()
        {
            redParameter.onValueChanged.AddListener(ColorChanged);
            greenParameter.onValueChanged.AddListener(ColorChanged);
            blueParameter.onValueChanged.AddListener(ColorChanged);
            
            ColorChanged();
        }

        private void ColorChanged(float unUsed = 0)
        {
            Color newColor = Color.white;

            newColor.r = redParameter.value;
            newColor.g = greenParameter.value;
            newColor.b = blueParameter.value;

            colorDisplayer.color = newColor;
            OnColorChanged?.Invoke(newColor);
        }
    }
}
