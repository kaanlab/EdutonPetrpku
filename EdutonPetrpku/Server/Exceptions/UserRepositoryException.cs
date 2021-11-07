using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdutonPetrpku.Server.Exceptions
{
    public class UserRepositoryException : Exception
    {
        public UserRepositoryException(Exception innerException)
            : base(message: "Service error occurred, contact support.", innerException) { }
    }
}
