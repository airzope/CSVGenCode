using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace CSVGenCode {
    public class ConfigHelper {

        public static void ReadConfig(string ConfigFile) {
            var content = File.ReadAllLines(ConfigFile);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < content.Length; i++) {
                var line = content[i];
                line = line.TrimStart();
                if (line.StartsWith("#")) {
                    continue;
                }
                sb.Append(line);
            }
            var str = sb.ToString();
            var map = str.Split(new string[] { "$$" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var km in map) {
                var strs = km.Split('=');
                if (strs.Length < 2) {
                    Debug.LogError("ConfigError!!: " + km);
                    return;
                }
                var key = strs[0].Trim();
                var val = strs[1].Trim();
                DealConfig(key, val);
            }
        }

        static void DealConfig(string key, string val) {
            if (string.IsNullOrEmpty(key))
                return;
            switch (key) {
                case "Type2FuncNameMap": {
                        var map = ParseMap(val);
                        GlobalVal.Type2FuncNameMap = map;
                    }
                    break;
                case "Type2CodeTypeMap": {
                        var map = ParseMap(val);
                        GlobalVal.Type2CodeTypeMap = map;
                    }
                    break;
                case "StructNamePrefix": {
                        GlobalVal.StructNamePrefix = val.Trim();
                    }
                    break;
                case "ClassNamePrefix": {
                        GlobalVal.ClassNamePrefix = val.Trim();
                    }
                    break;
                default:
                    Debug.LogError("Error Key = " + key);
                    break;
            }
        }

        static Dictionary<string, string> ParseMap(string val) {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            var lines = val.Split(';');
            foreach (var item in lines) {
                if (string.IsNullOrEmpty(item))
                    continue;
                var strs = item.Split(':');
                if (strs.Length < 2) {
                    Debug.LogError("ConfigError!!: " + val);
                    return ret;
                }
                ret.Add(strs[0].Trim(), strs[1].Trim());
            }
            return ret;
        }
    }
}
