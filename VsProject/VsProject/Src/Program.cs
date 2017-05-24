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
        bool hasNoArgs = args.Length == 0;
        if (hasNoArgs) {
            const string numFileName = "num.txt";
            const string configFilePrefix = "config{0}.txt";
            //模拟输入
            var numStr = File.ReadAllText("num.txt");
            int num = 0;
            if (int.TryParse(numStr, out num)) {
                num = num % 2;
            } else {
                Debug.LogError("hasNoArgs and num.txt parse error" + numStr);
                return;
            }
            File.WriteAllText(numFileName,(num +1).ToString());
            args = File.ReadAllLines(string.Format(configFilePrefix, num));
        }
        foreach (var arg in args) {
            DealParams(arg);
        }
        if (isHelp) {
            return;
        }

        if (isExl2Csv) {
            if ( CheckInvalid(InputDir, "InputDir", "i") ||
                CheckInvalid(OutputDir, "OutputDir", "o")||
                CheckInvalid(ConfigFile, "ConfigFile", "r")
                ) {
                Console.Read();
                return;
            }
            ConfigHelper.ReadConfig(_P(ConfigFile));
            var gener = new CSVGenCode.Exl2CSV();
            gener.ConvertCSV(DealInputDir(), _P(OutputDir));
        } else {
            if (CheckInvalid(TempletPath, "TempletPath", "t") ||
                CheckInvalid(InputDir, "InputDir", "i") ||
                CheckInvalid(OutputDir, "OutputDir", "o") ||
                CheckInvalid(ConfigFile, "ConfigFile", "r")
                ) {
                Console.Read();
                return;
            }
            ConfigHelper.ReadConfig(_P(ConfigFile));
            var gener = new CSVGenCode.CSVToCppCodeGen();
            gener.GenCode(DealInputDir(), _P(OutputDir), _P(TempletPath));
        }
        if (hasNoArgs) {
            Console.Read();
        }
    }

    static private List<string> DealInputDir() {
        var inputs = InputDir.Split('|');
        List<string> inputDirs = new List<string>();
        foreach (var input in inputs) {
            inputDirs.Add(_P(input));
        }
        foreach (var input in inputDirs) {
            Debug.LogError(input);
        }
        Debug.LogError("---------------------");
        return inputDirs;
    }
    static bool CheckInvalid(string val, string valName, string key) {
        if (string.IsNullOrEmpty(val)) {
            Debug.LogError("Error: missing val " + valName + " please use -" + key + "=XxxxXx to input the val");
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
-xls2csv Convert XLS to CSV format
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
        ParseParam(para, "-xls2csv", Exl2CSV);
        ParseParam(para, "-i", ref InputDir);
        ParseParam(para, "-o", ref OutputDir);
        ParseParam(para, "-t", ref TempletPath);
        ParseParam(para, "-r", ref ConfigFile);
    }


    static bool isExl2Csv = false;
    static void Exl2CSV() {
        isExl2Csv = true;
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
