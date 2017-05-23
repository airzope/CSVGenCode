using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace CSVGenCode {

    public class GlobalVal {
        public static Dictionary<string, string> Type2FuncNameMap = new Dictionary<string, string>();
        public static Dictionary<string, string> Type2CodeTypeMap = new Dictionary<string, string>();
        public static string StructNamePrefix = "";
        public static string ClassNamePrefix = "";
        public static string CSVType2FuncName(string typeName) {
            if (Type2FuncNameMap.ContainsKey(typeName)) {
                return Type2FuncNameMap[typeName];
            } else {
                Debug.LogError("GetConvertFunc ErrorType " + typeName);
                return "";
            }
        }
        public static string CSVType2CodeType(string typeName) {
            if (Type2CodeTypeMap.ContainsKey(typeName)) {
                return Type2CodeTypeMap[typeName];
            } else {
                Debug.LogError("CSVType2CodeTypeMap ErrorType " + typeName);
                return typeName;
            }
        }
        public static int CommentIdx = 0;
        public static int AttrNameIdx = 1;
        public static int TypeIdx = 2;
        public static int MaxHeadIdx = 3;

    }
}
