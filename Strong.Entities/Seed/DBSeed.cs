using Strong.Common;
using Strong.Entities.DBModel;
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

        private static string SeedDataFolder = "Strong.Data.json\\{0}.tsv";
        private static string bllusing = $"using {CreateMethods.SolutionName}.Entities.DBModel;\n using {CreateMethods.SolutionName}.IBussiness;\n using {CreateMethods.SolutionName}.IRepository;\n";
        private static string ibllusing = $"using  {CreateMethods.SolutionName}.Entities.DBModel;\n using  {CreateMethods.SolutionName}.IBussiness;";
        private static string irepositoryusing = $"using  {CreateMethods.SolutionName}.Entities.DBModel;\n";
        private static string repositoryusing = $"using  {CreateMethods.SolutionName}.Entities.DBModel;\n using {CreateMethods.SolutionName}.IRepository;";

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
                myContext.Db.DbMaintenance.CreateDatabase(Appsettings.app(new string[] { "BaseConfig", "DataBaseName" }), null);


                var modelTypes = from t in Assembly.GetExecutingAssembly().GetTypes()
                                 where t.IsClass && t.Namespace == "Strong.Entities.DBModel"
                                 select t;
                modelTypes.ToList().ForEach(t =>
                {
                    string msg = string.Empty;
 
                    //生成数据库表
                    if (!myContext.Db.DbMaintenance.IsAnyTable(t.Name))
                    {
                        myContext.Db.CodeFirst.InitTables(t);
                        //生成对应层类文件
                        msg += CreateMethods.GenerationLogic(t.Name, bllusing, "Bussiness", "Bussiness", "Bussiness");
                        msg += CreateMethods.GenerationLogic(t.Name, ibllusing, "IBussiness", "IBussiness", "Bussiness");
                        msg += CreateMethods.GenerationLogic(t.Name, repositoryusing, "Repository", "Repository", "Repository");
                        msg += CreateMethods.GenerationLogic(t.Name, irepositoryusing, "IRepository", "IRepository", "Repository");
                        Console.WriteLine(msg );
                    }
                });

                //生成种子数据,数据库没有的表，创建的时候，同时创建数据
                #region TB_User
                if (!await myContext.Db.Queryable<TB_User>().AnyAsync())
                {
                    myContext.GetEntityDB<TB_User>().InsertRange(JsonHelper.ParseFormByJson<List<TB_User>>(FileHelper.ReadFile(string.Format(SeedDataFolder, "TB_User"), Encoding.UTF8)));
                    Console.WriteLine("Table:TB_User Data created success!");
                }
                else
                {
                    Console.WriteLine("Table:TB_User already exists...");
                }
                #endregion

                #region TB_Role
                if (!await myContext.Db.Queryable<TB_Role>().AnyAsync())
                {
                    myContext.GetEntityDB<TB_Role>().InsertRange(JsonHelper.ParseFormByJson<List<TB_Role>>(FileHelper.ReadFile(string.Format(SeedDataFolder, "TB_Role"), Encoding.UTF8)));
                    Console.WriteLine("Table:TB_Role Data created success!");
                }
                else
                {
                    Console.WriteLine("Table:TB_Role already exists...");
                }
                #endregion

                #region TB_Menu
                if (!await myContext.Db.Queryable<TB_Menu>().AnyAsync())
                {
                    myContext.GetEntityDB<TB_Menu>().InsertRange(JsonHelper.ParseFormByJson<List<TB_Menu>>(FileHelper.ReadFile(string.Format(SeedDataFolder, "TB_Menu"), Encoding.UTF8)));
                    Console.WriteLine("Table:TB_Menu Data created success!");
                }
                else
                {
                    Console.WriteLine("Table:TB_Menu already exists...");
                }
                #endregion

                #region TB_Role_Menu
                if (!await myContext.Db.Queryable<TB_Role_Menu>().AnyAsync())
                {
                    myContext.GetEntityDB<TB_Role_Menu>().InsertRange(JsonHelper.ParseFormByJson<List<TB_Role_Menu>>(FileHelper.ReadFile(string.Format(SeedDataFolder, "TB_Role_Menu"), Encoding.UTF8)));
                    Console.WriteLine("Table:TB_Role_Menu Data created success!");
                }
                else
                {
                    Console.WriteLine("Table:TB_Role_Menu already exists...");
                }
                #endregion

                #region TB_User_Role
                if (!await myContext.Db.Queryable<TB_User_Role>().AnyAsync())
                {
                    myContext.GetEntityDB<TB_User_Role>().InsertRange(JsonHelper.ParseFormByJson<List<TB_User_Role>>(FileHelper.ReadFile(string.Format(SeedDataFolder, "TB_User_Role"), Encoding.UTF8)));
                    Console.WriteLine("Table:TB_User_Role Data created success!");
                }
                else
                {
                    Console.WriteLine("Table:TB_User_Role already exists...");
                }
                #endregion

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
