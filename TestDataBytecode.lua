-- 加载字节码文件
local bytecode = io.open("TestDataBytecode.luac", "rb")
if not bytecode then
    error("无法打开字节码文件")
end
local data = load(bytecode:read("*all"))()
bytecode:close()
return data