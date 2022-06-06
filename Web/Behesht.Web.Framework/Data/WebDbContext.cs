using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Behesht.Core;
using Behesht.Data;
using System;
using System.Linq;
using System.Security.Claims;

namespace Behesht.Web.Framework.Data
{
    public class WebDbContext : BeheshtDbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WebDbContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            SavingChanges += WebDbContext_SavingChanges;
        }

        

        private void WebDbContext_SavingChanges(object sender, SavingChangesEventArgs e)
        {
            DateTime now = DateTime.Now;
            var savingContext = (DbContext)sender;
            var user = _httpContextAccessor.HttpContext?.User;
            long userId = user == null ? 0 : Convert.ToInt64(user.FindFirstValue("id"));

            var changedEntries = ChangeTracker.Entries().Where(p => p.State != EntityState.Unchanged);
            foreach (var item in changedEntries)
            {
                if (item.Entity is BaseEntity entity)
                {
                    switch (item.State)
                    {
                        case EntityState.Modified:
                            {
                                if (entity.IsDelete)
                                {
                                    entity.DeleteDate = now;
                                    entity.DeleteUserId = userId;
                                }
                                else
                                {
                                    entity.ModifyDate = now;
                                    entity.ModifyUserId = userId;
                                }
                                break;
                            }
                        case EntityState.Added:
                            {
                                entity.CreateDate = now;
                                entity.CreateUserId = Convert.ToInt64(user.FindFirstValue("id"));
                                break;
                            }
                        default:
                            break;
                    }
                }
            }
        }
    }
}
