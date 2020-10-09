using Strong.Entities.DBModel;
 using Strong.IBussiness;
 using Strong.IRepository;
using Strong.IRepository.Base;
using System;
using System.Collections.Generic;
namespace Strong.Bussiness
{
	public class TasksQzBussiness : BaseBussiness<TasksQz>, ITasksQzBussiness
	{

		//当前类已经继承了增、删、查、改的方法

		//这里面写的代码不会给覆盖,如果要重新生成请删除 TasksQzBussiness.cs

		IBaseRepository<TasksQz> _dal;
 
        public TasksQzBussiness(IBaseRepository<TasksQz> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
 
	}
}