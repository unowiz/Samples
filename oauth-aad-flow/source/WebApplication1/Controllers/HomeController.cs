using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers {

    public class HomeController : Controller {

        public ActionResult Index() {
            return this.View();
        }

    }

}