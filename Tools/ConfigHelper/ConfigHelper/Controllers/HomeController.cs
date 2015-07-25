using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConfigHelper.Controllers
{
    using ConfigHelper.Tool;
    using Entity;
    using Entity.Models;
    using Newtonsoft.Json;
    using System.IO;
    using System.Text;

    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        #region build
        public ActionResult BuildList()
        {
            var data = GetAllBuild();
            ViewData["data"] = data;
            return View();
        }

        public ActionResult BuildEdit(Guid? id)
        {
            if (null == id) { return View(new Build() { ID = EmptyGuid }); }
            var model = DBHelper.Query<Build>(string.Format("select * from build where ID='{0}' ", id.ToString())).FirstOrDefault();
            return View(model);
        }

        [HttpPost]
        public ActionResult BuildEdit(Build model)
        {
            string res = string.Empty;
            if (model.ID == EmptyGuid)
            {
                model.ID = Guid.NewGuid();
                res = DBHelper.Insert(model).ToString();
            }
            else
            {
                res = model.ID.ToString();
                DBHelper.Update(model);
            }
            TempData["res"] = "ok";
            return RedirectToAction("BuildEdit", new { id = new Guid(res) });
        }

        #endregion

        #region Item
        public ActionResult ItemList()
        {
            var data = GetAllItem();
            ViewData["data"] = data;
            return View();
        }

        public ActionResult ItemEdit(Guid? id)
        {

            if (null == id) { return View(new Item() { ID = EmptyGuid }); }
            var model = DBHelper.Query<Item>(string.Format("select * from item where ID='{0}'", id.ToString())).FirstOrDefault();
            return View(model);
        }

        [HttpPost]
        public ActionResult ItemEdit(Item model)
        {
            string res = string.Empty;
            if (model.ID == EmptyGuid)
            {
                model.ID = Guid.NewGuid();
                res = DBHelper.Insert(model).ToString();
            }
            else
            {
                res = model.ID.ToString();
                DBHelper.Update(model);
            }
            TempData["res"] = "ok";
            return RedirectToAction("ItemEdit", new { id = new Guid(res) });
        }
        #endregion

        #region Product
        public ActionResult ProductList()
        {
            var data = DBHelper.Query<Models.SimpleModel>("select ID,Name from product");
            ViewData["data"] = data;
            return View();
        }

        public ActionResult ProductEdit(Guid? id)
        {
            if (null == id) { return View(new Product() { ID = EmptyGuid }); }
            var model = DBHelper.Query<Product>(string.Format("select * from product where ID='{0}'", id.ToString())).FirstOrDefault();
            return View(model);
        }

        [HttpPost]
        public ActionResult ProductEdit(Product model)
        {

            model.Resource = JsonStrToJsonStr(model.Resource);
            model.Products = JsonStrToJsonStr(model.Products);
            string res = string.Empty;
            if (model.ID == EmptyGuid)
            {
                model.ID = Guid.NewGuid();
                res = DBHelper.Insert(model).ToString();
            }
            else
            {
                res = model.ID.ToString();
                DBHelper.Update(model);
            }
            TempData["res"] = res;
            return RedirectToAction("ProductEdit", new { id = new Guid(res) });
        }
        #endregion


        public ActionResult ConstructionList()
        {
            var data = DBHelper.Query<Construction>("select * from construction");
            ViewData["data"] = data;
            return View();
        }

        public ActionResult ConstructionEdit(Guid? id)
        {
            if (null == id) { return View(new Construction() { ID = EmptyGuid }); }
            var model = DBHelper.Query<Construction>(string.Format("select * from construction where ID='{0}'", id.ToString())).FirstOrDefault();
            return View(model);
        }

        [HttpPost]
        public ActionResult ConstructionEdit(Construction model)
        {
            model.Items = JsonStrToJsonStr(model.Items);
            string res = string.Empty;
            if (model.ID == EmptyGuid)
            {
                model.ID = Guid.NewGuid();
                res = DBHelper.Insert(model).ToString();
            }
            else
            {
                res = model.ID.ToString();
                DBHelper.Insert(model);
            }
            TempData["res"] = "ok";
            return RedirectToAction("ConstructionEdit", new { id = res });
        }

        private static string JsonStrToJsonStr(string jsonStr)
        {
            var resourceJson = JsonConvert.DeserializeObject<Models.SimpleJsonModel[]>(jsonStr);
            var sb = new StringBuilder();
            sb.Append("[");
            foreach (var r in resourceJson)
            {
                sb.Append("{\"" + r.ID + "\":" + r.Num + "},");
            }
            if (sb[sb.Length - 1] == ',') sb.Remove(sb.Length - 1, 1);
            sb.Append("]");
            return sb.ToString();
        }

        public ActionResult CreateJson()
        {
            //build
            var build = DBHelper.Query<Build>("select * from build");
            var buildstr = JsonConvert.SerializeObject(build);
            WriteFile("build.json", buildstr);
            //Item
            var item = DBHelper.Query<Item>("select * from item");
            var itemstr = JsonConvert.SerializeObject(item);
            WriteFile("item.json", itemstr);
            //constructor
            var con = DBHelper.Query<Construction>("select * from construction");
            var constr = new StringBuilder();
            constr.Append("[");
            foreach (var c in con)
            {
                constr.Append("{");
                constr.Append(ConstructionJsonItem("ID", c.ID.ToString()));
                constr.Append(ConstructionJsonItem("ID", c.Name));
                constr.Append(ConstructionJsonItem("OriginBuilding", c.OriginBuilding.ToString()));
                constr.Append(ConstructionJsonItem("Building", c.Building.ToString()));
                constr.Append(ConstructionJsonItem("Description", c.Description));
                constr.Append(ConstructionJsonItem("Workers", c.Workers));
                constr.Append("\"Resources\":{");
                constr.Append(ConstructionJsonItem("Time", c.TIme));
                constr.Append("\"Items\":" + c.Items);
                constr.Append("}");
                constr.Append("},");
            }
            if (constr[constr.Length - 1] == ',') constr.Remove(constr.Length - 1, 1);
            constr.Append("]");
            WriteFile("construction.json", constr.ToString());
            //product
            var product = DBHelper.Query<Product>("select * from product");
            var productstr = new StringBuilder();
            productstr.Append("[");
            foreach (var p in product)
            {
                productstr.Append("{");
                productstr.Append(ConstructionJsonItem("ID", p.ID.ToString()));
                productstr.Append(ConstructionJsonItem("Name", p.Name));
                productstr.Append(ConstructionJsonItem("Description", p.Description));
                productstr.Append(ConstructionJsonItem("Building", p.Description));
                productstr.Append("\"Products\":" + p.Products + ",");
                productstr.Append("\"Resource\":{");
                productstr.Append(ConstructionJsonItem("Workers", p.Workers));
                productstr.Append(ConstructionJsonItem("Time", p.Time));
                productstr.Append("\"Items\":");
                productstr.Append(p.Resource);
                productstr.Append("}");
                productstr.Append("},");
            }
            if (productstr[productstr.Length - 1] == ',') productstr.Remove(productstr.Length - 1, 1);
            productstr.Append("]");
            // var prostr = JsonConvert.SerializeObject(product);
            WriteFile("product.json", productstr.ToString());


            return Content("ok");
        }

        private void WriteFile(string fileName, string data)
        {
            System.IO.File.WriteAllText(Server.MapPath("/JsonFIle/" + fileName), data);
        }

        private string ConstructionJsonItem(string name, string value)
        {
            return string.Format("\"{0}\":\"{1}\",", name, value);
        }

        private string ConstructionJsonItem(string name, int value)
        {
            return string.Format("\"{0}\":{1},", name, value);
        }

        public static Item[] GetAllItem()
        {
            return DBHelper.Query<Item>("select * from item");
        }

        public static Build[] GetAllBuild()
        {
            return DBHelper.Query<Build>("select * from build");
        }

        private static readonly Guid EmptyGuid = new Guid("00000000-0000-0000-0000-000000000000");

        public static string TOJsonStr(string input)
        {
            if (null == input) return string.Empty;
            return input.Replace("\"", string.Empty).Replace("[", string.Empty).Replace("]", string.Empty).Replace("{", string.Empty).Replace("}", string.Empty);
        }
    }
}
