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

        public static T[] Query<T>(Sql sqlstr)
        {
            using (var db = new Database("conn"))
            {
                return db.Query<T>(sqlstr).ToArray();
            }
        }

        public static Page<T> Page<T>(int pageindex,int itemperpages,string sqlstr)
        {
            using (var db = new Database("conn"))
            {
                return db.Page<T>(pageindex, itemperpages, sqlstr);
            }
        }

        public static Page<T> Page<T>(int pageindex, int itemperpages, Sql sqlstr)
        {
            using (var db = new Database("conn"))
            {
                return db.Page<T>(pageindex, itemperpages, sqlstr);
            }
        }

        public static object Insert(object model)
        {
            using (var db = new Database("conn"))
            {
                return db.Insert(model);
            }
        }

        public static int Update(object model)
        {
            using (var db = new Database("conn"))
            {
                return db.Update(model);
            }
        }

    }
}