using Strong.Model;
using Strong.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Strong.Extensions.Account
{
    public interface IUser
    {
        string IP { get; }
        string GetToken();
        UserModel UserModel { get; }
    }

 
}
