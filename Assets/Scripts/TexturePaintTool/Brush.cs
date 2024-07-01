using UnityEngine;

namespace TexturePaintTool
{
    [CreateAssetMenu(fileName = "Brush", menuName = "Painting/Brush")]
    public class Brush : ScriptableObject
    {
        [SerializeField] private Texture2D texture;
        [SerializeField] private string displayedName;

        private Color[] cachedColors = null;

        private float _currentScale = -1;
        
        public int Width { get; private set; } = -1;
        public int Height { get; private set; } = -1;

        public Texture2D Texture => texture;
        public string DisplayedName => displayedName;

        public void SetScale(float scale)
        {
            bool isDirty = false;
            
            if (Mathf.Abs(_currentScale - scale) > Mathf.Epsilon)
            {
                Width = (int)(texture.width * scale);
                Height = (int)(texture.height * scale);
                isDirty = true;
            }

            if (isDirty)
            {
                cachedColors = new Color[Width * Height];
                
                for (int y = 0; y < Height; y++)
                {
                    float uvY = (float)y / Height;

                    for (int x = 0; x < Width; x++)
                    {
                        float uvX = (float)x / Width;

                        cachedColors[x + y * Width] = texture.GetPixelBilinear(uvX, uvY);
                    }
                }
            }
        }
        
        public Color GetPixel(int x, int y)
        {
            return cachedColors[x + y * Width];
        }
    }
}
