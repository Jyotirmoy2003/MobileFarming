using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;

public class FixCSProject
{
    [DidReloadScripts]
    private static void OnScriptsReloaded()
    {
        string projectPath = Directory.GetCurrentDirectory();
        string[] csprojFiles = Directory.GetFiles(projectPath, "*.csproj");

        foreach (var file in csprojFiles)
        {
            string content = File.ReadAllText(file);
            if (content.Contains("<TargetFrameworkVersion>net4.7.1</TargetFrameworkVersion>"))
            {
                content = content.Replace("<TargetFramework>net4.7.1</TargetFramework>", "<TargetFramework>net7.0</TargetFramework>");
                File.WriteAllText(file, content);
            }
        }
    }
}
