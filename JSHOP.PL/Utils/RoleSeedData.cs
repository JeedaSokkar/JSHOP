using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace JSHOP.PL.Utils
{
    public enum Roles
    {
        Admin,
        User
    }
    public class RoleSeedData : ISeedData
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleSeedData(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task DataSeed()
        {
            foreach (var role in Enum.GetValues<Roles>())
            {
                if (!await _roleManager.RoleExistsAsync(role.ToString()))
                {
                    await _roleManager.CreateAsync(
                        new IdentityRole(role.ToString())
                    );
                }
            }
        }
    }
}
