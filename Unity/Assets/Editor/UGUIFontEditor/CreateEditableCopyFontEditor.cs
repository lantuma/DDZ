using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace MMGame.Framework
{
    public class CreateEditableCopyFontEditor : EditorWindow
    {
        //[MenuItem("Assets/Create Editable Copy Font")]
        static void CreateFont()
        {
            var txts = UnityEngine.Object.FindObjectsOfType(typeof(Text));
            return;

            TextAsset sourceTextAsset = null;
            try
            {
                sourceTextAsset = (TextAsset)Selection.activeObject;
            } catch (InvalidCastException e)
            {
                Debug.Log("Selected Object is not a txt file: " + Environment.NewLine + e.Message);
            }

            if (sourceTextAsset == null)
            {
                EditorUtility.DisplayDialog("No Config selected", "Please select a TxtFile Config...\nSuch as Exsample.txt:\nname=Assets/GameAssets/Fonts/impact.ttf,size=40\n123456789abcdefghijk", "Cancel");
                return;
            }

            int targetFontSize;
            string sourceFontPath, targetFontPath, targetFontCharacters = "";
            try
            {
                string sourceConfigPath = AssetDatabase.GetAssetPath(Selection.activeObject);
                string[] sourceConfigInfos = sourceTextAsset.text.Split('\n');
                string headInfo = sourceConfigInfos [0];
                string[] headInfos = headInfo.Split(',');

                sourceFontPath = headInfos [0].Split('=') [1];
                targetFontPath = sourceConfigPath.Replace(".txt", "_copy");
                targetFontSize = int.Parse(headInfos [1].Split('=') [1]);
                for (int i = 1; i < sourceConfigInfos.Length; i++)
                {
                    targetFontCharacters += sourceConfigInfos [i];
                }
            } catch (Exception ex)
            {
                EditorUtility.DisplayDialog("Config Error", "The config header data error...", "Cancel");
                return;
            }

            // 重新生成字体文件会导致场景中已存在的丢失，
            // 所以需要生成后再次赋值
            string[] targetFontPathInfos = targetFontPath.Split('/');
            string textCheckName = targetFontPathInfos [targetFontPathInfos.Length - 1];
            var listTexts = new List<Text>();
            foreach (Text text in UnityEngine.Object.FindObjectsOfType(typeof(Text)))
            {
                if (text.font.name == textCheckName)
                {
                    listTexts.Add(text);
                }
            }       

            UnityEngine.Object f = AssetDatabase.LoadMainAssetAtPath(sourceFontPath);
            string path = AssetDatabase.GetAssetPath(f);
            TrueTypeFontImporter fontImporter = AssetImporter.GetAtPath(path) as TrueTypeFontImporter;
            fontImporter.fontTextureCase = FontTextureCase.CustomSet;
            fontImporter.customCharacters = targetFontCharacters;
            fontImporter.fontSize = targetFontSize;
            fontImporter.SaveAndReimport();
            AssetDatabase.Refresh();
            Font font = fontImporter.GenerateEditableFont(targetFontPath);

            foreach (Text item in listTexts)
            {
                item.font = font;
            }

            // 还原ttf设置
            fontImporter.fontTextureCase = FontTextureCase.Dynamic;
            fontImporter.SaveAndReimport();
            AssetDatabase.Refresh();

            System.GC.Collect();
        }
    }
}

