using ManyRoomStudio.Repository;
using System.Linq.Expressions;

namespace ManyRoomStudio.Gateways.Interfaces
{
    public interface IBaseGateway <T> where T : class
    {
        public Task<IEnumerable<T>> GetAll();
        public Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> predicate);
        public Task<T> Get(Expression<Func<T, bool>> expression);
        public Task<T> GetById(int id);
        public  Task<List<T>> AddRange(List<T> entities);
        public Task<T> Add(T entity);
        public Task<T> Update(T entity);
        public Task<List<T>> UpdateRange(List<T> entities);
        public Task<T> Delete(int id);
        public Task<List<T>> RemoveAll(Expression<Func<T, bool>> expressions);

        public Task<List<T>> RemoveRange(List<T> entities);
        public ManyRoomStudioContext CreateDbContext();

        public void SetDb(ManyRoomStudioContext context);
    }
}
