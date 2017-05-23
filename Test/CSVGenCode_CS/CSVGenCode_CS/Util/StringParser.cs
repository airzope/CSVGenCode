using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CSVGenCode {
    //无类型检测 TODO
    public class StringParser {
        public StringParser(string[][] contents, char seprator = '|') {
            this.contents = contents;
            this.ARRAY_SEPRATOR = seprator;
        }
        private string[][] contents;//内容
        public char ARRAY_SEPRATOR;
        public float ReadFloat(int row, int col) { return float.Parse(contents[row][col]); }
        public double ReadDouble(int row, int col) { return double.Parse(contents[row][col]); }
        public int ReadInt32(int row, int col) { return int.Parse(contents[row][col]); }
        public uint ReadUInt32(int row, int col) { return uint.Parse(contents[row][col]); }
        public long ReadInt64(int row, int col) { return long.Parse(contents[row][col]); }
        public ulong ReadUInt64(int row, int col) { return ulong.Parse(contents[row][col]); }
        public bool ReadBool(int row, int col) { return bool.Parse(contents[row][col]); }
        public string ReadString(int row, int col) { return contents[row][col]; }

        public int[] ReadArrayInt(int row, int col) {
            var str = ReadString(row, col);
            var allStr = str.Split(ARRAY_SEPRATOR);
            var retVals = new int[allStr.Length];
            for (int i = 0; i < allStr.Length; i++) {
                retVals[i] = int.Parse(allStr[i]);
            }
            return retVals;
        }
        public float[] ReadArrayFloat(int row, int col) {
            var str = ReadString(row, col);
            var allStr = str.Split(ARRAY_SEPRATOR);
            var retVals = new float[allStr.Length];
            for (int i = 0; i < allStr.Length; i++) {
                retVals[i] = float.Parse(allStr[i]);
            }
            return retVals;
        }
    }
}