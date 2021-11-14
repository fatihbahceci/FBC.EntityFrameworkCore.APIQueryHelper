using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// AC: API Class
/// </summary>
namespace FBC.EntityFrameworkCore.APIQueryHelper
{

    public class ACListResponse<T> : APIResponse<int, List<T>>
    {
        public ACListResponse()
        {
            this.ResponseStatus = 0;// EAPIResponseStatus.Data;
        }
        public int Echo { get; set; }

        public int Offset { get; set; }
        public int TotalFilteredCount { get; set; }
    }
}
