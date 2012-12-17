using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskListApp.Models;
using System.Web.Security;
using WebMatrix.WebData;
using TaskListApp.Filters;

namespace TaskListApp.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class TaskController : Controller
    {

        private TaskModelDataContext _context = new TaskModelDataContext();
        
        //
        // GET: /Task/

        public ActionResult Index()
        {
            var tasks = _context.Task.ToList();
            return View(tasks);
        }

        //
        // GET: /Task/Details/5

        public ActionResult Details(int id)
        {
            Task task = _context.Task.First(t => t.TaskId.Equals(id));
            if (task == null)
                return HttpNotFound();
            return View(task);
        }

        //
        // GET: /Task/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Task/Create

        [HttpPost]
        public ActionResult Create(Task task)
        {
            if (ModelState.IsValid)
            {
                task.UserId = WebSecurity.GetUserId(User.Identity.Name);
                task.IsComplete = false;
                _context.Task.InsertOnSubmit(task);
                _context.SubmitChanges();
                return RedirectToAction("Index");
            }

            return View(task);
        }

        //
        // GET: /Task/Edit/5

        public ActionResult Edit(int id)
        {
            Task findTask = _context.Task.First(t => t.TaskId.Equals(id));
            if (findTask == null)
                return HttpNotFound();
            return View(findTask);
        }

        //
        // POST: /Task/Edit/5

        [HttpPost]
        public ActionResult Edit(Task task)
        {
            if (ModelState.IsValid)
            {
                Task findTask = _context.Task.SingleOrDefault(t => t.TaskId.Equals(task.TaskId));
                findTask.Title = task.Title;
                findTask.Content = task.Content;
                findTask.Level = task.Level;
                findTask.IsComplete = task.IsComplete;

                _context.SubmitChanges();
                return RedirectToAction("Index");
            }

            return View(task);
        }

        //
        // GET: /Task/Delete/5

        public ActionResult Delete(int id)
        {
            Task findTask = _context.Task.First(t => t.TaskId.Equals(id));
            if (findTask == null)
                return HttpNotFound();
            return View(findTask);
        }

        //
        // POST: /Task/Delete/5

        [HttpPost]
        public ActionResult Delete(int id,FormCollection collection)
        {
            Task findTask = _context.Task.SingleOrDefault(t => t.TaskId.Equals(id));
            if (findTask == null)
                return HttpNotFound();
            _context.Task.DeleteOnSubmit(findTask);
            _context.SubmitChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Complete(int id)
        {
            Task findTask = _context.Task.SingleOrDefault(t => t.TaskId.Equals(id));
            if (findTask == null)
                return HttpNotFound();
            findTask.IsComplete = true;
            _context.SubmitChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Overview(string datetime)
        {
            var tasks=_context.Task.Where(t => t.Date == Convert.ToDateTime(datetime)).ToList();
            ViewBag.Date = datetime;
            return View(tasks);
        }

        [HttpPost]
        public ActionResult Overview(FormCollection collection)
        {
            string datetime = collection["taskDate"];
            return Overview(datetime);
        }


        public ActionResult Chart()
        {
            var tasks = _context.Task.ToList();
            return View(tasks);
        }

        [HttpGet]
        public ActionResult Analysize()
        {
            var tasks = _context.Task.ToList();
            return View(tasks);
        }
    }
}
