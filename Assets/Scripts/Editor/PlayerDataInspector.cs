using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(Player))]
public class PlayerDataInspector : Editor
{
    public static readonly float LABEL_WIDTH = 50;

    private Player player;
    private PlayerData[] playerData;
    private int playerDataIndex;

    private GUIStyle redButton;

    private bool HasData
    {
        get => playerData != null && playerData.Length > 0;
    }

    private PlayerData CurrentPlayerData
    {
        get => HasData && playerDataIndex >= 0 ? playerData[playerDataIndex] : null;
    }

    private void OnEnable()
    {
        redButton = new GUIStyle("button");
        redButton.normal.textColor = Color.red;

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

        EditorGUILayout.LabelField("Select Data:", EditorStyles.boldLabel);
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
                if (GUILayout.Button("Save Asset"))
                {
                    if (playerDataIndex >= 0)
                    {
                        player.playerDataId = playerData[playerDataIndex].ID.Value;
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

        EditorGUILayout.BeginHorizontal();
        {
            // New Entry Button
            if (GUILayout.Button("New Blank Entry"))
            {
                CreateNewEntry();
            }

            // Delete Entry Button
            EditorGUI.BeginDisabledGroup(CurrentPlayerData == null);
            {
                if (GUILayout.Button("Delete Entry", redButton))
                {
                    DeleteCurrentEntry();
                }
            }
            EditorGUI.EndDisabledGroup();
        }
        EditorGUILayout.EndHorizontal();

        // My dirty hack ♥♥
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        // Player Data Fields
        if (CurrentPlayerData != null)
        {
            HorizontalIntField("ID", LABEL_WIDTH, CurrentPlayerData.ID.Value, true);
            CurrentPlayerData.Name = HorizontalStringField("Name", LABEL_WIDTH, CurrentPlayerData.Name);
            CurrentPlayerData.Health = HorizontalIntField("Health", LABEL_WIDTH, CurrentPlayerData.Health);
            CurrentPlayerData.Mana = HorizontalIntField("Mana", LABEL_WIDTH, CurrentPlayerData.Mana);
            CurrentPlayerData.Credits = HorizontalIntField("Credits", LABEL_WIDTH, CurrentPlayerData.Credits);
            CurrentPlayerData.Class = (PlayerClass)HorizontalEnumField("Class", LABEL_WIDTH, CurrentPlayerData.Class);

            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Save Data"))
                {
                    SaveCurrentPlayerData();
                }

                if (GUILayout.Button("Save As Copy"))
                {
                    CreateNewEntry(CurrentPlayerData);
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        else
        {
            EditorGUILayout.HelpBox("No data selected", MessageType.Warning);
        }
    }

    private void CreateNewEntry(PlayerData data = null)
    {
        if (data == null)
        {
            data = new PlayerData()
            {
                ID =  null,
                Name = "New Player"
            };
        } else
        {
            data = new PlayerData()
            {
                ID = null,
                Name = "Copy of " + data.Name,
                Health = data.Health,
                Mana = data.Mana,
                Credits = data.Credits,
                Class = data.Class
            };
        }

        DatabaseWindow.Connection.Insert(data, typeof(PlayerData));
        ReloadPlayerData();
    }

    private void SaveCurrentPlayerData()
    {
        if (CurrentPlayerData == null)
            return;

        DatabaseWindow.Connection.Update(CurrentPlayerData, typeof(PlayerData));
    }

    private void DeleteCurrentEntry()
    {
        if (CurrentPlayerData == null)
            return;

        DatabaseWindow.Connection.Delete<PlayerData>(CurrentPlayerData.ID.Value);
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

    private int HorizontalIntField(string label, float labelWidth, int value, bool disabled = false)
    {
        int returnValue = 0;
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField(label, GUILayout.Width(labelWidth));
            EditorGUI.BeginDisabledGroup(disabled);
            {
                returnValue = EditorGUILayout.IntField(value);
            }
            EditorGUI.EndDisabledGroup();
        }
        EditorGUILayout.EndHorizontal();

        return returnValue;
    }

    private string HorizontalStringField(string label, float labelWidth, string value, bool disabled = false)
    {
        string returnValue = string.Empty;
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField(label, GUILayout.Width(labelWidth));
            EditorGUI.BeginDisabledGroup(disabled);
            {
                returnValue = EditorGUILayout.TextField(value);
            }
            EditorGUI.EndDisabledGroup();
        }
        EditorGUILayout.EndHorizontal();

        return returnValue;
    }

    private Enum HorizontalEnumField(string label, float labelWidth, Enum value, bool disabled = false)
    {
        Enum returnValue = default;
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField(label, GUILayout.Width(labelWidth));
            EditorGUI.BeginDisabledGroup(disabled);
            {
                returnValue = EditorGUILayout.EnumFlagsField(value);
            }
            EditorGUI.EndDisabledGroup();
        }
        EditorGUILayout.EndHorizontal();

        return returnValue;
    }
}
