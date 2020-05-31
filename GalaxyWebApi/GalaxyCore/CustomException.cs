using System;
using System.Net;
using System.Runtime.Serialization;

namespace GalaxyCore
{
    [Serializable]
    public class CustomException : Exception
    {
        public int StatusCode { get; set; }

        public CustomException()
        {
        }

        public CustomException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            StatusCode = info.GetInt32("StatusCode");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("StatusCode", StatusCode);
        }

        public CustomException(string message, int statusCode = (int) HttpStatusCode.InternalServerError) :
            base(message)
        {
            StatusCode = statusCode;
        }

        public CustomException(Exception exception, int statusCode = (int) HttpStatusCode.InternalServerError) : base(
            exception.Message, exception)
        {
            StatusCode = statusCode;
        }

        public CustomException(string message, Exception innerException,
            int statusCode = (int) HttpStatusCode.InternalServerError)
            : base(message, innerException)
        {
            StatusCode = statusCode;
        }
    }
}