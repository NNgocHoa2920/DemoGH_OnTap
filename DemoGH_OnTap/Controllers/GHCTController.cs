using DemoGH_OnTap.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.Xml;

namespace DemoGH_OnTap.Controllers
{
    public class GHCTController : Controller
    {
        private readonly SD18406CartDbContext _db;
        public GHCTController(SD18406CartDbContext db)
        {
            _db = db;
        }
      
        public IActionResult Index()
        {
            var user = HttpContext.Session.GetString("username");

            //đầu tiên check xem trong session có dư liệu đăng nhập hay chưa
            if (user == null)
            {
                return Content("Đăng nhập đê bạn ek");
            }
            else
            {
                var getUser = _db.Accounts.FirstOrDefault(x => x.UserName == user);
                var giohang = _db.GioHang.FirstOrDefault(x => x.AccountID == getUser.Id);
                var GHCTdata = _db.GHCTs.Where(x => x.GioHangId == giohang.Id).ToList();

                return View(GHCTdata);
            }
        }
    }
}
