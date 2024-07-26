using DemoGH_OnTap.Models;
using Microsoft.AspNetCore.Mvc;

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
            // Đầu tiên Check xem trong Session có dữ liệu đăng nhập chưa?
            if (user == null)
            {
                //var dataSession = HttpContext.Session.GetString("ghSession");
                //var ghcts = JsonConvert.DeserializeObject<List<GHCT>>(dataSession);
                return Content("Chưa đăng nhập không có giỏ đâu");
            }
            else
            {
                var getUser = _db.Accounts.FirstOrDefault(p => p.UserName == user);
                var gioHang = _db.GioHang.FirstOrDefault(x => x.AccountID == getUser.Id);
                var GHCTdata = _db.GHCTs.Where(p => p.GioHangId == gioHang.Id).ToList();
                return View(GHCTdata);
            }
          
            
        }
    }
}
