namespace GPG.Multiplayer.Quazal
{
    using System;
    using System.Collections;

    public interface ISqlExec
    {
        bool ExecuteQuery(string queryname, Hashtable parameters);
        bool GetBool(string queryname, Hashtable parameters);
        int GetCount(string queryname, Hashtable parameters);
        Hashtable GetQueryData(string queryname, Hashtable parameters);
        Hashtable MakeParams(params object[] p);
    }
}

