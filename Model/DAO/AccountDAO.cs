using Model.Entity;
using Model.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class AccountDAO
    {
        public CodeRumDbContext db = null;
        public AccountDAO()
        {
            db = new CodeRumDbContext();
        }
        public IEnumerable<Account> ListAll()
        {
            return db.Accounts;
        }
        public IEnumerable<Account> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Account> model = db.Accounts;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Username.Contains(searchString) || x.Name.Contains(searchString));
            }
            return model.OrderByDescending(x => x.CreateAt).ToPagedList(page, pageSize);
        }
        public IEnumerable<Role> ListAllRole()
        {
            return db.Roles.ToList();
        }
        public Account GetByUsername(string username)
        {
            return db.Accounts.SingleOrDefault(x => x.Username == username);
        }
        public async Task<Account> GetByIdAsync(long id)
        {
            return await db.Accounts.FindAsync(id);
        }
        public Account GetById(long id)
        {
            return db.Accounts.Find(id);
        }
        public async Task<long> InsertAsync(Account account)
        {
            account.CreateAt = DateTime.Now;
            db.Accounts.Add(account);
            await db.SaveChangesAsync();

            return account.Id;
        }
        public async Task<bool> Update(Account entity)
        {
            try
            {
                var user = await GetByIdAsync(entity.Id);
                if (!string.IsNullOrEmpty(entity.Password))
                {
                    user.Password = entity.Password;
                }
                user.Name = entity.Name;
                user.Email = entity.Email;
                user.RoleId = entity.RoleId;
                user.Avatar = entity.Avatar;
                user.ModifyBy = entity.ModifyBy;
                user.ModifyDate = DateTime.Now;
                user.Status = entity.Status;

                await db.SaveChangesAsync();
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }
        public async Task<bool> Delete(long id)
        {
            try
            {
                var user = await GetByIdAsync(id);
                // Delete the user's directory.
                var userDirectory = $@"d:\DZHosts\LocalUser\mrkatsu2212\www.mrkatsu.somee.com\Resourse\data\{user.Username}\";
                if (Directory.Exists(userDirectory))
                {
                    Directory.Delete(userDirectory, true);
                }
                db.Accounts.Remove(user);
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<UserContentViewModel> DetailAsyn(long id)
        {
            var account = await GetByIdAsync(id);
            var content = await db.Contents.Where(c => c.CreateBy == account.Username).ToListAsync();

            return new UserContentViewModel
            {
                Account = account,
                Contents = content
            };
        }
        public async Task<int> CountContentsByCreator(string creatorUsername)
        {
            return await db.Contents.CountAsync(c => c.CreateBy == creatorUsername);
        }

        public void Register()
        {

        }
        public bool ChangeStatus(long id)
        {
            var user = db.Accounts.Find(id);

            user.Status = !user.Status;
            db.SaveChanges();

            return user.Status;
        }
        public void ChangePassword()
        {

        }
        public bool IsUsenameExist(string username)
        {
            var v = db.Accounts.Where(e => e.Username == username).FirstOrDefault();
            return v != null;
        }
        public bool IsEmailExist(string email)
        {
            var v = db.Accounts.Where(e => e.Email == email).FirstOrDefault();
            return v != null;
        }
        /// <summary>
        /// ^: Bắt đầu của chuỗi.
        /// [a-zA-Z]: Ký tự đầu tiên phải là một chữ cái(chữ hoa hoặc chữ thường).
        /// [a-zA-Z0-9_-]: Các ký tự tiếp theo có thể là chữ cái(chữ hoa hoặc chữ thường), số, dấu gạch dưới hoặc dấu gạch ngang.
        /// {2,19}: Độ dài của username phải từ 3 đến 19 ký tự.
        /// $: Kết thúc của chuỗi.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool IsValidUsername(string username)
        {
            // Regex pattern cho username: bắt đầu bằng chữ cái, chỉ chứa chữ cái, số, dấu gạch dưới, dấu gạch ngang, có độ dài từ 3 đến 16 ký tự
            string pattern = @"^[a-zA-Z][a-zA-Z0-9_-]{5,20}$";

            // Kiểm tra sự khớp của username với pattern
            return Regex.IsMatch(username, pattern);
        }
        public int Login(string username, string password)
        {
            var result = GetByUsername(username);
            if (result == null)
            {
                return 0;
            }
            else
            {
                if (result.Status == false)
                {
                    return -1;
                }
                else
                {
                    if (result.Password.Equals(password))
                    {
                        return 1;
                    }

                    else
                    {
                        return -2;
                    }

                }
            }
        }
    }
}
