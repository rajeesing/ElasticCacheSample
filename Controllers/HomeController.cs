using ElasticCacheSample.Models;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

/*********************************************************************************************************************************
 * 1. Set-ExecutionPolicy Bypass -Scope Process -Force; iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))
 * 2. choco install redis-64 
 * 3. run > redis-server.exe
 ********************************************************************************************************************************/

namespace ElasticCacheSample.Controllers
{
    public class HomeController : Controller
    {
        #region Cache Properties
        private static ConnectionMultiplexer Connection = CacheConfig.Get();
        #endregion

        public ActionResult Index()
        {
            IDatabase db = Connection.GetDatabase();
            string serializeSalesOrder = db.StringGet("listitems");            

            if (!String.IsNullOrEmpty(serializeSalesOrder))
            {
                ViewBag.Source = "Cache";
                List<Items> dbList = JsonConvert.DeserializeObject<List<Items>>(serializeSalesOrder);
                return View(dbList);
            }
            else
            {
                ViewBag.Source = "Non-Cache";
                List<Items> dbList = GetAllItems();
                db.StringSet("listitems", JsonConvert.SerializeObject(dbList));
                return View(dbList);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

       [HttpPost]
        public ActionResult FlushCache(string key)
        {
            Connection.GetDatabase().KeyDeleteAsync(key);
            return RedirectToAction("Index");
        }

        public List<Items> GetAllItems()
        {
            List<Items> lstItems = new List<Items>();
            lstItems.Add(new Items { Id = 1, Name = "Item 1", Price = 10, Description = "Sample description 1" });
            lstItems.Add(new Items { Id = 2, Name = "Item 2", Price = 20, Description = "Sample description 2" });
            lstItems.Add(new Items { Id = 3, Name = "Item 3", Price = 30, Description = "Sample description 3" });
            lstItems.Add(new Items { Id = 4, Name = "Item 4", Price = 40, Description = "Sample description 4" });
            lstItems.Add(new Items { Id = 5, Name = "Item 5", Price = 50, Description = "Sample description 5" });
            lstItems.Add(new Items { Id = 6, Name = "Item 6", Price = 60, Description = "Sample description 6" });

            return lstItems;
        }
    }
}