using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nKanban.Persistence;

namespace nKanban.Services.Impl
{
    public abstract class AbstractBaseService
    {
        private readonly IRepository _repository;

        public AbstractBaseService(IRepository repository)
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
