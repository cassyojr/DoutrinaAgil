using System.Threading.Tasks;
using System.Web.Mvc;
using DoutrinaAgil.Service.Api;
using DoutrinaAgil.Util;

namespace DoutrinaAgil.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApiClient _api;

        public HomeController()
        {
            _api = new ApiClient();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetTotalDoctrines()
        {
            var result = await _api.GetTotalDoctrines();

            if (result == null)
                return Json(ResponseData.GetResponseError("Não foram encontrados resultados para a pesquisa"));

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}