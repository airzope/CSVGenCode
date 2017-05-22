using Excel;
using System.Data;
using System.IO;
using System.Collections.Generic;
using System;
using System.Text;

namespace CSVGenCode {
    class Exl2CSV {
        const int CLIENT_IDX = 2;
        const int PLATFORM_COL_IDX = 3;
        const int SO_COL_IDX = 4;

        const int COMMENT_IDX = 0;
        const int ATTR_NAM_IDX = 1;
        const int SO_TYPE_IDX = 4;

        public const string LINE_END_TAG = "\r\n";
        public const string CSV_SEP = ",";
        const string IGNORE_tYPE_TAG = "Reject";
        string OutputPath;
        public Exl2CSV() { }
        public class ExlInfo {
            public string headPath;
            public string contentPath;
            public ExlInfo(string typePath, string contentPath) {
                this.contentPath = contentPath;
                this.headPath = typePath;
            }
        }
        public class AttrInfo {
            public string comment;
            public string typeName;
            public string attrName;
            public int idx;
            public AttrInfo(string comment, string typeName, string attrName, int idx) {
                this.comment = comment;
                this.typeName = typeName;
                this.attrName = attrName;
                this.idx = idx;
            }
        }

        public void ConvertCSV(string InputPath, string OutputPath) {
            this.OutputPath = OutputPath;
            var paths = new Dictionary<string, string>();
            Action<string> GetTypeFunc = (path) => {
                Debug.Log(path);
                var fileName = Path.GetFileName(path);
                var dirName = Path.GetDirectoryName(path);
                if (!fileName.Contains("+")) {
                    fileName = fileName.Split('.')[0];
                    paths.Add(Path.Combine(dirName, fileName), path);
                }
            };
            List<ExlInfo> exlInfo = new List<ExlInfo>();
            Action<string> GetContentFunc = (path) => {
                var fileName = Path.GetFileName(path);
                if (fileName.Contains("+")) {
                    var dirName = Path.GetDirectoryName(path);
                    fileName = fileName.Split('.')[0];
                    fileName = fileName.Remove(fileName.IndexOf("+"));
                    var prefixPath = Path.Combine(dirName, fileName);
                    if (paths.ContainsKey(prefixPath)) {
                        exlInfo.Add(new ExlInfo(paths[prefixPath], path));
                    }
                }
            };

            Util.Walk(InputPath, "*.xls|*.xlsx", GetTypeFunc);
            Util.Walk(InputPath, "*.xls|*.xlsx", GetContentFunc);
            foreach (var item in exlInfo) {
                var attrInfo = ExtractAttrInfo(item.headPath, item.contentPath);
                curHeadPath = item.headPath;
                curContentPath = item.contentPath;

                SaveToCSV(attrInfo, item.contentPath);
            }
        }
        static string curContentPath;
        static string curHeadPath;
        //一个是内容信息  一个是类型信息存放在两个不同的文件里面
        public List<AttrInfo> ExtractAttrInfo(string typeExlPath, string contentExlPath) {
            List<AttrInfo> attrInfos = new List<AttrInfo>();
            Dictionary<string, int> tile2Idx = new Dictionary<string, int>();
            {
                var mResultSet = Open(contentExlPath);
                if (mResultSet.Tables.Count < 1) {
                    Debug.LogError("mResultSet.Tables.Count < 1");
                    return null;
                }
                DataTable mSheet = mResultSet.Tables[0];
                if (mSheet.Rows.Count < 1) {
                    Debug.LogError("mResultSet.Rows.Count < 1");
                    return null;
                }
                int rowCount = mSheet.Rows.Count;
                int colCount = mSheet.Columns.Count;

                int i = 0;
                for (int j = 0; j < colCount; j++) {
                    tile2Idx[( mSheet.Rows[i][j] ).ToString()] = j;
                }
            }
            {
                var mResultSet = Open(typeExlPath);
                if (mResultSet.Tables.Count < 1) {
                    Debug.LogError("mResultSet.Tables.Count < 1");
                    return null;
                }
                DataTable mSheet = mResultSet.Tables[0];
                if (mSheet.Rows.Count < 1) {
                    Debug.LogError("mResultSet.Rows.Count < 1");
                    return null;
                }
                if (mSheet.Columns.Count < SO_COL_IDX + 1) {
                    Debug.LogError("mSheet.Columns.Count < SO_COL_IDX + 1");
                    return null;
                }

                int rowCount = mSheet.Rows.Count;
                int colCount = mSheet.Columns.Count;
                for (int i = 0; i < rowCount; i++) {
                    var comment = mSheet.Rows[i][COMMENT_IDX].ToString().Trim();
                    var attrName = mSheet.Rows[i][ATTR_NAM_IDX].ToString().Trim();
                    var soType = mSheet.Rows[i][SO_TYPE_IDX].ToString().Trim();
                    if (string.IsNullOrEmpty(attrName) || string.IsNullOrEmpty(soType)) {
                        continue;
                    }
                    if (!tile2Idx.ContainsKey(comment)) {
                        Debug.LogError(string.Format("{0} do not have col {1}", contentExlPath, comment));
                        continue;
                    }
                    if (soType == IGNORE_tYPE_TAG) {
                        continue;
                    }
                    attrInfos.Add(new AttrInfo(comment, soType, attrName, tile2Idx[comment]));
                }
            }
            return attrInfos;
        }

        public void SaveToCSV(List<AttrInfo> infos, string contentExlPath) {
            curContentPath = contentExlPath;
            var mResultSet = Open(contentExlPath);
            if (mResultSet.Tables.Count < 1)
                return;
            DataTable mSheet = mResultSet.Tables[0];
            if (mSheet.Rows.Count < 1)
                return;

            int rowCount = mSheet.Rows.Count;
            int colCount = mSheet.Columns.Count;
            StringBuilder sb = new StringBuilder();
            WriteHeader(sb, infos);
            WriteContent(infos, mSheet, sb, rowCount, colCount);
            var path = GetOutPutPath(curHeadPath);
            Debug.Log("Done " + path);
            SaveTo(path, sb, Encoding.UTF8);
        }
        private static void WriteContent(List<AttrInfo> infos, DataTable mSheet, StringBuilder sb, int rowCount, int colCount) {
            for (int i = 1; i < rowCount; i++) {
                for (int j = 0; j < infos.Count; j++) {
                    var info = infos[j];
                    var col = info.idx;
                    var str = mSheet.Rows[i][col].ToString();
                    if (str.Contains(",")) {
                        if (!( str.StartsWith("\"") && str.EndsWith("\"") ) && str.Contains("\"")) {
                            Debug.LogError(string.Format("Content error:{0} at {1}", curContentPath, str));
                            return;
                        } else {
                            str = "\"" + str + "\"";
                        }
                    }
                    sb.Append(str);
                    if (j != infos.Count - 1) {
                        sb.Append(CSV_SEP);
                    }
                }
                sb.Append(LINE_END_TAG);
            }
        }

        string GetOutPutPath(string path) {
            var fileName = Path.GetFileName(path);
            fileName = fileName.Split('.')[0];
            return Path.Combine(OutputPath, fileName + ".csv");
        }
        void WriteHeader(StringBuilder sb, List<AttrInfo> infos) {
            for (int i = 0; i < GlobalVal.MaxHeadIdx; i++) {
                if (i == GlobalVal.TypeIdx) {
                    WriteType(sb, infos);
                }
                if (i == GlobalVal.CommentIdx) {
                    WriteComment(sb, infos);
                }
                if (i == GlobalVal.AttrNameIdx) {
                    WriteAttrName(sb, infos);
                }
            }
        }
        void WriteType(StringBuilder sb, List<AttrInfo> infos) {
            for (int i = 0; i < infos.Count; i++) {
                var info = infos[i];
                sb.Append(info.typeName);
                if (i != infos.Count - 1) {
                    sb.Append(CSV_SEP);
                }
            }
            sb.Append(LINE_END_TAG);
        }
        void WriteComment(StringBuilder sb, List<AttrInfo> infos) {
            for (int i = 0; i < infos.Count; i++) {
                var info = infos[i];
                sb.Append(info.comment);
                if (i != infos.Count - 1) {
                    sb.Append(CSV_SEP);
                }
            }
            sb.Append(LINE_END_TAG);
        }
        void WriteAttrName(StringBuilder sb, List<AttrInfo> infos) {
            for (int i = 0; i < infos.Count; i++) {
                var info = infos[i];
                sb.Append(info.attrName);
                if (i != infos.Count - 1) {
                    sb.Append(CSV_SEP);
                }
            }
            sb.Append(LINE_END_TAG);
        }
        public DataSet Open(string excelFile) {
            FileStream mStream = File.Open(excelFile, FileMode.Open, FileAccess.Read);
            IExcelDataReader mExcelReader = null;
            if (excelFile.EndsWith("xls")) {
                mExcelReader = ExcelReaderFactory.CreateBinaryReader(mStream);
            } else {
                mExcelReader = ExcelReaderFactory.CreateOpenXmlReader(mStream);
            }
            return mExcelReader.AsDataSet();
        }
        public void ConvertToCSV(DataSet mResultSet, string CSVPath, Encoding encoding) {
            if (mResultSet.Tables.Count < 1)
                return;

            DataTable mSheet = mResultSet.Tables[0];

            if (mSheet.Rows.Count < 1)
                return;

            int rowCount = mSheet.Rows.Count;
            int colCount = mSheet.Columns.Count;

            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < rowCount; i++) {
                for (int j = 0; j < colCount; j++) {
                    stringBuilder.Append(mSheet.Rows[i][j] + CSV_SEP);
                }
                stringBuilder.Append(LINE_END_TAG);
            }

            SaveTo(CSVPath, stringBuilder, encoding);
        }

        private static void SaveTo(string CSVPath, StringBuilder stringBuilder, Encoding encoding) {
            using (FileStream fileStream = new FileStream(CSVPath, FileMode.Create, FileAccess.Write)) {
                using (TextWriter textWriter = new StreamWriter(fileStream, encoding)) {
                    textWriter.Write(stringBuilder.ToString());
                }
            }
        }
    }
}
