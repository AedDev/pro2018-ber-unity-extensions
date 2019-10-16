using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite;

public class DatabaseManager : MonoBehaviour
{
    public string databasePath;
    public SQLiteConnection connection;

    private static DatabaseManager _instance;
    public static DatabaseManager Instance
    {
        get
        {
            if (!_instance)
            {
                return _instance = new GameObject("DatabaseManager", typeof(DatabaseManager)).GetComponent<DatabaseManager>();
            }
            else
            {
                return _instance;
            }
        }
    }

    public bool IsConnected
    {
        get => connection != null;
    }
    
    private void Awake()
    {
        if (_instance)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            _instance = this;
        }
    }

    public void Connect()
    {
        if (IsConnected)
            return;

        SQLiteConnectionString connectionString = new SQLiteConnectionString(databasePath);
        connection = new SQLiteConnection(connectionString);
    }

    public void Disconnect()
    {
        if (!IsConnected)
            return;

        connection.Close();
        connection = null;
    }
}
