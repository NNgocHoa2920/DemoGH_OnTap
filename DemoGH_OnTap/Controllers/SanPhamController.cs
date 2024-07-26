using DemoGH_OnTap.Models;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DemoGH_OnTap.Controllers
{
    public class SanPhamController : Controller
    {
        SD18406CartDbContext _db;
        public SanPhamController(SD18406CartDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var spData = _db.SanPhams.ToList();
            return View(spData);
        }
        public IActionResult Create() // dùng đẻ hiển thị ra trang create
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(SanPham sanPham)
        {
            _db.SanPhams.Add(sanPham);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Edit(Guid id)
        {
            var spEdit = _db.SanPhams.Find(id);

            return View(spEdit);
        }
        [HttpPost]
        public IActionResult Edit(SanPham sanPham)
        {

            _db.SanPhams.Update(sanPham);
            _db.SaveChanges();
            return RedirectToAction("Index");


        }
        //public IActionResult AddToCart(Guid id, int amount)
        //{
        //    var sessionData = HttpContext.Session.GetString("username");

        //    if (sessionData == null)
        //    {
        //        return Content("Chưa đăng nhập không có giỏ đâu");
        //    }

        //    var getUser = _db.Accounts.FirstOrDefault(p => p.UserName == sessionData);
        //    if (getUser == null)
        //    {
        //        return Content("Người dùng không tồn tại");
        //    }

        //    Kiểm tra xem giỏ hàng của người dùng có tồn tại không
        //    var gioHang = _db.GioHang.Find(getUser.Id);
        //    var gioHang = _db.GioHang.FirstOrDefault(x => x.AccountID == getUser.Id);
        //    đoạn này ko dùng find được, vì m tìm trong GioHang thì ID của nó là của GIỎ HÀNG,
        //    cái find là tìm ID trùng với ID truyền vào, thì truyền ID user vào để tìm trong giỏ hàng làm sao được,
        //     cái này phải đổi thành FirstOrDefault tìm ra giỏ hàng nào có account ID giống với user id

        //    if (gioHang == null)
        //    {
        //        return Content("Giỏ hàng của người dùng không tồn tại");
        //    }

        //    var ghctSP = new GHCT
        //    {
        //        Id = Guid.NewGuid(),
        //        Amount = amount,
        //        GioHangId = gioHang.Id,
        //        SanPhamId = id
        //    };

        //    try
        //    {
        //        _db.GHCTs.Add(ghctSP);
        //        _db.SaveChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content($"Lỗi khi thêm sản phẩm vào giỏ hàng: {ex.Message}");
        //    }

        //    return RedirectToAction("Index", "GHCT");
        //}


        public IActionResult AddToCart(Guid id, int amount) // id này là id của sản phẩm
        {
            // Thực hiện việc kiểm tra xem đã đunk nhặp chưa
            //var logindata = HttpContext.Session.GetString("username");
            var logindata = HttpContext.Session.GetString("userID"); // nếu lưu id thì dùng cái lày

            if (logindata == null) return Content("Đã đặng nhập đâu mà đòi thêm thắt cái gì?");
            else
            {
                //var getUser = _db.Accounts.FirstOrDefault(p => p.UserName == logindata);
                //var gioHang = _db.GioHang.FirstOrDefault(x => x.AccountID == getUser.Id);
                // Lấy ra tất cả sản phẩm trong giỏ hàng của khách hàng vừa đăng nhập

                //var userCart = _db.GHCTs.Where(p => p.GioHangId == gioHang.Id).ToList();
                // lấy ra giỏ hàng của người dùng
                var gioHang = _db.GioHang.FirstOrDefault(x => x.AccountID == Guid.Parse(logindata));
                if (gioHang == null)
                {
                    return Content("Khong tim thay gio hang");
                }
                var userCart = _db.GHCTs.Where(p => p.GioHangId == gioHang.Id).ToList();
                bool checkSelected = false;
                Guid idGHCT = Guid.Empty;
                foreach (var item in userCart)
                {
                    if (item.SanPhamId == id)
                    {
                        //nếu id sp trong giỏ hàng của user đã trùng vs cái đc chọn
                        checkSelected = true;
                        idGHCT = item.Id; //Lấy ra cái ID của GHCT để tẹo nữa ta update
                        break;
                    }
                }
                if (!checkSelected) // Nếu sản phẩm chưa từng được chọn
                {
                    // tạo ra 1 GHCT ứng với Sản phẩm đó
                    GHCT ghct = new GHCT() {  SanPhamId = id, GioHangId = gioHang.Id };
                    _db.GHCTs.Add(ghct);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else // Nếu SP đã được chọn
                {
                    var ghctUpdate = _db.GHCTs.FirstOrDefault(x => x.GioHangId == gioHang.Id); // tìm theo giỏ hàng tìm được ở trên
                    ghctUpdate.Amount = ghctUpdate.Amount + amount;
                    _db.GHCTs.Update(ghctUpdate);
                    _db.SaveChanges(); // Lưu lại
                    return RedirectToAction("Index");
                }
            }

        }
    }
}

