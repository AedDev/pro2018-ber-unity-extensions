using SQLite;
using System;
using UnityEngine;

public enum PlayerClass
{
    Bard,
    Warrior,
    Mage,
    Ranger,
    Priest
}

[Serializable]
[Table("PlayerData")]
public class PlayerData
{
    [Column("ID")]
    public int ID { get; set; }

    [Column("Name")]
    public string Name { get; set; }

    [Column("Health")]
    public int Health { get; set; }

    [Column("Mana")]
    public int Mana { get; set; }

    [Column("Credits")]
    public int Credits { get; set; }

    [Column("Class")]
    public PlayerClass Class { get; set; }

    public override string ToString()
    {
        return $"Player (ID: {ID}; Name: {Name}; Health: {Health}; Mana: {Mana}; Credits: {Credits}; Class: {Class})";
    }
}
