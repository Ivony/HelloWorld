using ConfigHelper.Tool;
using Entity.Models;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConfigHelper.Controllers
{
    using ConfigHelper.Models;
    using Newtonsoft.Json;
    using System.IO;
    using System.Text;

    public class SimpleViewModel<T>
    {
        public List<T> Items { get; set; }

        public long PageIndex { get; set; }

        public long ItemsPerpage { get; set; }

        public long TotalPageCount { get; set; }
    }



    public class DefaultController : Controller
    {
        private static Guid EmptyGUID = new Guid("00000000-0000-0000-0000-000000000000");

        //
        // GET: /Default/

        public ActionResult Index()
        {
            return View();
        }

        #region Item
        public ActionResult ItemList()
        {
            return View();
        }

        public ActionResult GetItemList(string key, int pageIndex = 1, int itemsPerPage = 20)
        {
            var sql = Sql.Builder.Select("*").From("HW_Item");
            if (false == string.IsNullOrWhiteSpace(key)) sql.Where(string.Format("Name like '%{0}%'", key));
            sql.OrderBy("UpdateTime desc");
            var data = DBHelper.Page<Entity.Models.HW_Item>(pageIndex, itemsPerPage, sql);
            return Json(new SimpleViewModel<HW_Item>()
            {
                Items = data.Items,
                ItemsPerpage = data.ItemsPerPage,
                PageIndex = data.CurrentPage,
                TotalPageCount = data.TotalPages
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ItemAdd(Guid? id)
        {
            if (null == id || id.Value == EmptyGUID)
            {
                var model = new HW_Item();
                model.ID = EmptyGUID;
                return View(model);
            }
            var sql = Sql.Builder.Select("*").From("HW_Item").Where("ID=@0", id.ToString());
            var res = DBHelper.Query<HW_Item>(sql).FirstOrDefault();
            return View(res);
        }

        [HttpPost]
        public ActionResult ItemAdd(Entity.Models.HW_Item model)
        {
            model.UpdateTime = DateTime.Now;
            if (model.ID == EmptyGUID)
            {
                model.ID = Guid.NewGuid();
                DBHelper.Insert(model);
            }
            else
            {
                DBHelper.Update(model);
            }
            return RedirectToAction("ItemList");
        }



        #endregion

        #region build

        public ActionResult BuildList()
        {
            return View();
        }

        public ActionResult GetBuildList(string key, int pageIndex = 1, int itemsPerPage = 20)
        {
            var sql = Sql.Builder.Select("*").From("hw_build");
            if (false == string.IsNullOrWhiteSpace(key)) sql.Where("Name=@0", key);
            sql.OrderBy("UpdateTime desc");
            var result = DBHelper.Page<HW_Build>(pageIndex, itemsPerPage, sql);
            var data = new SimpleViewModel<HW_Build>()
            {
                Items = result.Items,
                ItemsPerpage = result.ItemsPerPage,
                PageIndex = result.CurrentPage,
                TotalPageCount = result.TotalPages
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BuildAdd(Guid? id)
        {
            if (null == id || id.Value == EmptyGUID)
            {
                return View(new HW_Build() { ID = EmptyGUID });
            }
            var sql = Sql.Builder.Select("*").From("hw_build").Where("ID=@0", id.ToString());
            var res = DBHelper.Query<HW_Build>(sql).FirstOrDefault();
            return View(res);
        }

        [HttpPost]
        public ActionResult BuildAdd(HW_Build model)
        {
            model.UpdateTime = DateTime.Now;
            if (model.ID == EmptyGUID)
            {
                model.ID = Guid.NewGuid();
                DBHelper.Insert(model);
            }
            else
            {
                DBHelper.Update(model);
            }
            return RedirectToAction("BuildList");
        }


        #endregion

        #region Terrain

        public ActionResult TerrainList()
        {
            return View();
        }

        public ActionResult GetTerrainList(string key, int pageIndex = 1, int itemsPerPage = 20)
        {
            var sql = Sql.Builder.Select("*").From("hw_terrain");
            if (false == string.IsNullOrWhiteSpace(key)) sql.Where("Name=@0", key);
            sql.OrderBy("UpdateTime desc");
            var result = DBHelper.Page<HW_Terrain>(pageIndex, itemsPerPage, sql);
            var data = new SimpleViewModel<HW_Terrain>()
            {
                Items = result.Items,
                ItemsPerpage = result.ItemsPerPage,
                PageIndex = result.CurrentPage,
                TotalPageCount = result.TotalPages
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TerrainAdd(Guid? id)
        {
            if (null == id || id.Value == EmptyGUID)
            {
                return View(new HW_Terrain() { ID = EmptyGUID });
            }
            var sql = Sql.Builder.Select("*").From("hw_terrain").Where("ID=@0", id.ToString());
            var res = DBHelper.Query<HW_Terrain>(sql).FirstOrDefault();
            return View(res);
        }

        [HttpPost]
        public ActionResult TerrainAdd(HW_Terrain model)
        {
            model.UpdateTime = DateTime.Now;
            if (model.ID == EmptyGUID)
            {
                model.ID = Guid.NewGuid();
                DBHelper.Insert(model);
            }
            else
            {
                DBHelper.Update(model);
            }
            return RedirectToAction("TerrainList");
        }


        #endregion

        #region action

        public ActionResult ActionLIst()
        {
            return View();
        }

        public ActionResult GetActionList(string key, int pageIndex = 1, int itemsPerPage = 20)
        {
            var sql = Sql.Builder.Select("*").From("hw_action");
            if (false == string.IsNullOrWhiteSpace(key)) sql.Where("Name=@0", key);
            sql.OrderBy("UpdateTime desc");
            var result = DBHelper.Page<HW_Action>(pageIndex, itemsPerPage, sql);
            var data = new SimpleViewModel<HW_Action>()
            {
                Items = result.Items,
                ItemsPerpage = result.ItemsPerPage,
                PageIndex = result.CurrentPage,
                TotalPageCount = result.TotalPages
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ActionAdd(Guid? id)
        {
            var bSql = Sql.Builder.Append(@"select ID,Name from hw_build
UNION 
SELECT ID,Name from hw_terrain");
            var buildData = DBHelper.Query<SimpleModel>(bSql);
            var buildList = new List<SelectListItem>();
            foreach (var b in buildData)
            {
                buildList.Add(new SelectListItem() { Text = b.Name, Value = b.ID.ToString() });
            }

            if (null == id || id == EmptyGUID)
            {
                ViewData["buildData"] = buildList;
                return View(new HW_Action() { ID = EmptyGUID });
            }

            var sql = Sql.Builder.Select("*").From("hw_action").Where("ID=@0", id.ToString());
            var res = DBHelper.Query<HW_Action>(sql).FirstOrDefault();

            foreach (var b in buildList)
            {
                if (b.Value == res.Building.ToString()) b.Selected = true;
            }
            ViewData["buildData"] = buildList;
            return View(res);
        }

        [HttpPost]
        public ActionResult ActionAdd(HW_Action model)
        {
            var re = Request.Form["Return"];
            model.UpdateTime = DateTime.Now;
            if (model.ID == EmptyGUID)
            {
                model.ID = Guid.NewGuid();
                DBHelper.Insert(model);
            }
            else
            {
                DBHelper.Update(model);
            }
            return RedirectToAction("ActionLIst");
        }

        public ActionResult GetSimleItemByName(string key)
        {
            var sql = Sql.Builder.Select("ID,Name").From("HW_Item").Where(string.Format(" Name like '%{0}%' ", key));
            var data = DBHelper.Query<SimpleModel>(sql);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSimleItemByID(Guid id)
        {
            var sql = Sql.Builder.Select("ID,Name").From("HW_Item").Where("ID=@0", id.ToString());
            var data = DBHelper.Query<SimpleModel>(sql);
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region JsonFile
        public ActionResult CreateJsonFile()
        {
            //创建ItemJson
            var itemFilePath = Server.MapPath("/JsonFile2/Item");
            if (Directory.Exists(itemFilePath) == false) Directory.CreateDirectory(itemFilePath);
            var sql1 = Sql.Builder.Select("*").From("HW_Item");
            var itemData = DBHelper.Query<HW_Item>(sql1);
            foreach (var item in itemData)
            {
                var tmpItem = new
                {
                    ID = item.ID,
                    Type = "HelloWorld.ItemDescriptor",
                    Name = item.Name,
                    Description = item.Description,
                    Weight = string.Empty
                };
                using (var file = System.IO.File.Open(Path.Combine(itemFilePath, item.Name + ".json"), FileMode.Create))
                {
                    var jsonItem = JsonConvert.SerializeObject(tmpItem);
                    var itembuffer = Encoding.UTF8.GetBytes(jsonItem);
                    file.Write(itembuffer, 0, itembuffer.Length);
                }
            }

            //buildItem
            var buildFilePath = Server.MapPath("/JsonFile2/Build");
            if (Directory.Exists(buildFilePath) == false) Directory.CreateDirectory(buildFilePath);
            var sql2 = Sql.Builder.Select("*").From("hw_build");
            var buildData = DBHelper.Query<HW_Build>(sql2);
            foreach (var build in buildData)
            {
                var tmpBuild = new
                {
                    ID = build.ID,
                    Type = "HelloWorld.BuildingDescriptor",
                    Name = build.Name,
                    Description = build.Description
                };
                var buildJson = JsonConvert.SerializeObject(tmpBuild);
                using (var file = System.IO.File.Open(Path.Combine(buildFilePath, build.Name + ".json"), FileMode.Create))
                {
                    var buildBuffer = Encoding.UTF8.GetBytes(buildJson);
                    file.Write(buildBuffer, 0, buildBuffer.Length);
                }
            }


            //terrain
            var terrainFilePath = Server.MapPath("/JsonFile2/Build/Terrain");
            if (Directory.Exists(terrainFilePath) == false) Directory.CreateDirectory(terrainFilePath);
            var sql3 = Sql.Builder.Select("*").From("hw_terrain");
            var terrainData = DBHelper.Query<HW_Terrain>(sql3);
            foreach (var terrain in terrainData)
            {
                var tmpTerrain = new
                {
                    ID = terrain.ID,
                    Type = "HelloWorld.GreatCivilization.Terrain",
                    Name = terrain.Name,
                    Description = terrain.Description
                };
                var terrainBuffer = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(tmpTerrain));
                using (var file = System.IO.File.Open(Path.Combine(terrainFilePath, terrain.Name + ".json"), FileMode.Create))
                {
                    file.Write(terrainBuffer, 0, terrainBuffer.Length);
                }
            }

            //action
            var sql4 = Sql.Builder.Select("*").From("hw_action");
            var actionData = DBHelper.Query<HW_Action>(sql4);
            var buildGroup = actionData.GroupBy(s => s.Building);
            var x = buildGroup.ToList();
            foreach (var b in buildGroup)
            {
                var groupData = from a in actionData where a.Building == b.Key select a;
                string dirName = string.Empty;
                var sql41 = Sql.Builder.Select("*").From("hw_build").Where("ID=@0", b.Key);
                var data41 = DBHelper.Query<HW_Build>(sql41).FirstOrDefault();
                if (null != data41)
                {
                    dirName = data41.Name;
                }
                else
                {
                    var sql42 = Sql.Builder.Select("*").From("hw_terrain").Where("ID=@0", b.Key);
                    var data42 = DBHelper.Query<HW_Build>(sql42).FirstOrDefault();
                    if (null != data42)
                    {
                        dirName = data42.Name;
                    }
                    else
                    {
                        continue;
                    }
                }
                var actionFilePath = Server.MapPath("/JsonFile2/Action/" + dirName);
                if (Directory.Exists(actionFilePath) == false) Directory.CreateDirectory(actionFilePath);
                foreach (var g in groupData)
                {
                    //json
                    var actionsb = new StringBuilder();
                    actionsb.Append("{");
                    actionsb.Append(string.Format("\"{0}\":\"{1}\",", "ID", g.ID));
                    actionsb.Append(string.Format("\"{0}\":\"{1}\",", "Type", g.Type));
                    actionsb.Append(string.Format("\"{0}\":\"{1}\",", "Building", g.Building));
                    actionsb.Append(string.Format("\"{0}\":\"{1}\",", "Name", g.Name));
                    actionsb.Append(string.Format("\"{0}\":\"{1}\",", "Description", g.Description));
                    //Returns
                    actionsb.Append("\"Returns\":{");
                    actionsb.Append("\"Items\":[");
                    if (false == string.IsNullOrWhiteSpace(g.Return))
                    {
                        var returnItems = JsonConvert.DeserializeObject<SimpleModel2[]>(g.Return);
                        foreach (var item in returnItems)
                        {
                            actionsb.Append("{");
                            actionsb.Append(string.Format("\"{0}\":{1}", item.ID, item.Num));
                            actionsb.Append("},");
                        }
                        if (actionsb[actionsb.Length - 1] == ',') actionsb.Remove(actionsb.Length - 1, 1);
                    }
                    actionsb.Append("]");
                    actionsb.Append("},");
                    //Require
                    actionsb.Append("\"Requirment\":{");
                    actionsb.Append("\"Items\":[");
                    if (false == string.IsNullOrWhiteSpace(g.Require))
                    {
                        var reqItem = JsonConvert.DeserializeObject<SimpleModel2[]>(g.Require);
                        foreach (var item in reqItem)
                        {
                            actionsb.Append("{");
                            actionsb.Append(string.Format("\"{0}\":{1}", item.ID, item.Num));
                            actionsb.Append("},");
                        }
                        if (actionsb[actionsb.Length - 1] == ',') actionsb.Remove(actionsb.Length - 1, 1);
                    }
                    actionsb.Append("],");
                    actionsb.Append(string.Format("\"{0}\":\"{1}\"", "Time", g.Time));
                    actionsb.Append("}");

                    actionsb.Append("}");

                    var actionBuffer = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(actionsb.ToString()));
                    using (var file = System.IO.File.Open(Path.Combine(actionFilePath, g.Name + ".json"), FileMode.Create))
                    {
                        file.Write(actionBuffer, 0, actionBuffer.Length);
                    }
                }


            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
