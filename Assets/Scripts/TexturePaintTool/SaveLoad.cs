using System.IO;
using UnityEngine;

namespace TexturePaintTool
{
    public static class SaveLoad
    {
        private const string Path = "/SavedTextures/";
        private const string DefaultFileName = "Texture.bytes";
        
        public static void SaveTexture(Texture2D texture)
        {
            byte[] bytes = texture.EncodeToPNG();

            Directory.CreateDirectory(Application.persistentDataPath + Path);
            File.WriteAllBytes(Application.persistentDataPath + Path + DefaultFileName, bytes);
        }

        public static Texture2D LoadLastTexture()
        {
            Texture2D output = new Texture2D(2, 2);
            byte[] bytes = File.ReadAllBytes(Application.persistentDataPath + Path + DefaultFileName);

            output.LoadImage(bytes);
            
            return output;
        }
    }
}
