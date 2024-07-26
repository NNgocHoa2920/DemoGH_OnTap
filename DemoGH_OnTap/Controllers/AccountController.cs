using DemoGH_OnTap.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;

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
                    UserName = account.UserName,
                    AccountID = account.Id

                };
                _db.GioHang.Add(gioHang);
                _db.SaveChanges();
                TempData["Status"] = "Tạo tài khoản thành công";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                // Ghi log lỗi và trả về phản hồi lỗi
                
                return BadRequest("Lỗi khi tạo tài khoản và giỏ hàng.");
            }
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]


        public IActionResult Login(string userName, string password)
        {
            if (userName == null || password == null)
            {
                return View();
            }
            //tìm ra kiếm tài khoản đc nhập
            var acc = _db.Accounts.ToList().FirstOrDefault(x => x.UserName == userName && x.Password == password);
            if (acc == null) // trong trường hợp không tìm thấy dữ liệu Account tương ứng
            {
                return Content("Đăng nhập thất bại mời kiểm tra lại");
            }
            else // trong trường hợp thành công sẽ trả về trang chủ
            {
                HttpContext.Session.SetString("username", userName); // Lưu dữ liệu login vào Session
                HttpContext.Session.SetString("userID", acc.Id.ToString());  // lưu id
                return RedirectToAction("Index", "Home");
            }

        }

        //hiển thị tất cả danh sách account
        public IActionResult Index(string name)
        {
            //lấy giá trị session có tên account
            var sessionData = HttpContext.Session.GetString("username");
            if (sessionData == null)
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
            if (string.IsNullOrEmpty(name))
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

        //Thêm 1 account = đăng kí
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Account account)
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

        //Xóa 1 ACCOUNT
        public IActionResult Delete(Guid id)
        {
            //lấy ra đói tượng cần xóa
            var deleteAccount = _db.Accounts.Find(id);
            //nếu mà là roll back hoặc muốn xem lại dữ liệu đã xóa thì làm còn  thì thôi
            var jsonData = JsonConvert.SerializeObject(deleteAccount);// ép kiểu dữ liệu sang kiểu json
            HttpContext.Session.SetString("deleted", jsonData);
            _db.Remove(deleteAccount);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        //Xem lại dữ liệu đã xóa
        public IActionResult RetriewDeleteData()
        {
            //lấy dữ liệu đã xóa đc lưu vào sessiomn

           var jsonData =  HttpContext.Session.GetString("deleted");
            if(jsonData!=null)
            {
                var deleteAcc = JsonConvert.DeserializeObject<Account>(jsonData);
                return View("DeletedUserDetails", deleteAcc);
            }
            else
            {
                //nếu k tìm thấy dữ liệu lưu trong session
                return RedirectToAction("Index");
            }
        }

        //roll back, add lại dữ liệu đã xóa
        public IActionResult RollBack()
        {
            if (HttpContext.Session.Keys.Contains("deleted")) 
            {
                var jsonData = HttpContext.Session.GetString("deleted");
                //tạo đối jg có dữ liệu y hệt như dữ liệu cũ
                var deleteAccount = JsonConvert.DeserializeObject<Account> (jsonData);
                _db.Accounts.Add(deleteAccount);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return Content("Error");
            }

            
        }
    }
}
