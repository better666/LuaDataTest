using XLua;
using System.IO;

[LuaCallCSharp]
public class CharacterData
{
    public int ID { get; set; }
    public string Name { get; set; }
    public int HP { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int Speed { get; set; }
    public int Luck { get; set; }
    public int Magic { get; set; }
    public int Resistance { get; set; }
    public int Agility { get; set; }

    public override string ToString()
    {
        return $"ID: {ID}, Name: {Name}, HP: {HP}, Attack: {Attack}, Defense: {Defense}, Speed: {Speed}";
    }
}
