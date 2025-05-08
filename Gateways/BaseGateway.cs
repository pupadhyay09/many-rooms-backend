using ManyRoomStudio.Gateways.Interfaces;
using ManyRoomStudio.Infrastructure;
using ManyRoomStudio.Models.Entities;
using ManyRoomStudio.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Security.Claims;

namespace ManyRoomStudio.Gateways
{
    public class BaseGateway<T> : IBaseGateway<T>
         where T : class
    {
        private ManyRoomStudioContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        public BaseGateway(ManyRoomStudioContext context , IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            var context = CreateDbContext();
            return await context.Set<T>().ToListAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> predicate)
        {
            var context = CreateDbContext();
            var data =
                await context.Set<T>()
                    .Where(predicate).ToListAsync().ConfigureAwait(false);

            return data;
        }

        public async Task<T> Get(Expression<Func<T, bool>> expression)
        {
            var context = CreateDbContext();
            return await context.Set<T>()
                .FirstOrDefaultAsync(expression)
                .ConfigureAwait(false);
        }

        public async Task<T> GetById(int id)
        {
            var context = CreateDbContext();
            return await context.FindAsync<T>(id).ConfigureAwait(false);
        }

        public async Task<T> Add(T entity)
        {
            var context = CreateDbContext();
            UpdateCreatorInfo(ref entity);
            await context.AddAsync(entity).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);
            return entity;
        }

        public async Task<List<T>> AddRange(List<T> entities)
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));
            if (entities.Count == 0)
                throw new ArgumentException("Value cannot be an empty collection.", nameof(entities));

            var context = CreateDbContext();
            UpdateCreatorInfo(ref entities);

            await context.AddRangeAsync(entities).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);
            return entities;
        }

        public async Task<T> Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var context = CreateDbContext();
            UpdateModifierInfo(ref entity);
            context.Update(entity);
            await context.SaveChangesAsync().ConfigureAwait(false);
            return entity;
        }

        public async Task<List<T>> UpdateRange(List<T> entities)
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));
            if (entities.Count == 0)
                throw new ArgumentException("Value cannot be an empty collection.", nameof(entities));

            var context = CreateDbContext();
            UpdateModifierInfo(ref entities);
            context.UpdateRange(entities);
            await context.SaveChangesAsync().ConfigureAwait(false);
            return entities;
        }



        public virtual async Task<T> Get(int id)
        {
            var context = CreateDbContext();
            return await context.FindAsync<T>(id).ConfigureAwait(false);
        }

        private async Task<T> Get(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var context = CreateDbContext();
            var prop = entity.GetType().GetProperties().FirstOrDefault(p => p.Name.ToLower() == "id");
            int id = 0;
            if (prop != null)
                id = int.Parse(prop.GetValue(entity)?.ToString() ?? "0");

            var response = await context.FindAsync<T>(id).ConfigureAwait(false);
            if (response != null)
                context.Entry(response).State = EntityState.Detached;
            return response;
        }
        public async Task<T> Delete(int id)
        {
            var result = await Get(id).ConfigureAwait(false);
            if (result != null)
                await Remove(result).ConfigureAwait(false);
            return result;
        }

        public async Task<T> Remove(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var result = await Get(entity).ConfigureAwait(false);
            if (result != null)
            {
                var context = CreateDbContext();
                context.Remove(result);
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
            return entity;
        }

        public async Task<List<T>> RemoveAll(Expression<Func<T, bool>> expressions)
        {
            var db = CreateDbContext();
            var data = await db.Set<T>()
                .Where(expressions).ToListAsync().ConfigureAwait(false);
            foreach (var entity in data)
                db.Remove(entity);

            await db.SaveChangesAsync().ConfigureAwait(false);
            return data;
        }

        public async Task<List<T>> RemoveRange(List<T> entities)
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));
            if (entities.Count == 0)
                throw new ArgumentException("Value cannot be an empty collection.", nameof(entities));

            var context = CreateDbContext();
            context.RemoveRange(entities);
            await context.SaveChangesAsync().ConfigureAwait(false);
            return entities;
        }

        public ManyRoomStudioContext CreateDbContext()
        {
            return _context ?? Create();
        }

        public static ManyRoomStudioContext Create()
        {
            DbContextOptionsBuilder<ManyRoomStudioContext> optionsBuilder = new DbContextOptionsBuilder<ManyRoomStudioContext>();

            var connectionString = AppConfig.Get("ConnectionStrings:DefaultConnection");
            if (connectionString != null)
                optionsBuilder.UseSqlServer(connectionString);
            else
                throw new Exception($"Connection string is null.");

            return new ManyRoomStudioContext(optionsBuilder.Options);
        }


        public void SetDb(ManyRoomStudioContext context)
        {
            _context = context;
        }



        private void UpdateCreatorInfo(ref T entity)
        {
            if (typeof(T).GetProperties().Any(s => s.Name.ToLower() == ManyRoomStudioConstants .CreatedByColName.ToLower()))
            {
                if (_contextAccessor?.HttpContext?.User.Identity != null && _contextAccessor.HttpContext.User.Identity.IsAuthenticated)
                {
                    var userId = Convert.ToInt32(_contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
                    typeof(T).GetProperty(nameof(Auditable.CreatedBy))!.SetValue(entity, userId);
                    typeof(T).GetProperty(nameof(Auditable.UpdatedBy))!.SetValue(entity, userId);
                }
                typeof(T).GetProperty(nameof(Auditable.CreatedAt))!.SetValue(entity, DateTime.UtcNow);
                typeof(T).GetProperty(nameof(Auditable.UpdatedAt))!.SetValue(entity, DateTime.UtcNow);
               
            }
        }
        private void UpdateCreatorInfo(ref List<T> entities)
        {
            if (typeof(T).GetProperties().Any(s => s.Name.ToLower() == ManyRoomStudioConstants.CreatedByColName.ToLower()))
            {
                var userId = 0;
                if (_contextAccessor?.HttpContext?.User.Identity != null && _contextAccessor.HttpContext.User.Identity.IsAuthenticated)
                {
                    userId = Convert.ToInt32(_contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
                }
                var curDateTime = DateTime.UtcNow;
                foreach (var t in entities)
                {
                    typeof(T).GetProperty(nameof(Auditable.CreatedBy))!.SetValue(t, userId);
                    typeof(T).GetProperty(nameof(Auditable.UpdatedBy))!.SetValue(t, userId);
                    typeof(T).GetProperty(nameof(Auditable.CreatedAt))!.SetValue(t, curDateTime);
                    typeof(T).GetProperty(nameof(Auditable.UpdatedAt))!.SetValue(t, curDateTime);
                }
            }
        }

        private void UpdateModifierInfo(ref T entity)
        {
            if (typeof(T).GetProperties().Any(s => s.Name.ToLower() == ManyRoomStudioConstants.ModifiedByColName.ToLower()))
            {
                if (_contextAccessor?.HttpContext?.User.Identity != null && _contextAccessor.HttpContext.User.Identity.IsAuthenticated)
                {
                    var userId = Convert.ToInt32(_contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
                    typeof(T).GetProperty(nameof(Auditable.UpdatedBy))!.SetValue(entity, userId);
                }
                typeof(T).GetProperty(nameof(Auditable.UpdatedAt))!.SetValue(entity, DateTime.UtcNow);

            }
        }
        private void UpdateModifierInfo(ref List<T> entities)
        {
            if (typeof(T).GetProperties().Any(s => s.Name.ToLower() == ManyRoomStudioConstants.ModifiedByColName.ToLower()))
            {
                var userId = 0;
                if (_contextAccessor?.HttpContext?.User.Identity != null && _contextAccessor.HttpContext.User.Identity.IsAuthenticated)
                {
                    userId = Convert.ToInt32(_contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
                }
                var curDateTime = DateTime.UtcNow;
                foreach (var t in entities)
                {
                    typeof(T).GetProperty(nameof(Auditable.UpdatedBy))!.SetValue(t, userId);
                    typeof(T).GetProperty(nameof(Auditable.UpdatedAt))!.SetValue(t, curDateTime);
                }
            }
        }
    }
}
