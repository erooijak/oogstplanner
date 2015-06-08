using System;

namespace Oogstplanner.Data
{
    public class OogstplannerUnitOfWork : IOogstplannerUnitOfWork
    {
        readonly IRepositoryProvider repositoryProvider;

        public ICalendarRepository Calendars 
        { 
            get 
            { 
                return repositoryProvider.GetRepository<CalendarRepository>(); 
            } 
        }

        public ICropRepository Crops 
        { 
            get 
            { 
                return repositoryProvider.GetRepository<CropRepository>(); 
            } 
        }

        public IPasswordRecoveryRepository PasswordRecovery 
        { 
            get 
            { 
                return repositoryProvider.GetRepository<PasswordRecoveryRepository>(); 
            } 
        }

        public IFarmingActionRepository FarmingActions 
        { 
            get 
            { 
                return repositoryProvider.GetRepository<FarmingActionRepository>(); 
            } 
        }

        public IUserRepository Users 
        { 
            get 
            { 
                return repositoryProvider.GetRepository<UserRepository>(); 
            } 
        }

        public ILikesRepository Likes 
        { 
            get 
            { 
                return repositoryProvider.GetRepository<LikesRepository>(); 
            } 
        }

        public OogstplannerUnitOfWork(IRepositoryProvider repositoryProvider)
        {
            if (repositoryProvider == null)    
            {
                throw new ArgumentNullException("repositoryProvider");
            }

            this.repositoryProvider = repositoryProvider;
        }

        public void Commit()
        {
            repositoryProvider.Db.SaveChanges();
        }
    }
}
