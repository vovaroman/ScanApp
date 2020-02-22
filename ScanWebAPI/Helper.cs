using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ScanWebAPI
{
    public class Helper
    {
        public static dynamic @SwitchType = new Dictionary<Type, SqlDbType> {
            { typeof(string), SqlDbType.NChar },
            { typeof(int), SqlDbType.Int},
            { typeof(DateTime), SqlDbType.DateTime},
        };


    }
}
