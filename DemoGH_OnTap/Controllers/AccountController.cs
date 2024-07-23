using DemoGH_OnTap.Models;
using Microsoft.AspNetCore.Mvc;

namespace DemoGH_OnTap.Controllers
{
    public class AccountController : Controller
    {
        private readonly SD18406CartDbContext _db;
        public AccountController(SD18406CartDbContext db)
        {
            _db = db;
        }
 
        public IActionResult DangKy() // tạo ra view đăng kí thui
        {
            return View();
        }
        [HttpPost]
        public IActionResult Dangky(Account account)
        {

            try
            {
                //tạo mới 1 account
                _db.Accounts.Add(account);
                //Đồng thời tạo luon 1 giỏ hàng
                GioHang gioHang = new GioHang()
                {
                    UserName = account.UserName
                };
                _db.GioHang.Add(gioHang);
                _db.SaveChanges();
                TempData["Status"] = "Tạo tài khoản thành công";
                return RedirectToAction("Login");
            }
            catch (Exception ex) 
            {
                return BadRequest();
            }
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string userName, string password)
        {
            if(userName == null || password == null)
            {
                return View();
            }
            //tìm ra kiếm tài khoản đc nhập
            var acc = _db.Accounts.ToList().FirstOrDefault(x=>x.UserName == userName && x.Password==password ) ;
            if (acc == null) // trong trường hợp không tìm thấy dữ liệu Account tương ứng
            {
                return Content("Đăng nhập thất bại mời kiểm tra lại");
            }
            else // trong trường hợp thành công sẽ trả về trang chủ
            {
                HttpContext.Session.SetString("username", userName); // Lưu dữ liệu login vào Session
                return RedirectToAction("Index", "Home");
            }

        }

        //hiển thị tất cả danh sách account
        public IActionResult Index(string name)
        { 
            //lấy giá trị session có tên account
            var sessionData = HttpContext.Session.GetString("username");
            if(sessionData == null)
            {
                ViewData["message"] = "Bạn chưa đăng nhập hoặc phiên đăng nhập hết hạn";
            }
            else
            {
                ViewData["message"] = $"Chào mừng {sessionData} đến với bình ngyên vô tận";
            }
            //lấy toàn bộ account
            var accountData = _db.Accounts.ToList();

            //làm phần tìm kiếm
            //nếu name tìm kiếm rỗng thì nó sẽ trả về toàn bộ dữ liệu
            if(string.IsNullOrEmpty(name))
            {
                return View(accountData);
            }
            else
            {
                var searchData = _db.Accounts.Where(x => x.Name.ToLower().Contains(name.ToLower())).ToList();
                //lưu số lượng kết quả tìm thấy vào viewdara và viewbag
                ViewData["count"] = searchData.Count;
                ViewBag.Count = searchData.Count;
                //check tìm kiesm nếu k có
                if (searchData.Count == 0)
                {
                    return View(searchData);
                }
                else
                    return View(searchData);
            }

           
        }
    }
}
