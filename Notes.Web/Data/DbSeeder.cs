using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Notes.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notes.Web.Data
{
    public class DbSeeder
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SetupUser _user;

        public DbSeeder(ApplicationDbContext db, UserManager<ApplicationUser> userManager, IOptions<SetupUser> user)
        {
            _db = db;
            _userManager = userManager;
            _user = user.Value;
        }

        public async Task Seed()
        {
            await _db.Database.EnsureCreatedAsync();

            // Clear if needed
            // await ClearDatabase();

            if (await _db.Users.CountAsync() == 0)
            {
                await CreateUsersAsync();
            }
        }

        private async Task ClearDatabase()
        {
            _db.TagItems.RemoveRange(await _db.TagItems.ToListAsync());
            _db.Tags.RemoveRange(await _db.Tags.ToListAsync());
            _db.Assets.RemoveRange(await _db.Assets.ToListAsync());
            _db.Notes.RemoveRange(await _db.Notes.ToListAsync());
            _db.Notebooks.RemoveRange(await _db.Notebooks.ToListAsync());
            _db.Users.RemoveRange(await _db.Users.ToListAsync());

            await _db.SaveChangesAsync();
        }

        private async Task CreateUsersAsync()
        {
            var user = new ApplicationUser { UserName = _user.Email, Email = _user.Email };
            var result = await _userManager.CreateAsync(user, _user.Password);
            if (!result.Succeeded)
            {
                throw new Exception("Failed creating user");
            }
        }
    }
}
