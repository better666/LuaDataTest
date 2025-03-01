using System.Text;

public class LuaBytecodeConverter 
{
    private static readonly string LUAC_PATH = @"E:\Downloads\lua-5.3_Win64_bin\luac53.exe";

    public static void ConvertToLuaBytecode(string csvPath, string outputPath)
    {
        var data = CsvReader.ReadCharacterData(csvPath);
        
        // 生成优化后的Lua源代码
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

        // 保存临时Lua源代码文件
        var tempLuaFile = Path.GetTempFileName() + ".lua";
        File.WriteAllText(tempLuaFile, sb.ToString());
        
        try 
        {
            if (!File.Exists(LUAC_PATH))
            {
                throw new FileNotFoundException($"找不到Lua编译器: {LUAC_PATH}");
            }

            // 获取当前目录下的输出路径
            var currentDirOutputPath = Path.Combine(Directory.GetCurrentDirectory(), outputPath);

            // 调用luac编译为字节码
            var process = new System.Diagnostics.Process
            {
                StartInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = LUAC_PATH,
                    Arguments = $"-o \"{currentDirOutputPath}.luac\" \"{tempLuaFile}\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    WorkingDirectory = Path.GetDirectoryName(LUAC_PATH)
                }
            };

            process.Start();
            var error = process.StandardError.ReadToEnd();
            process.WaitForExit();
            
            if (process.ExitCode != 0)
            {
                throw new Exception($"Lua编译失败: {error}");
            }

            // 生成加载器Lua文件
            var loaderContent = $@"-- 加载字节码文件
local bytecode = io.open(""{Path.GetFileName(outputPath)}.luac"", ""rb"")
if not bytecode then
    error(""无法打开字节码文件"")
end
local data = load(bytecode:read(""*all""))()
bytecode:close()
return data";

            // 保存加载器文件
            File.WriteAllText($"{currentDirOutputPath}.lua", loaderContent);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"编译Lua字节码时出错: {ex.Message}");
            throw;
        }
        finally
        {
            // 清理临时文件
            if (File.Exists(tempLuaFile))
            {
                File.Delete(tempLuaFile);
            }
        }
    }
} 