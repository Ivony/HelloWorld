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

        public ActionResult BuildList()
        {
            return View();
        }

        public ActionResult BuildEdit(int? id)
        {
            return View();
        }

        public ActionResult BuildEdit(Build model)
        {
            return null;
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
            var prostr = JsonConvert.SerializeObject(product);
            WriteFile("product.json", prostr);


            return null;
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

    }
}
