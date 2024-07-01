using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TexturePaintTool.UI.ColorPick
{
    public class ColorWheel : MonoBehaviour
    {
        public event Action<float, float> OnNewColorPicked; 
        
        [SerializeField] private RawImage colorWheelImage;
        [SerializeField] private int wheelBufferInPixels;
        [SerializeField] private Image point;

        private RectTransform _pointTransform;
        private int _wheelTextureSize;
        private float _maxDistance;
        private float _maxDistanceSq;

        private void Awake()
        {
            CreateColorWheel();
            _pointTransform = point.GetComponent<RectTransform>();
        }

        private void CreateColorWheel()
        {
            Canvas owningCanvas = colorWheelImage.GetComponentInParent<Canvas>();
            Rect actualRectangle = RectTransformUtility.PixelAdjustRect(colorWheelImage.rectTransform, owningCanvas);

            _wheelTextureSize = Mathf.FloorToInt(Mathf.Min(actualRectangle.width, actualRectangle.height));

            Texture2D wheelTexture = new Texture2D(_wheelTextureSize, _wheelTextureSize, TextureFormat.RGB24, false);

            _maxDistance = (_wheelTextureSize / 2f) - wheelBufferInPixels;
            _maxDistanceSq = _maxDistance * _maxDistance;

            for (int y = 0; y < _wheelTextureSize; y++) 
            { 
                for (int x = 0; x < _wheelTextureSize; x++) 
                {
                    Vector2 vectorFromCentre = new Vector2(x - (_wheelTextureSize / 2f),
                        y - (_wheelTextureSize / 2f));
                    float distanceFromCentreSq = vectorFromCentre.sqrMagnitude;

                    if (distanceFromCentreSq < _maxDistanceSq)
                    {
                        float angle = Mathf.Atan2(vectorFromCentre.y, vectorFromCentre.x);
                        if (angle < 0)
                            angle += Mathf.PI * 2f;

                        float hue = Mathf.Clamp01(angle / (Mathf.PI * 2f));
                        float saturation = Mathf.Clamp01(Mathf.Sqrt(distanceFromCentreSq) / _maxDistance);

                        wheelTexture.SetPixel(x, y, Color.HSVToRGB(hue, saturation, 1f));
                    }
                    else
                        wheelTexture.SetPixel(x, y, Color.white);
                }
            }

            wheelTexture.Apply();
            colorWheelImage.texture = wheelTexture;
        }
        
        public void OnColourClicked(BaseEventData eventData)
        {
            if (eventData is PointerEventData)
            {
                Vector2 screenPosition = (eventData as PointerEventData).pointerCurrentRaycast.screenPosition;
                Vector2 localPosition = colorWheelImage.rectTransform.InverseTransformPoint(screenPosition);

                float distanceFromCentreSq = localPosition.sqrMagnitude;

                if (distanceFromCentreSq > _maxDistanceSq)
                    return;

                float angle = Mathf.Atan2(localPosition.y, localPosition.x);
                if (angle < 0)
                    angle += Mathf.PI * 2f;

                float hue = Mathf.Clamp01(angle / (Mathf.PI * 2f));
                float saturation = Mathf.Clamp01(Mathf.Sqrt(distanceFromCentreSq) / _maxDistance);

                OnNewColorPicked.Invoke(hue, saturation);
                UpdateReticle(hue, saturation);
            }
        }

        public void SetCurrentColour(Color inColour, float inHue, float inSaturation, float inValue)
        {
            UpdateReticle(inHue, inSaturation);

            point.color = inColour;
        }

        void UpdateReticle(float inHue, float inSaturation)
        {
            Vector2 pointPosition = new Vector2(Mathf.Cos(inHue * Mathf.PI * 2f),
                Mathf.Sin(inHue * Mathf.PI * 2f));
            pointPosition *= _maxDistance * inSaturation;

            _pointTransform.anchoredPosition = pointPosition;
        }
    }
}