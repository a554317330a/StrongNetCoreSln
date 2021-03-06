﻿using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Strong.IRepository.Base
{
    public interface IBaseRepository<TEntity> where TEntity : class, new()
    {


 

        #region 添加数据
        //int Add(TEntity model);
        //int Add(List<TEntity> model);
        //int Add(TEntity entity, Expression<Func<TEntity, object>> insertColumns = null);
        Task<int> AddAsync(TEntity model);
        Task<int> AddAsync(TEntity entity, Expression<Func<TEntity, object>> insertColumns = null);
        Task<int> AddAsync(List<TEntity> entity);
        #endregion

        #region 删除数据
        //bool Delete(int id);
        //bool Delete(TEntity entity);
        //bool Delete(int[] ids);
        //bool Delete(List<TEntity> entitys);
        //bool Delete(Expression<Func<TEntity, bool>> whereExpression);
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteAsync(TEntity entity);
        Task<bool> DeleteAsync(int[] ids);
        Task<bool> DeleteAsync(List<TEntity> entitys);
        Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> whereExpression);
        #endregion

        #region 修改数据
        //bool Update(TEntity entity);
        //bool Update(TEntity entity, params string[] columns);
        //bool Update(List<TEntity> entitys);
        //bool Update(List<TEntity> entitys, params string[] columns);
        Task<bool> UpdateAsync(TEntity entity);
        Task<bool> UpdateAsync(TEntity entity, params string[] columns);
        Task<bool> UpdateAsync(List<TEntity> entitys);
        Task<bool> UpdateAsync(List<TEntity> entitys, params string[] columns);
        #endregion

        #region 查询

        #region 同步
        ///// <summary>
        ///// 按条件查询单条
        ///// </summary>
        ///// <param name="whereExpression"></param>
        ///// <returns></returns>
        //TEntity FindWhere(Expression<Func<TEntity, bool>> whereExpression);

        ///// <summary>
        ///// 按主键查找
        ///// </summary>
        ///// <param name="objId"></param>
        ///// <param name="blnUseCache"></param>
        ///// <returns></returns>
        //TEntity Query(int id);
        ///// <summary>
        ///// 查询所有数据
        ///// </summary>
        ///// <returns></returns>
        //List<TEntity> Query();
        ///// <summary>
        ///// 按条件查询多条
        ///// </summary>
        ///// <param name="whereExpression"></param>
        ///// <returns></returns>
        //List<TEntity> Query(Expression<Func<TEntity, bool>> whereExpression);



        ///// <summary>
        ///// 按条件查询多条
        ///// </summary>
        ///// <param name="whereExpression">查询条件</param>
        ///// <param name="strOrderByFileds">排序条件</param>
        ///// <returns></returns>
        //List<TEntity> Query(Expression<Func<TEntity, bool>> whereExpression, string strOrderByFileds);

        ///// <summary>
        ///// 按条件查询多条
        ///// </summary>
        ///// <param name="whereExpression"></param>
        ///// <param name="orderByExpression"></param>
        ///// <param name="isAsc"></param>
        ///// <returns></returns>
        //List<TEntity> Query(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true);


        ///// <summary>
        ///// 按条件查询前N条
        ///// </summary>
        ///// <param name="whereExpression"></param>
        ///// <param name="orderByExpression"></param>
        ///// <param name="intTop"></param>
        ///// <param name="isAsc"></param>
        ///// <returns></returns>
        //List<TEntity> Query(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, int intTop, bool isAsc = true);

        ///// <summary>
        ///// 单表分页查询
        ///// </summary>
        ///// <param name="whereExpression"></param>
        ///// <param name="intPageIndex"></param>
        ///// <param name="intPageSize"></param>
        ///// <param name="orderByExpression"></param>
        ///// <param name="intTotalCount"></param>
        ///// <param name="isAsc"></param>
        ///// <returns></returns>
        //List<TEntity> Query(
        //  Expression<Func<TEntity, bool>> whereExpression, int intPageIndex, int intPageSize, Expression<Func<TEntity, object>> orderByExpression, ref int intTotalCount, bool isAsc = true);



        ///// <summary>
        ///// 同步分页条件查询
        ///// </summary>
        ///// <param name="where"></param>
        ///// <param name="intPageIndex"></param>
        ///// <param name="intPageSize"></param>
        ///// <param name="orderby"></param>
        ///// <param name="intTotalCount"></param>
        ///// <returns></returns>
        //List<TEntity> Query(
        //   string where,
        //   int intPageIndex, int intPageSize,
        //string orderby,
        //   ref int intTotalCount);

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
        Task<TEntity> QueryAsync(int id);
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
        /// 功能描述:查询一个列表
        /// </summary>
        /// <param name="strWhere">条件</param>
        Task<List<TEntity>> QueryAsync(string strWhere);

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
         Expression<Func<TEntity, bool>> whereExpression, int intPageIndex, int intPageSize, Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true);


        /// <summary>
        /// 异步分页条件查询
        /// </summary>
        /// <param name="where"></param>
        /// <param name="intPageIndex"></param>
        /// <param name="intPageSize"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
         Task<List<TEntity>> QueryAsync(
           string where,
           int intPageIndex, int intPageSize,
        string orderby);

        /// <summary>
        /// 按条件返回记录数（异步）
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        Task<int> GetTotalAsync(Expression<Func<TEntity, bool>> whereExpression);

        Task<int> GetTotalAsync(string where);


        Task<List<TResult>> QueryMuch<T, T2, T3, TResult>(
          Expression<Func<T, T2, T3, object[]>> joinExpression,
          Expression<Func<T, T2, T3, TResult>> selectExpression,
          Expression<Func<T, T2, T3, bool>> whereLambda = null) where T : class, new();



        #endregion

        #endregion

        #region SqlQuery

        //DataTable SqlQuery(string sql);

        Task<DataTable> SqlQueryAsync(string sql);
        #endregion



    }
}
