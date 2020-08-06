using Microsoft.EntityFrameworkCore;
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
    public abstract class BaseRepository<TEntity,TKey> : IBaseRepository<TEntity, TKey> where TEntity : class, new()
    {
        private readonly DbSet<TEntity> _dbSet;
        public MyContext _Db { get; } = null;

        public BaseRepository(MyContext Db) 
        {
            _Db = Db ?? throw new ArgumentNullException(nameof(MyContext));
            _dbSet = _Db.Set<TEntity>();
        }


       

        public async Task<TEntity> QueryById(TKey id)
        {

            if (id == null)
            {
                return default(TEntity);
            }
            return await _dbSet.FindAsync(id);

        }

        public TEntity FindWhere(Expression<Func<TEntity, bool>> whereExpression) 
        {
            return _dbSet.FindAsync 
        }

      
      
        /// <summary>
        ///     功能描述:根据ID查询一条数据
        /// </summary>
        /// <param name="objId">id（必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]），如果是联合主键，请使用Where条件</param>
        /// <param name="blnUseCache">是否使用缓存</param>
        /// <returns>数据实体</returns>
        public TEntity QueryById(object objId, bool blnUseCache = false)
        {
            return Db.Queryable<TEntity>().WithCacheIF(blnUseCache).In(objId).Single();
        }

        /// <summary>
        ///     功能描述:根据ID查询数据
        /// </summary>
        /// <param name="lstIds">id列表（必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]），如果是联合主键，请使用Where条件</param>
        /// <returns>数据实体列表</returns>
        public List<TEntity> QueryByIDs(object[] lstIds)
        {
            return Db.Queryable<TEntity>().In(lstIds).ToList();
        }

        /// <summary>
        ///     写入实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        public int Add(TEntity entity)
        {
            
            //这里返回ID的话要保证在同一个会话中
            Db.BeginTran();
            var insert = Db.Insertable<TEntity>(entity).ExecuteReturnIdentity();
            Db.CommitTran();


            Db.Aop.OnError = (sql) =>//执行SQL 错误事件
            {
                Console.WriteLine(sql.Sql+"\r\n");
                Console.WriteLine();
            };
            
           return insert;
        }
        public int Add(string sqlStr)
        {

            //这里返回ID的话要保证在同一个会话中
            Db.BeginTran();
            var insert = Db.Ado.ExecuteCommand(sqlStr);
            Db.CommitTran();


            Db.Aop.OnError = (sql) =>//执行SQL 错误事件
            {
                Console.WriteLine(sql.Sql + "\r\n");
                Console.WriteLine();
            };

            return insert;
        }
        /// <summary>
        /// 添加数据返回受影响的行数，并返回添加后实体的id
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="tableName">返回的表名</param>
        /// <param name="id">返回id </param>
        /// <returns></returns>
        public int Add(string sqlStr,string tableName,ref string id)
        {

            //这里返回ID的话要保证在同一个会话中
            Db.BeginTran();
            var insert = Db.Ado.ExecuteCommand(sqlStr);
            id=Db.Ado.GetString(" select IDENT_CURRENT('"+ tableName + "')");
            Db.CommitTran();


            Db.Aop.OnError = (sql) =>//执行SQL 错误事件
            {
                Console.WriteLine(sql.Sql + "\r\n");
                Console.WriteLine();
            };

            return insert;
        }
        /// <summary>
        ///     更新实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        public bool Update(TEntity entity)
        {
            //这种方式会以主键为条件
            var i =  Task.Run(() => Db.Updateable(entity).ExecuteCommand()).Result;
            return i > 0;
            //这种方式会以主键为条件
            //return Db.Updateable(entity).ExecuteCommandHasChange();
        }

        public bool Update(TEntity entity, string strWhere)
        {
            //return  Task.Run(() => Db.Updateable(entity).Where(strWhere).ExecuteCommand() > 0);
            return Db.Updateable(entity).Where(strWhere).ExecuteCommandHasChange();
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="sql">sql语句更新</param>
        /// <returns></returns>
        public int Update(string sql)
        {
            return Db.Ado.ExecuteCommand(sql); ;
        }
        public bool Update(
            TEntity entity,
            List<string> lstColumns = null,
            List<string> lstIgnoreColumns = null,
            string strWhere = ""
        )
        {
            var up = Db.Updateable(entity);
            if (lstIgnoreColumns != null && lstIgnoreColumns.Count > 0)
                up = up.IgnoreColumns(lstIgnoreColumns.ToArray());
            if (lstColumns != null && lstColumns.Count > 0) up = up.UpdateColumns(lstColumns.ToArray());
            if (!string.IsNullOrEmpty(strWhere)) up = up.Where(strWhere);
            return up.ExecuteCommandHasChange();
        }

        /// <summary>
        ///     根据实体删除一条数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        public bool Delete(TEntity entity)
        {
            return Db.Deleteable(entity).ExecuteCommandHasChange();
        }
        public bool Delete(Expression<Func<TEntity, bool>> whereExpression)
        {
            return Db.Deleteable<TEntity>().Where(whereExpression).ExecuteCommandHasChange();
        }
        /// <summary>
        ///     删除指定ID的数据
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns></returns>
        public bool DeleteById(object id)
        {
            return Db.Deleteable<TEntity>(id).ExecuteCommandHasChange();
        }

        /// <summary>
        ///     删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids">主键ID集合</param>
        /// <returns></returns>
        public bool DeleteByIds(object[] ids)
        {
            return Db.Deleteable<TEntity>().In(ids).ExecuteCommandHasChange();
        }


        /// <summary>
        ///     功能描述:查询所有数据
        /// </summary>
        /// <returns>数据列表</returns>
        public List<TEntity> Query()
        {
            return Db.Queryable<TEntity>().ToList();
        }

        /// <summary>
        ///     功能描述:查询数据列表
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <returns>数据列表</returns>
        public List<TEntity> Query(string strWhere)
        {
            var a = Db.Queryable<TEntity>().WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).ToSql();
            return Db.Queryable<TEntity>().WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).ToList();
        }

        /// <summary>
        ///     功能描述:查询数据列表
        /// </summary>
        /// <param name="whereExpression">whereExpression</param>
        /// <returns>数据列表</returns>
        public List<TEntity> Query(Expression<Func<TEntity, bool>> whereExpression)
        {
            try
            {           
            //Db.BeginTran();
            var res = Db.Queryable<TEntity>().WhereIF(whereExpression != null, whereExpression).With(SqlWith.NoLock).ToList();
            //Db.CommitTran();
            return res;
            }
            catch (Exception e)
            {

                throw;
            }
        }

        /// <summary>
        ///     功能描述:查询一个列表
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public List<TEntity> Query(Expression<Func<TEntity, bool>> whereExpression, string strOrderByFileds)
        {
            return Db.Queryable<TEntity>().WhereIF(whereExpression != null, whereExpression)
                .OrderByIF(strOrderByFileds != null, strOrderByFileds).ToList();
        }

        /// <summary>
        ///     功能描述:查询一个列表
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <param name="orderByExpression"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public List<TEntity> Query(Expression<Func<TEntity, bool>> whereExpression,
            Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true)
        {
            return Db.Queryable<TEntity>()
                .OrderByIF(orderByExpression != null, orderByExpression, isAsc ? OrderByType.Asc : OrderByType.Desc)
                .WhereIF(whereExpression != null, whereExpression).ToList();
        }

        /// <summary>
        ///     功能描述:查询一个列表
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public List<TEntity> Query(string strWhere, string strOrderByFileds)
        {
            return Db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds)
                .WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).ToList();
        }


        /// <summary>
        ///     功能描述:查询前N条数据
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="intTop">前N条</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public List<TEntity> Query(
            Expression<Func<TEntity, bool>> whereExpression,
            int intTop,
            string strOrderByFileds)
        {
            return Db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds)
                .WhereIF(whereExpression != null, whereExpression).Take(intTop).ToList();
        }

        /// <summary>
        ///     功能描述:查询前N条数据
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <param name="intTop">前N条</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public List<TEntity> Query(
            string strWhere,
            int intTop,
            string strOrderByFileds)
        {
            return Db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds)
                .WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).Take(intTop).ToList();
        }


        /// <summary>
        ///     功能描述:分页查询
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="intPageIndex">页码（下标0）</param>
        /// <param name="intPageSize">页大小</param>
        /// <param name="intTotalCount">数据总量</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public List<TEntity> Query(
            Expression<Func<TEntity, bool>> whereExpression,
            int intPageIndex,
            int intPageSize,
            string strOrderByFileds,ref int intTotalCount)
        {
            return Db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds)
                .WhereIF(whereExpression != null, whereExpression).ToPageList(intPageIndex, intPageSize, ref intTotalCount);
        }

        public DataSet Querys(string sql)
        {
            return Db.Ado.GetDataSetAll(sql);
        }

        public DataTable Query(string tablename ,string orderby , string strwhere, List<SugarParameter> sugarParameter, string key = "*")
        {

            string strsql = $"select {key} from {tablename} ";
            if (!string.IsNullOrEmpty(strwhere)) 
            {
                strsql += $"where {strwhere} ";
            }
            if (!string.IsNullOrEmpty(orderby)) 
            {
                strsql += $"order by {orderby} ";
            }
            
            return Db.Ado.GetDataTable(strsql, sugarParameter);
        }


        /// <summary>
        ///     功能描述:分页查询
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <param name="intPageIndex">页码（下标0）</param>
        /// <param name="intPageSize">页大小</param>
        /// <param name="intTotalCount">数据总量</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public List<TEntity> Query(
            string strWhere,
            int intPageIndex,
            int intPageSize,
            string strOrderByFileds,ref int intTotalCount)
        {


            var A = Db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds)
               .WhereIF(!string.IsNullOrEmpty(strWhere), strWhere, new List<SugarParameter>() { }).ToSql();

            return Db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds)
                .WhereIF(!string.IsNullOrEmpty(strWhere), strWhere, new List<SugarParameter>() { }).ToPageList(intPageIndex, intPageSize,ref intTotalCount);
        }


        ///// <summary>
        ///// 分页查询[使用版本，其他分页未测试]
        ///// </summary>
        ///// <param name="whereExpression">条件表达式</param>
        ///// <param name="intPageIndex">页码（下标0）</param>
        ///// <param name="intPageSize">页大小</param>
        ///// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        ///// <returns></returns>
        //public PageResult<List<TEntity>> QueryPage(Expression<Func<TEntity, bool>> whereExpression, int intPageIndex = 1,
        //    int intPageSize = 20, string strOrderByFileds = null)
        //{
        //    var totalCount = 0;
        //    var list = Db.Queryable<TEntity>()
        //        .OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds)
        //        .WhereIF(whereExpression != null, whereExpression)
        //        .ToPageList(intPageIndex, intPageSize, ref totalCount);

        //    var pageCount = Math.Ceiling(totalCount.ObjToDecimal() / intPageSize.ObjToDecimal()).ObjToInt();
        //    return new PageResult<List<TEntity>>
        //    {
        //        dataCount = totalCount, pageCount = pageCount, page = intPageIndex, PageSize = intPageSize, data = list
        //    };
        //}


        /// <summary>
        ///     查询-多表查询
        /// </summary>
        /// <typeparam name="T">实体1</typeparam>
        /// <typeparam name="T2">实体2</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="joinExpression">关联表达式 (join1,join2) => new object[] {JoinType.Left,join1.UserNo==join2.UserNo}</param>
        /// <param name="selectExpression">返回表达式 (s1, s2) => new { Id =s1.UserNo, Id1 = s2.UserNo}</param>
        /// <param name="whereLambda">查询表达式 (w1, w2) =>w1.UserNo == "")</param>
        /// <returns>值</returns>
        public List<TResult> QueryMuch<T, T2, TResult>(
            Expression<Func<T, T2, object[]>> joinExpression,
            Expression<Func<T, T2 ,TResult>> selectExpression,
            Expression<Func<T, T2, bool>> whereLambda = null) where T : class, new()
        {

            if (whereLambda == null) return Db.Queryable(joinExpression).Select(selectExpression).ToList();
            return Db.Queryable(joinExpression).Where(whereLambda).Select(selectExpression).ToList();
        }
        /// <summary>
        ///     查询-多表查询
        /// </summary>
        /// <typeparam name="T">实体1</typeparam>
        /// <typeparam name="T2">实体2</typeparam>
        /// <typeparam name="T3">实体3</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="joinExpression">关联表达式 (join1,join2) => new object[] {JoinType.Left,join1.UserNo==join2.UserNo}</param>
        /// <param name="selectExpression">返回表达式 (s1, s2) => new { Id =s1.UserNo, Id1 = s2.UserNo}</param>
        /// <param name="whereLambda">查询表达式 (w1, w2) =>w1.UserNo == "")</param>
        /// <returns>值</returns>
        public List<TResult> QueryMuch<T, T2, T3, TResult>(
            Expression<Func<T, T2, T3, object[]>> joinExpression,
            Expression<Func<T, T2, T3, TResult>> selectExpression,
            Expression<Func<T, T2, T3, bool>> whereLambda = null) where T : class, new()
        {
            if (whereLambda == null) return Db.Queryable(joinExpression).Select(selectExpression).ToList();
            return Db.Queryable(joinExpression).Where(whereLambda).Select(selectExpression).ToList();
        }

        /// <summary>
        ///     查询-多表查询-分页
        /// </summary>
        /// <typeparam name="T">实体1</typeparam>
        /// <typeparam name="T2">实体2</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="joinExpression">关联表达式 (join1,join2) => new object[] {JoinType.Left,join1.UserNo==join2.UserNo}</param>
        /// <param name="selectExpression">返回表达式 (s1, s2) => new { Id =s1.UserNo, Id1 = s2.UserNo}</param>
        /// <param name="strWhere">条件</param>
        /// <param name="intPageIndex">页码（下标0）</param>
        /// <param name="intPageSize">页大小</param>
        /// <param name="intTotalCount">数据总量</param>
        /// <returns>值</returns>
        public List<TResult> QueryMuch<T, T2, T3, TResult>(
            Expression<Func<T, T2, T3, object[]>> joinExpression,
            Expression<Func<T, T2, T3, TResult>> selectExpression,
            string strWhere,
            int intPageIndex,
            int intPageSize,
            string strOrderByFileds,
            ref int intTotalCount)
        {
            var a = Db.Queryable(joinExpression).OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(!string.IsNullOrEmpty(strWhere), strWhere, new List<SugarParameter>() { }).Select<TResult>(selectExpression).ToSql();
            return Db.Queryable(joinExpression).WhereIF(!string.IsNullOrEmpty(strWhere), strWhere, new List<SugarParameter>() { }).Select<TResult>(selectExpression).ToPageList(intPageIndex, intPageSize, ref intTotalCount);
        }


        /// <summary>
        ///     写入实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="insertColumns">指定只插入列</param>
        /// <returns>返回自增量列</returns>
        public int Add(TEntity entity, Expression<Func<TEntity, object>> insertColumns = null)
        {
            var insert = Db.Insertable(entity);
            if (insertColumns == null)
                return insert.ExecuteReturnIdentity();
            return insert.InsertColumns(insertColumns).ExecuteReturnIdentity();
        }

        /// <summary>
        ///     批量插入实体(速度快)
        /// </summary>
        /// <param name="listEntity">实体集合</param>
        /// <returns>影响行数</returns>
        public int Add(List<TEntity> listEntity)
        {
            return Db.Insertable(listEntity.ToArray()).ExecuteCommand();
        }

        public bool Update(string strSql, SugarParameter[] parameters = null)
        {
            //return  Task.Run(() => Db.Ado.ExecuteCommand(strSql, parameters) > 0);
            return Db.Ado.ExecuteCommand(strSql, parameters) > 0;
        }


        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="insertColumns">指定只更新列</param>
        /// <returns>返回影响的记录数</returns>
        public int Update(TEntity entity, Expression<Func<TEntity, object>> updateColumns = null)
        {
            var update = Db.Updateable(entity);
            if (updateColumns == null)
                return update.ExecuteCommand();
            return update.UpdateColumns(updateColumns).ExecuteCommand();
        }


        /// <summary>
        /// 根据条件获取记录数
        /// </summary>
        /// <param name="strwhere"></param>
        /// <returns></returns>
        public int GetTotal(Expression<Func<TEntity, bool>> whereExpression)
        {
 
           var dt =  Db.Queryable<TEntity>().Where(whereExpression);
          
            return dt.Count();
        }


        public int GetTotal(string where) 
        {
            
            var dt = Db.Queryable<TEntity>("t").Where(where).ToList();
            Db.Aop.OnLogExecuting = (sql, pars) =>
            {
                Console.WriteLine(sql + "\r\n" + Db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                Console.WriteLine();
            };
            return dt.Count();
        }
        public int UpdateTran(List<string> list)
        {
            int count = 0;
            try
            {
                
                Db.Ado.BeginTran();
                foreach (var item in list)
                {
                    Db.Ado.ExecuteCommand(item);
                    count++;
                }
                Db.Ado.CommitTran();
            }
            catch (Exception ex)
            {
                Db.Ado.RollbackTran();
                throw ex;
            }
            return count;
        }

        //public DataSet Querys(string sql)
        //{
        //    return Db.Ado.GetDataSetAll(sql);
        //}

        //public DataTable Query(string tablename, string orderby, string strwhere, List<SugarParameter> sugarParameter, string key = "*")
        //{

        //    string strsql = $"select {key} from {tablename} ";
        //    if (!string.IsNullOrEmpty(strwhere))
        //    {
        //        strsql += $"where {strwhere} ";
        //    }
        //    if (!string.IsNullOrEmpty(orderby))
        //    {
        //        strsql += $"order by {orderby} ";
        //    }

        //    return Db.Ado.GetDataTable(strsql, sugarParameter);
        //}
        public DataTable dataQuery(string tablename, string strWhere, int intPageIndex, int intPageSize, string strOrderByFileds, ref int total)
        {
            StringBuilder strSql = new StringBuilder();
            //strSql.Append($" select * from (select rownum r,e. * from {tablename} e ");
            //strSql.Append($" where rownum<= {intPageIndex * intPageSize }  {strWhere} {strOrderByFileds} ) t where r > {intPageSize*(intPageIndex-1)} ");
            //strSql.Append($" select * from {tablename} where 1=1 {strWhere} {strOrderByFileds} ");
            strSql.Append($" {tablename} where 1=1 {strWhere} {strOrderByFileds} ");
            DataTable dt = Db.SqlQueryable<DataTable>(strSql.ToString()).ToDataTablePage(intPageIndex, intPageSize, ref total);
            return dt;
        }
        public DataTable dataBUSQuery(string strWhere, int intPageIndex, int intPageSize, string strOrderByFileds, ref int total)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append($" {strWhere} {strOrderByFileds} ");
            return Db.SqlQueryable<DataTable>(strSql.ToString()).ToDataTablePage(intPageIndex, intPageSize, ref total);
        }
        public DataTable dataBUSQuerys(string strWhere, int intPageIndex, int intPageSize, string strOrderByFileds, ref int total)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append($" {strWhere}  ");
            return Db.SqlQueryable<DataTable>(strSql.ToString()).OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).ToDataTablePage(intPageIndex, intPageSize, ref total);
        }
    }
}