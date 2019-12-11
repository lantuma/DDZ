using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;
using Object = UnityEngine.Object;

namespace Assets.Editor.AssetBundleEditor
{
    public class AssetsBundleNameEditor
    {
        [MenuItem("Assets/AssetBundleName/Set Prefab Or Atlas Name")]
        private static void SetPrefabAbName()
        {
            var selectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);
            foreach (var o in selectedAsset)
            {
                var path = AssetDatabase.GetAssetPath(o);
                var direction = new DirectoryInfo(path);
                if (!direction.Exists && o is GameObject)
                {
                    AssetImporter.GetAtPath(path).SetAssetBundleNameAndVariant($"{o.name.ToLower()}.unity3d", "");
                }
                if (!direction.Exists) continue;
                var files = direction.GetFiles("*", SearchOption.AllDirectories);
                foreach (var f in files)
                {
                    if (!f.Name.EndsWith(".prefab") &&
                        !f.Name.EndsWith(".spriteatlas")) continue;
                    var assetImporter = AssetImporter.GetAtPath($"{path}/{f.Name}");
                    var abname = f.Name.Split('.');
                    assetImporter.SetAssetBundleNameAndVariant($"{abname[0].ToLower()}.unity3d", "");
                }
                Debug.Log($"Set prefab or atlas AssetBundle name to success!");
            }
            AssetDatabase.Refresh();
        }

        [MenuItem("Assets/AssetBundleName/Set Prefab Or Atlas Name", true)]
        private static bool ValidateSetPrefabAbName()
        {
            var selectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.Deep);
            return selectedAsset.Select(o => o is GameObject || o is SpriteAtlas ||
                                             new DirectoryInfo(AssetDatabase.GetAssetPath(o)).Exists).FirstOrDefault();
        }

        [MenuItem("Assets/AssetBundleName/Set Texture Name")]
        private static void SetTextureAbName()
        {
            var selectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.Deep);
            foreach (var o in selectedAsset)
            {
                var path = AssetDatabase.GetAssetPath(o);
                var direction = new DirectoryInfo(path);

                if (direction.Exists)
                {
                    AssetImporter.GetAtPath(path).SetAssetBundleNameAndVariant($"{o.name.ToLower()}atlas.unity3d", "");
                }
                else if (o is Texture)
                {
                    var dirs = direction.FullName.Split('\\');
                    var name = dirs[dirs.Length - 2].ToLower();
                    AssetImporter.GetAtPath(path).SetAssetBundleNameAndVariant($"{name}atlas.unity3d", "");
                }
                if (!direction.Exists) continue;
                var subdirs = direction.GetDirectories();
                var files = direction.GetFiles("*", SearchOption.AllDirectories);
                foreach (var subdi in subdirs)
                {
                    var ai = AssetImporter.GetAtPath($"{path}/{subdi.Name}");
                    ai.SetAssetBundleNameAndVariant($"{o.name.ToLower()}atlas.unity3d", "");
                }
                foreach (var f in files)
                {
                    if (!f.Name.EndsWith(".png")) continue;

                    var assetImporter = AssetImporter.GetAtPath($"{f.FullName.Substring(f.FullName.IndexOf("Assets", StringComparison.Ordinal))}");

                    assetImporter.SetAssetBundleNameAndVariant($"{o.name.ToLower()}atlas.unity3d", "");
                }
                Debug.Log($"Set Texture AssetBundle name success!");
            }
        }

        [MenuItem("Assets/AssetBundleName/Set Texture Name", true)]
        private static bool ValidataSetTextureAbName()
        {
            var selectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.Deep);
            return selectedAsset.Select(o => o is Texture || new DirectoryInfo(AssetDatabase.GetAssetPath(o)).Exists).FirstOrDefault();
        }

        [MenuItem("Assets/AssetBundleName/Set Self Name")]
        private static void SetSelfName2AbName()
        {
            var selectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.Deep);
            foreach (var o in selectedAsset)
            {
                var path = AssetDatabase.GetAssetPath(o);
                var direction = new DirectoryInfo(path);
                if (!direction.Exists)
                {
                    var name = o.name.Split('/');
                    AssetImporter.GetAtPath(path).SetAssetBundleNameAndVariant($"{name[name.Length - 1].ToLower()}.unity3d", "");
                }
                else
                {
                    var subdirs = direction.GetDirectories();
                    var files = direction.GetFiles("*", SearchOption.AllDirectories);
                    foreach (var f in files)
                    {
                        if (f.Name.EndsWith(".meta")) continue;
                        var assetImporter = AssetImporter.GetAtPath($"{f.FullName.Substring(f.FullName.IndexOf("Assets", StringComparison.Ordinal))}");
                        var abname = f.Name.Split('.');
                        assetImporter.SetAssetBundleNameAndVariant($"{abname[0].ToLower()}.unity3d", "");
                    }
                }
            }
        }

        [MenuItem("Assets/AssetBundleName/Remove Name", false, 1100)]
        private static void RemoveAbName()
        {
            var selectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.Deep);
            foreach (var o in selectedAsset)
            {
                var path = AssetDatabase.GetAssetPath(o);
                AssetImporter.GetAtPath(path).SetAssetBundleNameAndVariant("", "");
                var direction = new DirectoryInfo(path);
                if (!direction.Exists) continue;
                var subdirs = direction.GetDirectories();
                foreach (var subdir in subdirs)
                {
                    var ai = AssetImporter.GetAtPath($"{path}/{subdir.Name}");
                    ai.SetAssetBundleNameAndVariant("", "");
                }
                var files = direction.GetFiles("*", SearchOption.AllDirectories);
                foreach (var f in files)
                {
                    if (f.Name.EndsWith(".meta")) continue;

                    var assetImporter = AssetImporter.GetAtPath($"{f.FullName.Substring(f.FullName.IndexOf("Assets", StringComparison.Ordinal))}");
                    assetImporter.SetAssetBundleNameAndVariant("", "");
                }
                Debug.Log($"Remove {path}/* all AssetBundle name.");
            }
            AssetDatabase.Refresh();
        }

        [MenuItem("Assets/AssetBundleName/Remove Unused Name", false, 1101)]
        private static void RemoveUnusedName()
        {
            AssetDatabase.RemoveUnusedAssetBundleNames();
        }
    }
}