using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CSVGenCode;

class Program {
    static void Main(string[] args) {
        var configMgr = ConfigMgr.Instance;
        var ret = configMgr.LoadAll();
        if (ret != ECSVReadResult.Succ) {
            Debug.LogError(string.Format("Load file error:file = {0} reason {1}  {2}", configMgr.CurReadPath, ret.ToString(),configMgr.ExceptionMsg));
        } else {
            Debug.LogError(configMgr.PrintAll());
        }
        var scene = configMgr.m_SJScene.Find(1040101);
        Debug.LogError(scene.ToString());
        Console.Read();
    }
}
