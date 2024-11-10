using System.IO;
using UnityEditor;
using UnityEngine;

namespace Nix.Core.Editor
{
    public class NixPanelWindow : EditorWindow
    {
        private string packagePath = "Packages/com.nix.package/NixCore/Packages~";
        private string destinationPath = "Assets/NIX";

        [MenuItem("NIX/NIX Panel")]
        public static void ShowWindow()
        {
            GetWindow<NixPanelWindow>("NIX Panel");
        }

        private void OnGUI()
        {
            GUILayout.Label("Available Modules", EditorStyles.boldLabel);

            if (Directory.Exists(packagePath))
            {
                string[] modules = Directory.GetDirectories(packagePath);

                foreach (string modulePath in modules)
                {
                    string moduleName = Path.GetFileName(modulePath);

                    if (GUILayout.Button($"Import {moduleName}"))
                    {
                        ImportModule(modulePath, moduleName);
                    }
                }
            }
            else
            {
                GUILayout.Label("No modules found in package path.");
            }
        }

        private void ImportModule(string modulePath, string moduleName)
        {
            string destinationModulePath = Path.Combine(destinationPath, moduleName);

            if (Directory.Exists(destinationModulePath))
            {
                Debug.LogWarning($"{moduleName} has already been imported.");
                return;
            }

            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }

            FileUtil.CopyFileOrDirectory(modulePath, destinationModulePath);
            AssetDatabase.Refresh();

            Debug.Log($"{moduleName} imported successfully to {destinationPath}.");
        }
    }
}
