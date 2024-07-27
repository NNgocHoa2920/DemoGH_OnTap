namespace DemoGH_OnTap.Models
{
    public class GHCT
    {
        public Guid Id { get; set; }
        public Guid? SanPhamId { get; set; }
        public Guid ? GioHangId { get; set; }
     
        public int Amount { get; set; }
        public GioHang? GioHang { get; set; }  ///khi tạo câu lệnh này sẽ tự động tạo khóa ngoại => đại diện khóa ngoại luon
        public SanPham? SanPham { get; set; }
    }
}
