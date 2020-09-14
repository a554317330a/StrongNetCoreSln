using System;
using System.IO;
using System.Text;

namespace Strong.Common
{
    public class FileHelper
    {

        public static void CreateFile(string filePath, string text, Encoding encoding)
        {
            try
            {
                if (IsExistFile(filePath))
                {
                    DeleteFile(filePath);
                }
                if (!IsExistFile(filePath))
                {
                    string directoryPath = GetDirectoryFromFilePath(filePath);
                    CreateDirectory(directoryPath);

                    //Create File
                    FileInfo file = new FileInfo(filePath);
                    using (FileStream stream = file.Create())
                    {
                        using (StreamWriter writer = new StreamWriter(stream, encoding))
                        {
                            writer.Write(text);
                            writer.Flush();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static bool IsExistDirectory(string directoryPath)
        {
            return Directory.Exists(directoryPath);
        }
        public static void CreateDirectory(string directoryPath)
        {
            if (!IsExistDirectory(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }
        public static void DeleteFile(string filePath)
        {
            if (IsExistFile(filePath))
            {
                File.Delete(filePath);
            }
        }
        public static string GetDirectoryFromFilePath(string filePath)
        {
            FileInfo file = new FileInfo(filePath);
            DirectoryInfo directory = file.Directory;
            return directory.FullName;
        }
        public static bool IsExistFile(string filePath)
        {
            return File.Exists(filePath);
        }

        /// <summary>
        /// 读文件
        /// </summary>
        /// <param name="Path">文件路径</param>
        /// <param name="encode">编码格式</param>
        /// <returns></returns>
        public static string ReadFile(string Path, Encoding encode)
        {
            string s = "";
            if (!File.Exists(Path))
                s = "不存在相应的目录";
            else
            {
                StreamReader f2 = new StreamReader(Path, encode);
                s = f2.ReadToEnd();
                f2.Close();
                f2.Dispose();
            }

            return s;
        }



    }
}
