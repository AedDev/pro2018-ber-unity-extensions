using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(Player))]
public class PlayerDataInspector : Editor
{
    private Player player;
    private PlayerData[] playerData;
    private int playerDataIndex;

    private bool HasData
    {
        get => playerData != null && playerData.Length > 0;
    }

    private void OnEnable()
    {
        player = target as Player;
        ReloadPlayerData();
    }

    public override void OnInspectorGUI()
    {
        if (!DatabaseWindow.IsConnected)
        {
            EditorGUILayout.HelpBox("No database connection", MessageType.Error);
            return;
        }

        if (HasData)
        {
            playerDataIndex = EditorGUILayout.Popup(playerDataIndex + 1, playerData.ToStringArray("None")) - 1;
        }
        else
        {
            EditorGUILayout.Popup(0, new string[] { "<No Entries>" });
        }
        
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUI.BeginDisabledGroup(!HasData);
            {
                if (GUILayout.Button("Save"))
                {
                    if (playerDataIndex >= 0)
                    {
                        player.playerDataId = playerData[playerDataIndex].ID;
                    }
                    else
                    {
                        player.playerDataId = -1;
                    }
                    
                    EditorUtility.SetDirty(target);
                }
            }
            EditorGUI.EndDisabledGroup();

            if (GUILayout.Button("Reload"))
            {
                ReloadPlayerData();
            }
        }
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("New Entry"))
        {
            CreateNewEntry();
        }
    }

    private void CreateNewEntry()
    {
        PlayerData data = new PlayerData()
        {
            ID = 0,
            Name = "New Player"
        };

        DatabaseWindow.Connection.Insert(data, typeof(PlayerData));
        ReloadPlayerData();
    }

    private void ReloadPlayerData()
    {
        if (!DatabaseWindow.IsConnected)
            return;

        playerData = DatabaseWindow.Connection.Query<PlayerData>("SELECT * FROM PlayerData").ToArray();
        
        //for (int i = 0; i < playerData.Length; i++)
        //{
        //    if (playerData[i].ID == player.playerDataId)
        //    {
        //        playerDataIndex = i;
        //        break;
        //    }
        //}
        //
        // ^ this is equal to the following

        playerDataIndex = playerData.FindIndex((data) =>
        {
            return data.ID == player.playerDataId;
        });
    }
}
