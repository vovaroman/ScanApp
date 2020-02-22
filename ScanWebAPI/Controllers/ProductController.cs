using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using ScanWebAPI.Models;
using ScanWebAPI.Connectors;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using System.Net.Http;
using System.Net;

namespace ScanWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private static DatabaseConnector connector = new DatabaseConnector(@"Server=WIN-AAUUCG1C6J9;Database=Scan;User Id=scan_adm;Password=scan;");

        [HttpGet]
        public IEnumerable<Product> Get()
        {
            var data = connector.Select("select * from Product");
            var output = new List<Product>();

            foreach (var i in data)
            {
                output.Add(
                        new Product()
                        {
                            ID = int.Parse(i["ID"].ToString()),
                            Name = i["Name"].ToString(),
                            Quantity = int.Parse(i["Quantity"].ToString())
                        }
                    );
            }
            return output;
        }

        [HttpPost]
        public int Post([FromBody] Product product)
        {
            connector.Insert(@"INSERT INTO[dbo].[Product] ([Name],[Quantity]) VALUES (@param1, @param2)", product.Name, product.Quantity);

            var temp = connector.Select($"select * from Product where Name = '{product.Name}'").LastOrDefault();

            connector.Insert("INSERT INTO [dbo].[Daily] ([ID],[Date],[Quantity_D],[Status]) VALUES (@param1, @param2, @param3, @param4)",
                int.Parse(temp["ID"].ToString()), DateTime.Now, product.Quantity, "Added");
            return int.Parse(temp["ID"].ToString());

        }

        [HttpPut("{id}")]
        public HttpResponseMessage SellProduct(long id, [FromBody]Product product)
        {
            var currentItem = connector.Select($"select * from Product where ID = {id}").FirstOrDefault();
            var currentQ = int.Parse(currentItem["Quantity"].ToString());
            if(currentQ <= 0 && int.Parse(currentItem["ID"].ToString()) == 0)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
            if (int.Parse(currentItem["ID"].ToString()) == 0)
                currentQ--;
            else
                currentQ++;

            connector.Update($"update Product SET Quantity = @param1 where ID = {id}", currentQ);
            connector.Insert("INSERT INTO [dbo].[Daily] ([ID],[Date],[Quantity_D],[Status]) VALUES (@param1 , @param2 , @param3 , @param4)",
                int.Parse(currentItem["ID"].ToString()), DateTime.Now, product.Quantity, "Sales");

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}