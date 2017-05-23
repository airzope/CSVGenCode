using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVGenCode {

    public static class StringExternal {
        public static String ToMacro(this String name) {
            StringBuilder result = new StringBuilder();
            if (name != null && name.Length > 0) {
                for (int i = 0; i < name.Length - 1; i++) {
                    var s = name[i];
                    var sp = name[i + 1];
                    if (char.IsLower(s) && char.IsUpper(sp)) {
                        result.Append("_");
                    }
                    result.Append(s);
                }
                result.Append(name[name.Length - 1]);
            }
            return result.ToString();
        }

        public static String ToBigCamel(this String name) {
            StringBuilder result = new StringBuilder();
            if (string.IsNullOrEmpty(name)) {
                return "";
            } else if (!name.Contains("_")) {
                return name.Substring(0, 1).ToUpper() + name.Substring(1);
            }
            var camels = name.Split('_');
            foreach (var camel in camels) {
                if (string.IsNullOrEmpty(camel)) {
                    continue;
                }
                result.Append(camel.Substring(0, 1).ToUpper());
                result.Append(camel.Substring(1).ToLower());
            }
            return result.ToString();
        }
    }
}