using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QrAttendanceApi.Domain.Enums;

namespace QrAttendanceApi.Infrastructure.Configurations
{
    public class RolesConfigurations : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole
                {
                    Id = new Guid("BEF477B0-E65D-4C85-A6F3-8B44E89AF17E").ToString(),
                    Name = Roles.Student.ToString(),
                    NormalizedName = Roles.Student.ToString().ToUpper(),
                    //ConcurrencyStamp = DateTime.UtcNow.ToString()
                    ConcurrencyStamp = "STUDENT_ROLE"
                    //ConcurrencyStamp = "c1f0d0e0-0000-0000-0000-000000000001"

                },
                new IdentityRole
                {
                    Id = new Guid("32751251-5533-4004-B248-D8D8EF427CE2").ToString(),
                    Name = Roles.Staff.ToString(),
                    NormalizedName = Roles.Staff.ToString().ToUpper(),
                    //ConcurrencyStamp = DateTime.UtcNow.ToString()
                    ConcurrencyStamp = "STAFF_ROLE"
                    //ConcurrencyStamp = "c1f0d0e0-0000-0000-0000-000000000001"

                },
                new IdentityRole
                {
                    Id = new Guid("504EA7A6-FB72-43A8-8C1E-628BD4ABABD1").ToString(),
                    Name = Roles.Admin.ToString(),
                    NormalizedName = Roles.Admin.ToString().ToUpper(),
                    //ConcurrencyStamp = DateTime.UtcNow.ToString()
                    ConcurrencyStamp = "ADMIN_ROLE"
                    //ConcurrencyStamp = "c1f0d0e0-0000-0000-0000-000000000001"

                },
                new IdentityRole
                {
                    Id = new Guid("41DE4677-6D68-4854-8262-CBEAA486FE4C").ToString(),
                    Name = Roles.SuperAdmin.ToString(),
                    NormalizedName = Roles.SuperAdmin.ToString().ToUpper(),
                    //ConcurrencyStamp = DateTime.UtcNow.ToString()
                    ConcurrencyStamp = "SUPERADMIN_ROLE"
                    //ConcurrencyStamp = "c1f0d0e0-0000-0000-0000-000000000001"

                }
            );
        }
    }
}