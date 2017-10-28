using System.Threading.Tasks;
using System.Web.Mvc;
using DoutrinaAgil.Service.Api;
using DoutrinaAgil.Util;

namespace DoutrinaAgil.Web.Controllers
{
    public class SearchController : Controller
    {
        private readonly ApiClient _api;

        public SearchController()
        {
            _api = new ApiClient();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<JsonResult> Search(string query)
        {
            var queryTrim = query.Trim();

            if (string.IsNullOrEmpty(queryTrim))
                return Json(ResponseData.GetResponseError("Pesquisa inválida"));

            var result = await _api.GetApiBooks(queryTrim);

            if (result == null)
                return Json(ResponseData.GetResponseError("Não foram encontrados resultados para a pesquisa"));

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}