using RazorEngine;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Strong.Common
{


   
    public static class CreateMethods
    {
        public static string SolutionName = Appsettings.app(new string[] { "AppSettings", "ProjectName" });
          public static Dictionary<string, string> ProjectIds = new Dictionary<string, string>();
        public static string GetCurrentProjectPath
        {

            get
            {
                return Environment.CurrentDirectory.Replace(@"\bin\Debug", "").Replace("\\netcoreapp3.1", "");
            }
        }
        public static string GetSlnPath
        {

            get
            {
                var path = Directory.GetParent(GetCurrentProjectPath).FullName;
                return path;

            }
        }




        public static string CreateLogic(string templatePath, string savePath, string tables, string usingstr, string classNamespace, string keyname, string suffix)
        {

            try
            {
                string template = System.IO.File.ReadAllText(templatePath); //从文件中读出模板内容
                string templateKey = keyname; //取个名字

                BLLParameter model = new BLLParameter()
                {
                    Name = tables,
                    ClassNamespace = classNamespace,
                    usingstr = usingstr
                };
                var result = Engine.Razor.RunCompile(template, templateKey, model.GetType(), model);
                if (classNamespace == "YunTuCore.IBussiness" || classNamespace == "YunTuCore.IRepository")
                {
                    tables = "I" + tables;
                }
                var cp = savePath + "\\" + tables + $"{suffix}.cs";
                if (FileHelper.IsExistFile(cp) == false)
                    FileHelper.CreateFile(cp, result, System.Text.Encoding.UTF8);
                return $"{suffix}生成成功";
            }
            catch (Exception)
            {

                return $"{suffix}生成失败";
            }
        }

        /// <summary>
        /// 生成底层逻辑
        /// </summary>
        /// <param name="tables">表名称</param>
        /// <param name="usingstr">引用字符串，读取配置文件appsetting</param>
        /// <param name="projectname">项目名称</param>
        /// <param name="tempname">模板名称</param>
        /// <param name="suffix">生成文件后缀名称</param>
        /// <returns></returns>
        public static string GenerationLogic(string tables, string usingstr, string projectname, string tempname, string suffix)
        {
            var bllProjectName2 = SolutionName + $".{projectname}";//具体项目

            var savePath2 = GetSlnPath + "\\" + bllProjectName2;//保存目录

            var templatePath = GetCurrentProjectPath + $"\\Template\\{tempname}.txt";//bll模版地址

            return CreateLogic(templatePath, savePath2, tables, usingstr, bllProjectName2, tempname.ToLower(), suffix);
            //AddTask(bllProjectName, bllPath);

        }

    }


    public class BLLParameter
    {
        public string Name { get; set; }
        public string ClassNamespace { get; set; }

        public string usingstr { get; set; }
    }
}
