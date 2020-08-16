
using Strong.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YunTuCore.IRepository;

namespace YunTuCore.Repository
{
    public abstract class BaseRepository<TEntity,TKey> : IBaseRepository<TEntity,TKey> where TEntity : class, new()
    {
        public SqlSugarClient  _Db { get; } = null;

        public BaseRepository(SqlSugarClient Db) 
        {
            _Db = Db ?? throw new ArgumentNullException(nameof(MyContext));

        }

        public int Add(TEntity model)
        {
            throw new NotImplementedException();
        }

        public int Add(List<TEntity> model)
        {
            throw new NotImplementedException();
        }

        public int Add(TEntity entity, Expression<Func<TEntity, object>> insertColumns = null)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddAsync(TEntity model)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddAsync(TEntity entity, Expression<Func<TEntity, object>> insertColumns = null)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddAsync(List<TEntity> entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(TKey id)
        {
            throw new NotImplementedException();
        }

        public bool Delete(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(TKey[] ids)
        {
            throw new NotImplementedException();
        }

        public bool Delete(List<TEntity> entitys)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Expression<Func<TEntity, bool>> whereExpression)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(TKey id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(TKey[] ids)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(List<TEntity> entitys)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> whereExpression)
        {
            throw new NotImplementedException();
        }

        public bool Update(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public int Update(TEntity entity, List<string> fileds)
        {
            throw new NotImplementedException();
        }

        public bool Update(List<TEntity> entitys)
        {
            throw new NotImplementedException();
        }

        public int Update(List<TEntity> entitys, List<string> fileds)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(TEntity entity, List<string> fileds)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(List<TEntity> entitys)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(List<TEntity> entitys, List<string> fileds)
        {
            throw new NotImplementedException();
        }

        public TEntity FindWhere(Expression<Func<TEntity, bool>> whereExpression)
        {
            throw new NotImplementedException();
        }

        public TEntity Query(TKey id, bool blnUseCache = false)
        {
            throw new NotImplementedException();
        }

        public List<TEntity> Query()
        {
            throw new NotImplementedException();
        }

        public List<TEntity> Query(Expression<Func<TEntity, bool>> whereExpression)
        {
            throw new NotImplementedException();
        }

        public List<TEntity> Query(Expression<Func<TEntity, bool>> whereExpression, string strOrderByFileds)
        {
            throw new NotImplementedException();
        }

        public List<TEntity> Query(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true)
        {
            throw new NotImplementedException();
        }

        public List<TEntity> Query(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, int intTop, bool isAsc = true)
        {
            throw new NotImplementedException();
        }

        public List<TEntity> Query(Expression<Func<TEntity, bool>> whereExpression, int intPageIndex, int intPageSize, Expression<Func<TEntity, object>> orderByExpression, ref int intTotalCount, bool isAsc = true)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetTotal(Expression<Func<TEntity, bool>> whereExpression)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> FindWhereAsync(Expression<Func<TEntity, bool>> whereExpression)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> QueryAsync(TKey id, bool blnUseCache = false)
        {
            throw new NotImplementedException();
        }

        public Task<List<TEntity>> QueryAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> whereExpression)
        {
            throw new NotImplementedException();
        }

        public Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> whereExpression, string strOrderByFileds)
        {
            throw new NotImplementedException();
        }

        public Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true)
        {
            throw new NotImplementedException();
        }

        public Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, int intTop, bool isAsc = true)
        {
            throw new NotImplementedException();
        }

        public Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> whereExpression, int intPageIndex, int intPageSize, Expression<Func<TEntity, object>> orderByExpression, ref int intTotalCount, bool isAsc = true)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetTotalAsync(Expression<Func<TEntity, bool>> whereExpression)
        {
            throw new NotImplementedException();
        }

        public DataSet SqlQuery(string sql)
        {
            throw new NotImplementedException();
        }

        public Task<DataSet> SqlQueryAsync(string sql)
        {
            throw new NotImplementedException();
        }
    }
}