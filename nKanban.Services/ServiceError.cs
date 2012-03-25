using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nKanban.Services
{
    public class ServiceError
    {
        public string PropertyName { get; set; }
        public string ErrorMessage { get; set; }

        public ServiceError(string propertyName, string errorMessage)
        {
            PropertyName = propertyName;
            ErrorMessage = errorMessage;
        }

        public ServiceError(string errorMessage)
            : this(String.Empty, errorMessage)
        {

        }

        public ServiceError()
            : this(String.Empty, String.Empty)
        {

        }
    }
}
