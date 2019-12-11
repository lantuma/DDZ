/******************************************************************************************
*         【模块】{ 工具类 }                                                                                                                      
*         【功能】{ 日志重定向 }                                                                                                                   
*         【修改日期】{ 2019年11月26日 }                                                                                                                        
*         【贡献者】{ 周瑜 整合 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditorInternal;
using UnityEngine;


internal static class LogRedirection
{
    private static readonly Regex LogRegex = new Regex(@" \(at (.+)\:(\d+)\)\r?\n");

    [OnOpenAsset(0)]
    private static bool OnOpenAsset(int instanceId, int line)
    {
        string selectedStackTrace = GetSelectedStackTrace();
        if (string.IsNullOrEmpty(selectedStackTrace))
        {
            return false;
        }

        if (!selectedStackTrace.Contains("ETModel.Log"))
        {
            return false;
        }

        Match match = LogRegex.Match(selectedStackTrace);
        if (!match.Success)
        {
            return false;
        }

        // 跳过第一次匹配的堆栈
        match = match.NextMatch();
        if (!match.Success)
        {
            return false;
        }
        if (!selectedStackTrace.Contains("ETHotfix.Log"))
        {
            InternalEditorUtility.OpenFileAtLineExternal(Application.dataPath.Replace("Assets", "") + match.Groups[1].Value, int.Parse(match.Groups[2].Value));

            return true;
        }
        else
        {
            // 跳过第2次匹配的堆栈
            match = match.NextMatch();
            if (!match.Success)
            {
                return false;
            }
            InternalEditorUtility.OpenFileAtLineExternal(Application.dataPath.Replace("Assets", "") + match.Groups[1].Value, int.Parse(match.Groups[2].Value));

            return true;
        }
        
    }

    private static string GetSelectedStackTrace()
    {
        Assembly editorWindowAssembly = typeof(EditorWindow).Assembly;

        System.Type consoleWindowType = editorWindowAssembly.GetType("UnityEditor.ConsoleWindow");
        if (consoleWindowType == null)
        {
            return null;
        }

        FieldInfo consoleWindowFieldInfo = consoleWindowType.GetField("ms_ConsoleWindow", BindingFlags.Static | BindingFlags.NonPublic);
        if (consoleWindowFieldInfo == null)
        {
            return null;
        }

        EditorWindow consoleWindow = consoleWindowFieldInfo.GetValue(null) as EditorWindow;
        if (consoleWindow == null)
        {
            return null;
        }

        if (consoleWindow != EditorWindow.focusedWindow)
        {
            return null;
        }

        FieldInfo activeTextFieldInfo = consoleWindowType.GetField("m_ActiveText", BindingFlags.Instance | BindingFlags.NonPublic);
        if (activeTextFieldInfo == null)
        {
            return null;
        }

        return (string)activeTextFieldInfo.GetValue(consoleWindow);
    }
}