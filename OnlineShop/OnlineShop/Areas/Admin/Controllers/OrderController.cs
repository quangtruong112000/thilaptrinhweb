using Common;
using Model.Dao;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace OnlineShop.Areas.Admin.Controllers
{
    public class OrderController : BaseController
    {
        // GET: Admin/Order

        class tinhtrang
        {
           public int ID { get; set; }
           public string Name { get; set; }
            public List<tinhtrang> ListAll() {
                List<tinhtrang> tinhtrangs = new List<tinhtrang>();
                tinhtrang tinhtrang = new tinhtrang();
                tinhtrang.ID = 0;
                tinhtrang.Name = "Đang chờ";
                tinhtrangs.Add(tinhtrang);
                tinhtrang = new tinhtrang();
                tinhtrang.ID = 1;
                tinhtrang.Name = "Đang đóng gói";
                tinhtrangs.Add(tinhtrang);
                tinhtrang = new tinhtrang();
                tinhtrang.ID = 2;
                tinhtrang.Name = "Đang vẫn chuyển ";
                tinhtrangs.Add(tinhtrang);
                tinhtrang = new tinhtrang();
                tinhtrang.ID = 4;
                tinhtrang.Name = "Đã giao";
                tinhtrangs.Add(tinhtrang);
                return tinhtrangs;

            }
        }
        public ActionResult Index(string searchString, int page = 1, int pageSize = 5)
        {
            var dao = new OrderDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }
        public void SetViewBag(long? selectedId = null)
        {
 
            ViewBag.CategoryID = new SelectList(new tinhtrang().ListAll(), "ID", "Name", selectedId);

        }


        public ActionResult Edit(int id)
        {

            var order = new OrderDao().ViewDetail(id);
            SetViewBag(order.Status);
            return View(order);
        }
        public string guimail(string Lydo, long ID)
        {
            var dao = new OrderDao();
            var order = new OrderDao().ViewDetail(ID);
            order.Status = 5;
            var result = dao.Update(order);
            // var rep = new FeedbackDao().Reply(feedback);
            string content = "Xin chào "+ order.ShipName+ " Shop mình xin lỗi quý khách đơn hàng của quý khách đã bị hủy vì  : " + Lydo + " xin quý khách thông cảm vì sự cố của bên tôi.........";
          //  var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();
            new MailHelper().SendEmail(order.ShipEmail, "Thông báo từ OnlineShop" , content);

            return "";

        }

        [HttpPost]
        public ActionResult Edit(Order order)
        {
            if (ModelState.IsValid)
            {

                var dao = new OrderDao();
                var order1 = new OnlineShopDbContext().Orders.Find(order.ID);


                var result = dao.Update(order);
                if (result)
                {
                    SetAlert("Cập nhật thành công", "success");
                    return RedirectToAction("Index", "Order");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật không thành công");
                }
            }
            SetViewBag(order.Status);
            return View("Index");
        }
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            new OrderDao().Delete(id);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var result = new OrderDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }       
    }
}