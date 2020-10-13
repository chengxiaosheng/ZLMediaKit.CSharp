using System;
using System.Collections.Generic;
using System.Text;
using ZLMediaKit.CSharp.Models;

namespace ZLMediaKit.CSharp.dtos
{
    public class ShellLoginDto
    {

        internal ShellLoginDto(string username,string password, SockInfo sockInfo)
        {
            this.Username = username;
            this.Password = password;
            this.SockInfo = sockInfo;
        }
        private ShellLoginDto() { }
        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; internal set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; internal set; }
        /// <summary>
        /// 网络套接字
        /// </summary>
        public SockInfo SockInfo { get; internal set; }
    }
}
