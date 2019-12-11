/******************************************************************************************
*         【模块】{ 可视化资源清理工具 }                                                                                                                      
*         【功能】{ 通过遍历预置体引用，找出废弃资源 }                                                                                                                   
*         【修改日期】{ 2019年11月22日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/

using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Linq;


public class ResEditorCleaner
{
    //Log输出目录
    private static readonly string OutLogPath = "Assets/ResOutLog";
    //日志输出目录
    private static readonly string ResCleanerInfoPath = OutLogPath + "/Cleaner.txt";
    
    //资源目录
    private static readonly string ResourcesPath = "Assets/Bundles/";
    
    //需要动态加载资源的目录，一般默认是Image（动态设置图片）和Sound（动态加载音效）两个!（该目录文件在检测无用文件时，会被忽略）
    private static readonly List<string> DynamicLoadPaths = new List<string>()
    {
        //大厅
        ResourcesPath + "_GameLobby/Hall/Image",
        
        //子游戏
        ResourcesPath + "_GameModule/BJLPoker/Image",
        ResourcesPath + "_GameModule/BlackJack/Image",
        ResourcesPath + "_GameModule/DDZPoker/Image",
        ResourcesPath + "_GameModule/FruitMachine/Image",
        ResourcesPath + "_GameModule/HongHeiGame/Image",
        ResourcesPath + "_GameModule/NHDPoker/Image",
        ResourcesPath + "_GameModule/NNPoker/Image",
        ResourcesPath + "_GameModule/QZNNPoker/Image",
        ResourcesPath + "_GameModule/TexasPoker/Image",
        ResourcesPath + "_GameModule/ZJHGamePoker/Image",

        //下面两个目录手动清理

        ResourcesPath + "_GameLobby/Loading",
        ResourcesPath + "_GameLobby/Public",

    };
    //用于存放预制件和场景文件的目录，一般默认prefab和scene（该目录文件在检测无用文件时，会被忽略）
    private static readonly List<string> DependenciesPaths = new List<string>()
    {
        //大厅
        ResourcesPath + "_GameLobby/Hall/Prefab",
        ResourcesPath + "_GameLobby/Login/Prefab",
        
        //子游戏
        ResourcesPath + "_GameModule/BJLPoker/Prefab",
        ResourcesPath + "_GameModule/BlackJack/Prefab",
        ResourcesPath + "_GameModule/DDZPoker/Prefab",
        ResourcesPath + "_GameModule/FruitMachine/Prefab",
        ResourcesPath + "_GameModule/HongHeiGame/Prefab",
        ResourcesPath + "_GameModule/NHDPoker/Prefab",
        ResourcesPath + "_GameModule/NNPoker/Prefab",
        ResourcesPath + "_GameModule/QZNNPoker/Prefab",
        ResourcesPath + "_GameModule/TexasPoker/Prefab",
        ResourcesPath + "_GameModule/ZJHGamePoker/Prefab",
    };
    
    //移除的文件列表
    public static List<string> deleteFileList = new List<string>();

    private static FileStream fs = null ;
    private static StreamWriter sw  = null;

    //检查依赖
    public static void CheckHall()
    {
        StartCleaner(false);
    }

    //清理无用资源
    public static void CleanHall()
    {
        StartCleaner(true);
    }
    
    private static void StartCleaner(bool isClean)
    {
        deleteFileList.Clear();
        
        //记录日志
        StartLog(ResCleanerInfoPath);

        Log("查找资源依赖\n");
        EditorUtility.DisplayProgressBar("Cleaner", "查找资源依赖", 0);
        

        //所有依赖文件的路径
        List<string> dependentPaths = new List<string>();
        foreach (var floder in DependenciesPaths)
        {
            if (!Directory.Exists(floder))
            {
                continue;
            }
            string[] depentFiles = Directory.GetFiles(floder, "*.*", SearchOption.AllDirectories);
            int counter = 0;
            foreach (var file in depentFiles)
            {
                counter ++;
                if(file.EndsWith(".meta")) continue;
                Log("依赖源：" + file.Replace('\\', '/'));
                string[] dps = AssetDatabase.GetDependencies(file);
                foreach (var dp in dps)
                {
                    Log("依赖：" + dp);
                    dependentPaths.Add(dp);
                }
                Log("\n");
                EditorUtility.DisplayProgressBar("Cleaner", "检测依赖源：" + file, (float)counter / depentFiles.Count());
            }
        }

        //是否清除文件
        if (isClean)
        {
            Log("过滤动态加载的资源");
            foreach (var floder in DynamicLoadPaths)
            {
                if (!Directory.Exists(floder)) continue;
                string[] files = Directory.GetFiles(floder, "*.*", SearchOption.AllDirectories);
                for (int i = 0; i < files.Length; i++)
                {
                    files[i] = files[i].Replace('\\', '/');
                }
                foreach (var file in files)
                {
                    if(file.EndsWith(".meta")) continue;
                    Log("动态加载：" + file);
                    dependentPaths.Add(file);
                }
            }
            Log("\n");

            Log("准备处理无用资源");
            List<string> resPaths = new List<string>
            {
                ResourcesPath,
            };
            for (int i = 0; i < resPaths.Count; i++)
            {
                EditorUtility.DisplayProgressBar("Cleaner", "准备处理无用资源" + (i+1), 0);
                string[] fileAssets = Directory.GetFiles(resPaths[i], "*.*", SearchOption.AllDirectories);
                for (int j = 0; j < fileAssets.Length; j++)
                {
                    fileAssets[j] = fileAssets[j].Replace('\\', '/');
                }
                int counter = 0;
                foreach (var file in fileAssets)
                {
                    counter++;
                    if (file.EndsWith(".meta")) continue;
                    if (dependentPaths.Contains(file) == false)
                    {
                        Log("无用资源：" + file);
                        EditorUtility.DisplayProgressBar("Cleaner", "删除无用资源：" + file, (float)counter / fileAssets.Count());
                        var guid = AssetDatabase.AssetPathToGUID(file);
                        deleteFileList.Add(guid);
                        // File.Delete(file);//暂时注掉
                    }
                }
            }
        }

        Log("\n");
        Log("处理完毕");
        EditorUtility.DisplayProgressBar("Cleaner", "正在刷新,请稍后", 1);
        AssetDatabase.Refresh();
        EditorUtility.ClearProgressBar();
        StopLog();
    }

    private static void StartLog(string path)
    {
        if (!Directory.Exists(OutLogPath)) Directory.CreateDirectory(OutLogPath);
        if (File.Exists(path)) File.Delete(path);

        fs = new FileStream(path, FileMode.CreateNew);
        sw = new StreamWriter(fs);
    }

    private static void StopLog()
    {
        sw.Close();
        fs.Close();
        sw = null;
        fs = null;
    }

    private static void Log(string str)
    {
        UnityEngine.Debug.Log(str);

        if (sw != null)
        {
            sw.WriteLine(str);
        }
    }
}
