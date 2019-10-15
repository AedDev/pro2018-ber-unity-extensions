﻿using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SQLite;

public class DatabaseWindow : EditorWindow
{
    /// <summary>
    /// SQLite database connection
    /// </summary>
    public static SQLiteConnection Connection
    {
        get;
        private set;
    }

    /// <summary>
    /// Returns <code>true</code> if a connection is established, otherwise <code>false</code>
    /// </summary>
    public static bool IsConnected
    {
        get
        {
            return Connection != null;
        }
    }

    /// <summary>
    /// Path to database file
    /// </summary>
    private string dbPath;

    [MenuItem("Unterricht/Database Connection")]
    public static void OpenWindow()
    {
        var w = CreateInstance<DatabaseWindow>();
        w.Show();
    }

    private void OnEnable()
    {
        dbPath = EditorPrefs.GetString("DB_PATH");
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
        
        GUILayout.BeginHorizontal();
        {
            // Database connect button
            EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(dbPath) || !File.Exists(dbPath) || IsConnected);
            {
                if (GUILayout.Button("Connect"))
                {
                    Connect();
                }
            }
            EditorGUI.EndDisabledGroup();

            // Database disconnect button
            EditorGUI.BeginDisabledGroup(!IsConnected);
            {
                if (GUILayout.Button("Disconnect"))
                {
                    Disconnect();
                }
            }
            EditorGUI.EndDisabledGroup();
        }
        GUILayout.EndHorizontal();
    }

    private void Connect()
    {
        EditorPrefs.SetString("DB_PATH", dbPath);

        SQLiteConnectionString connectionString = new SQLiteConnectionString(dbPath);
        Connection = new SQLiteConnection(connectionString);

        
    }

    private void Disconnect()
    {
        if (Connection == null)
            return;

        Connection.Close();
        Connection = null;
    }
}