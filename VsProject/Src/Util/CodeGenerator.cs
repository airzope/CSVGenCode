/*
MIT License
Copyright(c) 2017 JiepengTan
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
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

    public class GenTypeInfo {
        public string RawFileName;        //原始文件名字   
        public string FileName;           //文件名字 大驼峰法         
        public string KeyTypeColIdx;      //CSV主键类型  所在列的小标 0开始         
        public string KeyTypeName;        //CSV主键类型   
        public string KeyType2FuncName;   //CSV中主键属性到方法的映射 参考Type2FuncNameMap
        public string KeyName;            //CSV主键名字
        public string StructName;         //结构体名字
        public string ClsName;            //类名                    
        public List<ColInfo> colInfo = new List<ColInfo>();

        public void Init() {
            FileName = RawFileName.ToBigCamel();
            StructName = GlobalVal.StructNamePrefix + FileName;
            ClsName = GlobalVal.ClassNamePrefix + FileName;
            KeyType2FuncName = GlobalVal.CSVType2FuncName(KeyTypeName);
        }
    }
    public class ColInfo {
        public string AttriCommment;  //CSV中属性的名字
        public string AttriTypeName;  //CSV中属性的类型
        public string AttriName;      //CSV中属性的注释
        //gen by rule
        public string AttriType2FuncName;//CSV中属性到方法的映射 参考Type2FuncNameMap
        public void Init() {
            AttriType2FuncName = GlobalVal.CSVType2FuncName(AttriTypeName);
            AttriTypeName = GlobalVal.CSVType2CodeType(AttriTypeName);
        }
    }
    public class SpliteInfo {
        public string content;//Exclude Tag
        public string RawStr;//Include Tag
        public int startIdx;
        public int endIdx;
    }
    public class CSVToCppCodeGen {
        private string OutputPath = "";
        private string clsTagBegin = "#Begin_Replace_Tag_Class";
        private string clsTagEnd = "#End_Replace_Tag_Class";
        private string attrTagBegin = "#Begin_Replace_Tag_Attri";
        private string attrTagEnd = "#End_Replace_Tag_Attri";
        private List<GenTypeInfo> infos = new List<GenTypeInfo>();
        private int totalFileNum = 0;

        private GenTypeInfo curTypeInfo;//当前处理的类的信息
        private ColInfo curColInfo;//当前处理的类的信息
        private bool isSkipEnter = true;//是否跳过换行符
        private int skipNum;//跳过的字数
        const string mainKeyTag = "#";

        string GetOutPath(string path) {
            var fileName = Path.GetFileName(path);
            return Path.Combine(OutputPath, fileName);
        }
        public void GenCode(string InputPath, string OutputPath, string TempletPath, string ConfigFile) {
            ConfigHelper.ReadConfig(ConfigFile);
            this.OutputPath = OutputPath;
            Util.Walk(InputPath, "*.csv", ReadCsv);
            Util.Walk(TempletPath, "*.*", GenCodeFormTemplet);
            Debug.Log("Done config file" + totalFileNum);
        }

        void ReadCsv(string path) {
            var csvText = File.ReadAllText(path, Encoding.UTF8);
            var fileName = Path.GetFileName(path);
            fileName = fileName.Split('.')[0];
            Debug.Log(fileName);
            totalFileNum++;
            if (string.IsNullOrEmpty(csvText))
                return;
            string[][] grid = CsvParser.Parse(csvText);
            if (grid.Length < 3) {
                Debug.LogError("CSV2Cpp CodeGen Csv table grid.Length < 3");
                return;
            }
            int len = grid[0].Length;
            foreach (var row in grid) {
                if (row.Length != len) {
                    Debug.LogError(string.Format("{2} csv parse error not each row's len is the same row.Length{0} != titleLen{1}", row.Length, len, path));
                }
            }
            var info = new GenTypeInfo();
            var comment = grid[GlobalVal.CommentIdx];
            var attrName = grid[GlobalVal.AttrNameIdx];
            var attrType = grid[GlobalVal.TypeIdx];
            info.RawFileName = fileName;
            int keyIdx = GetMainKeyIdx(attrName);
            info.KeyTypeName = GlobalVal.CSVType2CodeType(attrType[keyIdx]);
            info.KeyTypeColIdx = keyIdx.ToString();
            info.KeyName = attrName[0];
            for (int i = 0; i < len; i++) {
                var col = new ColInfo();
                col.AttriCommment = comment[i];
                col.AttriName = attrName[i];
                col.AttriTypeName = attrType[i];
                col.Init();
                info.colInfo.Add(col);
            }
            info.Init();
            infos.Add(info);
            //Print(grid);
            return;
        }
        //获取主键下标 默认为第一列
        int GetMainKeyIdx(string[] attrNames) {
            for (int i = 0; i < attrNames.Length; i++) {
                if (attrNames[i].StartsWith(mainKeyTag)) {
                    attrNames[i] = attrNames[i].Replace(mainKeyTag, "");
                    return i;
                }
            }
            return 0;
        }
        private void Print(string[][] grid) {
            StringBuilder sb = new StringBuilder();
            foreach (var row in grid) {
                foreach (var col in row) {
                    sb.Append(col + " ");
                }
                sb.AppendLine();
            }
            Debug.Log(sb.ToString());
        }
        void GenCodeFormTemplet(string path) {
            var content = File.ReadAllText(path);
            skipNum = GetSkipNum(content);
            var ret = ParseFile(content);
            SaveTo(ret, GetOutPath(path));
        }
        int GetSkipNum(string content) {
            if (!isSkipEnter)
                return 0;
            if (content.Contains("\r\n")) {
                return 2;
            } else {
                return 1;
            }
        }
        //
        string ParseFile(string content) {
            return ParseAllClsTag(content);
        }

        //替换所有类标志
        string ParseAllClsTag(string content) {
            var ret = Replace(content, clsTagBegin, clsTagEnd, (str) => {
                //正对一个类标志 需要遍历所有的类 
                StringBuilder sb = new StringBuilder();
                foreach (var info in infos) {
                    curTypeInfo = info;
                    var _replaceStr = ParseAllAttrTag(str, curTypeInfo);
                    _replaceStr = ReplaceClsInfo(_replaceStr, curTypeInfo);
                    sb.Append(_replaceStr);
                }
                return sb.ToString();
            });
            return ret;
        }

        //替换所有属性标志
        string ParseAllAttrTag(string content, GenTypeInfo typeInfo) {
            var ret = Replace(content, attrTagBegin, attrTagEnd, (str) => {
                //正对一个属性标志 需要遍历所有的属性 
                StringBuilder sb = new StringBuilder();
                foreach (var colInfo in typeInfo.colInfo) {
                    curColInfo = colInfo;
                    var _replaceStr = ReplaceAttrInfo(str, curTypeInfo, curColInfo);
                    sb.Append(_replaceStr);
                }
                return sb.ToString();
            });
            return ret;
        }
        string ReplaceClsInfo(string info, GenTypeInfo typeInfo) {
            info = info.Replace("#RawFileName", typeInfo.RawFileName);
            info = info.Replace("#FileName", typeInfo.FileName);
            info = info.Replace("#KeyTypeColIdx", typeInfo.KeyTypeColIdx);
            info = info.Replace("#KeyTypeName", typeInfo.KeyTypeName);
            info = info.Replace("#KeyName", typeInfo.KeyName);
            info = info.Replace("#StructName", typeInfo.StructName);
            info = info.Replace("#ClsName", typeInfo.ClsName);
            info = info.Replace("#KeyType2FuncName", typeInfo.KeyType2FuncName);

            return info;
        }
        string ReplaceAttrInfo(string info, GenTypeInfo typeInfo, ColInfo colInfo) {
            info = ReplaceClsInfo(info, typeInfo);

            info = info.Replace("#AttriName", colInfo.AttriName);
            info = info.Replace("#AttriType2FuncName", colInfo.AttriType2FuncName);
            info = info.Replace("#AttriTypeName", colInfo.AttriTypeName);
            info = info.Replace("#AttriCommment", colInfo.AttriCommment);
            return info;
        }

        string Replace(string content, string tagBegin, string tagEnd, Func<string, string> DealFunc) {
            var infos = SplitToBlocks(content, tagBegin, tagEnd);
            foreach (var info in infos) {
                var FinalStr = DealFunc(info.content);
                if (!content.Contains(info.RawStr)) {
                    Debug.LogError(string.Format("Replace error: !content.Contains(info.RawStr)\nStr ={ 0}\nSubStr ={1}", content, info.RawStr));
                    return content;
                }
                content = content.Replace(info.RawStr, FinalStr);
            }
            return content;
        }
        List<SpliteInfo> SplitToBlocks(string content, string begTag, string endTag) {
            var allCls = content.Split(new string[] { begTag }, StringSplitOptions.None);
            List<SpliteInfo> ret = new List<SpliteInfo>();
            int startIdx = 0;
            int endIdx = 0;
            while (endIdx < content.Length) {
                startIdx = content.IndexOf((string)begTag, endIdx);
                if (startIdx == -1)
                    break;
                SpliteInfo info = new SpliteInfo();
                endIdx = content.IndexOf((string)endTag, startIdx);
                info.startIdx = startIdx;
                info.content = content.Substring(startIdx + begTag.Length + skipNum, endIdx - ( startIdx + begTag.Length ) - skipNum);
                info.endIdx = endIdx + endTag.Length;
                info.RawStr = content.Substring(startIdx, endIdx + endTag.Length - startIdx);
                ret.Add(info);
            }
            return ret;
        }
        void SaveTo(string content, string outPath) {
            var outDir = Path.GetDirectoryName(outPath);
            if (!Directory.Exists(outDir)) {
                Directory.CreateDirectory(outDir);
            }
            File.WriteAllText(outPath, content, Encoding.UTF8);
        }
    }
}

