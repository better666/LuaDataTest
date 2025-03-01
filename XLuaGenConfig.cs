using System.Collections.Generic;
using XLua;

public static class XLuaGenConfig
{
    [LuaCallCSharp]
    public static List<System.Type> LuaCallCSharp = new List<System.Type>()
    {
        typeof(CharacterData),
        typeof(Dictionary<int, CharacterData>)
    };
} 