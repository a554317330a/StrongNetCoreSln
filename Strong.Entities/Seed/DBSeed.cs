using Strong.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Strong.Entities.Seed
{
    public class DBSeed
    {

        private static string SeedDataFolder = "StrongNet.Data.json/{0}.tsv";
        private static string bllusing = $"using {CreateMethods.SolutionName}.Entity.DBModel;\n using {CreateMethods.SolutionName}.IBussiness;\n using {CreateMethods.SolutionName}.IRepository;\n using {CreateMethods.SolutionName}.Repository";
        private static string ibllusing = $"using  {CreateMethods.SolutionName}.Entity.DBModel";
        private static string irepositoryusing = $"using  {CreateMethods.SolutionName}.Entity.DBModel;";
        private static string repositoryusing = $"using  {CreateMethods.SolutionName}.Entity.DBModel;\n using YunTuCore.IRepository;";

        public static async Task SeedAsync(MyContext myContext, string WebRootPath)
        {
            try
            {
                if (string.IsNullOrEmpty(WebRootPath))
                {
                    //到时候数据文件在接口项目的网站文件中
                    throw new Exception("获取wwwroot路径时，异常！");
                }
                SeedDataFolder = Path.Combine(WebRootPath, SeedDataFolder);

                //创建一个数据库
                myContext.Db.DbMaintenance.CreateDatabase(Appsettings.app(new string[] { "BaseConfig", "DataBaseName" }), "");


                var modelTypes = from t in Assembly.GetExecutingAssembly().GetTypes()
                                 where t.IsClass && t.Namespace == "Strong.Entities.DBModel"
                                 select t;
                modelTypes.ToList().ForEach(t =>
                {
                //生成数据库表
                if (!myContext.Db.DbMaintenance.IsAnyTable(t.Name))
                    {
                        Console.WriteLine(t.Name);
                        myContext.Db.CodeFirst.InitTables(t);
                    }

                //生成对应层类文件
                CreateMethods.GenerationLogic(t.Name, bllusing, "Bussiness", "BLL", "BLL");
                    CreateMethods.GenerationLogic(t.Name, ibllusing, "IBussiness", "IBLL", "BLL");
                    CreateMethods.GenerationLogic(t.Name, repositoryusing, "Repository", "Repository", "Repository");
                    CreateMethods.GenerationLogic(t.Name, irepositoryusing, "IRepository", "IRepository", "Repository");
                });



                //生成种子数据

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
