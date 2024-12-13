using Microsoft.EntityFrameworkCore;
using RentSmart.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentSmart.Data.Seeding
{
    internal class UserLikeSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.UserLikes.Any())
            {
                return;
            }

            // 300 UserLikes for renters
            for (int i = 0; i < 300; i++)
            {
                var user = await dbContext.Users.OrderBy(x => Guid.NewGuid()).FirstOrDefaultAsync();
                var property = await dbContext.Properties.OrderBy(x => Guid.NewGuid()).FirstOrDefaultAsync();
                var userLike = await dbContext.UserLikes.FirstOrDefaultAsync(x => x.UserId == user.Id && x.Property == property);
                if (userLike == null)
                {
                    userLike = new UserLike { Property = property, UserId = user.Id };
                    await dbContext.UserLikes.AddAsync(userLike);
                }
            }
        }
    }
}
