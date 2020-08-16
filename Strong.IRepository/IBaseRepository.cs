using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data;
using System.Threading.Tasks;

namespace YunTuCore.IRepository
{
    public interface IBaseRepository<TEntity,TKey> where TEntity : class, new()
    {
        #region 添加数据
        int Add(TEntity model);
        int Add(List<TEntity> model);
        int Add(TEntity entity, Expression<Func<TEntity, object>> insertColumns = null);
        Task<int> AddAsync(TEntity model);
        Task<int> AddAsync(TEntity entity, Expression<Func<TEntity, object>> insertColumns = null);
        Task<int> AddAsync(List<TEntity> entity);
        #endregion

        #region 删除数据
        bool Delete(TKey id);
        bool Delete(TEntity entity);
        bool Delete(TKey[] ids);
        bool Delete(List<TEntity> entitys);
        bool Delete(Expression<Func<TEntity, bool>> whereExpression);
        Task<bool> DeleteAsync(TKey id);
        Task<bool> DeleteAsync(TEntity entity);
        Task<bool> DeleteAsync(TKey[] ids);
        Task<bool> DeleteAsync(List<TEntity> entitys);
        Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> whereExpression);
        #endregion

        #region 修改数据
        bool Update(TEntity entity);
        int Update(TEntity entity, List<string> fileds);
        bool Update(List<TEntity> entitys);
        int Update(List<TEntity> entitys, List<string> fileds);
        Task<bool> UpdateAsync(TEntity entity);
        Task<int> UpdateAsync(TEntity entity, List<string> fileds);
        Task<bool> UpdateAsync(List<TEntity> entitys);
        Task<int> UpdateAsync(List<TEntity> entitys, List<string> fileds);
        #endregion

        #region 查询

        #region 同步
        /// <summary>
        /// 按条件查询单条
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        TEntity FindWhere(Expression<Func<TEntity, bool>> whereExpression);

        /// <summary>
        /// 按主键查找
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="blnUseCache"></param>
        /// <returns></returns>
        TEntity Query(TKey id, bool blnUseCache = false);
        /// <summary>
        /// 查询所有数据
        /// </summary>
        /// <returns></returns>
        List<TEntity> Query();
        /// <summary>
        /// 按条件查询多条
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        List<TEntity> Query(Expression<Func<TEntity, bool>> whereExpression);


        /// <summary>
        /// 按条件查询多条
        /// </summary>
        /// <param name="whereExpression">查询条件</param>
        /// <param name="strOrderByFileds">排序条件</param>
        /// <returns></returns>
        List<TEntity> Query(Expression<Func<TEntity, bool>> whereExpression, string strOrderByFileds);

        /// <summary>
        /// 按条件查询多条
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <param name="orderByExpression"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        List<TEntity> Query(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true);


        /// <summary>
        /// 按条件查询前N条
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <param name="orderByExpression"></param>
        /// <param name="intTop"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        List<TEntity> Query(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, int intTop, bool isAsc = true);

        /// <summary>
        /// 单表分页查询
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <param name="intPageIndex"></param>
        /// <param name="intPageSize"></param>
        /// <param name="orderByExpression"></param>
        /// <param name="intTotalCount"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
       List<TEntity> Query(
         Expression<Func<TEntity, bool>> whereExpression, int intPageIndex, int intPageSize, Expression<Func<TEntity, object>> orderByExpression, ref int intTotalCount, bool isAsc = true);


        /// <summary>
        /// 按条件返回记录数（异步）
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        Task<int> GetTotal(Expression<Func<TEntity, bool>> whereExpression);

        #endregion

        #region 异步

        /// <summary>
        /// 按条件查询单条（异步）
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        Task<TEntity> FindWhereAsync(Expression<Func<TEntity, bool>> whereExpression);
        /// <summary>
        /// 按照主键查询（异步）
        /// </summary>
        /// <param name="id"></param>
        /// <param name="blnUseCache"></param>
        /// <returns></returns>
        Task<TEntity> QueryAsync(TKey id, bool blnUseCache = false);
        /// <summary>
        /// 查询所有数据（异步）
        /// </summary>
        /// <returns></returns>
        Task<List<TEntity>> QueryAsync();

        /// <summary>
        /// 按条件查询多条（异步）
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> whereExpression);

        /// <summary>
        /// 按条件查询多条（异步）
        /// </summary>
        /// <param name="whereExpression">查询条件</param>
        /// <param name="strOrderByFileds">排序条件</param>
        /// <returns></returns>
        Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> whereExpression, string strOrderByFileds);

        /// <summary>
        /// 按条件查询多条（异步）
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <param name="orderByExpression"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true);

        /// <summary>
        /// 按条件查询前N条（异步）
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <param name="orderByExpression"></param>
        /// <param name="intTop"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, int intTop, bool isAsc = true);

        /// <summary>
        /// 单表分页查询(异步)
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <param name="intPageIndex"></param>
        /// <param name="intPageSize"></param>
        /// <param name="orderByExpression"></param>
        /// <param name="intTotalCount"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        Task<List<TEntity>> QueryAsync(
         Expression<Func<TEntity, bool>> whereExpression, int intPageIndex, int intPageSize, Expression<Func<TEntity, object>> orderByExpression, ref int intTotalCount, bool isAsc = true);

        /// <summary>
        /// 按条件返回记录数（异步）
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        Task<int> GetTotalAsync(Expression<Func<TEntity, bool>> whereExpression);


        #endregion

        #endregion

        #region 历史遗留，仅供参考
        //PageResult<TEntity> QueryPage(Expression<Func<TEntity, bool>> whereExpression, int intPageIndex = 1, int intPageSize = 20, string strOrderByFileds = null);

        //  List<TResult> QueryMuch<T, T2, TResult>(
        //Expression<Func<T, T2, object[]>> joinExpression,
        //Expression<Func<T, T2, TResult>> selectExpression,
        //Expression<Func<T, T2, bool>> whereLambda = null) where T : class, new();

        //  List<TResult> QueryMuch<T, T2, T3, TResult>(
        //      Expression<Func<T, T2, T3, object[]>> joinExpression,
        //      Expression<Func<T, T2, T3, TResult>> selectExpression,
        //      Expression<Func<T, T2, T3, bool>> whereLambda = null) where T : class, new();
        //  List<TResult> QueryMuch<T, T2, T3, TResult>(
        //      Expression<Func<T, T2, T3, object[]>> joinExpression,
        //      Expression<Func<T, T2, T3, TResult>> selectExpression,
        //      string strWhere,
        //      int intPageIndex,
        //      int intPageSize,
        //      string strOrderByFileds,
        //      ref int intTotalCount);
        //int UpdateTran(List<string> list);
        //DataTable dataQuery(string tablename, string strWhere, int intPageIndex, int intPageSize, string strOrderByFileds, ref int total);
        //DataTable dataBUSQuery(string strWhere, int intPageIndex, int intPageSize, string strOrderByFileds, ref int total);
        //DataTable dataBUSQuerys(string strWhere, int intPageIndex, int intPageSize, string strOrderByFileds, ref int total);
        #endregion

        #region SqlQuery

        DataSet SqlQuery(string sql);

        Task<DataSet> SqlQueryAsync(string sql);
        #endregion



    }
}
