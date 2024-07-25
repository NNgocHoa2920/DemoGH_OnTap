using DemoGH_OnTap.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoGH_OnTap.Configurations
{
    public class GHCTConfig : IEntityTypeConfiguration<GHCT>
    {
        public void Configure(EntityTypeBuilder<GHCT> builder)
        {
            builder.HasKey(x => x.Id);

            //csu hình mqh 1-n giữa sanpham và ghct
            builder.HasOne(x => x.SanPham).WithMany(x => x.GHCTs)
                .HasForeignKey(x => x.SanPhamId);
            builder.HasOne(x => x.GioHang).WithMany(x => x.GHCTs)
              .HasForeignKey(x => x.GioHangId);
        }
    }
}
