using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class EditorHelper : MonoBehaviour {

	[MenuItem("Assets/BatchCreateArtistFont")]
	static public void BatchCreateArtistFont()
	{
		ArtistFont.BatchCreateArtistFont();
	}
}
