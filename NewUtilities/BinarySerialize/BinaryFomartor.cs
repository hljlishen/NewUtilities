using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Utilities.BinarySerialize
{
    public class BinaryFomartor
    {
        /// <summary>
        /// 写入磁盘
        /// </summary>
        /// <param name="obj">实例对象</param>
        /// <param name="file">保存的文件路径跟文件名称</param>
        public static void WriteBinary(object obj, string file)
        {

            using (Stream input = File.OpenWrite(file))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(input, obj);
            }
        }

        // <summary>
        /// 读出磁盘文件
        /// </summary>
        /// <param name="file">保存的文件路径跟文件名称</param>
        /// <returns>object对象</returns>
        public static object ReadBinary(string file)
        {
            using (Stream outPut = File.OpenRead(file))
            {
                BinaryFormatter bf = new BinaryFormatter();
                object user = bf.Deserialize(outPut);
                if (user != null)
                {
                    return user;
                }
            }
            return null;
        }
    }
}