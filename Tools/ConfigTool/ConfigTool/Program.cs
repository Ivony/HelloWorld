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
                        ID = row[0].ToString(),
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
                        ID = row["ID"].ToString(),
                        Name = row[1].ToString(),
                        Description = row[2].ToString(),
                        RequiredTime = Convert.ToInt64(row[3]),
                        Workers = Convert.ToInt32(row[4]),
                        DependentBuild = row[5].ToString(),
                        ResourceNeed = row[6].ToString(),
                        HP=Convert.ToInt32(row["HP"]),
                        AP = Convert.ToInt32(row["AP"])
                    });
                }
                var buildSer = JsonConvert.SerializeObject(buildList);
                File.WriteAllText(baseSavePath + "Build.json", buildSer);

                //出建筑功能
                var buildFetureResource = MySqlHelper.ExecuteDataset(conn, "select * from BuildFeture");
                var buildFetureList = new List<BuildFeture>();
                for (int i = 0; i < buildFetureResource.Tables[0].Rows.Count; i++)
                {
                    var row=buildFetureResource.Tables[0].Rows[i];
                    buildFetureList.Add(new BuildFeture()
                    {
                        BuildID = Convert.ToInt32(row["BuildID"]),
                        Description = row["Description"].ToString(),
                        ID = row["ID"].ToString(),
                        Name = row["Name"].ToString(),
                        RequiredTime = Convert.ToInt64(row["RequiredTime"]),
                        ResourceNeed = row["ResourceNeed"].ToString(),
                        Result = row["Result"].ToString(),
                        Workers = Convert.ToInt32(row["Workers"])
                    });
                }
                var buildFetureSeri = JsonConvert.SerializeObject(buildFetureList);
                File.WriteAllText(baseSavePath + "BuildFeture.json", buildFetureSeri);




                 //出物品
                var itemResource = MySqlHelper.ExecuteDataset(conn, "select * from item");
                var itemList = new List<Item>();
                for (int i = 0; i < itemResource.Tables[0].Rows.Count; i++)
                {
                    var r=itemResource.Tables[0].Rows[i];
                    itemList.Add(new Item()
                    {
                        ID = r["ID"].ToString(),
                        Description = r["Description"].ToString(),
                        Name = r["Name"].ToString(),
                        ResourceNeed = r["ResourceNeed"].ToString()
                    });
                }
                var itemser = JsonConvert.SerializeObject(itemList);
                File.WriteAllText(baseSavePath + "Item.json", itemser);


                //出人物
                var personResource = MySqlHelper.ExecuteDataset(conn, "select * from person");
                var personList = new List<Person>();
                for (int i = 0; i < personResource.Tables[0].Rows.Count; i++)
                {
                    var r = personResource.Tables[0].Rows[i];
                    personList.Add(new Person()
                    {
                        AP = Convert.ToInt32(r["AP"]),
                        DependentPerson = r["DependentPerson"].ToString(),
                        Description = r["Description"].ToString(),
                        HP = Convert.ToInt32(r["HP"]),
                        ID =r["ID"].ToString(),
                        Name = r["Name"].ToString(),
                        ResourceNeed = r["ResourceNeed"].ToString()
                    });
                }
                var personSer = JsonConvert.SerializeObject(personList);
                File.WriteAllText(baseSavePath + "Person.json", personSer);






            }

           

            Console.WriteLine("OK");
            Console.ReadKey();
        }
    }
}