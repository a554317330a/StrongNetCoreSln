using Strong.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Strong.Extensions.Account
{
    public interface IUser
    {
        string IP { get; }
        string GetToken();
        UserModel UserModel { get; }
    }

 
}
