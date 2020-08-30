
using SqlSugar;
using Strong.Entities;
using Strong.IRepository;
using Strong.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace Strong.Repository
{
    public abstract class BaseRepository<TEntity,TKey> : IBaseRepository<TEntity,TKey> where TEntity : class, new()
    {
        private DbContext context;
        private SqlSugarClient db;
        private SimpleClient<TEntity> entityDB;

        public DbContext Context
        {
            get { return context; }
            set { context = value; }
        }
        internal SqlSugarClient Db
        {
            get { return db; }
            private set { db = value; }
        }
        internal SimpleClient<TEntity> EntityDB
        {
            get { return entityDB; }
            private set { entityDB = value; }
        }
        public BaseRepository()
        {
            DbContext.Init(BaseDBConfig.ConnectionString);
            context = DbContext.GetDbContext();
            db = context.Db;
            entityDB = context.GetEntityDB<TEntity>(db);
        }

        public int Add(TEntity model)
        {
           return db.Insertable<TEntity>(model).ExecuteReturnIdentity() ;
        }

        public int Add(List<TEntity> model)
        {
            return db.Insertable<TEntity>(model).ExecuteReturnIdentity();
        }

        public int Add(TEntity entity, Expression<Func<TEntity, object>> insertColumns = null)
        {
            return db.Insertable<TEntity>(entity).InsertColumns(insertColumns).ExecuteReturnIdentity();
        }

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

        public bool Delete(TKey id)
        {
            return db.Deleteable<TEntity>(id).ExecuteCommand()>0;
            
        }

        public bool Delete(TEntity entity)
        {
            return db.Deleteable<TEntity>(entity).ExecuteCommand() > 0;
        }

        public bool Delete(TKey[] ids)
        {
            return db.Deleteable<TEntity>().In(ids).ExecuteCommand() > 0;
        }

        public bool Delete(List<TEntity> entitys)
        {
            return db.Deleteable<TEntity>(entitys).ExecuteCommand() > 0;
        }

        public bool Delete(Expression<Func<TEntity, bool>> whereExpression)
        {
            return db.Deleteable<TEntity>().Where(whereExpression).ExecuteCommand() > 0;
        }

        public async Task<bool> DeleteAsync(TKey id)
        {
            return await db.Deleteable<TEntity>(id).ExecuteCommandAsync() > 0; ;
        }

        public async Task<bool> DeleteAsync(TEntity entity)
        {
            return await  db.Deleteable(entity).ExecuteCommandAsync()>0;
            
        }

        public async Task<bool> DeleteAsync(TKey[] ids)
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
            var i = await  db.Deleteable<TEntity>().Where(whereExpression).ExecuteCommandAsync();
            return i > 0;
        }

        public  bool Update(TEntity entity)
        {
            return  db.Updateable(entity).ExecuteCommand()>0;
        }

        public bool Update(TEntity entity, params string[] columns)
        {
            return db.Updateable(entity).UpdateColumns(columns).ExecuteCommand() > 0;
        }

        public bool Update(List<TEntity> entitys)
        {
            return db.Updateable(entitys).ExecuteCommand() > 0;
        }

        public bool Update(List<TEntity> entitys, params string[] columns)
        {
            return db.Updateable(entitys).UpdateColumns(columns).ExecuteCommand() > 0;
        }

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

       
        public TEntity FindWhere(Expression<Func<TEntity, bool>> whereExpression)
        {
            return db.Queryable<TEntity>().WhereIF(whereExpression!=null, whereExpression).First();
        }

        public TEntity Query(TKey id)
        {
          
            return entityDB.GetById(id); 
        }

        public List<TEntity> Query()
        {
            return entityDB.GetList();
        }

        public List<TEntity> Query(Expression<Func<TEntity, bool>> whereExpression)
        {
            return entityDB.GetList(whereExpression);
        }

        public List<TEntity> Query(Expression<Func<TEntity, bool>> whereExpression, string strOrderByFileds)
        {
            return db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(whereExpression != null, whereExpression).ToList();
        }

        public List<TEntity> Query(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true)
        {
            return  db.Queryable<TEntity>().OrderByIF(orderByExpression != null, orderByExpression, isAsc ? OrderByType.Asc : OrderByType.Desc).WhereIF(whereExpression != null, whereExpression).ToList();
        }

        public List<TEntity> Query(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, int intTop, bool isAsc = true)
        {

            return  db.Queryable<TEntity>().OrderByIF(orderByExpression!=null, orderByExpression, isAsc?OrderByType.Asc:OrderByType.Desc).WhereIF(whereExpression != null, whereExpression).Take(intTop).ToList();

        }

        public List<TEntity> Query(
            Expression<Func<TEntity, bool>> whereExpression,
            int intPageIndex, int intPageSize,
            Expression<Func<TEntity, object>> orderByExpression,
            ref int intTotalCount,
            bool isAsc = true)
        {

            return db.Queryable<TEntity>().
                OrderByIF(orderByExpression != null, orderByExpression, isAsc ? OrderByType.Asc : OrderByType.Desc).
                WhereIF(whereExpression != null, whereExpression).
                ToPageList(intPageIndex, intPageSize, ref intTotalCount);

        }
        public DataTable SqlQuery(string sql)
        {
            return db.Ado.GetDataTable(sql);
        }

        public async Task<TEntity> FindWhereAsync(Expression<Func<TEntity, bool>> whereExpression)
        {
            return await db.Queryable<TEntity>().Where(whereExpression).FirstAsync();
        }

        public async Task<TEntity> QueryAsync(TKey id)
        {
            return await Task.Run(() => entityDB.GetById(id));
        }

        public async Task<List<TEntity>> QueryAsync()
        {
            return await db.Queryable<TEntity>().ToListAsync();
        }

        public async Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> whereExpression)
        {
            return await db.Queryable<TEntity>().WhereIF(whereExpression!=null, whereExpression).ToListAsync();
        }

        public async Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> whereExpression, string strOrderByFileds)
        {
            return await db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds),strOrderByFileds).WhereIF(whereExpression != null, whereExpression).ToListAsync();
        }

        public async Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true)
        {
            return await db.Queryable<TEntity>().OrderByIF(null!=orderByExpression, orderByExpression, isAsc?OrderByType.Asc:OrderByType.Desc).WhereIF(whereExpression != null, whereExpression).ToListAsync();

        }

        public async Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, int intTop, bool isAsc = true)
        {
            return await db.Queryable<TEntity>().OrderByIF(null != orderByExpression, orderByExpression, isAsc ? OrderByType.Asc : OrderByType.Desc).WhereIF(whereExpression != null, whereExpression)
                .Take(intTop).ToListAsync();

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

        public async Task<int> GetTotalAsync(Expression<Func<TEntity, bool>> whereExpression)
        {
            return await Task.Run(()=> entityDB.Count(whereExpression)) ;
        }

      

        public async Task<DataTable> SqlQueryAsync(string sql)
        {
            return await db.Ado.GetDataTableAsync(sql);
        }

        #endregion
    }
}