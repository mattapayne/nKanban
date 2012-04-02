using System;
using nKanban.Persistence;

namespace nKanban.Services.Impl
{
    public abstract class AbstractBaseService
    {
        private readonly IRepository _repository;

        protected AbstractBaseService(IRepository repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }

            _repository = repository;
        }

        protected IRepository Repository
        {
            get
            {
                return _repository;
            }
        }
    }
}
