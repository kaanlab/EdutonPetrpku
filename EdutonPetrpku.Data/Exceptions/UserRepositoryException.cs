using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdutonPetrpku.Data.Exceptions
{
    public class UserRepositoryException : Exception
    {
        public UserRepositoryException(Exception innerException)
            : base(message: "Service error occurred, contact support.", innerException) { }
    }
}
