using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBC.EntityFrameworkCore.APIQueryHelper
{
    public class APIResponse<TResponseStatus, TResultData> 
    {
        public TResponseStatus ResponseStatus { get; set; }

        public TResultData Data { get; set; }

        public APIResponse(TResponseStatus responseStatus, TResultData data)
        {
            ResponseStatus = responseStatus;
            Data = data;
        }
        public APIResponse(TResponseStatus responseStatus) : this(responseStatus, default(TResultData))
        {

        }

        public APIResponse() : this(default(TResponseStatus), default(TResultData))
        {

        }


    }

}
