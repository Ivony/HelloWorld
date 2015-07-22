using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConfigHelper.Tool
{
    using PetaPoco;

    public class DBHelper
    {

        public static T[] Query<T>(string sqlstr)
        {
            using (var db = new Database("conn"))
            {
                return db.Query<T>(sqlstr).ToArray();
            }
        }

        public static object Insert(object model)
        {
            using (var db = new Database("conn"))
            {
                return db.Insert(model);
            }
        }

    }
}