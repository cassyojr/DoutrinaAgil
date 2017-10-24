using System.Threading.Tasks;
using System.Web.Mvc;
using DoutrinaAgil.Service.Api;
using DoutrinaAgil.Util;

namespace DoutrinaAgil.Controllers
{
    public class SearchController : Controller
    {
        private readonly ApiClient _api;

        public SearchController()
        {
            _api = new ApiClient();
        }

        [HttpGet]
        public async Task<JsonResult> Search(string query)
        {
            if (string.IsNullOrEmpty(query))
                return Json(ResponseData.GetResponseError("Pesquisa inválida"));

            var result = await _api.GetApiBooks(query);

            if (result == null)
                return Json(ResponseData.GetResponseError("Não foram encontrados resultados para a pesquisa"));

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}