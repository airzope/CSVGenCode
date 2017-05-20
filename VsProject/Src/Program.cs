using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
#endif

using CSVGenCode;
#if UNITY_EDITOR
//生成CPP 代码
public class EditorGenCodeTool {
    static string TempletPath = Path.Combine(Application.dataPath, "../CSVToCpp/Templet");
    static string InputDir = Path.Combine(Application.dataPath, "../CSVToCpp/Input");
    static string OutputDir = Path.Combine(Application.dataPath, "../CSVToCpp/Output");
    static string ConfigFile = Path.Combine(Application.dataPath, "../CSVToCpp/KeywordMapRule.txt");
    [MenuItem("Tools/CSVGenCode")]
    public static void CSVGenCode() {
        var gener = new CSVGenCode.CSVToCppCodeGen();
        gener.GenCode(InputDir, OutputDir, TempletPath, ConfigFile);
    }
}
#else

class Program {
    static string TempletPath = null;
    static string InputDir = null;
    static string OutputDir = null;
    static string ConfigFile = null;

    static void Main(string[] args) {
        if (args.Length == 0) {
            args = new string[] {
                "-i=../Input",
                "-o=../Config/Cpp/Output",
                "-t=../Config/Cpp/Templet",
                "-r=../Config/Cpp/KeywordMapRule.txt"
            };
        }
        foreach (var arg in args) {
            DealParams(arg);
        }
        if (isHelp) {
            return;
        }
        if (CheckInvalid(TempletPath, "TempletPath", "t") ||
            CheckInvalid(InputDir, "InputDir", "i") ||
            CheckInvalid(OutputDir, "OutputDir", "o") ||
            CheckInvalid(ConfigFile, "ConfigFile", "r")
            ) {
            Console.Read();
            return;
        }
        var gener = new CSVGenCode.CSVToCppCodeGen();
        gener.GenCode(_P(InputDir), _P(OutputDir), _P(TempletPath), _P(ConfigFile));
    }
    static bool CheckInvalid(string val, string valName,string key) {
        if (string.IsNullOrEmpty(val)) {
            Debug.LogError("Error: missing val "+valName +" please use -" + key + "=XxxxXx to input the val");
            return true;
        }
        return false;
    }
    static string _P(string path) {
        return Path.Combine(Environment.CurrentDirectory, path);
    }

    static bool isHelp = false;
    static void Help() {
        isHelp = true;
        string msg = @"
-i      Input directory which contained csv files
-o      Output directory which contained Generated Code
-t      Templet directory which contained code templet
-r      Rule file for generate code
-h      show command info
-help   sameAs the -h
-----------------------------------------------------
example 
CSVCodeGen.exe -i=../Input -o=../Config/Cpp/Output -t=../Config/Cpp/Templet -r=../Config/Cpp/KeywordMapRule.txt 
";
        Debug.Log(msg);
        Console.Read();
    }
    static void DealParams(string para) {
        ParseParam(para, "-h", Help);
        ParseParam(para, "-help", Help);
        ParseParam(para, "-i", ref InputDir);
        ParseParam(para, "-o", ref OutputDir);
        ParseParam(para, "-t", ref TempletPath);
        ParseParam(para, "-r", ref ConfigFile);
    }

    private static void ParseParam(string para, string tag, ref string val, char sep = '=') {
        var tempPara = para.ToLower();
        if (tempPara.StartsWith(tag)) {
            var strs = para.Split(sep);
            if (strs.Length < 2) {
                Debug.LogError("Param parse error:" + para);
            }
            val = strs[1].Trim();
        }
    }
    private static void ParseParam(string para, string tag, Action CallBack) {
        if (para.Equals(tag)) {
            if (CallBack != null) {
                CallBack();
            }
        }
    }

}
#endif
