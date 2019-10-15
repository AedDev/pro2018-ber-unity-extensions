using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DatabaseWindow : EditorWindow
{
    private string dbPath;

    [MenuItem("Unterricht/Database Connection")]
    public static void OpenWindow()
    {
        var w = CreateInstance<DatabaseWindow>();
        w.Show();
    }

    private void OnGUI()
    {
        GUILayout.Space(5);

        // Database file selection
        GUILayout.BeginHorizontal();
        {
            // Label and disabled text field for file path
            GUILayout.Label("Database:", GUILayout.Width(70));
            EditorGUI.BeginDisabledGroup(true);
            {
                GUILayout.TextField(dbPath);
            }
            EditorGUI.EndDisabledGroup();

            // Open file dialog button
            if (GUILayout.Button("...", GUILayout.Width(20), GUILayout.Height(13)))
            {
                string defaultDirectory = Application.dataPath;
                string[] fileFilter = new string[] { "SQLite Database", "sqlite,db" };

                dbPath = EditorUtility.OpenFilePanelWithFilters("Open Database", defaultDirectory, fileFilter);
            }
        }
        GUILayout.EndHorizontal();

        // Database connect button
        GUILayout.BeginHorizontal();
        {
            EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(dbPath) || !File.Exists(dbPath));
            {
                if (GUILayout.Button("Connect"))
                {
                    
                }
            }
            EditorGUI.EndDisabledGroup();
        }
        GUILayout.EndHorizontal();
    }
}
