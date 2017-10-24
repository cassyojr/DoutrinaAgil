using System;
using DoutrinaAgil.Data.Datacontext;
using System.Data.Entity;
using System.Linq;

namespace DoutrinaAgil.Data.Repository
{
    public class UserRepository
    {
        public virtual bool Add(User user)
        {
            if (user == null)
                return false;

            if (GetEmailExists(user.Email))
                return false;

            using (var context = BaseRepository.GetDbContext())
            {
                user.Active = true;
                context.Entry(user).State = EntityState.Added;
                context.SaveChanges();

                return true;
            }
        }

        public bool GetEmailExists(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new NullReferenceException("email cannot be empty");

            using (var context = BaseRepository.GetDbContext())
            {
                var user = context.User.SingleOrDefault(x => x.Email.Equals(email));

                return user != null;
            }
        }

        public User Get(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return null;

            using (var context = BaseRepository.GetDbContext())
                return context.User.SingleOrDefault(x => x.Email.Equals(email) && x.Password.Equals(password));
        }
    }
}