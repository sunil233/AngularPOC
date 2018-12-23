using System;
using System.Data.Entity;
using System.Linq;
using POC.Repository.Interface;
using POC.Models;

namespace POC.Repository.Implementation
{
    public class LoginRepository : ILoginRepository
    {
        public Registration ValidateUser(string userName, string passWord)
        {
            try
            {
                using (var _context = new DatabaseContext())
                {
                    var validate = (from user in _context.Registration
                                    where user.Username == userName && user.Password == passWord
                                    select user).SingleOrDefault();

                    return validate;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public bool UpdatePassword(string NewPassword, int UserID)
        {
            using (var _context = new DatabaseContext())
            {
                var user = (from register in _context.Registration.Where(x => x.RegistrationID == UserID)
                            select register).FirstOrDefault();
                if (user != null)
                {
                    user.Password = NewPassword;
                    user.ConfirmPassword = NewPassword;
                    _context.Entry(user).State = EntityState.Modified;
                    _context.SaveChanges();
                    return true; ;
                }
                else
                {
                    return false;
                }
            }
        }

        public string GetPasswordbyUserID(int UserID)
        {
            try
            {
                using (var _context = new DatabaseContext())
                {
                    var password = (from temppassword in _context.Registration
                                    where temppassword.RegistrationID == UserID
                                    select temppassword.Password).FirstOrDefault();

                    return password;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
