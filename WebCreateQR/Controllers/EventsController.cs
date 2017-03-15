﻿using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebCreateQR.Models;
using ZXing;

namespace WebCreateQR.Controllers
{
    public class EventsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Events
        public ActionResult Index()
        {
            return View(db.Events.ToList());
        }

        // GET: Events/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // GET: Events/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EventId,EventName,EventLocation,StartDateTime,EndDateTime,TicketsAvailable")] Event @event)
        {
            if (ModelState.IsValid)
            {
                @event.RemainingTickets = @event.TicketsAvailable;
               
                db.Events.Add(@event);
                db.SaveChanges();
                Bitmap storedQr;
                string qrData = "event" + "," + @event.EventId + "," + @event.EventName + "," + @event.EventLocation + "," + @event.StartDateTime + "," + @event.EndDateTime;
                storedQr = QrCreate(qrData, @event.EventName);
                storedQr.Save(@"C:\Users\kevin\Desktop\" + @event.EventName + ".bmp");
                return RedirectToAction("Index");
            }

            return View(@event);
        }
        // GET: Events/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EventId,EventName,EventLocation,StartDateTime,EndDateTime,TicketsAvailable,RemainingTickets")] Event @event)
        {
            if (ModelState.IsValid)
            {
                if (@event.TicketsAvailable >= @event.RemainingTickets)
                {
                    db.Entry(@event).State = EntityState.Modified;
                    db.SaveChanges();
                    Bitmap storedQr;
                    string qrData = "event" + "," + @event.EventId + "," + @event.EventName + "," + @event.EventLocation + "," + @event.StartDateTime + "," + @event.EndDateTime;
                    storedQr = QrCreate(qrData, @event.EventName);
                    storedQr.Save(@"C:\Users\kevin\Desktop\" + @event.EventName + ".bmp");
                    return RedirectToAction("Index");
                }
            }
            return View(@event);
        }

        public ActionResult EditPoster(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Events/EditPoster/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPoster([Bind(Include = "EventId,EventName,EventLocation,StartDateTime,EndDateTime,TicketsAvailable,RemainingTickets")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Entry(@event).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(@event);
        }

        // GET: Events/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Event @event = db.Events.Find(id);
            db.Events.Remove(@event);
            db.SaveChanges();
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
        //creating and saving QrCode
        public Bitmap QrCreate(string qr, string saveTitle)
        {
            Bitmap storedQr;
            var writer = new ZXing.BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new ZXing.Common.EncodingOptions
                {
                    Height = 400,
                    Width = 400
                }
            };
            storedQr = writer.Write(qr);
            return storedQr;
        }
    }
}
