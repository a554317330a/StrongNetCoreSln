using Strong.Entities;
using Strong.Entities.DBModel;
using Strong.IBussiness;
using Strong.IRepository;
using System;


namespace Strong.Bussiness
{
    public class TBApilogBussiness : BaseBussiness<TbApilog,int>, ITBApilogBussiness
    {
        ITBApilogRepository _dal;
        public TBApilogBussiness(ITBApilogRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }



        public int retrunExp()
        {
            int a = 1;
            int b = 0;
            int c = a / b;
            return c;
        }
    }
}
