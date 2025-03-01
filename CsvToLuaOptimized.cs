using System.Text;
using System.Linq;

public class CsvToLuaOptimized
{
    public static void ConvertToLua(string csvPath, string luaPath)
    {
        var data = CsvReader.ReadCharacterData(csvPath);
        var sb = new StringBuilder();

        // 定义列名和它们的索引（单行）
        sb.Append("local columns={ID=1,HP=2,Attack=3,Defense=4,Speed=5,Luck=6,Magic=7,")
          .AppendLine("Resistance=8,Agility=9,Name=10}");

        // 定义元表（压缩格式）
        sb.AppendLine("local meta={__index=function(t,k)local i=columns[k]if i then return t[i]end return nil end}");

        // 创建数据表（使用连续数组）
        sb.Append("local data={");
        
        // 确保按ID排序，以生成连续数组
        var sortedData = data.Values.OrderBy(c => c.ID).ToList();
        foreach (var c in sortedData)
        {
            sb.Append("{")
              .Append($"{c.ID},{c.HP},{c.Attack},{c.Defense},{c.Speed},{c.Luck},")
              .Append($"{c.Magic},{c.Resistance},{c.Agility},'{c.Name}'}},");
        }
        sb.AppendLine("}");

        // 设置元表并返回数据（压缩到两行）
        sb.AppendLine("for _,v in pairs(data)do setmetatable(v,meta)end")
          .Append("return data");

        File.WriteAllText(luaPath, sb.ToString());
    }
} 