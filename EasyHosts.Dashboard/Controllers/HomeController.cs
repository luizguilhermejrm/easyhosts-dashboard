using EasyHosts.Dashboard.Models;
using EasyHosts.Dashboard.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace EasyHosts.Dashboard.Controllers
{
    public class HomeController : Controller
    {
        private Context db = new Context();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Access ac, string ReturnUrl)
        {
            string passcrip = Functions.HashText(ac.Password, "SHA512");
            User user = db.User.Where(t => t.Email == ac.Email && t.Password == passcrip)
                .Where(t => t.Status == 1)
                .Where(t => t.PerfilId == 1)
                .FirstOrDefault();

            if (user != null)
            {
                FormsAuthentication.SetAuthCookie(user.Id + "|" + user.Name, false);
                return RedirectToAction("Dashboard", "Home");
            }
            else
            {
                TempData["MSG"] = "error|Email ou Senha inválidos!";
                return View();
            }
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Register register)
        {
            if (ModelState.IsValid)
            {
                if (db.User.Where(x => x.Email == register.Email).ToList().Count > 0)
                {
                    TempData["MSG"] = "warning|E-mail já utilizado!";
                    return View(register);
                }
                User user = new User();
                user.Name = register.Name;
                user.Email = register.Email;
                user.Password = Functions.HashText(register.Password, "SHA512");
                user.ConfirmPassword = Functions.HashText(register.ConfirmPassword, "SHA512");
                user.Cpf = register.Cpf;
                user.Perfil = db.Perfil.Find(1);
                if (user.Perfil == null)
                {
                    TempData["MSG"] = "warning|Não existe o perfil para cadastro!";
                    return View(register);
                }
                db.User.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(register);
        }

        public ActionResult Dashboard()
        {
            var resultado1 = db.Bedroom.ToList().GroupBy(x => x.TypeBedroom.NameTypeBedroom).Select(g => new { g.Key, Total = g.Count()});
            string dados = "";
            foreach (var item in resultado1)
            {
                dados += "['" + item.Key + "'," + item.Total.ToString().Replace(",", ".") + "],";
            }
            dados = dados.Substring(0, dados.Length);
            ViewBag.GraficoPizza = Functions.GerarGraficoPizza("", dados);

            var resultado2 = db.Booking.ToList().GroupBy(x => new { x.DateCheckin.Month }).Select(g => new { g.Key.Month, Total = g.Count()});
            string dadostopo2 = "[''";
            string dadoscorpo2 = "['Mesês'";
            foreach (var item in resultado2)
            {
                dadostopo2 += ",'" + DateTimeFormatInfo.CurrentInfo.GetMonthName(item.Month) + "'";
                dadoscorpo2 += "," + item.Total.ToString().Replace(",", ".");
            }
            dadostopo2 += "],";
            dadoscorpo2 += "]";
            ViewBag.GraficoColuna = Functions.GerarGraficoBarraColuna("", "", dadostopo2 + dadoscorpo2, false);

            var dashboard = new DashboardVm();
            return View(dashboard);
        }

        public ActionResult Email()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Email(Message msg)
        {
            if (ModelState.IsValid)
            {
                TempData["MSG"] = Functions.SendEmail(msg.Email,
                msg.Subject, msg.BodyMessage);
            }
            else
            {
                TempData["MSG"] = "warning|Preencha todos os campos!";
            }
            return View(msg);
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPassword fgt)
        {
            if (ModelState.IsValid)
            {
                Context db = new Context();
                var usu = db.User.Where(x => x.Email == fgt.Email).ToList().FirstOrDefault();
                if (usu != null)
                {
                    usu.Hash = Functions.Encode(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                    db.Entry(usu).State = EntityState.Modified;
                    db.SaveChanges();
                    string msg = "<h3>Sistema</h3>";
                    msg += "Para alterar sua senha <a href='https://localhost:44348/Home/ResetPassword/" + usu.Hash + "'target = '_blank'>clique aqui</a>";
                    Functions.SendEmail(usu.Email, "Redefinição de senha", msg);
                    TempData["MSG"] = "success|Senha redefinida com sucesso!";
                    return RedirectToAction("Index");
                }
                TempData["MSG"] = "error|E-mail não encontrado!";
                return View();
            }
            TempData["MSG"] = "warning|Preencha todos os campos!";
            return View();
        }

        public ActionResult ResetPassword(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                Context db = new Context();
                var usu = db.User.Where(x => x.Hash == id).ToList().FirstOrDefault();
                if (usu != null)
                {
                    try
                    {
                        DateTime dt = Convert.ToDateTime(Functions.Decode(usu.Hash));
                        if (dt > DateTime.Now)
                        {
                            ResetPassword reset = new ResetPassword();
                            reset.Hash = usu.Hash;
                            reset.Email = usu.Email;
                            return View(reset);
                        }
                        TempData["MSG"] = "warning|Esse link já expirou!";
                        return RedirectToAction("Index");
                    }
                    catch
                    {
                        TempData["MSG"] = "error|Hash inválida!";
                        return RedirectToAction("Index");
                    }
                }
                TempData["MSG"] = "error|Hash inválida!";
                return RedirectToAction("Index");
            }
            TempData["MSG"] = "error|Acesso inválido!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPassword reset)
        {
            if (ModelState.IsValid)
            {
                Context db = new Context();
                var usu = db.User.Where(x => x.Hash == reset.Hash).ToList().FirstOrDefault();
                if (usu != null)
                {
                    usu.Hash = null;
                    usu.Password = Functions.HashText(reset.Password, "SHA512");
                    usu.ConfirmPassword = Functions.HashText(reset.ConfirmPassword, "SHA512");
                    db.Entry(usu).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["MSG"] = "success|Senha redefinida com sucesso!";
                    return RedirectToAction("Index");
                }
                TempData["MSG"] = "error|E-mail não encontrado!";
                return View(reset);
            }
            TempData["MSG"] = "warning|Preencha todos os campos!";
            return View(reset);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
    }
}