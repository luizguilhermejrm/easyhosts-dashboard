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
    public class ProductController : Controller
    {
        private Context db = new Context();

        // GET: Product
        public ActionResult Index()
        {
            return View(db.Product.ToList());
        }

        // GET: Product/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Produto não encontrado!" });
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Não encontramos o produto solicitado!" });
            }
            return View(product);
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        // Para se proteger de mais ataques, habilite as propriedades específicas às quais você quer se associar. Para 
        // obter mais detalhes, veja https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Value,QuantityProduct")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Product.Add(product);
                db.SaveChanges();
                TempData["MSG"] = "success|Produto cadastrado com sucesso!";
                return RedirectToAction("Index");
            }
            TempData["MSG"] = "warning|Preencha todos os campos!";
            return View(product);
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Produto não encontrado!" });
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Não encontramos o produto solicitado!" });
            }
            return View(product);
        }

        // POST: Product/Edit/5
        // Para se proteger de mais ataques, habilite as propriedades específicas às quais você quer se associar. Para 
        // obter mais detalhes, veja https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Value,QuantityProduct")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                TempData["MSG"] = "success|Produto editado com sucesso!";
                return RedirectToAction("Index");
            }
            TempData["MSG"] = "warning|Preencha todos os campos!";
            return View(product);
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Produto não encontrado!" });
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Não encontramos o produto solicitado!" });
            }
            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Product.Find(id);
            db.Product.Remove(product);
            db.SaveChanges();
            TempData["MSG"] = "success|Produto deletado com sucesso!";
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
            Response.AddHeader("content-disposition", $"attachment;filename=TABELA_PRODUTOS_{DateTime.Now.ToString("yyyy_MM_dd")}.xlsx");

            XLWorkbook wbook = new XLWorkbook();
            List<Product> products = db.Product.ToList();

            DataTable dataTableProducts = Functions.GenerateDataTableProducts(products);

            wbook.Worksheets.Add(dataTableProducts, "TABELA_PRODUTOS");

            wbook.SaveAs(Response.OutputStream);
            wbook.Dispose();

            Response.End();

            return null;
        }
    }
}
