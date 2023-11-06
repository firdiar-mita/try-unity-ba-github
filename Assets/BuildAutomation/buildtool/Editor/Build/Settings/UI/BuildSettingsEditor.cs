using UnityEditor;
using UnityEngine;

namespace SuperUnityBuild.BuildTool
{
    [CustomEditor(typeof(BuildSettings))]
    public class BuildSettingsEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            BuildSettings targetObj = (BuildSettings)this.target;
            GUI.enabled = BuildSettings.instance != targetObj;
            if (GUILayout.Button("Set as Active Preset", GUILayout.MinHeight(10)))
            {
                targetObj.SetAsActivePreset();
            }
            GUI.enabled = true;

            GUILayout.Space(3);

            if (GUILayout.Button("Open in SuperUnityBuild", GUILayout.ExpandWidth(true), GUILayout.MinHeight(30)))
            {
                if(targetObj != null)
                {
                    //Open this asset in the UnityBuildWindow
                    targetObj.OpenInUnityBuildWindow();
                }
            }
        }
    }
}
