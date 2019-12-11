using System.IO;
using ETModel;
using UnityEditor;
using UnityEngine;

namespace ETEditor
{
    public class GlobalProtoEditor : EditorWindow
    {
        const string path = @"./Assets/Res/Config/GlobalProto.txt";

        private GlobalProto globalProto;

        [MenuItem("Tools/全局配置")]
        public static void ShowWindow()
        {
            GetWindow<GlobalProtoEditor>("全局配置");
        }

        public void Awake()
        {
            if (File.Exists(path))
            {
                this.globalProto = JsonHelper.FromJson<GlobalProto>(File.ReadAllText(path));
            }
            else
            {
                this.globalProto = new GlobalProto();
            }
        }

        public void OnGUI()
        {
            GUILayout.Space(10);
            this.globalProto.AssetBundleServerUrl = EditorGUILayout.TextField("资源路径:", this.globalProto.AssetBundleServerUrl);
            this.globalProto.Address = EditorGUILayout.TextField("服务器地址:", this.globalProto.Address);
            EditorGUILayout.LabelField("游戏大版本号:" + BundleHelper.LargeVersion);
            if (GUILayout.Button("保存",GUILayout.Height(20)))
            {
                File.WriteAllText(path, JsonHelper.ToJson(this.globalProto));
                AssetDatabase.Refresh();
            }

            if (GUILayout.Button("一键切换至本地", GUILayout.Height(20)))
            {
                this.globalProto.Address = NetHelper.GetAddressIPs()[1] + ":10002";

                File.WriteAllText(path, JsonHelper.ToJson(this.globalProto));

                AssetDatabase.Refresh();
            }

            GUILayout.Space(20);

            if (GUILayout.Button("一键切换至外网", GUILayout.Height(20)))
            {
                this.globalProto.Address = "119.23.74.108" + ":10002";

                this.globalProto.AssetBundleServerUrl = "http://119.23.74.108:8080/";//8088

                File.WriteAllText(path, JsonHelper.ToJson(this.globalProto));

                AssetDatabase.Refresh();
            }
        }
    }
}
