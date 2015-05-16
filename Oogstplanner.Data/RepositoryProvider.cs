using System;
using System.Collections.Generic;

namespace Oogstplanner.Data
{
    public class RepositoryProvider : IRepositoryProvider
    {
        readonly Dictionary<Type, Func<IOogstplannerContext, object>> repositoryFactories;

        public IOogstplannerContext Db { get; private set;}

        public RepositoryProvider(IOogstplannerContext db, RepositoryFactories repositoryFactories)
        {
            if (db == null)
            {
                throw new ArgumentNullException("db");
            }
            if (repositoryFactories == null)
            {
                throw new ArgumentNullException("repositoryFactories");
            }

            this.repositoryFactories = repositoryFactories.Get();
            Db = db;
        }
            
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// Caching the repository has no notable performance difference.
        /// </remarks>
        /// <returns>An instance of the repository.</returns>
        /// <typeparam name="T">The type of repository.</typeparam>
        public T GetRepository<T>() where T : class
        {
            Func<IOogstplannerContext, object> repositoryFactory;
            if (repositoryFactories.TryGetValue(typeof(T), out repositoryFactory))
            {
                var repository = repositoryFactory.Invoke(Db);
                return (T)repository;
            }
            else
            {
                throw new NotImplementedException("No repository for " + typeof(T).FullName);
            }
        }            
    }
}
