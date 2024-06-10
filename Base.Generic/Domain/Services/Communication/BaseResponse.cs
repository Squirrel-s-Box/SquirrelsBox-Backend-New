using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Generic.Domain.Services.Communication
{
    public class BaseResponse<T>
    {
        public bool Success { get; protected set; }
        public string Message { get; protected set; }
        public T Resource { get; private set; }
        public BaseResponse(string message)
        {
            Success = false;
            Message = message;
        }
        public BaseResponse(T resource)
        {
            if (resource == null)
            {
                Success = false;
                Resource = resource;
            }
            else
            {
                Success = true;
                Resource = resource;
            }
        }

        public BaseResponse(ICollection<T> resource)
        {
            if (resource == null)
            {
                Success = false;
            }
            else
            {
                Success = true;
            }
        }

        public BaseResponse(ICollection<int> resource)
        {
            if (resource == null)
            {
                Success = false;
            }
            else
            {
                Success = true;
            }
        }
    }
}
