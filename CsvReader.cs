using XLua;
using System.IO;

[LuaCallCSharp]
public class CsvReader
{
    public static Dictionary<int, CharacterData> ReadCharacterData(string filePath)
    {
        var result = new Dictionary<int, CharacterData>();
        var lines = File.ReadAllLines(filePath);
        
        // 跳过表头
        for (int i = 1; i < lines.Length; i++)
        {
            var parts = lines[i].Split(',');
            var character = new CharacterData
            {
                ID = int.Parse(parts[0]),
                Name = parts[1],
                HP = int.Parse(parts[2]),
                Attack = int.Parse(parts[3]),
                Defense = int.Parse(parts[4]),
                Speed = int.Parse(parts[5]),
                Luck = int.Parse(parts[6]),
                Magic = int.Parse(parts[7]),
                Resistance = int.Parse(parts[8]),
                Agility = int.Parse(parts[9])
            };
            result[character.ID] = character;
        }
        return result;
    }
} 