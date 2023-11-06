using SuperUnityBuild.BuildTool;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace BuildAutomation
{
    public static class BuildHelper
    {
        public static void PerformBuild() 
        {
            string[] args = System.Environment.GetCommandLineArgs();
            foreach (var arg in args)
            {
                Debug.Log("Command Arg : " + arg);
            }

            if (!SetActiveSettings("BA_BuildPreset_2"))
            {
                Debug.LogError("Set Active Settings Failed");
            }

            BuildCLI.PerformBuild();
        }

        #region Helper Method
        public static bool SetActiveSettings(string settingsName)
        {
            try
            {
                var guids = AssetDatabase.FindAssets(settingsName);
                if (guids.Length <= 0)
                {
                    Debug.LogError("Build Setting Not Exist : " + settingsName);
                    return false;
                }

                var assetBuildSettings = AssetDatabase.LoadAssetAtPath<BuildSettings>(AssetDatabase.GUIDToAssetPath(guids[0]));
                assetBuildSettings.SetAsActivePreset();
                return true;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return false;
            }
        }

        #endregion
    }
}
