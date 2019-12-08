using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace _1250865___MVC_Project.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        public ActionResult AddImage()
        {
            Image img = new Image();
            return View(img);
        }

        [HttpPost]
        public ActionResult AddImage(Image model, HttpPostedFileBase image1)
        {
            var db = new OnlineShopEntities();
            try
            {
                if (image1 != null)
                {
                    model.ProductImage = new byte[image1.ContentLength];
                    image1.InputStream.Read(model.ProductImage, 0, image1.ContentLength);
                }
                image1.SaveAs(Server.MapPath("~/Images/" + image1.FileName));
                db.Images.Add(model);
                db.SaveChanges();
                //return View(model);
                return RedirectToAction("Index1");
            }
            catch (Exception e)
            {

                throw e;
            }
            

        }
        [AllowAnonymous]
        public ActionResult Index1()
        {
            var db = new OnlineShopEntities();
            var item = (from d in db.Images
                        select d).ToList();
            return View(item);
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        OnlineShopEntities db = new OnlineShopEntities();

        public ActionResult Index2()
        {
            return View();
        }
        public ActionResult PartialViews()
        {
            return View(db.Orders.ToList());
        }

        // Return all students
        public PartialViewResult All()
        {
            List<Order> model = db.Orders.ToList();
            return PartialView("_Orders", model);
        }

        // Return Top3 students
        public PartialViewResult Top3()
        {
            List<Order> model = db.Orders.OrderByDescending(x => x.Amount).Take(3).ToList();
            return PartialView("_Orders", model);
        }

        // Return Bottom3 students
        public PartialViewResult Bottom3()
        {
            List<Order> model = db.Orders.OrderBy(x => x.Amount).Take(3).ToList();
            return PartialView("_Orders", model);
        }
        
        public ActionResult CustomerList()
        {
            return View(db.Customers.ToList());
        }

        public ActionResult exportReport()
        {
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Report"), "CrystalReport.rpt"));
            rd.SetDataSource(db.Customers.ToList());
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "Customer_list.pdf");
            }
            catch
            {
                throw;
            }
        }
    }
}