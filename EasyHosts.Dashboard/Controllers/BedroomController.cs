using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using EasyHosts.Dashboard.Models;
using EasyHosts.Dashboard.ViewModel;

namespace EasyHosts.Dashboard.Controllers
{
    public class BedroomController : Controller
    {
        private Context db = new Context();

        // GET: Bedroom
        public ActionResult Index()
        {
            var bedroom = db.Bedroom.Include(b => b.TypeBedroom);
            return View(bedroom.ToList());
        }

        // GET: Bedroom/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Quarto não encontrado!" });
            }
            Bedroom @bedroom = db.Bedroom.Find(id);
            if (@bedroom == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Não encontramos o quarto solicitado!" });
            }
            return View(@bedroom);
        }

        // GET: Bedroom/Create
        public ActionResult Create()
        {
            ViewBag.TypeBedroomId = new SelectList(db.TypeBedroom, "Id", "ApartmentAmenities");
            return View();
        }

        // POST: Bedroom/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,NameBedroom,Value,Description,Picture,Status,TypeBedroomId")] Bedroom @bedroom, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    MemoryStream memoryStream = new MemoryStream();
                    file.InputStream.CopyTo(memoryStream);
                    byte[] data = memoryStream.ToArray();
                    @bedroom.Picture = data;
                    db.Bedroom.Add(@bedroom);
                    db.SaveChanges();
                    TempData["MSG"] = "success|Quarto Criado com sucesso!";
                    return RedirectToAction("Index");
                }
            }
            TempData["MSG"] = "warning|Preencha todos os campos!";
            ViewBag.TypeBedroomId = new SelectList(db.TypeBedroom, "Id", "ApartmentAmenities", @bedroom.TypeBedroomId);
            return View(@bedroom);
        }

        // GET: Bedroom/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Quarto não encontrado!" });
            }
            Bedroom @bedroom = db.Bedroom.Find(id);
            if (@bedroom == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Não encontramos o quarto solicitado!" });
            }
            ViewBag.TypeBedroomId = new SelectList(db.TypeBedroom, "Id", "ApartmentAmenities", @bedroom.TypeBedroomId);
            return View(@bedroom);
        }

        // POST: Bedroom/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,NameBedroom,Value,Description,Picture,Status,TypeBedroomId")] Bedroom @bedroom, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    MemoryStream memoryStream = new MemoryStream();
                    file.InputStream.CopyTo(memoryStream);
                    byte[] data = memoryStream.ToArray();
                    @bedroom.Picture = data;
                    db.Entry(@bedroom).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["MSG"] = "success|Quarto editado com sucesso!";
                    return RedirectToAction("Index");
                }
            }
            return View(@bedroom);
        }

        // GET: Bedroom/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Quarto não encontrado!" });
            }
            Bedroom @bedroom = db.Bedroom.Find(id);
            if (@bedroom == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Não encontramos o quarto solicitado!" });
            }
            return View(@bedroom);
        }

        // POST: Bedroom/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Bedroom @bedroom = db.Bedroom.Find(id);
            db.Bedroom.Remove(@bedroom);
            db.SaveChanges();
            TempData["MSG"] = "success|Quarto deletado com sucesso!";
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
            Response.AddHeader("content-disposition", $"attachment;filename=TABELA_QUARTOS_{DateTime.Now.ToString("yyyy_MM_dd")}.xlsx");

            XLWorkbook wbook = new XLWorkbook();
            List<Bedroom> bedrooms = db.Bedroom.ToList();

            DataTable dataTableBedrooms = Functions.GenerateDataTableBedrooms(bedrooms);

            wbook.Worksheets.Add(dataTableBedrooms, "TABELA_QUARTOS");

            wbook.SaveAs(Response.OutputStream);
            wbook.Dispose();

            Response.End();

            return null;
        }

        public FileContentResult GetImage(int id)
        {
            byte[] byteArray = db.Bedroom.Find(id).Picture;

            return byteArray != null ? new FileContentResult(byteArray, "image/jpeg") : null;
        }
    }
}
