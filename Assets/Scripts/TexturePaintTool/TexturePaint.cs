using TexturePaintTool.UI;
using TexturePaintTool.UI.ColorPick;
using UnityEngine;

namespace TexturePaintTool
{
    public class TexturePaint : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [Range(2, 512), SerializeField] private int textureSize = 128;
        [SerializeField] private FilterMode filterMode;
        [SerializeField] private TextureWrapMode wrapMode;
        [SerializeField] private PaintObject paintObject;
        [SerializeField] private BrushesPicker brushesPicker;
        [SerializeField] private ColorPicker colorPicker;
        [SerializeField] private SaveLoadPanel saveLoadPanel;

        private Texture2D _texture;
        private Brush _brush;
        private int _lastHitX;
        private int _lastHitY;
        private float _currentScale;
        private Color _color = Color.black;

        private void Awake()
        {
            _texture = new Texture2D(textureSize, textureSize)
            {
                filterMode = filterMode,
                wrapMode = wrapMode
            };
            paintObject.Material.mainTexture = _texture;
            _texture.Apply();

            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }
        }

        private void OnEnable()
        {
            brushesPicker.OnBrushSelected += BrushSelected;
            brushesPicker.OnScaleChanged += ScaleChanged;
            colorPicker.OnColorChanged += ColorChanged;
            saveLoadPanel.OnSaveClicked += SaveTexture;
            saveLoadPanel.OnLoadClicked += LoadTexture;
        }

        private void OnDisable()
        {
            brushesPicker.OnBrushSelected -= BrushSelected;
            brushesPicker.OnScaleChanged += ScaleChanged;
            colorPicker.OnColorChanged -= ColorChanged;
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

                if (paintObject.MeshCollider.Raycast(ray, out var hit, 100f))
                {
                    int hitX = (int)(hit.textureCoord.x * textureSize);
                    int hitY = (int)(hit.textureCoord.y * textureSize);

                    if (_lastHitX == hitX && _lastHitY == hitY)
                    {
                        return;
                    }

                    _lastHitX = hitX;
                    _lastHitY = hitY;

                    PaintBrushImage(hitX, hitY);
                    _texture.Apply();
                }
            }
        }

        private void LoadTexture()
        {
            _texture = SaveLoad.LoadLastTexture();

            paintObject.Material.mainTexture = _texture;
        }

        private void SaveTexture()
        {
            SaveLoad.SaveTexture(_texture);
        }

        private void BrushSelected(Brush newBrush)
        {
            _brush = newBrush;
            _brush.SetScale(_currentScale);
        }

        private void ScaleChanged(float newScale)
        {
            _currentScale = newScale;

            if (_brush)
                _brush.SetScale(newScale);
        }

        private void ColorChanged(Color newColor)
        {
            _color = newColor;
        }

        private void PaintBrushImage(int positionX, int positionY)
        {
            for (int brushY = 0; brushY < _brush.Height; brushY++)
            {
                int pixelY = positionY + brushY - (_brush.Height / 2);
                if (pixelY < 0 || pixelY >= _texture.height)
                    continue;

                for (int brushX = 0; brushX < _brush.Width; brushX++)
                {
                    int pixelX = positionX + brushX - (_brush.Width / 2);
                    if (pixelX < 0 || pixelX >= _texture.width)
                        continue;

                    Color brushPixel = _brush.GetPixel(brushX, brushY);
                    Color newPixelColor = _texture.GetPixel(pixelX, pixelY);

                    newPixelColor = GetNewPixelColor(newPixelColor, _color, brushPixel);
                    _texture.SetPixel(pixelX, pixelY, newPixelColor);
                }
            }
        }

        private Color GetNewPixelColor(Color baseColor, Color wantedColor, Color brushColor)
        {
            return Color.Lerp(baseColor, wantedColor, brushColor.r);
        }
    }
}