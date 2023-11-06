using SuperUnityBuild.BuildTool;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace BuildAutomation
{
    public static class BuildHelper
    {
        public static void PerformBuild() 
        {
            Debug.Log("Preparing Automation...");
            Dictionary<string, string> args = GetValidatedOptions();
            foreach (var arg in args)
            {
                Debug.Log("CommandLineArg > " + arg.Key+":"+arg.Value);
            }

            if (!SetActiveSettings(args["buildPreset"]))
            {
                Debug.LogError("Set Active Settings Failed");
            }

            Debug.Log("Performing Build Automation Start!");
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
                Debug.Log("Build Preset Changed : " + assetBuildSettings.name);
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
                return false;
            }
        }

        private static readonly string Eol = Environment.NewLine;
        private static readonly string[] Secrets =
            {"androidKeystorePass", "androidKeyaliasName", "androidKeyaliasPass"};

        private static Dictionary<string, string> GetValidatedOptions()
        {
            ParseCommandLineArguments(out Dictionary<string, string> validatedOptions);
            return validatedOptions;
        }

        private static void ParseCommandLineArguments(out Dictionary<string, string> providedArguments)
        {
            providedArguments = new Dictionary<string, string>();
            string[] args = Environment.GetCommandLineArgs();

            Console.WriteLine(
                $"{Eol}" +
                $"###########################{Eol}" +
                $"#    Parsing settings     #{Eol}" +
                $"###########################{Eol}" +
                $"{Eol}"
            );

            // Extract flags with optional values
            for (int current = 0, next = 1; current < args.Length; current++, next++)
            {
                // Parse flag
                bool isFlag = args[current].StartsWith("-");
                if (!isFlag) continue;
                string flag = args[current].TrimStart('-');

                // Parse optional value
                bool flagHasValue = next < args.Length && !args[next].StartsWith("-");
                string value = flagHasValue ? args[next].TrimStart('-') : "";
                bool secret = Secrets.Contains(flag);
                string displayValue = secret ? "*HIDDEN*" : "\"" + value + "\"";

                // Assign
                Console.WriteLine($"Found flag \"{flag}\" with value {displayValue}.");
                providedArguments.Add(flag, value);
            }
        }

        #endregion
    }
}
