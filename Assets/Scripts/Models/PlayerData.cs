using SQLite;
using System;
using UnityEngine;

[Flags]
public enum PlayerClass
{
    Bard    = 1,  // 0000 0001
    Warrior = 2,  // 0000 0010
    Mage    = 4,  // 0000 0100
    Ranger  = 8,  // 0000 1000
    Priest  = 16  // 0001 0000
}

[Serializable]
[Table("PlayerData")]
public class PlayerData
{
    [Column("ID"), PrimaryKey]
    public int? ID { get; set; }

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
