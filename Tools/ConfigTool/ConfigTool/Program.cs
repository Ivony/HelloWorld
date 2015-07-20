namespace ConfigTool
{
    using ConfigTool.Model;
    using MySql.Data.MySqlClient;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;

    internal class Program
    {
        private static void Main(string[] args)
        {
            var connectionStr = "Server=127.0.0.1;Database=hw;Uid=root;Pwd=root;Port=3306;";
            var baseSavePath = AppDomain.CurrentDomain.BaseDirectory;

            using (var conn = new MySqlConnection(connectionStr))
            {
                //出基础资源
                var baseResource = MySqlHelper.ExecuteDataset(conn, "select * from BaseResource");
                var brList = new List<BaseResource>();
                for (int i = 0; i < baseResource.Tables[0].Rows.Count; i++)
                {
                    var row = baseResource.Tables[0].Rows[i];
                    brList.Add(new BaseResource()
                    {
                        ID = Convert.ToInt32(row[0]),
                        Name = row[1].ToString()
                    });
                }
                var brSer = JsonConvert.SerializeObject(brList);
                File.WriteAllText(baseSavePath + "BaseResource.json", brSer);

                //出建筑
                var buildResource = MySqlHelper.ExecuteDataset(conn, "select * from Build");
                var buildList = new List<Build>();
                for (int i = 0; i < buildResource.Tables[0].Rows.Count; i++)
                {
                    var row = buildResource.Tables[0].Rows[i];
                    buildList.Add(new Build()
                    {
                        ID = Convert.ToInt32(row[0]),
                        Name = row[1].ToString(),
                        Description = row[2].ToString(),
                        RequiredTime = Convert.ToInt64(row[3]),
                        Workers = Convert.ToInt32(row[4]),
                        DependentBuild = Convert.ToInt32(row[5]),
                        ResourceNeed = row[6].ToString()
                    });
                }
                var buildSer = JsonConvert.SerializeObject(buildList);
                File.WriteAllText(baseSavePath + "Build.json", buildSer);
            }

            Console.WriteLine("OK");
            Console.ReadKey();
        }
    }
}