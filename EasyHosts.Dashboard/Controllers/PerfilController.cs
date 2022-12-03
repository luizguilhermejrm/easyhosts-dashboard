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
    public class PerfilController : Controller
    {
        private Context db = new Context();

        // GET: Perfil
        public ActionResult Index()
        {
            return View(db.Perfil.ToList());
        }

        // GET: Perfil/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Perfil não encontrado!" });
            }
            Perfil perfil = db.Perfil.Find(id);
            if (perfil == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Não encontramos o perfil solicitado!" });
            }
            return View(perfil);
        }

        // GET: Perfil/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Perfil/Create
        // Para se proteger de mais ataques, habilite as propriedades específicas às quais você quer se associar. Para 
        // obter mais detalhes, veja https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Description")] Perfil perfil)
        {
            if (ModelState.IsValid)
            {
                db.Perfil.Add(perfil);
                db.SaveChanges();
                TempData["MSG"] = "success|Perfil cadastrado com sucesso!";
                return RedirectToAction("Index");
            }
            TempData["MSG"] = "warning|Preencha todos os campos!";
            return View(perfil);
        }

        // GET: Perfil/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Perfil não encontrado!" });
            }
            Perfil perfil = db.Perfil.Find(id);
            if (perfil == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Não encontramos o perfil solicitado!" });
            }
            return View(perfil);
        }

        // POST: Perfil/Edit/5
        // Para se proteger de mais ataques, habilite as propriedades específicas às quais você quer se associar. Para 
        // obter mais detalhes, veja https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Description")] Perfil perfil)
        {
            if (ModelState.IsValid)
            {
                db.Entry(perfil).State = EntityState.Modified;
                db.SaveChanges();
                TempData["MSG"] = "success|Perfil editado com sucesso!";
                return RedirectToAction("Index");
            }
            TempData["MSG"] = "warning|Preencha todos os campos!";
            return View(perfil);
        }

        // GET: Perfil/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Perfil não encontrado!" });
            }
            Perfil perfil = db.Perfil.Find(id);
            if (perfil == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Não encontramos o perfil solicitado!" });
            }
            return View(perfil);
        }

        // POST: Perfil/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Perfil perfil = db.Perfil.Find(id);
            db.Perfil.Remove(perfil);
            db.SaveChanges();
            TempData["MSG"] = "success|Perfil deletado com sucesso!";
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

        public ActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel
            {
                Message = message,
            };
            return View(viewModel);
        }

        public ActionResult ExportExcel()
        {

            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", $"attachment;filename=TABELA_PERFIS_{DateTime.Now.ToString("yyyy_MM_dd")}.xlsx");

            XLWorkbook wbook = new XLWorkbook();
            List<Perfil> perfils = db.Perfil.ToList();

            DataTable dataTablePerfils = Functions.GenerateDataTablePerfils(perfils);

            wbook.Worksheets.Add(dataTablePerfils, "TABELA_PERFIS");

            wbook.SaveAs(Response.OutputStream);
            wbook.Dispose();

            Response.End();

            return null;
        }
    }
}
