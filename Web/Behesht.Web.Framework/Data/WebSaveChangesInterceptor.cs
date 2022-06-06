using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Behesht.Core;

namespace Behesht.Web.Framework.Data
{
    public class WebSaveChangesInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            var _httpContextAccessor = ServiceProviderStore.Provider.GetService<IHttpContextAccessor>();
            DateTime now = DateTime.Now;
            var savingContext = eventData.Context;
            var user = _httpContextAccessor.HttpContext?.User;
            long userId = user == null ? 0 : Convert.ToInt64(user.Claims.FirstOrDefault(p => p.Type == "sub" || p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value);

            var changedEntries = savingContext.ChangeTracker.Entries().Where(p => p.State != EntityState.Unchanged);
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
                                entity.ModifyDate = now;
                                entity.CreateUserId = userId;
                                break;
                            }
                        default:
                            break;
                    }
                }
            }
            return base.SavingChanges(eventData, result);
        }
    }
}
