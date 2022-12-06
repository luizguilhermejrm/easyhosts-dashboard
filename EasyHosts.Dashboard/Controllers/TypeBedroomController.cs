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
    [Authorize]
    public class TypeBedroomController : Controller
    {
        private Context db = new Context();

        // GET: TypeBedroom
        public ActionResult Index()
        {
            return View(db.TypeBedroom.ToList());
        }

        // GET: TypeBedroom/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Tipo de quarto não encontrado!" });
            }
            TypeBedroom typeBedroom = db.TypeBedroom.Find(id);
            if (typeBedroom == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Não encontramos o tipo de quarto solicitado!" });
            }
            return View(typeBedroom);
        }

        // GET: TypeBedroom/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TypeBedroom/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,NameTypeBedroom,AmountOfPeople,AmountOfBed,ApartmentAmenities")] TypeBedroom typeBedroom)
        {
            if (ModelState.IsValid)
            {
                db.TypeBedroom.Add(typeBedroom);
                db.SaveChanges();
                TempData["MSG"] = "success|Tipo de Quarto cadastrado com sucesso!";
                return RedirectToAction("Index");
            }
            TempData["MSG"] = "warning|Preencha todos os campos!";
            return View(typeBedroom);
        }

        // GET: TypeBedroom/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Tipo de quarto não encontrado!" });
            }
            TypeBedroom typeBedroom = db.TypeBedroom.Find(id);
            if (typeBedroom == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Não encontramos o tipo de quarto solicitado!" });
            }
            return View(typeBedroom);
        }

        // POST: TypeBedroom/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,NameTypeBedroom,AmountOfPeople,AmountOfBed,ApartmentAmenities")] TypeBedroom typeBedroom)
        {
            if (ModelState.IsValid)
            {
                db.Entry(typeBedroom).State = EntityState.Modified;
                db.SaveChanges();
                TempData["MSG"] = "success|Tipo de Quarto editado com sucesso!";
                return RedirectToAction("Index");
            }
            TempData["MSG"] = "warning|Preencha todos os campos!";
            return View(typeBedroom);
        }

        // GET: TypeBedroom/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Tipo de quarto não encontrado!" });
            }
            TypeBedroom typeBedroom = db.TypeBedroom.Find(id);
            if (typeBedroom == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Não encontramos o tipo de quarto solicitado!" });
            }
            return View(typeBedroom);
        }

        // POST: TypeBedroom/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TypeBedroom typeBedroom = db.TypeBedroom.Find(id);
            db.TypeBedroom.Remove(typeBedroom);
            db.SaveChanges();
            TempData["MSG"] = "success|Tipo de Quarto deletado com sucesso!";
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
            Response.AddHeader("content-disposition", $"attachment;filename=TABELA_TIPO_DE_QUARTOS_{DateTime.Now.ToString("yyyy_MM_dd")}.xlsx");

            XLWorkbook wbook = new XLWorkbook();
            List<TypeBedroom> typebedrooms = db.TypeBedroom.ToList();

            DataTable dataTableTypeBedrooms = Functions.GenerateDataTableTypeBedrooms(typebedrooms);

            wbook.Worksheets.Add(dataTableTypeBedrooms, "TABELA_TIPO_DE_QUARTOS");

            wbook.SaveAs(Response.OutputStream);
            wbook.Dispose();

            Response.End();

            return null;
        }
    }
}
