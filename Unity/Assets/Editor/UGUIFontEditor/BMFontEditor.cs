using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;

namespace MMGame.Framework
{
    public class BMFontEditor : EditorWindow
    {
        //[MenuItem("Tools/BMFont Maker")]
        static public void OpenBMFontMaker()
        {
            EditorWindow.GetWindow<BMFontEditor>(false, "BMFont Maker", true).Show();
        }

        //    [SerializeField]
        private Font _TargetFont;
        //    [SerializeField]
        private TextAsset _FntData;
        //    [SerializeField]
        private Material _FntMat;
        //    [SerializeField]
        private Texture2D _FntTexture;
	
        private BMFont bmFont = new BMFont();

        public BMFontEditor()
        {
        }

        void OnGUI()
        {
//        _TargetFont = EditorGUILayout.ObjectField("Target Font", _TargetFont, typeof(Font), false) as Font;
//        _FntData = EditorGUILayout.ObjectField("Fnt Data", _FntData, typeof(TextAsset), false) as TextAsset;
//        _FntMat = EditorGUILayout.ObjectField("Font Material", _FntMat, typeof(Material), false) as Material;
            _FntTexture = EditorGUILayout.ObjectField("Font Texture", _FntTexture, typeof(Texture2D), false) as Texture2D;
		
            if (GUILayout.Button("Load BMFont"))
            {
                if (_FntTexture == null)
                    return;
            
                LoadFont(_FntTexture);

                EditorGUILayout.HelpBox("Load Complete!", MessageType.Info, true);
            }
            if (GUILayout.Button("Create BMFont"))
            {
                CreateFont(); 
            }
        }

        public bool LoadFont(Texture2D fntTexture)
        {
            string AssetFile = AssetDatabase.GetAssetPath(fntTexture);
            //修改贴图导入方式
            TextureImporter texImporter = AssetImporter.GetAtPath(AssetFile) as TextureImporter;
            if (texImporter == null)
            {
                return false;
            }
            if (texImporter.textureType != TextureImporterType.GUI)
            {
                texImporter.textureType = TextureImporterType.GUI;
                texImporter.textureFormat = TextureImporterFormat.AutomaticCompressed;
//            texImporter.SaveAndReimport();
            }
            _FntTexture = fntTexture;
            var tex = (Texture2D)AssetDatabase.LoadAssetAtPath(AssetFile, typeof(Texture2D));
            string PathNameNotExt = AssetFile.Remove(AssetFile.LastIndexOf('.'));
            string FileNameNotExt = PathNameNotExt.Remove(0, PathNameNotExt.LastIndexOf('/') + 1);
            string TextPathName = PathNameNotExt + ".fnt";
            string MatPathName = PathNameNotExt + ".mat";
            string FontPathName = PathNameNotExt + ".fontsettings";
            TextAsset text = (TextAsset)AssetDatabase.LoadAssetAtPath(TextPathName, typeof(TextAsset));
            if (text == null)
            {
                return false;
            }
            _FntData = text;
            Material mat = (Material)AssetDatabase.LoadAssetAtPath(MatPathName, typeof(Material));
            if (mat == null)
            {
                //创建材质球
                mat = new Material(Shader.Find("GUI/Text Shader"));
                mat.SetTexture("_MainTex", tex);
                AssetDatabase.CreateAsset(mat, MatPathName);
            }
            _FntMat = mat;
            Font font = (Font)AssetDatabase.LoadAssetAtPath(FontPathName, typeof(Font));
            if (font == null)
            {
                font = new Font(FileNameNotExt);
                AssetDatabase.CreateAsset(font, FontPathName);
            }
            _TargetFont = font;
            return true;
        }

        public void CreateFont()
        {
            BMFontReader.Load(bmFont, _FntData.name, _FntData.bytes);
            // 借用NGUI封装的读取类
            CharacterInfo[] characterInfo = new CharacterInfo[bmFont.glyphs.Count];
            for (int i = 0; i < bmFont.glyphs.Count; i++)
            {
                BMGlyph bmInfo = bmFont.glyphs [i];
                CharacterInfo info = new CharacterInfo();
                info.index = bmInfo.index;
           
                info.uv.x = (float)bmInfo.x / (float)bmFont.texWidth;
                info.uv.y = 1 - (float)bmInfo.y / (float)bmFont.texHeight;
                info.uv.width = (float)bmInfo.width / (float)bmFont.texWidth;
                info.uv.height = -1f * (float)bmInfo.height / (float)bmFont.texHeight;
                info.vert.x = 0;
                info.vert.y = -(float)bmInfo.height;
                info.vert.width = (float)bmInfo.width;
                info.vert.height = (float)bmInfo.height;
                info.width = (float)bmInfo.advance;

                // 居中对齐
                if (bmInfo.offsetY > 0)
                {
                    info.vert.y -= bmInfo.offsetY;
                    if (bmInfo.height > bmFont.baseOffset)
                        info.vert.y = -Mathf.Clamp(bmInfo.height + bmInfo.offsetY, bmFont.baseOffset, bmFont.charSize - 2);
                }


                if (bmInfo.index > 255 && bmInfo.offsetY < 0)
                {
                    info.vert.y += bmInfo.offsetY;
                    if (bmInfo.height > bmFont.baseOffset)
                        info.vert.y = -Mathf.Clamp(bmInfo.height - bmInfo.offsetY, bmFont.baseOffset, bmFont.charSize - 2);
                }

                characterInfo [i] = info;
            }
            _TargetFont.characterInfo = characterInfo;
            if (_FntMat)
            {
                _FntMat.mainTexture = _FntTexture;
            }
            _TargetFont.material = _FntMat;
            _FntMat.shader = Shader.Find("UI/Default");

            var fontPath = AssetDatabase.GetAssetPath(_TargetFont);
            var fontText = File.ReadAllText(fontPath);
//            const string pattern = "m_Texture: ({fileID: 0})?({fileID: 2800000, guid: [a-z0-9]{32}, type: 3})?";
            string pattern = "m_FontSize:\\s\\d+";

            if (Regex.IsMatch(fontText, pattern))
            {
                string texturePath = AssetDatabase.GetAssetPath(_FntTexture);
//                var replacement = ("m_Texture: {fileID: 2800000, guid: {0}, type: 3}").Replace("{0}", AssetDatabase.AssetPathToGUID(texturePath));
                var replacement = string.Format("m_FontSize:\t{0}", bmFont.fontSize);
                fontText = Regex.Replace(fontText, pattern, replacement);
                File.WriteAllText(fontPath, fontText);
                AssetDatabase.ImportAsset(fontPath, ImportAssetOptions.ForceUpdate);
                AssetDatabase.Refresh();
            }

            pattern = "m_LineSpacing:\\s(\\d*\\.)?\\d+";
            if (Regex.IsMatch(fontText, pattern))
            {
                string texturePath = AssetDatabase.GetAssetPath(_FntTexture);
                var replacement = string.Format("m_LineSpacing:\t{0}", bmFont.fontSize + .5f);
                fontText = Regex.Replace(fontText, pattern, replacement);
                File.WriteAllText(fontPath, fontText);
                AssetDatabase.ImportAsset(fontPath, ImportAssetOptions.ForceUpdate);
                AssetDatabase.Refresh();
            }

//            var importer = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(_FntData));


            //这一行很关键，如果用standard的shader，放到Android手机上，第一次加载会很慢
//        EditorGUILayout.HelpBox("Create Font " + _TargetFont.name + " Success!", MessageType.Info);
            Debug.Log("create font <" + _TargetFont.name + "> success");
        }
    }
}