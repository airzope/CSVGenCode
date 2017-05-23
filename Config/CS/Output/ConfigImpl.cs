﻿/********************************************************************
**       This head file is generated by program,                   **
**            Please do not change it directly.                    **
********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CSVGenCode {
    public enum ECSVReadResult {
        Succ,
        FailedUnkonw,
        FileOpenError,
        FormatError,
    }
    public class CCFG_SJScene {
        public ECSVReadResult Load(string path) {
            var csvText = File.ReadAllText(path, Encoding.UTF8);
            if (string.IsNullOrEmpty(csvText))
                return ECSVReadResult.FileOpenError;
            string[][] grid = CsvParser.Parse(csvText);
            if (grid == null )  return ECSVReadResult.FormatError;
            int rowLen = grid.Length;
            if (rowLen < 1)  return ECSVReadResult.FormatError;
            int colLen = grid[0].Length;
            if (colLen < 1)   return ECSVReadResult.FormatError;
            var parser = new StringParser(grid);
            int keyIdx = 0;
            for (int i = 3; i < rowLen; i++) {
                var info = new SCFG_SJScene();
                var key = parser.ReadUInt32(i, keyIdx);
                int colIdx = 0;
                info.SceneLevel = parser.ReadUInt32(i, colIdx++);
                info.SceneName = parser.ReadString(i, colIdx++);
                info.AutoStart = parser.ReadUInt32(i, colIdx++);
                info.TicketFee = parser.ReadUInt32(i, colIdx++);
                info.MinCoin = parser.ReadUInt32(i, colIdx++);
                info.MaxCoin = parser.ReadUInt32(i, colIdx++);
                info.BaseCoin = parser.ReadUInt32(i, colIdx++);
                info.EscapeShapreRate = parser.ReadUInt32(i, colIdx++);
                info.MaxLose = parser.ReadUInt32(i, colIdx++);
                info.EnableDouble = parser.ReadUInt32(i, colIdx++);
                info.DoubleLimit = parser.ReadUInt32(i, colIdx++);
                info.BaseMulity = parser.ReadUInt32(i, colIdx++);
                info.BaseExp = parser.ReadUInt32(i, colIdx++);
                info.GameType = parser.ReadUInt32(i, colIdx++);
                info.MatchGame = parser.ReadUInt32(i, colIdx++);
                info.FirstGiveTime = parser.ReadUInt32(i, colIdx++);
                info.GiveTime = parser.ReadUInt32(i, colIdx++);
                info.LogFlag = parser.ReadUInt32(i, colIdx++);
                info.ErrLogFlag = parser.ReadUInt32(i, colIdx++);
                info.LogPath = parser.ReadString(i, colIdx++);
                info.RobotCfg = parser.ReadString(i, colIdx++);

                _mapContent.Add(key, info);
            }
            return ECSVReadResult.Succ;
        }

        public SCFG_SJScene Find(uint	SceneLevel)
        {
            SCFG_SJScene ret = null;
            _mapContent.TryGetValue(SceneLevel,out ret);
            return ret;
        }

        public void Clear() { _mapContent.Clear(); }
        public void ToString(StringBuilder sb) {
            sb.AppendLine("---------------------------------------- SJScene---------------------------------------");
            foreach (var item in _mapContent) {
                var info = item.Value;
                sb.Append("\t\t" + info.SceneLevel);
                sb.Append("\t\t" + info.SceneName);
                sb.Append("\t\t" + info.AutoStart);
                sb.Append("\t\t" + info.TicketFee);
                sb.Append("\t\t" + info.MinCoin);
                sb.Append("\t\t" + info.MaxCoin);
                sb.Append("\t\t" + info.BaseCoin);
                sb.Append("\t\t" + info.EscapeShapreRate);
                sb.Append("\t\t" + info.MaxLose);
                sb.Append("\t\t" + info.EnableDouble);
                sb.Append("\t\t" + info.DoubleLimit);
                sb.Append("\t\t" + info.BaseMulity);
                sb.Append("\t\t" + info.BaseExp);
                sb.Append("\t\t" + info.GameType);
                sb.Append("\t\t" + info.MatchGame);
                sb.Append("\t\t" + info.FirstGiveTime);
                sb.Append("\t\t" + info.GiveTime);
                sb.Append("\t\t" + info.LogFlag);
                sb.Append("\t\t" + info.ErrLogFlag);
                sb.Append("\t\t" + info.LogPath);
                sb.Append("\t\t" + info.RobotCfg);

                sb.AppendLine();
            }
        }

        private Dictionary<uint, SCFG_SJScene> _mapContent = new Dictionary<uint, SCFG_SJScene>();
    }
    public class CCFG_VipLevelTest {
        public ECSVReadResult Load(string path) {
            var csvText = File.ReadAllText(path, Encoding.UTF8);
            if (string.IsNullOrEmpty(csvText))
                return ECSVReadResult.FileOpenError;
            string[][] grid = CsvParser.Parse(csvText);
            if (grid == null )  return ECSVReadResult.FormatError;
            int rowLen = grid.Length;
            if (rowLen < 1)  return ECSVReadResult.FormatError;
            int colLen = grid[0].Length;
            if (colLen < 1)   return ECSVReadResult.FormatError;
            var parser = new StringParser(grid);
            int keyIdx = 0;
            for (int i = 3; i < rowLen; i++) {
                var info = new SCFG_VipLevelTest();
                var key = parser.ReadInt32(i, keyIdx);
                int colIdx = 0;
                info.id1 = parser.ReadInt32(i, colIdx++);
                info.id2 = parser.ReadString(i, colIdx++);
                info.id3 = parser.ReadInt32(i, colIdx++);
                info.id4 = parser.ReadString(i, colIdx++);
                info.id6 = parser.ReadString(i, colIdx++);

                _mapContent.Add(key, info);
            }
            return ECSVReadResult.Succ;
        }

        public SCFG_VipLevelTest Find(int	id1)
        {
            SCFG_VipLevelTest ret = null;
            _mapContent.TryGetValue(id1,out ret);
            return ret;
        }

        public void Clear() { _mapContent.Clear(); }
        public void ToString(StringBuilder sb) {
            sb.AppendLine("---------------------------------------- VipLevelTest---------------------------------------");
            foreach (var item in _mapContent) {
                var info = item.Value;
                sb.Append("\t\t" + info.id1);
                sb.Append("\t\t" + info.id2);
                sb.Append("\t\t" + info.id3);
                sb.Append("\t\t" + info.id4);
                sb.Append("\t\t" + info.id6);

                sb.AppendLine();
            }
        }

        private Dictionary<int, SCFG_VipLevelTest> _mapContent = new Dictionary<int, SCFG_VipLevelTest>();
    }
    public class CCFG_VipLevelTest2 {
        public ECSVReadResult Load(string path) {
            var csvText = File.ReadAllText(path, Encoding.UTF8);
            if (string.IsNullOrEmpty(csvText))
                return ECSVReadResult.FileOpenError;
            string[][] grid = CsvParser.Parse(csvText);
            if (grid == null )  return ECSVReadResult.FormatError;
            int rowLen = grid.Length;
            if (rowLen < 1)  return ECSVReadResult.FormatError;
            int colLen = grid[0].Length;
            if (colLen < 1)   return ECSVReadResult.FormatError;
            var parser = new StringParser(grid);
            int keyIdx = 0;
            for (int i = 3; i < rowLen; i++) {
                var info = new SCFG_VipLevelTest2();
                var key = parser.ReadInt32(i, keyIdx);
                int colIdx = 0;
                info.id1 = parser.ReadInt32(i, colIdx++);
                info.id2 = parser.ReadString(i, colIdx++);
                info.id3 = parser.ReadInt32(i, colIdx++);
                info.id4 = parser.ReadString(i, colIdx++);
                info.id6 = parser.ReadString(i, colIdx++);

                _mapContent.Add(key, info);
            }
            return ECSVReadResult.Succ;
        }

        public SCFG_VipLevelTest2 Find(int	id1)
        {
            SCFG_VipLevelTest2 ret = null;
            _mapContent.TryGetValue(id1,out ret);
            return ret;
        }

        public void Clear() { _mapContent.Clear(); }
        public void ToString(StringBuilder sb) {
            sb.AppendLine("---------------------------------------- VipLevelTest2---------------------------------------");
            foreach (var item in _mapContent) {
                var info = item.Value;
                sb.Append("\t\t" + info.id1);
                sb.Append("\t\t" + info.id2);
                sb.Append("\t\t" + info.id3);
                sb.Append("\t\t" + info.id4);
                sb.Append("\t\t" + info.id6);

                sb.AppendLine();
            }
        }

        private Dictionary<int, SCFG_VipLevelTest2> _mapContent = new Dictionary<int, SCFG_VipLevelTest2>();
    }

}