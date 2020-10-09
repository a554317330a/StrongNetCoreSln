
using SqlSugar;
using Strong.IRepository.Base;
using Strong.IRepository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace Strong.Repository.Base
{
    public  class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, new()
    {
        public ISqlSugarClient db;
        
        private readonly IUnitOfWork _unitOfWork;

        private SqlSugarClient _dbBase;

       

        // 构造函数，通过 unitofwork，来控制sqlsugar 实例
        public BaseRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            db = unitOfWork.GetDbClient();
        }

        //public int Add(TEntity model)
        //{
        //    return db.Insertable<TEntity>(model).ExecuteReturnIdentity();
        //}

        //public int Add(List<TEntity> model)
        //{
        //    return db.Insertable<TEntity>(model).ExecuteReturnIdentity();
        //}

        //public int Add(TEntity entity, Expression<Func<TEntity, object>> insertColumns = null)
        //{
        //    return db.Insertable<TEntity>(entity).InsertColumns(insertColumns).ExecuteReturnIdentity();
        //}

        public async Task<int> AddAsync(TEntity model)
        {
            var i = await db.Insertable(model).ExecuteReturnIdentityAsync();
            return i;
        }

        public async Task<int> AddAsync(TEntity entity, Expression<Func<TEntity, object>> insertColumns = null)
        {
            var i = await db.Insertable<TEntity>(entity).InsertColumns(insertColumns).ExecuteReturnIdentityAsync();
            return i;
        }

        public async Task<int> AddAsync(List<TEntity> entity)
        {
            var i = await db.Insertable(entity).ExecuteReturnIdentityAsync();
            return i;
        }

        //public bool Delete(int id)
        //{
        //    return db.Deleteable<TEntity>(id).ExecuteCommand() > 0;

        //}

        //public bool Delete(TEntity entity)
        //{
        //    return db.Deleteable<TEntity>(entity).ExecuteCommand() > 0;
        //}

        //public bool Delete(int[] ids)
        //{
        //    return db.Deleteable<TEntity>().In(ids).ExecuteCommand() > 0;
        //}

        //public bool Delete(List<TEntity> entitys)
        //{
        //    return db.Deleteable<TEntity>(entitys).ExecuteCommand() > 0;
        //}

        //public bool Delete(Expression<Func<TEntity, bool>> whereExpression)
        //{
        //    return db.Deleteable<TEntity>().Where(whereExpression).ExecuteCommand() > 0;
        //}

        public async Task<bool> DeleteAsync(int id)
        {
            return await db.Deleteable<TEntity>(id).ExecuteCommandAsync() > 0; ;
        }

        public async Task<bool> DeleteAsync(TEntity entity)
        {
            return await db.Deleteable(entity).ExecuteCommandAsync() > 0;

        }

        public async Task<bool> DeleteAsync(int[] ids)
        {
            var i = await db.Deleteable<TEntity>().In(ids).ExecuteCommandAsync();
            return i > 0;
        }

        public async Task<bool> DeleteAsync(List<TEntity> entitys)
        {
            var i = await db.Deleteable<TEntity>(entitys).ExecuteCommandAsync();
            return i > 0;
        }

        public async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> whereExpression)
        {
            var i = await db.Deleteable<TEntity>().Where(whereExpression).ExecuteCommandAsync();
            return i > 0;
        }

        //public bool Update(TEntity entity)
        //{
        //    return db.Updateable(entity).ExecuteCommand() > 0;
        //}

        //public bool Update(TEntity entity, params string[] columns)
        //{
        //    return db.Updateable(entity).UpdateColumns(columns).ExecuteCommand() > 0;
        //}

        //public bool Update(List<TEntity> entitys)
        //{
        //    return db.Updateable(entitys).ExecuteCommand() > 0;
        //}

        //public bool Update(List<TEntity> entitys, params string[] columns)
        //{
        //    return db.Updateable(entitys).UpdateColumns(columns).ExecuteCommand() > 0;
        //}

        public async Task<bool> UpdateAsync(TEntity entity)
        {
            return await db.Updateable(entity).ExecuteCommandAsync() > 0;
        }

        public async Task<bool> UpdateAsync(TEntity entity, params string[] columns)
        {
            return await db.Updateable(entity).UpdateColumns(columns).ExecuteCommandAsync() > 0;
        }

        public async Task<bool> UpdateAsync(List<TEntity> entitys)
        {
            return await db.Updateable(entitys).ExecuteCommandAsync() > 0;
        }

        public async Task<bool> UpdateAsync(List<TEntity> entitys, params string[] columns)
        {
            return await db.Updateable(entitys).UpdateColumns(columns).ExecuteCommandAsync() > 0;

        }
        #region 查询

        #region 同步

       
        //public TEntity FindWhere(Expression<Func<TEntity, bool>> whereExpression)
        //{
        //    return db.Queryable<TEntity>().WhereIF(whereExpression != null, whereExpression).First();
        //}

        //public TEntity Query(int id)
        //{

        //    return db.GetSimpleClient().GetById<TEntity>(id);
        //}

        //public List<TEntity> Query()
        //{
        //    return db.Queryable<TEntity>().ToList();
        //}

        //public List<TEntity> Query(Expression<Func<TEntity, bool>> whereExpression)
        //{
        //    return db.Queryable<TEntity>().Where(whereExpression).ToList();
        //}

        //public List<TEntity> Query(Expression<Func<TEntity, bool>> whereExpression, string strOrderByFileds)
        //{
        //    return db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(whereExpression != null, whereExpression).ToList();
        //}

        //public List<TEntity> Query(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true)
        //{
        //    return db.Queryable<TEntity>().OrderByIF(orderByExpression != null, orderByExpression, isAsc ? OrderByType.Asc : OrderByType.Desc).WhereIF(whereExpression != null, whereExpression).ToList();
        //}

        //public List<TEntity> Query(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, int intTop, bool isAsc = true)
        //{

        //    return db.Queryable<TEntity>().OrderByIF(orderByExpression != null, orderByExpression, isAsc ? OrderByType.Asc : OrderByType.Desc).WhereIF(whereExpression != null, whereExpression).Take(intTop).ToList();

        //}

        //public List<TEntity> Query(
        //    Expression<Func<TEntity, bool>> whereExpression,
        //    int intPageIndex, int intPageSize,
        //    Expression<Func<TEntity, object>> orderByExpression,
        //    ref int intTotalCount,
        //    bool isAsc = true)
        //{

        //    return db.Queryable<TEntity>().
        //        OrderByIF(orderByExpression != null, orderByExpression, isAsc ? OrderByType.Asc : OrderByType.Desc).
        //        WhereIF(whereExpression != null, whereExpression).
        //        ToPageList(intPageIndex, intPageSize, ref intTotalCount);

        //}

        //public List<TEntity> Query(
        //   string where,
        //   int intPageIndex, int intPageSize,
        //string orderby,
        //   ref int intTotalCount )
        //{

        //    return db.Queryable<TEntity>().
        //        OrderByIF(orderby != null, orderby).
        //        WhereIF(where != null, where).
        //        ToPageList(intPageIndex, intPageSize, ref intTotalCount);

        //}


        //public DataTable SqlQuery(string sql)
        //{
        //    return db.Ado.GetDataTable(sql);
        //}
        #endregion

        #region 异步


        public async Task<TEntity> FindWhereAsync(Expression<Func<TEntity, bool>> whereExpression)
        {
            return await db.Queryable<TEntity>().Where(whereExpression).FirstAsync();
        }

        public async Task<TEntity> QueryAsync(int id)
        {
            return await Task.Run(() => db.GetSimpleClient().GetById<TEntity>(id));
        }

        public async Task<List<TEntity>> QueryAsync()
        {
            return await db.Queryable<TEntity>().ToListAsync();
        }


        /// <summary>
        /// 功能描述:查询一个列表
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> QueryAsync(string strWhere)
        {
            //return await Task.Run(() => _db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).ToList());
            return await db.Queryable<TEntity>().WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).ToListAsync();
        }


        public async Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> whereExpression)
        {
            return await db.Queryable<TEntity>().WhereIF(whereExpression != null, whereExpression).ToListAsync();
        }

        public async Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> whereExpression, string strOrderByFileds)
        {
            return await db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(whereExpression != null, whereExpression).ToListAsync();
        }

        public async Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true)
        {
            return await db.Queryable<TEntity>().OrderByIF(null != orderByExpression, orderByExpression, isAsc ? OrderByType.Asc : OrderByType.Desc).WhereIF(whereExpression != null, whereExpression).ToListAsync();

        }

        public async Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, int intTop, bool isAsc = true)
        {
            return await db.Queryable<TEntity>().OrderByIF(null != orderByExpression, orderByExpression, isAsc ? OrderByType.Asc : OrderByType.Desc).WhereIF(whereExpression != null, whereExpression)
                .Take(intTop).ToListAsync();

        }
        /// <summary> 
        ///查询-多表查询
        /// </summary> 
        /// <typeparam name="T">实体1</typeparam> 
        /// <typeparam name="T2">实体2</typeparam> 
        /// <typeparam name="T3">实体3</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="joinExpression">关联表达式 (join1,join2) => new object[] {JoinType.Left,join1.UserNo==join2.UserNo}</param> 
        /// <param name="selectExpression">返回表达式 (s1, s2) => new { Id =s1.UserNo, Id1 = s2.UserNo}</param>
        /// <param name="whereLambda">查询表达式 (w1, w2) =>w1.UserNo == "")</param> 
        /// <returns>值</returns>
        public async Task<List<TResult>> QueryMuch<T, T2, T3, TResult>(
            Expression<Func<T, T2, T3, object[]>> joinExpression,
            Expression<Func<T, T2, T3, TResult>> selectExpression,
            Expression<Func<T, T2, T3, bool>> whereLambda = null) where T : class, new()
        {
            if (whereLambda == null)
            {
                return await db.Queryable(joinExpression).Select(selectExpression).ToListAsync();
            }
            return await db.Queryable(joinExpression).Where(whereLambda).Select(selectExpression).ToListAsync();
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
            return await db.Queryable<TEntity>().
                 OrderByIF(orderByExpression != null, orderByExpression, isAsc ? OrderByType.Asc : OrderByType.Desc).
                 WhereIF(whereExpression != null, whereExpression).
                 ToPageListAsync(intPageIndex, intPageSize);
        }


        public  async Task<List<TEntity>> QueryAsync(
           string where,
           int intPageIndex, int intPageSize,
        string orderby )
        {

            return await  db.Queryable<TEntity>().
                OrderByIF(orderby != null, orderby).
                WhereIF(where != null, where).
                ToPageListAsync(intPageIndex, intPageSize);

        }


        public async Task<int> GetTotalAsync(Expression<Func<TEntity, bool>> whereExpression)
        {
            return await Task.Run(() => db.GetSimpleClient().Count(whereExpression));
        }

        public async Task<int> GetTotalAsync(string where)
        { 
            return await Task.Run(() => db.Queryable<TEntity>().WhereIF(string.IsNullOrEmpty(where),where).Count());
        }

        public async Task<DataTable> SqlQueryAsync(string sql)
        {
            return await db.Ado.GetDataTableAsync(sql);
        }


        #endregion

        #endregion
    }
}