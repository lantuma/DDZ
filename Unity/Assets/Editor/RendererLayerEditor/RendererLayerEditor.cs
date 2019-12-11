using System;
using ETModel;
using UnityEditor;

namespace ETEditor
{
    [CustomEditor(typeof(RendererLayer)), CanEditMultipleObjects]
    public class RendererLayerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            RendererLayer rendererLayer = (RendererLayer)target;
            
            EditorGUILayout.Space();

            if (Enum.TryParse<RendererLayer.UiSortLayer>(rendererLayer.SortingLayer, out var selected))
            {
                rendererLayer.SortingLayer = EditorGUILayout.EnumPopup("Sorting Layer",selected).ToString();
            }

            rendererLayer.OrderInLayer = EditorGUILayout.IntField("Order In Layer", rendererLayer.OrderInLayer);

            if(!rendererLayer.IsTransparentQueue())
            EditorGUILayout.HelpBox("Render Queue is not 'Transparent' or not equal 3000, it may not desired effect.",MessageType.Warning);
        }
    }
}