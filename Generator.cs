using XLua;
using System;
using System.Collections.Generic;

public static class Generator
{
    [CSharpCallLua]
    public static List<Type> CSharpCallLua = new List<Type>()
    {
        typeof(Dictionary<int, CharacterData>)
    };

    [LuaCallCSharp]
    public static List<Type> LuaCallCSharp = new List<Type>()
    {
        typeof(CharacterData)
    };


} 