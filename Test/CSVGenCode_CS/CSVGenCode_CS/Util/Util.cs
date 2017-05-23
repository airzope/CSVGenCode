using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CSVGenCode {
    public class Singleton<T> where T : new() {
        private static T _instance;
        public static T Instance {
            get {
                if (_instance == null) {
                    _instance = Util.CreateInstance(ref _instance);
                }
                return _instance;
            }
        }
    };

    public class Util {
        public static T CreateInstance<T>(ref T ins) where T : new() {
            ins = new T();
            return ins;
        }

        public static void SaveTo(string content, string outPath) {
            var outDir = Path.GetDirectoryName(outPath);
            if (!Directory.Exists(outDir)) {
                Directory.CreateDirectory(outDir);
            }
            File.WriteAllText(outPath, content, Encoding.UTF8);
        }
        public static void Walk(string path, string exts, System.Action<string> callback, bool isEditor = false) {
            bool isAll = string.IsNullOrEmpty(exts) || exts == "*" || exts == "*.*";
            string[] extList = exts.Replace("*", "").Split('|');

            if (Directory.Exists(path)) {
                // 如果选择的是文件夹

                string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories).Where(file => {
                    if (isAll)
                        return true;
                    foreach (var ext in extList) {
                        if (file.EndsWith(ext, StringComparison.OrdinalIgnoreCase)) {
                            return true;
                        }
                    }
                    return false;
                }).ToArray();

                foreach (var item in files) {
                    if (callback != null) {
                        callback(item);
                    }
                }
                if (isEditor) {
#if UNITY_EDITOR
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
#endif
                }
            } else {
                if (isAll) {
                    if (callback != null) {
                        callback(path);
                    }
                } else {
                    // 如果选择的是文件
                    foreach (var ext in extList) {
                        if (path.EndsWith(ext, StringComparison.OrdinalIgnoreCase)) {
                            if (callback != null) {
                                callback(path);
                            }
                        }
                    }
                }
                if (isEditor) {
#if UNITY_EDITOR
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
#endif
                }
            }
        }
    }
}
