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


            return Json(true, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
