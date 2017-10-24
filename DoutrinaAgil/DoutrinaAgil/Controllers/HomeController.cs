using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DoutrinaAgil.Service.Api;
using DoutrinaAgil.Util;

namespace DoutrinaAgil.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApiClient _api;
        private const string COOKIE_KEY = @"total_doctrines";

        public HomeController()
        {
            _api = new ApiClient();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<JsonResult> GetTotalDoctrines()
        {
            var cookie = ReadCookie(COOKIE_KEY);

            if (!string.IsNullOrEmpty(cookie))
                return Json(cookie, JsonRequestBehavior.AllowGet);

            var result = await _api.GetTotalDoctrines();
            WriteCookie(COOKIE_KEY, result);

            return result == null
                ? Json(ResponseData.GetResponseError("Não foram encontrados resultados para a pesquisa"))
                : Json(result, JsonRequestBehavior.AllowGet);
        }

        private string ReadCookie(string key)
        {
            var myCookie = Request.Cookies[key];

            return myCookie?.Value ?? string.Empty;
        }
        private void WriteCookie(string key, string value)
        {
            var myCookie = new HttpCookie(key)
            {
                Value = value,
                Expires = DateTime.Now.AddMinutes(10)
            };

            Response.Cookies.Add(myCookie);
        }
    }
}