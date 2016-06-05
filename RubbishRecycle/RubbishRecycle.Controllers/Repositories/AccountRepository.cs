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
        #region Fields
        #endregion

        #region Constructors

        public AccountRepository(RubbishRecycleContext dbContext)
            : base(dbContext)
        {

        }

        #endregion

        #region IAccountRepository<TKey>接口

        public Account AddAccount(Account info)
        {
            if (info.RoleId == Account.Saler)
            {
                base.DbContext.Salers.Add((Saler)info);
            }
            else
            {
                base.DbContext.Buyers.Add((Buyer)info);
            }
            if (base.DbContext.SaveChanges() != 0)
            {
                return info;
            }
            return null;
        }

        private Boolean FindAccountByName(Account account, String name)
        {
            return (account.Name == name) || (account.BindingPhone == name);
        }

        public Account GetAccount(String name)
        {
            Saler saler = base.DbContext.Salers.SingleOrDefault(x => FindAccountByName(x, name));
            if (saler == null)
            {
                return base.DbContext.Buyers.SingleOrDefault(x => FindAccountByName(x, name));
            }
            return null;
        }

        public Boolean FreezeAccount(String name)
        {
            Account account = GetAccount(name);
            if (account != null)
            {
                account.IsFreezed = true;
                if(account.RoleId == Account.Saler)
                {
                    base.DbContext.Salers.Attach((Saler)account);
                }
                else
                {
                    base.DbContext.Buyers.Attach((Buyer)account);
                }
                base.DbContext.Entry(account).State = EntityState.Modified;
                return base.DbContext.SaveChanges() != 0;
            }
            return false;
        }

        public Boolean UnfreezeAccount(String name)
        {
            Account account = GetAccount(name);
            if (account != null)
            {
                account.IsFreezed = false;
                if (account.RoleId == Account.Saler)
                {
                    base.DbContext.Salers.Attach((Saler)account);
                }
                else
                {
                    base.DbContext.Buyers.Attach((Buyer)account);
                }
                base.DbContext.Entry(account).State = EntityState.Modified;
                return base.DbContext.SaveChanges() != 0;
            }
            return false;
        }

        public IQueryable<Account> GetAllAccounts()
        {
            throw new NotImplementedException();
        }

        public Boolean IsNameUsed(String name)
        {
            return base.DbContext.Salers.Any(x => x.Name == name) 
                || base.DbContext.Buyers.Any(x => x.Name == name);
        }

        public Boolean IsPhoneBinded(String phone)
        {
            return base.DbContext.Salers.Any(x => x.Name == phone)
                || base.DbContext.Buyers.Any(x => x.Name == phone);
        }

        public Account VerifyAccount(String name, String password)
        {
            Account account = GetAccount(name);
            if (account != null)
            {
                String md5Password = CryptoHelper.MD5Compute(password);
                if (account.Password == md5Password)
                {
                    return account;
                }
            }
            return null;
        }

        public Boolean ChangePassword(String name, String newPassword)
        {
            Account account = GetAccount(name);
            if (account != null)
            {
                String md5Password = CryptoHelper.MD5Compute(newPassword);
                if (account.Password != md5Password)
                {
                    account.Password = md5Password;
                    return UpdateAccount(account);
                }
            }
            return false;
        }

        public Boolean IsExisted(String name)
        {
            return GetAccount(name) != null;
        }

        public Boolean UpdateLastLoginTime(String id)
        {
            Account account = GetAccount(id);
            if (account != null)
            {
                account.LastLogin = DateTime.Now;
                return UpdateAccount(account);
            }
            return false;
        }

        public Boolean UpdateAccount(Account account)
        {
            if (account.RoleId == Account.Saler)
            {
                base.DbContext.Salers.Attach((Saler)account);
            }
            else
            {
                base.DbContext.Buyers.Attach((Buyer)account);
            }
            base.DbContext.Entry(account).State = EntityState.Modified;
            return base.DbContext.SaveChanges() != 0;
        }

        #endregion
    }
}
