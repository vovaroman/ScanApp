using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScanWebAPI.Models;
using ScanWebAPI.Connectors;

namespace ScanWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DailyController : ControllerBase
    {
        private static DatabaseConnector connector = new DatabaseConnector(@"Server=WIN-AAUUCG1C6J9;Database=Scan;User Id=scan_adm;Password=scan;");

        [HttpGet]
        public IEnumerable<Daily> Get()
        {
            var data = connector.Select("select * from Daily");
            var output = new List<Daily>();

            foreach (var i in data)
            {
                output.Add(
                        new Daily()
                        {
                            ID = int.Parse(i["ID"].ToString()),
                            Quantity_D = int.Parse(i["Quantity_D"].ToString()),
                            Date = DateTime.Parse(i["Date"].ToString()),
                            Status = i["Status"].ToString()
                        }
                    );
            }
            return output;
        }


    }
}