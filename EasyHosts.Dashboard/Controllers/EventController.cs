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
    [Authorize]
    public class EventController : Controller
    {
        private Context db = new Context();

        // GET: Event
        public ActionResult Index()
        {
            return View(db.Event.ToList());
        }

        // GET: Event/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Evento não encontrado!" });
            }
            Event @event = db.Event.Find(id);
            if (@event == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Não encontramos o evento solicitado!" });
            }
            return View(@event);
        }

        // GET: Event/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Event/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,NameEvent,Organizer,DateEvent,EventsPlace,Picture,DescriptionEvent,Attractions,TypeEvent")] Event @event, HttpPostedFileBase file)
        {
            {
                if (ModelState.IsValid)
                {
                    if (file != null)
                    {
                        MemoryStream memoryStream = new MemoryStream();
                        file.InputStream.CopyTo(memoryStream);
                        byte[] data = memoryStream.ToArray();
                        @event.Picture = data;
                        db.Event.Add(@event);
                        db.SaveChanges();
                        TempData["MSG"] = "success|Evento Criado com sucesso!";
                        return RedirectToAction("Index");
                    }

                }
                TempData["MSG"] = "warning|Preencha todos os campos!";
                return View(@event);
            }

        }

        // GET: Event/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Evento não encontrado!" });
            }
            Event @event = db.Event.Find(id);
            if (@event == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Não encontramos o evento solicitado!" });
            }
            return View(@event);
        }

        // POST: Event/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,NameEvent,Organizer,DateEvent,EventsPlace,Picture,DescriptionEvent,Attractions,TypeEvent")] Event @event, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    MemoryStream memoryStream = new MemoryStream();
                    file.InputStream.CopyTo(memoryStream);
                    byte[] data = memoryStream.ToArray();
                    @event.Picture = data;
                    db.Entry(@event).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["MSG"] = "success|Evento editado com sucesso!";
                    return RedirectToAction("Index");
                }
            }
            return View(@event);
        }

        // GET: Event/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Evento não encontrado!" });
            }
            Event @event = db.Event.Find(id);
            if (@event == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Não encontramos o evento solicitado!" });
            }
            return View(@event);
        }

        // POST: Event/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Event @event = db.Event.Find(id);
            db.Event.Remove(@event);
            db.SaveChanges();
            TempData["MSG"] = "success|Evento deletado com sucesso!";
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
            Response.AddHeader("content-disposition", $"attachment;filename=TABELA_EVENTOS_{DateTime.Now.ToString("yyyy_MM_dd")}.xlsx");

            XLWorkbook wbook = new XLWorkbook();
            List<Event> events = db.Event.ToList();

            DataTable dataTableEvents = Functions.GenerateDataTableEvents(events);

            wbook.Worksheets.Add(dataTableEvents, "TABELA_EVENTOS");

            wbook.SaveAs(Response.OutputStream);
            wbook.Dispose();

            Response.End();

            return null;
        }

        public FileContentResult GetImage(int id)
        {
            byte[] byteArray = db.Event.Find(id).Picture;

            return byteArray != null ? new FileContentResult(byteArray, "image/jpeg") : null;
        }
    }
}
