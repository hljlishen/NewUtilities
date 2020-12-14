using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utilities.IniSynconizer;

namespace Utilities.Ini
{
    public class IniSynconizer
    {
        private Dictionary<string, object> fileMap = new Dictionary<string, object>();
        public T CreateIniObject<T>(string iniFileName) where T : new()
        {
            var absPath = Path.GetFullPath(iniFileName);
            var key = MakeKey(typeof(T), absPath);
            if (!fileMap.ContainsKey(key))
            {
                var obj = new T();

                LoadSettings(absPath, obj);
                fileMap.Add(key, obj);
            }
            return (T)fileMap[key];
        }

        public void SaveSettings(object obj)
        {
            if(!fileMap.ContainsValue(obj))
                throw new ObjectIsNotCreatedByThisSynconizer("对象不是由本对象创建");

            var fileName = (from pair in fileMap where pair.Value == obj select pair.Key).Single().Split('#').First();

            if (!File.Exists(fileName))
                File.Create(fileName);

            foreach (var p in obj.GetType().GetProperties())
            {
                var keyName = p.Name;
                var value = p.GetValue(obj).ToString();
                IniFileOperator.WriteIniData(fileName, obj.GetType().Name, keyName, value);
            }
        }

        private string MakeKey(Type type, string file) => $"{file}#{type.FullName}";
        private void LoadSettings(string iniFile, object obj)
        {
            if (!File.Exists(iniFile))
                throw new FileNotFoundException("未找到文件", iniFile);

            var absPath = Path.GetFullPath(iniFile);

            foreach (var p in obj.GetType().GetProperties())
            {
                var name = p.Name;
                if (!IniFileOperator.HasKey(absPath, obj.GetType().Name, name))
                    continue;

                var _ = IniFileOperator.ReadIniData(absPath, obj.GetType().Name, name, out var value);

                if (p.PropertyType == typeof(string))
                    p.SetValue(obj, value);
                else
                {
                    var parseMethod = obj.GetType().GetMethod("Parse");
                    if (parseMethod == null)
                        continue;

                    var propertyTypeValue = parseMethod.Invoke(obj, new object[] { value });
                    p.SetValue(obj, propertyTypeValue);
                }
            }
        }
    }
}
