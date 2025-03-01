using System.Text;

public class CsvToLua
{
    public static string ConvertToLua(string csvContent)
    {
        var lines = csvContent.Split('\n');
        if (lines.Length < 2) return "return {}";

        var headers = lines[0].Trim().Split(',');
        var sb = new StringBuilder();
        sb.AppendLine("return {");

        for (int i = 1; i < lines.Length; i++)
        {
            var line = lines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue;

            var values = line.Split(',');
            sb.AppendLine($"    [{values[0]}] = {{");
            
            for (int j = 0; j < headers.Length; j++)
            {
                var value = values[j];
                var header = headers[j];
                
                // 如果是数字，直接输出，否则加引号作为字符串
                bool isNumber = int.TryParse(value, out _);
                sb.AppendLine($"        {header} = {(isNumber ? value : $"'{value}'")},");
            }
            
            sb.AppendLine("    },");
        }

        sb.AppendLine("}");
        return sb.ToString();
    }

    public static void ConvertFile(string csvPath, string luaPath)
    {
        var csvContent = File.ReadAllText(csvPath);
        var luaContent = ConvertToLua(csvContent);
        File.WriteAllText(luaPath, luaContent);
    }
} 