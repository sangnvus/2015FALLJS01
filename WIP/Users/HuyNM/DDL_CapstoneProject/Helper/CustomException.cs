using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace DDL_CapstoneProject.Helper
{
    public class DuplicateEmailException : Exception
    {
        public DuplicateEmailException()
        {
        }

        public DuplicateEmailException(string message)
            : base(message)
        {
        }

        public DuplicateEmailException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected DuplicateEmailException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }

    public class DuplicateUserNameException : Exception
    {
        public DuplicateUserNameException()
        {
        }

        public DuplicateUserNameException(string message)
            : base(message)
        {
        }

        public DuplicateUserNameException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected DuplicateUserNameException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }

    public class UserNotFoundException : Exception
    {
        public UserNotFoundException()
        {
        }

        public UserNotFoundException(string message) : base(message)
        {
        }

        public UserNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UserNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}