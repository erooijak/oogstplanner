using System;

using Oogstplanner.Data;

namespace Oogstplanner.Services
{
    public abstract class ServiceBase
    {
        protected IOogstplannerUnitOfWork UnitOfWork { get; set; }

        protected ServiceBase(IOogstplannerUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
            {
                throw new ArgumentNullException("unitOfWork");
            }

            UnitOfWork = unitOfWork;
        }

    }
}
