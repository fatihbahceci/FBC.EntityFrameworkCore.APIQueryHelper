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
        /// <summary>
        /// Skip from request
        /// </summary>
        public int SkippedCount { get; set; }
        /// <summary>
        /// Result data count
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// Total record count of filtered data; (Not all data, just filtered data)
        /// </summary>
        public int TotalFilteredCount { get; set; }
    }
}
