using Strong.IBussiness;
using Strong.IRepository.Base;
using Strong.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace Strong.Bussiness
{
    public class BaseBussiness<TEntity> : IBaseBussiness<TEntity> where TEntity : class, new()
    {

        public IBaseRepository<TEntity> BaseDal;//通过在子类的构造函数中注入，这里是基类，不用构造函数

        //public int Add(TEntity model)
        //{
        //    return BaseDal.Add(model);
        //}

        //public int Add(List<TEntity> model)
        //{
        //    return BaseDal.Add(model);
        //}

        //public int Add(TEntity entity, Expression<Func<TEntity, object>> insertColumns = null)
        //{
        //    return BaseDal.Add(entity, insertColumns);
        //}

        public async Task<int> AddAsync(TEntity model)
        {

            return await BaseDal.AddAsync(model);
        }

        public async Task<int> AddAsync(TEntity entity, Expression<Func<TEntity, object>> insertColumns = null)
        {
            return await BaseDal.AddAsync(entity, insertColumns);

        }

        public async Task<int> AddAsync(List<TEntity> entity)
        {
            return await BaseDal.AddAsync(entity);
        }

        //public bool Delete(int id)
        //{
        //    return BaseDal.Delete(id);
        //}

        //public bool Delete(TEntity entity)
        //{
        //    return BaseDal.Delete(entity);
        //}

        //public bool Delete(int[] ids)
        //{
        //    return BaseDal.Delete(ids);
        //}

        //public bool Delete(List<TEntity> entitys)
        //{
        //    return BaseDal.Delete(entitys);
        //}

        //public bool Delete(Expression<Func<TEntity, bool>> whereExpression)
        //{
        //    return BaseDal.Delete(whereExpression);
        //}

        public async Task<bool> DeleteAsync(int id)
        {
            return await BaseDal.DeleteAsync(id);
        }

        public async Task<bool> DeleteAsync(TEntity entity)
        {
            return await BaseDal.DeleteAsync(entity);

        }

        public async Task<bool> DeleteAsync(int[] ids)
        {
            return await BaseDal.DeleteAsync(ids);

        }

        public async Task<bool> DeleteAsync(List<TEntity> entitys)
        {

            return await BaseDal.DeleteAsync(entitys);
        }

        public async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> whereExpression)
        {
            return await BaseDal.DeleteAsync(whereExpression);
        }

        //public bool Update(TEntity entity)
        //{
        //    return BaseDal.Update(entity);
        //}

        //public bool Update(TEntity entity, params string[] columns)
        //{
        //    return BaseDal.Update(entity, columns);
        //}

        //public bool Update(List<TEntity> entitys)
        //{
        //    return BaseDal.Update(entitys);
        //}

        //public bool Update(List<TEntity> entitys, params string[] columns)
        //{
        //    return BaseDal.Update(entitys, columns);
        //}

        public async Task<bool> UpdateAsync(TEntity entity)
        {
            return await BaseDal.UpdateAsync(entity);
        }

        public async Task<bool> UpdateAsync(TEntity entity, params string[] columns)
        {
            return await BaseDal.UpdateAsync(entity, columns);
        }

        public async Task<bool> UpdateAsync(List<TEntity> entitys)
        {
            return await BaseDal.UpdateAsync(entitys);
        }

        public async Task<bool> UpdateAsync(List<TEntity> entitys, params string[] columns)
        {
            return await BaseDal.UpdateAsync(entitys, columns);

        }
        #region 查询


//        public TEntity FindWhere(Expression<Func<TEntity, bool>> whereExpression)
//        {
//            return BaseDal.FindWhere(whereExpression);
//        }

//        public TEntity Query(int id)
//        {

//            return BaseDal.Query(id);
//        }

//        public List<TEntity> Query()
//        {
//            return BaseDal.Query();
//        }

//        public List<TEntity> Query(Expression<Func<TEntity, bool>> whereExpression)
//        {
//            return BaseDal.Query(whereExpression);
//        }

//        public List<TEntity> Query(Expression<Func<TEntity, bool>> whereExpression, string strOrderByFileds)
//        {
//            return BaseDal.Query(whereExpression, strOrderByFileds);
//        }

//        public List<TEntity> Query(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true)
//        {
//            return BaseDal.Query(whereExpression, orderByExpression, isAsc);
//        }

//        public List<TEntity> Query(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, int intTop, bool isAsc = true)
//        {
//            return BaseDal.Query(whereExpression, orderByExpression, intTop, isAsc);
//        }

//        public List<TEntity> Query(
//            Expression<Func<TEntity, bool>> whereExpression,
//            int intPageIndex, int intPageSize,
//            Expression<Func<TEntity, object>> orderByExpression,
//            ref int intTotalCount,
//            bool isAsc = true)
//        {

//            return BaseDal.Query(whereExpression, intPageIndex, intPageSize, orderByExpression, ref intTotalCount, isAsc);

//        }


//        public List<TEntity> Query(
//    string where,
//    int intPageIndex, int intPageSize,
//string orderby,
//    ref int intTotalCount)
//        {

//            return BaseDal.Query(where, intPageIndex, intPageSize, orderby, ref intTotalCount);

//        }





//        public DataTable SqlQuery(string sql)
//        {
//            return BaseDal.SqlQuery(sql);
//        }

        public async Task<TEntity> FindWhereAsync(Expression<Func<TEntity, bool>> whereExpression)
        {
            return await BaseDal.FindWhereAsync(whereExpression);
        }

        public async Task<TEntity> QueryAsync(int id)
        {
            return await BaseDal.QueryAsync(id);
        }

        public async Task<List<TEntity>> QueryAsync()
        {
            return await BaseDal.QueryAsync();
        }

        public async Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> whereExpression)
        {
            return await BaseDal.QueryAsync(whereExpression);
        }

        public async Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> whereExpression, string strOrderByFileds)
        {
            return await BaseDal.QueryAsync(whereExpression, strOrderByFileds);
        }

        public async Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true)
        {
            return await BaseDal.QueryAsync(whereExpression, orderByExpression, isAsc);
        }

        public async Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, int intTop, bool isAsc = true)
        {
            return await BaseDal.QueryAsync(whereExpression, orderByExpression, intTop, isAsc);

        }
        /// <summary>
        /// 分页查询，异
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <param name="intPageIndex"></param>
        /// <param name="intPageSize"></param>
        /// <param name="orderByExpression"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public async Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> whereExpression, int intPageIndex, int intPageSize, Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true)
        {
            return await BaseDal.QueryAsync(whereExpression, intPageIndex, intPageSize, orderByExpression, isAsc);
        }

        public async Task<List<TEntity>> QueryAsync(
           string where,
           int intPageIndex, int intPageSize,
        string orderby)
        {
            return await BaseDal.QueryAsync(where, intPageIndex, intPageSize, orderby);


        }



        public async Task<int> GetTotalAsync(Expression<Func<TEntity, bool>> whereExpression)
        {
            return await BaseDal.GetTotalAsync(whereExpression);
        }


        public async Task<int> GetTotalAsync(string where)
        {
            return await BaseDal.GetTotalAsync(where);
        }



        public async Task<DataTable> SqlQueryAsync(string sql)
        {
            return await BaseDal.SqlQueryAsync(sql);
        }

        #endregion

    }
}
