using SQLite;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataService : IService<PlayerData>
{
    public bool Create(PlayerData model)
    {
        try
        {
            DatabaseManager.Instance.connection.Insert(model, typeof(PlayerData));

            return true;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            return false;
        }
    }

    public bool Delete(PlayerData model)
    {
        try
        {
            DatabaseManager.Instance.connection.Delete<PlayerData>(model);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            return false;
        }
    }

    public List<PlayerData> Find(Predicate<PlayerData> predicate)
    {
        try
        {
            return GetAll().FindAll(predicate);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            return new List<PlayerData>();
        }
    }

    public PlayerData Get(int id)
    {
        if (id < 0)
            return null;

        try
        {
            return DatabaseManager.Instance.connection.Get<PlayerData>(id);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            return null;
        }
    }

    public List<PlayerData> GetAll()
    {
        try
        {
            return DatabaseManager.Instance.connection.Query<PlayerData>("SELECT * FROM PlayerData");
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            return new List<PlayerData>();
        }
    }

    public bool Update(PlayerData model)
    {
        try
        {
            DatabaseManager.Instance.connection.Update(model, typeof(PlayerData));
            return true;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            return false;
        }
    }
}
