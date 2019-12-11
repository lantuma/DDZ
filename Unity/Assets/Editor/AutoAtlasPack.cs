    using UnityEngine;
    using UnityEditor;
    using System.Collections;
    using System.IO;
    using System.Collections.Generic;
    using UnityEditor.Animations;

public class AutoAtlasPack
{
    [MenuItem("Assets/Texture/Add Prefix Name")]
    public static void DoAddPrefixTextureName()
    {
        Object[] SelectedAsset = Selection.GetFiltered(typeof(Texture), SelectionMode.DeepAssets);

        foreach (Object o in SelectedAsset)
        {
            var assetpath = AssetDatabase.GetAssetPath(o);
            var pathname = GetFolderName(assetpath).ToLower();
            var prefix = pathname + "_";

            if (!o.name.StartsWith(prefix))
            {
                var newname = prefix + o.name;
                Debug.LogFormat("ADD PREFIX:<color=red>{0}</color> ===> <color=green>{1}</color>", o.name, newname);

                AssetDatabase.RenameAsset(assetpath, newname);
                AssetDatabase.MoveAssetToTrash(assetpath);
            }
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

    }

    [MenuItem("Assets/Texture/Remove Prefix Name")]
    public static void DoRemovePrefixTextureName()
    {
        Object[] SelectedAsset = Selection.GetFiltered(typeof(Texture), SelectionMode.DeepAssets);

        foreach (Object o in SelectedAsset)
        {
            var assetpath = AssetDatabase.GetAssetPath(o);
            var pathname = GetFolderName(assetpath).ToLower();
            var prefix = pathname + "_";
            var name = o.name.ToLower();

            if (name.StartsWith(prefix))
            {
                var newname = name.Replace(prefix, "");
                Debug.LogFormat("REMOVE PREFIX:<color=red>{0}</color> ===> <color=green>{1}</color>", o.name, newname);

                AssetDatabase.RenameAsset(assetpath, newname);
                AssetDatabase.MoveAssetToTrash(assetpath);
            }
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

    }

    private static string GetFolderName(string path)
    {
        string[] array = path.Split('/');
        if (array.Length > 2)
        {
            return (array[array.Length - 2]);
        }
        return "NotFindParentFolder";
    }

}

