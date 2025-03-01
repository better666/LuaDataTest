using System.IO;
using System.Text;

public class BinaryDataConverter
{
    public static void ConvertToBinary(string csvPath, string binaryPath)
    {
        var data = CsvReader.ReadCharacterData(csvPath);
        using (var writer = new BinaryWriter(File.Open(binaryPath, FileMode.Create)))
        {
            writer.Write(data.Count);
            foreach (var pair in data)
            {
                var character = pair.Value;
                writer.Write(character.ID);
                writer.Write(character.Name);
                writer.Write(character.HP);
                writer.Write(character.Attack);
                writer.Write(character.Defense);
                writer.Write(character.Speed);
            }
        }
    }

    public static Dictionary<int, CharacterData> LoadFromBinary(string binaryPath)
    {
        var result = new Dictionary<int, CharacterData>();
        using (var reader = new BinaryReader(File.Open(binaryPath, FileMode.Open)))
        {
            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                var character = new CharacterData
                {
                    ID = reader.ReadInt32(),
                    Name = reader.ReadString(),
                    HP = reader.ReadInt32(),
                    Attack = reader.ReadInt32(),
                    Defense = reader.ReadInt32(),
                    Speed = reader.ReadInt32()
                };
                result[character.ID] = character;
            }
        }
        return result;
    }
} 