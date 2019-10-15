using SQLite;

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
    public int Class { get; set; }
}
