using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ponto_Eletronico.Models;

namespace Ponto_Eletronico.Controllers
{
    public class PontosController : Controller
    {
        private Ponto_EletronicoEntities db = new Ponto_EletronicoEntities();

        // GET: Pontos
        public ActionResult Index()
        {
            var ponto = db.Ponto.Include(p => p.Funcionario);
            return View(ponto.ToList());
        }

        // GET: Pontos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ponto ponto = db.Ponto.Find(id);
            if (ponto == null)
            {
                return HttpNotFound();
            }
            return View(ponto);
        }

        // GET: Pontos/Create
        public ActionResult Create()
        {
            ViewBag.id_Funcionario = new SelectList(db.Funcionario, "Id", "nome");
            return View();
        }

        // POST: Pontos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,id_Funcionario,data_hora_entrada,data_hora_saida")] Ponto ponto)
        {
            if (ModelState.IsValid)
            {
                db.Ponto.Add(ponto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_Funcionario = new SelectList(db.Funcionario, "Id", "nome", ponto.id_Funcionario);
            return View(ponto);
        }

        // GET: Pontos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ponto ponto = db.Ponto.Find(id);
            if (ponto == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_Funcionario = new SelectList(db.Funcionario, "Id", "nome", ponto.id_Funcionario);
            return View(ponto);
        }

        // POST: Pontos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,id_Funcionario,data_hora_entrada,data_hora_saida")] Ponto ponto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ponto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_Funcionario = new SelectList(db.Funcionario, "Id", "nome", ponto.id_Funcionario);
            return View(ponto);
        }

        // GET: Pontos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ponto ponto = db.Ponto.Find(id);
            if (ponto == null)
            {
                return HttpNotFound();
            }
            return View(ponto);
        }

        // POST: Pontos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ponto ponto = db.Ponto.Find(id);
            db.Ponto.Remove(ponto);
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
    }
}
