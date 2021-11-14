using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBC.EntityFrameworkCore.APIQueryHelper
{
    //public enum EAPIResponseStatus
    //{
    //    UNKNOWN = 0,
    //    LoginRequired = 1,
    //    LoginResponse = 2,
    //    //NotPermission = 3,
    //    //ErrorOccured = 4,
    //    Data = 99,
    //}

    public class APIResponse<T>
    {
        public int ResponseCode { get; set; }

        public T Data { get; set; }

        public APIResponse(int responseCode, T data)
        {
            ResponseCode = responseCode;
            Data = data;
        }
        public APIResponse(int responseCode) : this(responseCode, default(T))
        {

        }

        public APIResponse() : this(-1, default(T))
        {

        }


    }

}
