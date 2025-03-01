using System.Text;

public class TestDataGenerator
{
    private static readonly string[] PrefixNames = { "Ancient", "Dark", "Light", "Fire", "Ice", "Storm", "Earth", "Wind", "Holy", "Shadow" };
    private static readonly string[] BaseNames = { "Warrior", "Mage", "Archer", "Knight", "Assassin", "Priest", "Berserker", "Paladin", "Ranger", "Necromancer" };
    private static readonly string[] SuffixNames = { "Elite", "Master", "Lord", "King", "Queen", "Prince", "Guardian", "Slayer", "Champion", "Legend" };
    private static readonly Random random = new Random(42); // 固定种子以保证可重复性

    public static void GenerateTestData(string csvPath, int count = 10000)
    {
        using (var writer = new StreamWriter(csvPath, false, Encoding.UTF8))
        {
            // 写入表头
            writer.WriteLine("ID,Name,HP,Attack,Defense,Speed,Luck,Magic,Resistance,Agility");

            // 生成数据行
            for (int i = 1; i <= count; i++)
            {
                var name = GenerateRandomName();
                var hp = random.Next(50, 200);
                var attack = random.Next(10, 50);
                var defense = random.Next(5, 40);
                var speed = random.Next(1, 20);
                var luck = random.Next(1, 100);
                var magic = random.Next(10, 60);
                var resistance = random.Next(5, 35);
                var agility = random.Next(1, 25);

                writer.WriteLine($"{i},{name},{hp},{attack},{defense},{speed},{luck},{magic},{resistance},{agility}");
            }
        }
    }

    private static string GenerateRandomName()
    {
        var prefix = PrefixNames[random.Next(PrefixNames.Length)];
        var baseName = BaseNames[random.Next(BaseNames.Length)];
        var suffix = SuffixNames[random.Next(SuffixNames.Length)];
        return $"{prefix} {baseName} {suffix}";
    }
} 