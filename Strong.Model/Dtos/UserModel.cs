using System;
using System.Collections.Generic;
using System.Text;

namespace Strong.Model
{
   public class UserModel
    {

        /// <summary>
        /// Desc:登陆名
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string LOGINNAME { get; set; }


        /// <summary>
        /// Desc:真实名称
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string REALNAME { get; set; }


        /// <summary>
        /// Desc:单位
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string UNIT { get; set; }

        /// <summary>
        /// Desc:职务
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string DUTY { get; set; }

        /// <summary>
        /// Desc:是否超级管理员
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string ISSYSADMIN { get; set; }


        /// <summary>
        /// Desc:机构编码
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? UNITID { get; set; }


        /// <summary>
        /// 角色名称
        /// </summary>
        public string roleName { get; set; }

        //存放用户菜单的字典
        public Dictionary<string, string> menudic { get; set; }
    }
}
