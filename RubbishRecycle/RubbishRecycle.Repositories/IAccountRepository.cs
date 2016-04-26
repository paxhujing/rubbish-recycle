﻿using RubbishRecycle.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Repositories
{
    public interface IAccountRepository<TKey> : IRepository<TKey>
        where TKey : DbContext
    {
        /// <summary>
        /// 获取AppKey的信息。
        /// </summary>
        /// <param name="appKey">应用程序的AppKey字符串。</param>
        /// <returns></returns>
        AppKeyInfo GetAppKeyInfo(String appKey);

        /// <summary>
        /// 添加账号。
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        Account AddAccount(Account info);

        /// <summary>
        /// 检查账户名是否已经被使用。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Boolean IsNameUsed(String name);

        /// <summary>
        /// 检查手机是否已经被绑定。
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        Boolean IsPhoneBinded(String phone);

        /// <summary>
        /// 查找账号。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Account FindAccount(String name);

        /// <summary>
        /// 验证账号。
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Account VerifyAccount(String name, String password);

        /// <summary>
        /// 冻结账号。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Boolean FreezeAccount(String name);

        /// <summary>
        /// 解冻账号。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Boolean UnfreezeAccount(String name);

        /// <summary>
        /// 获取所有账号。
        /// </summary>
        /// <returns></returns>
        IQueryable<Account> GetAllAccounts();
    }
}
