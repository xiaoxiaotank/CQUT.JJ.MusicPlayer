using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace CQUT.JJ.MusicPlayer.EntityFramework.Exceptions
{
    public class JMBasicException : Exception
    {
        //Http状态码
        public HttpStatusCode HttpStatusCode { get; }

        public JMBasicException(string message, HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError)
            : base(message)
        {
            HttpStatusCode = httpStatusCode;
        }
    }
}
