using System;

namespace POCServices.Filters
{

    public class POCException : Exception
    {
        public POCException(string message): base(message)
        {

        }
    }
    public class ResponseDTO
    {
        public ResponseDTO()
        {
        }
        public int Code { get; set; }
        public string Message { get; set; }
    }
}