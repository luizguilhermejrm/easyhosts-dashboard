using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using EasyHosts.Dashboard.Models;
using EasyHosts.Dashboard.ViewModel;

namespace EasyHosts.Dashboard.Controllers
{
    public class UserController : Controller
    {
        private Context db = new Context();

        // GET: User
        public ActionResult Index()
        {
            return View(db.User.ToList());
        }

        // GET: User/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Usúario não encontrado!" });
            }
            User user = db.User.Find(id);
            if (user == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Não encontramos o usuário solicitado!" });
            }
            return View(user);
        }

        // GET: User/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Usúario não encontrado!" });
            }
            User user = db.User.Find(id);
            if (user == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Não encontramos o usuário solicitado!" });
            }
            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.User.Find(id);
            db.User.Remove(user);
            db.SaveChanges();
            TempData["MSG"] = "success|Usuário deletado com sucesso!";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult ExportExcelClients()
        {

            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", $"attachment;filename=TABELA_USUARIOS_CLIENTES{DateTime.Now.ToString("yyyy_MM_dd")}.xlsx");

            XLWorkbook wbook = new XLWorkbook();
            List<User> users = db.User.Where(x => x.PerfilId == 1).ToList();

            DataTable dataTableUsers = Functions.GenerateDataTableUsers(users);

            wbook.Worksheets.Add(dataTableUsers, "TABELA_USUARIOS");

            wbook.SaveAs(Response.OutputStream);
            wbook.Dispose();

            Response.End();

            return null;
        }

        public ActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel
            {
                Message = message,
            };
            return View(viewModel);
        }

        public ActionResult ExportExcelEmployee()
        {

            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", $"attachment;filename=TABELA_USUARIOS_FUNCIONARIOS{DateTime.Now.ToString("yyyy_MM_dd")}.xlsx");

            XLWorkbook wbook = new XLWorkbook();
            List<User> users = db.User.Where(x => x.PerfilId == 2).ToList();

            DataTable dataTableUsers = Functions.GenerateDataTableUsers(users);

            wbook.Worksheets.Add(dataTableUsers, "TABELA_USUARIOS");

            wbook.SaveAs(Response.OutputStream);
            wbook.Dispose();

            Response.End();

            return null;
        }
    }
}
