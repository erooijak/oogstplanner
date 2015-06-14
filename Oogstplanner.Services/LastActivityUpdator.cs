using System;

using Oogstplanner.Data;

namespace Oogstplanner.Services
{
    public class LastActivityUpdator : ServiceBase, ILastActivityUpdator
    {
        public LastActivityUpdator(IOogstplannerUnitOfWork unitOfWork) 
            : base(unitOfWork)
        { }

        public void UpdateLastActivity(int userId)
        {
            var user = UnitOfWork.Users.GetById(userId);
            user.LastActive = DateTime.Now;

            UnitOfWork.Users.Update(user);
            UnitOfWork.Commit();
        }
    }
}
