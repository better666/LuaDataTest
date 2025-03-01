// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using XLua;

class Program
{
    static void Main()
    {
        // 生成测试数据
        TestDataGenerator.GenerateTestData("TestData.csv", 10000);

        // 首先将数据转换为各种格式
        CsvToLua.ConvertFile("TestData.csv", "TestData.lua");
        CsvToLuaOptimized.ConvertToLua("TestData.csv", "TestDataOptimized.lua");
        BinaryDataConverter.ConvertToBinary("TestData.csv", "TestData.bin");
        LuaBinaryConverter.ConvertToBinary("TestData.csv", "TestDataBinary.bin");
        LuaBytecodeConverter.ConvertToLuaBytecode("TestData.csv", "TestDataBytecode");

        // 创建LuaEnv
        LuaEnv luaenv = new LuaEnv();
        
        Console.WriteLine("=== 测试方法1：直接从CSV加载 ===");
        // 测试C#加载CSV的性能
        GC.Collect();
        var startMemory = GC.GetTotalMemory(true);
        var watch = Stopwatch.StartNew();
        
        var csharpData = CsvReader.ReadCharacterData("TestData.csv");
        
        watch.Stop();
        var endMemory = GC.GetTotalMemory(false);
        Console.WriteLine("C# CSV Loading Performance:");
        Console.WriteLine($"Loading Time: {watch.ElapsedMilliseconds}ms");
        Console.WriteLine($"Memory Usage: {(endMemory - startMemory) / 1024}KB\n");

        Console.WriteLine("=== 测试方法2：从二进制文件加载 ===");
        // 测试C#加载二进制文件的性能
        GC.Collect();
        startMemory = GC.GetTotalMemory(true);
        watch = Stopwatch.StartNew();
        
        var binaryData = BinaryDataConverter.LoadFromBinary("TestData.bin");
        
        watch.Stop();
        endMemory = GC.GetTotalMemory(false);
        Console.WriteLine("C# Binary Loading Performance:");
        Console.WriteLine($"Loading Time: {watch.ElapsedMilliseconds}ms");
        Console.WriteLine($"Memory Usage: {(endMemory - startMemory) / 1024}KB\n");

        // 注入数据到Lua环境
        luaenv.Global.Set("csharpData", csharpData);

        // 在Lua中进行性能测试
        var testResults = luaenv.DoString(@"
            local results = {}
            
            -- 测试方法3：从C#传入的数据（使用生成的代码）
            collectgarbage('collect')
            local startMemory1 = collectgarbage('count')
            -- C#数据已经通过Set注入，不需要加载时间
            local loadTime1 = 0
            
            -- 访问C#数据的测试（测试10条数据）
            local startTime1 = os.clock()
            local sum1 = 0
            for i = 1, 10000 do
                local char = csharpData[i]
                sum1 = sum1 + char.HP + char.Attack + char.Defense + char.Speed + 
                       char.Luck + char.Magic + char.Resistance + char.Agility
            end
            local accessTime1 = os.clock() - startTime1
            local endMemory1 = collectgarbage('count')
            
            results.csharp = {
                loadTime = loadTime1,
                accessTime = accessTime1,
                memoryUsage = endMemory1 - startMemory1,
                sum = sum1
            }

            -- 测试方法4：加载普通Lua数据
            collectgarbage('collect')
            local startMemory2 = collectgarbage('count')
            local startLoadTime2 = os.clock()
            local luaData = require 'TestData'
            local loadTime2 = os.clock() - startLoadTime2
            
            -- 访问Lua数据的测试（测试10条数据）
            local startTime2 = os.clock()
            local sum2 = 0
            for i = 1, 10000 do
                local char = luaData[i]
                sum2 = sum2 + char.HP + char.Attack + char.Defense + char.Speed +
                       char.Luck + char.Magic + char.Resistance + char.Agility
            end
            local accessTime2 = os.clock() - startTime2
            local endMemory2 = collectgarbage('count')
            
            results.lua = {
                loadTime = loadTime2,
                accessTime = accessTime2,
                memoryUsage = endMemory2 - startMemory2,
                sum = sum2
            }

            -- 测试方法5：加载优化后的Lua数据（使用元表）
            collectgarbage('collect')
            local startMemory3 = collectgarbage('count')
            local startLoadTime3 = os.clock()
            local luaDataOpt = require 'TestDataOptimized'
            local loadTime3 = os.clock() - startLoadTime3
            
            -- 访问优化后的Lua数据测试
            local startTime3 = os.clock()
            local sum3 = 0
            for i = 1, 10000 do
                local char = luaDataOpt[i]
                sum3 = sum3 + char.HP + char.Attack + char.Defense + char.Speed +
                       char.Luck + char.Magic + char.Resistance + char.Agility
            end
            local accessTime3 = os.clock() - startTime3
            local endMemory3 = collectgarbage('count')
            
            results.lua_opt = {
                loadTime = loadTime3,
                accessTime = accessTime3,
                memoryUsage = endMemory3 - startMemory3,
                sum = sum3
            }

            -- 测试方法6：加载Lua二进制数据
            collectgarbage('collect')
            local startMemory4 = collectgarbage('count')
            local startLoadTime4 = os.clock()
            local luaDataBin = require 'TestDataBinary'
            local loadTime4 = os.clock() - startLoadTime4
            
            -- 访问二进制数据测试
            local startTime4 = os.clock()
            local sum4 = 0
            for i = 1, 10000 do
                local char = luaDataBin[i]
                sum4 = sum4 + char.HP + char.Attack + char.Defense + char.Speed +
                       char.Luck + char.Magic + char.Resistance + char.Agility
            end
            local accessTime4 = os.clock() - startTime4
            local endMemory4 = collectgarbage('count')
            
            results.lua_bin = {
                loadTime = loadTime4,
                accessTime = accessTime4,
                memoryUsage = endMemory4 - startMemory4,
                sum = sum4
            }

            -- 测试方法7：加载Lua字节码数据
            collectgarbage('collect')
            local startMemory5 = collectgarbage('count')
            local startLoadTime5 = os.clock()
            local luaDataBytecode = require 'TestDataBytecode'
            local loadTime5 = os.clock() - startLoadTime5
            
            -- 访问字节码数据测试
            local startTime5 = os.clock()
            local sum5 = 0
            for i = 1, 10000 do
                local char = luaDataBytecode[i]
                sum5 = sum5 + char.HP + char.Attack + char.Defense + char.Speed +
                       char.Luck + char.Magic + char.Resistance + char.Agility
            end
            local accessTime5 = os.clock() - startTime5
            local endMemory5 = collectgarbage('count')
            
            results.lua_bytecode = {
                loadTime = loadTime5,
                accessTime = accessTime5,
                memoryUsage = endMemory5 - startMemory5,
                sum = sum5
            }
            
            return results
        ");

        // 获取测试结果
        var results = testResults[0] as LuaTable;
        var csharpResult = results.Get<LuaTable>("csharp");
        var luaResult = results.Get<LuaTable>("lua");
        var luaOptResult = results.Get<LuaTable>("lua_opt");
        var luaBinResult = results.Get<LuaTable>("lua_bin");
        var luaBytecodeResult = results.Get<LuaTable>("lua_bytecode");

        Console.WriteLine("=== 测试方法3：C# Data in Lua (Using Generated Code) ===");
        Console.WriteLine($"Loading Time: {csharpResult.Get<double>("loadTime") * 1000:F2}ms");
        Console.WriteLine($"Access Time: {csharpResult.Get<double>("accessTime") * 1000:F2}ms");
        Console.WriteLine($"Additional Memory in Lua: {csharpResult.Get<double>("memoryUsage"):F2}KB");
        Console.WriteLine($"Verification Sum: {csharpResult.Get<int>("sum")}\n");

        Console.WriteLine("=== 测试方法4：Lua Native Data ===");
        Console.WriteLine($"Loading Time: {luaResult.Get<double>("loadTime") * 1000:F2}ms");
        Console.WriteLine($"Access Time: {luaResult.Get<double>("accessTime") * 1000:F2}ms");
        Console.WriteLine($"Memory Usage: {luaResult.Get<double>("memoryUsage"):F2}KB");
        Console.WriteLine($"Verification Sum: {luaResult.Get<int>("sum")}\n");

        Console.WriteLine("=== 测试方法5：Optimized Lua Data (Using Metatable) ===");
        Console.WriteLine($"Loading Time: {luaOptResult.Get<double>("loadTime") * 1000:F2}ms");
        Console.WriteLine($"Access Time: {luaOptResult.Get<double>("accessTime") * 1000:F2}ms");
        Console.WriteLine($"Memory Usage: {luaOptResult.Get<double>("memoryUsage"):F2}KB");
        Console.WriteLine($"Verification Sum: {luaOptResult.Get<int>("sum")}\n");

        Console.WriteLine("=== 测试方法6：Lua Binary Data ===");
        Console.WriteLine($"Loading Time: {luaBinResult.Get<double>("loadTime") * 1000:F2}ms");
        Console.WriteLine($"Access Time: {luaBinResult.Get<double>("accessTime") * 1000:F2}ms");
        Console.WriteLine($"Memory Usage: {luaBinResult.Get<double>("memoryUsage"):F2}KB");
        Console.WriteLine($"Verification Sum: {luaBinResult.Get<int>("sum")}\n");

        Console.WriteLine("=== 测试方法7：Lua Bytecode Data ===");
        Console.WriteLine($"Loading Time: {luaBytecodeResult.Get<double>("loadTime") * 1000:F2}ms");
        Console.WriteLine($"Access Time: {luaBytecodeResult.Get<double>("accessTime") * 1000:F2}ms");
        Console.WriteLine($"Memory Usage: {luaBytecodeResult.Get<double>("memoryUsage"):F2}KB");
        Console.WriteLine($"Verification Sum: {luaBytecodeResult.Get<int>("sum")}");

        // 清理资源
        luaenv.Dispose();
    }
}
