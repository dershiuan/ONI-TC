using System;
using System.IO;
using System.Reflection;
using FontLoader.Config;
using TMPro;
using UnityEngine;

namespace FontLoader.Utils
{
    public static class FontUtil
    {
        private static readonly string WindowsShader = "Assets/TextMesh Pro/Resources/Shaders/TMP_SDF.shader";
        private static readonly string MobileShader = "Assets/TextMesh Pro/Resources/Shaders/TMP_SDF-Mobile.shader";

        public static TMP_FontAsset LoadFontAsset(FontConfig config)
        {
            TMP_FontAsset font = null;
            AssetBundle ab = null;

            try
            {
                if (config.Local)
                {
                    var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Assets", config.Filename);
                    ab = AssetBundle.LoadFromFile(path);
                }
                else
                {
                    var path = Path.Combine(Application.streamingAssetsPath, "fonts", config.Filename);
                    ab = AssetBundle.LoadFromFile(path);
                }

                font = ab.LoadAllAssets<TMP_FontAsset>()[0];
                font.fontInfo.Scale = config.Scale;
                Debug.Log("FontLoader: Load font successful.");
            }
            catch (Exception e)
            {
                Debug.Log($"FontLoader failure: {e.Message}");
            }

            if (Application.platform != RuntimePlatform.WindowsPlayer) {
                Debug.Log($"FontLoader: loading shader.");
                
                Shader shader = null;
                try {
                    shader = ab.LoadAsset<Shader>(MobileShader);
                    font.material.shader = shader;
                    Debug.Log($"FontLoader: using mobile shader.");
                }
                catch (Exception e)
                {
                    Debug.Log($"FontLoader failure: {e.Message}");
                }
            }

            AssetBundle.UnloadAllAssetBundles(false);
            return font;
        }
    }
}