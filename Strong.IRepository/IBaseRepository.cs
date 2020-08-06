using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data;
using System.Threading.Tasks;

namespace YunTuCore.IRepository
{
    public interface IBaseRepository<TEntity, TKey> where TEntity : class, new()
    {

        Task<TEntity> QueryById(TKey id);
        TEntity FindWhere(Expression<Func<TEntity, bool>> whereExpression);
       TEntity QueryById(object objId, bool blnUseCache = false);
        List<TEntity> QueryByIDs(object[] lstIds);

        int Add(TEntity model);
        int Add(string sqlStr);
        int Add(string sqlStr, string tableName, ref string id);
        int Add(TEntity entity, Expression<Func<TEntity, object>> insertColumns = null);
        int Add(List<TEntity> model);
        bool DeleteById(object id);

        bool Delete(TEntity model);

        bool DeleteByIds(object[] ids);

        int Update(string sql);
        bool Update(TEntity model);
        int Update(TEntity entity, Expression<Func<TEntity, object>> updateColumns = null);

        bool Delete(Expression<Func<TEntity, bool>> whereExpression);

        bool Update(TEntity entity, string strWhere);

        bool Update(TEntity entity, List<string> lstColumns = null, List<string> lstIgnoreColumns = null, string strWhere = "");

        List<TEntity> Query();
        List<TEntity> Query(string strWhere);
        List<TEntity> Query(Expression<Func<TEntity, bool>> whereExpression);
        List<TEntity> Query(Expression<Func<TEntity, bool>> whereExpression, string strOrderByFileds);
        List<TEntity> Query(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true);
        List<TEntity> Query(string strWhere, string strOrderByFileds);

        List<TEntity> Query(Expression<Func<TEntity, bool>> whereExpression, int intTop, string strOrderByFileds);
        List<TEntity> Query(string strWhere, int intTop, string strOrderByFileds);

        List<TEntity> Query(
            Expression<Func<TEntity, bool>> whereExpression, int intPageIndex, int intPageSize, string strOrderByFileds, ref int intTotalCount);
        List<TEntity> Query(string strWhere, int intPageIndex, int intPageSize, string strOrderByFileds,ref int intTotalCount);


        //PageResult<TEntity> QueryPage(Expression<Func<TEntity, bool>> whereExpression, int intPageIndex = 1, int intPageSize = 20, string strOrderByFileds = null);

        List<TResult> QueryMuch<T, T2, TResult>(
      Expression<Func<T, T2, object[]>> joinExpression,
      Expression<Func<T, T2, TResult>> selectExpression,
      Expression<Func<T, T2, bool>> whereLambda = null) where T : class, new();

        List<TResult> QueryMuch<T, T2, T3, TResult>(
            Expression<Func<T, T2, T3, object[]>> joinExpression,
            Expression<Func<T, T2, T3, TResult>> selectExpression,
            Expression<Func<T, T2, T3, bool>> whereLambda = null) where T : class, new();
        List<TResult> QueryMuch<T, T2, T3, TResult>(
            Expression<Func<T, T2, T3, object[]>> joinExpression,
            Expression<Func<T, T2, T3, TResult>> selectExpression,
            string strWhere,
            int intPageIndex,
            int intPageSize,
            string strOrderByFileds,
            ref int intTotalCount);

        int GetTotal(Expression<Func<TEntity, bool>> whereExpression);
        int GetTotal(string strwhere);


        //DataTable Query(string tablename, string orderby, string strwhere, List<> sugarParameter, string key="*");
        DataSet Querys(string sql);
        int UpdateTran(List<string> list);
        DataTable dataQuery(string tablename, string strWhere, int intPageIndex, int intPageSize, string strOrderByFileds, ref int total);
        DataTable dataBUSQuery(string strWhere, int intPageIndex, int intPageSize, string strOrderByFileds, ref int total);
        DataTable dataBUSQuerys(string strWhere, int intPageIndex, int intPageSize, string strOrderByFileds, ref int total);
    }
}
