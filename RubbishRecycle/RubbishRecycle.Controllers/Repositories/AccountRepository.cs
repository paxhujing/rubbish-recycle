using RubbishRecycle.Controllers.Assets;
using RubbishRecycle.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RubbishRecycle.Models;
using System.Data.Entity;
using RubbishRecycle.Toolkit;

namespace RubbishRecycle.Controllers.Repositories
{
    internal class AccountRepository : RepositoryBase<RubbishRecycleContext>, IAccountRepository<RubbishRecycleContext>
    {
        #region Constructors

        public AccountRepository(RubbishRecycleContext dbContext)
            : base(dbContext)
        {

        }

        #endregion

        #region IAccountRepository<TKey>接口

        public AppKeyInfo GetAppKeyInfo(String appKey)
        {
            if (String.IsNullOrWhiteSpace(appKey)) return null;
            return base.DbContext.AppKeyInfos.FirstOrDefault(x => x.AppKey == appKey);
        }

        public Account AddAccount(Account info)
        {
            base.DbContext.Accounts.Add(info);
            if (base.DbContext.SaveChanges() != 0)
            {
                return info;
            }
            return null;
        }

        public Account FindAccount(String name)
        {
            if (String.IsNullOrWhiteSpace(name)) return null;
            return base.DbContext.Accounts.FirstOrDefault(x => (x.Name == name) || (x.BindingPhone == name));
        }

        public Boolean FreezeAccount(String name)
        {
            if (String.IsNullOrWhiteSpace(name)) return false;
            Account account = FindAccount(name);
            if (account != null)
            {
                account.IsFreezed = true;
                base.DbContext.Accounts.Attach(account);
                base.DbContext.Entry(account).State = EntityState.Modified;
                return base.DbContext.SaveChanges() != 0;
            }
            return false;
        }

        public Boolean UnfreezeAccount(String name)
        {
            if (String.IsNullOrWhiteSpace(name)) return false;
            Account account = FindAccount(name);
            if (account != null)
            {
                account.IsFreezed = false;
                base.DbContext.Accounts.Attach(account);
                base.DbContext.Entry(account).State = EntityState.Modified;
                return base.DbContext.SaveChanges() != 0;
            }
            return false;
        }

        public IQueryable<Account> GetAllAccounts()
        {
            return base.DbContext.Accounts;
        }

        public Account VerifyAccount(String name, String password)
        {
            if (String.IsNullOrWhiteSpace(name) || String.IsNullOrWhiteSpace(password)) return null;
            String md5Password = CryptoHelper.MD5Compute(password);
            Account account = base.DbContext.Accounts.FirstOrDefault(x => ((x.Name == name) || (x.BindingPhone == name)) && (x.Password == md5Password));
            return account;
        }

        public Boolean IsNameUsed(String name)
        {
            if (String.IsNullOrWhiteSpace(name)) return false;
            return base.DbContext.Accounts.Any(x => x.Name == name);
        }

        public Boolean IsPhoneBinded(String phone)
        {
            if (String.IsNullOrWhiteSpace(phone)) return false;
            return base.DbContext.Accounts.Any(x => x.Name == phone);
        }

        #endregion
    }
}
