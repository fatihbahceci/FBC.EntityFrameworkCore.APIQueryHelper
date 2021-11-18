using FBC.EntityFrameworkCore.APIQueryHelper.Models.Requests;
using System.Collections.Generic;

namespace FBC.EntityFrameworkCore.APIQueryHelper
{
    public class ACGetListRequest
    {
        public List<ACGetListFilterItem> Filters { get; set; }
        public List<ACGetListOrderItem> Orders { get; set; }
        /// <summary>
        /// Skip records
        /// </summary>
        public int Skip { get; set; }
        /// <summary>
        /// Maximum record count per request
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// Use this area if you want detect which request (like request id)
        /// </summary>
        public int Echo { get; set; }

        public ACGetListRequest()
        {
            Filters = new List<ACGetListFilterItem>();
            Orders = new List<ACGetListOrderItem>();
        }
    }

}
