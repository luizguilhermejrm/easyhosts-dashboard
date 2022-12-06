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
    public class BookingController : Controller
    {
        private Context db = new Context();

        // GET: Booking
        public ActionResult Index()
        {
            var booking = db.Booking.Include(b => b.Bedroom).Include(b => b.User);
            return View(booking.ToList());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public JsonResult AlterStatus(string id)
        {
            Booking booking = db.Booking.Find(Convert.ToInt32(id));
            if (booking != null)
            {
                booking.Status = Models.Enums.BookingStatus.NoShow;
                db.Entry(booking).State = EntityState.Modified;
                db.SaveChanges();
                return Json(booking.Status);
            }
            else
            {
                return Json("n");
            }
        }

        // GET: Booking/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Reserva não encontrada!" });
            }
            Bedroom booking = db.Bedroom.Find(id);
            if (booking == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Não encontramos a reserva solicitada!" });
            }
            return View(booking);
        }

        // GET: Booking/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Booking.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            ViewBag.BedroomId = new SelectList(db.Bedroom, "Id", "NameBedroom", booking.BedroomId);
            ViewBag.UserId = new SelectList(db.User, "Id", "Name", booking.UserId);
            return View(booking);
        }

        // POST: Booking/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CodeBooking,UserId,DateCheckin,DateCheckout,ValueBooking,BedroomId,Status")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(booking).State = EntityState.Modified;
                db.SaveChanges();
                TempData["MSG"] = "success|Reserva editada com sucesso!";
                return RedirectToAction("Index");
            }
            ViewBag.BedroomId = new SelectList(db.Bedroom, "Id", "NameBedroom", booking.BedroomId);
            ViewBag.UserId = new SelectList(db.User, "Id", "Name", booking.UserId);
            return View(booking);
        }

        // GET: Booking/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Reserva não encontrada!" });
            }
            Booking booking = db.Booking.Find(id);
            if (booking == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Não encontramos a reserva solicitada!" });
            }
            return View(booking);
        }

        // POST: Booking/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Booking booking = db.Booking.Find(id);
            db.Booking.Remove(booking);
            db.SaveChanges();
            TempData["MSG"] = "success|Reserva deletada com sucesso!";
            return RedirectToAction("Index");
        }

        public ActionResult ExportExcel()
        {

            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", $"attachment;filename=TABELA_RESERVAS_{DateTime.Now.ToString("yyyy_MM_dd")}.xlsx");

            XLWorkbook wbook = new XLWorkbook();
            List<Booking> bookings = db.Booking.ToList();

            DataTable dataTableBookings = Functions.GenerateDataTableBookings(bookings);

            wbook.Worksheets.Add(dataTableBookings, "TABELA_RESERVAS");

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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
