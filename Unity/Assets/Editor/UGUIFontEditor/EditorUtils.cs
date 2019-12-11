using UnityEngine;
using System.Collections;
using System.IO;

public class EditorUtils : MonoBehaviour 
{	
	/// <summary>
	/// 
	/// </summary>
	/// <param name="dirName"></param>
	/// <returns>filename</returns>
	public static string SelectObjectPathInfo(ref string dirName)
	{
		if (UnityEditor.Selection.activeInstanceID < 0)
		{
			return "";
		}

		string path = UnityEditor.AssetDatabase.GetAssetPath(UnityEditor.Selection.activeInstanceID);

		dirName = Path.GetDirectoryName(path) + "/";
		return Path.GetFileName(path);
	}
}
