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
        /// 添加账号。
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        Account AddAccount(Account info);

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
