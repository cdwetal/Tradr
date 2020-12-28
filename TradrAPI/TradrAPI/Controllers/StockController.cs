using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradrDataAccess;
using TradrORM;

namespace TradrAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StockController : Controller
    {
        public List<Quote> GetStock()
        {
            Stock aapl = new Stock("AAPL");
            

            return TradierClient.GetQuotes(new Stock[] { aapl }); ;
        }
    }
}
