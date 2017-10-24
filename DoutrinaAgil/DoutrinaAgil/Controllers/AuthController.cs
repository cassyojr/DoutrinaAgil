using System.Web.Mvc;
using DoutrinaAgil.ViewModels;
using DoutrinaAgil.Data.Repository;
using DoutrinaAgil.Data.Datacontext;
using DoutrinaAgil.Util;
using System.Web.Security;

namespace DoutrinaAgil.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserRepository _repository;

        public AuthController()
        {
            _repository = new UserRepository();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public JsonResult RegisterUser(RegisterUserViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(ResponseData.GetResponseError("Erro ao cadastrar usuário"));

            var emailExists = _repository.GetEmailExists(model.Email);

            if (emailExists)
                return Json(ResponseData.GetResponseError("Esta e-mail já está cadastrado"));

            var response = _repository.Add(new User
            {
                Name = model.Name,
                Password = model.Password,
                Email = model.Email
            });

            return Json(!response
                ? ResponseData.GetResponseError("Erro ao cadastrar usuário")
                : Authenticate(model.Email, model.Password));
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult Login(string email, string password)
        {
            if (User.Identity.IsAuthenticated)
                return Json(ResponseData.GetResponseError("Já existe um usuário logado"));

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return Json(ResponseData.GetResponseError("Usuário e senha inválidos"));

            return Json(Authenticate(email, password));
        }

        private ResponseData Authenticate(string email, string password)
        {
            var user = _repository.Get(email, password);

            if (user == null)
                return ResponseData.GetResponseError("Usuário e senha inválidos");

            FormsAuthentication.SetAuthCookie(user.Email, false);

            return ResponseData.GetResponseSuccess("Usuário conectado com sucesso");
        }

        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}