/******************************************************************************************
*         【模块】{ 可视化资源清理工具 }                                                                                                                      
*         【功能】{ 清除游戏老旧资源，提供可视化实现 }                                                                                                                   
*         【修改日期】{ 2019年11月22日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class ResEditorBuilder: EditorWindow
{
    static List<DeleteAsset> deleteAssets = new List<DeleteAsset>();

    Vector2 scroll;

    public void Awake()
    {
        ClearDeleteFileList();
    }

    public void OnGUI()
    {
        GUILayout.BeginVertical();
        GUILayout.Space(10);
        GUI.skin.label.fontSize = 23;
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUILayout.Label("可视化资源清理工具");
        GUILayout.Label("-----------------------------------------------------------------------------");
        GUILayout.EndVertical();
        GUI.skin.label.fontSize = 15;
        GUI.skin.label.alignment = TextAnchor.LowerLeft;

        GUILayout.BeginHorizontal();
        GUILayout.Space(5);
        if (GUILayout.Button("\n查找资源依赖\n"))
        {
            FindHallResourcesDependences();
        };
        GUILayout.Space(5);
        if (GUILayout.Button("\n清理无用资源\n"))
        {
            CleanHallResources();
        };
        GUILayout.EndHorizontal();
        using (var horizonal = new EditorGUILayout.HorizontalScope("box"))
        {
            EditorGUILayout.LabelField("delete unreference assets from  resources");
            if (GUILayout.Button("Delete", GUILayout.Width(120), GUILayout.Height(40))&&deleteAssets.Count != 0)
            {
                RemoveFiles();
                Close();
            }
        }
        using (var scrollScope = new EditorGUILayout.ScrollViewScope(scroll))
        {
            scroll = scrollScope.scrollPosition;
            foreach (var asset in deleteAssets)
            {
                if (string.IsNullOrEmpty(asset.path))
                {
                    continue;
                }
                using (var horizonal = new EditorGUILayout.HorizontalScope())
                {
                    asset.isDelete = EditorGUILayout.Toggle(asset.isDelete, GUILayout.Width(20));
                    var icon = AssetDatabase.GetCachedIcon(asset.path);
                    GUILayout.Label(icon, GUILayout.Width(20), GUILayout.Height(20));
                    if (GUILayout.Button(asset.path, EditorStyles.largeLabel))
                    {
                        Selection.activeObject = AssetDatabase.LoadAssetAtPath<Object>(asset.path);
                    }
                }
            }

        }
    }
    [UnityEditor.MenuItem("VisualTools/VisualResClearUtil", false, 6)]
    public static void ShowWindow()
    {
        var exportWindow = EditorWindow.GetWindow(typeof(ResEditorBuilder),false,"资源清理工具");
        exportWindow.ShowUtility();
        exportWindow.minSize = new Vector3(700, 470);
        exportWindow.maxSize = exportWindow.minSize;
        var position = exportWindow.position;
        position.center = new Rect(0f, 0f, Screen.currentResolution.width, Screen.currentResolution.height).center;
        exportWindow.position = position;
    }
    
    public static void FindHallResourcesDependences()
    {
        var isMakeSure = EditorUtility.DisplayDialog("VisualResClearUtil", "查找资源依赖", "是", "否");
        if (!isMakeSure) return;
        ClearDeleteFileList();
        ResEditorCleaner.CheckHall();
    }

    public static void CleanHallResources()
	{
        var isMakeSure = EditorUtility.DisplayDialog("VisualResClearUtil", "清理无用资源", "是", "否");
        if (!isMakeSure) return;
        ClearDeleteFileList();
        ResEditorCleaner.CleanHall();
        InitDeleteAssets();

    }
 
    public static void ClearDeleteFileList()
    {
        deleteAssets = new List<DeleteAsset>();
    }
    public static void InitDeleteAssets()
    {
        foreach (var asset in ResEditorCleaner.deleteFileList)
        {
            var filePath = AssetDatabase.GUIDToAssetPath(asset);
            if (string.IsNullOrEmpty(filePath) == false)
            {
                deleteAssets.Add(new DeleteAsset() { path = filePath });
            }
        }
    }

    public static void RemoveFiles()
    {
        try
        {
            string exportDirectry = "BackupUnusedAssets";
            Directory.CreateDirectory(exportDirectry);
            var files = deleteAssets.Where(item => item.isDelete == true).Select(item => item.path).ToArray();
            string backupPackageName = exportDirectry + "/package" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".unitypackage";
            EditorUtility.DisplayProgressBar("export package", backupPackageName, 0);
            AssetDatabase.ExportPackage(files, backupPackageName);

            int i = 0;
            int length = deleteAssets.Count;

            foreach (var assetPath in files)
            {
                i++;
                EditorUtility.DisplayProgressBar("delete unused assets", assetPath, (float)i / length);
                AssetDatabase.DeleteAsset(assetPath);
            }
            EditorUtility.DisplayProgressBar("clean directory", "", 1);
            foreach (var dir in Directory.GetDirectories("Assets"))
            {
                RemoveEmptyDirectry(dir);
            }
            System.Diagnostics.Process.Start(exportDirectry);
            AssetDatabase.Refresh();
        }
        catch (System.Exception e)
        {

            Debug.Log(e.Message);
        }
        finally
        {
            EditorUtility.ClearProgressBar();
        }
    }

    static void CleanDir()
    {
        RemoveEmptyDirectry("Assets");
        AssetDatabase.Refresh();
    }

    static void RemoveEmptyDirectry(string path)
    {
        var dirs = Directory.GetDirectories(path);
        foreach (var dir in dirs)
        {
            RemoveEmptyDirectry(dir);
        }
        var files = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly).Where(item => Path.GetExtension(item) != ".meta");
        if (files.Count() == 0 && Directory.GetDirectories(path).Count() == 0)
        {
            var metaFile = AssetDatabase.GetTextMetaFilePathFromAssetPath(path);
            UnityEditor.FileUtil.DeleteFileOrDirectory(path);
            UnityEditor.FileUtil.DeleteFileOrDirectory(metaFile);
        }
    }
}
