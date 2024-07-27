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

        ////CÁCH 1: THÊM VÀO GIỎ HÀNG
        //public IActionResult AddToCart(Guid id, int amount) // id này là id của sp khi add
        //{
        //    //lây ra username tương ứng với giỏ hàng đó
        //    var user = HttpContext.Session.GetString("username");
        //    if(user == null)
        //    {
        //        return Content("Chưa đăng nhập hoặc phiên đăng nhập hét hạn");
        //    } 
        //    //lấy thông của user của người dùng/ getUser là 1 đối tượng người
        //    var getUser = _db.Accounts.FirstOrDefault(x=>x.UserName == user);
        //    //lấy giỏ hàng tương ứng với người dùng
        //    var giohang = _db.GioHang.FirstOrDefault(x=>x.AccountID == getUser.Id);
        //    if(giohang == null)
        //    {
        //        return Content("giỏ hàng không tồn tại");
        //    }
        //    //bắt đầu thêm
        //    var ghct = new GHCT
        //    {
        //        //Id = Guid.NewGuid(), //bỏ đi vì khi tạo mới nó tự lấy id lắp vào
        //        Amount = amount,
        //        GioHangId = giohang.Id,
        //        SanPhamId = id

        //    };
        //    _db.GHCTs.Add(ghct);
        //    _db.SaveChanges();
        //    return RedirectToAction("Index");

        //}

        //CÁCH 2: Cộng dồn san phảm trong giỏ hàng
        //CÁCH 1: THÊM VÀO GIỎ HÀNG
        public IActionResult AddToCart(Guid id, int amount) // id này là id của sp khi add
        {
            //lây ra username tương ứng với giỏ hàng đó
            var user = HttpContext.Session.GetString("username");
            if (user == null)
            {
                return Content("Chưa đăng nhập hoặc phiên đăng nhập hét hạn");
            }
            //lấy thông của user của người dùng/ getUser là 1 đối tượng người
            var getUser = _db.Accounts.FirstOrDefault(x => x.UserName == user);
            //lấy giỏ hàng tương ứng với người dùng
            var giohang = _db.GioHang.FirstOrDefault(x => x.AccountID == getUser.Id);
            if (giohang == null)
            {
                return Content("giỏ hàng không tồn tại");
            }
            
            //lấy ra toàn bộ GHCT của user
            var userCart = _db.GHCTs.Where(x=>x.GioHangId == giohang.Id).ToList();
            bool check = false;
            Guid idGHCT = Guid.Empty;
            //duyệt ghct
            foreach(var item in  userCart)
            {
                if( item.SanPhamId == id)
                {
                    //nếu id sp trong giỏ hàng của user trùng với cái được chọn
                    check = true;
                    idGHCT = item.Id; //lấy ra id của GHCT để tí nữa update
                    break;
                      
                }    
            } 
            if(!check) // nếu sp của mình chưa từng đc chọn
            {
                //tạo ra 1 GHCT ứng với srn phẩm đó
                GHCT ghct = new GHCT()
                {
                    SanPhamId = id,
                    GioHangId = giohang.Id,
                    Amount = amount,

                };
                _db.GHCTs.Add(ghct);
                _db.SaveChanges();
                return RedirectToAction("index");

            }
            else // sp được dc chọn dồi
            {
                var ghctUpdate = _db.GHCTs.FirstOrDefault(x => x.Id == idGHCT); // tìm theo ghct
                ghctUpdate.Amount = ghctUpdate.Amount + amount;
                _db.GHCTs.Update(ghctUpdate);
                _db.SaveChanges();
                return RedirectToAction("index");
            }
            

        }

    }
}

