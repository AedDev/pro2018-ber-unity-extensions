using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Player))]
public class PlayerDataInspector : Editor
{
    private Player player;
    private PlayerData[] playerData;
    private int playerDataIndex;

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

        if (playerData == null)
            return;

        playerDataIndex = EditorGUILayout.Popup(playerDataIndex, playerData.ToStringArray());

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save"))
        {
            player.playerDataId = playerData[playerDataIndex].ID;
            EditorUtility.SetDirty(target);
        }

        if (GUILayout.Button("Reload"))
        {
            ReloadPlayerData();
        }
        EditorGUILayout.EndHorizontal();
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
