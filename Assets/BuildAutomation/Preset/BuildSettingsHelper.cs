using SuperUnityBuild.BuildTool;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace EditorTool.BuildAutomation
{
    public static class BuildSettingsHelper
    {
        public static void PerformBuild() 
        {
            string[] args = System.Environment.GetCommandLineArgs();
            foreach (var arg in args)
            {
                Debug.Log("Command Arg : " + arg);
            }
            
            BuildCLI.PerformBuild();
        }

        #region All Presets
        const string Preset1 = "BuildAutomation/Preset/SetToPreset_1";
        const string Preset1_Name = "BA_BuildPreset_1";
        [MenuItem(Preset1)]
        static void SetPreset_1()
        {
            if (!SetActiveSettings(Preset1_Name))
            {
                Debug.LogError("Set Active Settings Failed");
            }
        }
        [MenuItem(Preset1, true)]
        private static bool SetPreset_1Validate()
        {
            Menu.SetChecked(Preset1, BuildSettings.instance.name == Preset1_Name);
            return true;
        }

        const string Preset2 = "BuildAutomation/Preset/SetToPreset_2";
        const string Preset2_Name = "BA_BuildPreset_2";
        [MenuItem(Preset2)]
        static void Check2()
        {
            if (!SetActiveSettings("BA_BuildPreset_2"))
            {
                Debug.LogError("Set Active Settings Failed");
            }
        }
        [MenuItem(Preset2, true)]
        private static bool SetPreset_2Validate()
        {
            Menu.SetChecked(Preset2, BuildSettings.instance.name == Preset2_Name);
            return true;
        }
        #endregion

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