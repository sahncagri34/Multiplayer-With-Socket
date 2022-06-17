using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScriptExtends {

    public static E TypeToEnum<E,Type>(string deleteStr) {
        int mi = typeof(Type).Name.LastIndexOf(deleteStr);
        string name = typeof(Type).Name.Substring(0, mi);
        return (E)Enum.Parse(typeof(E), name);
    }


}
