using FBC.EntityFrameworkCore.APIQueryHelper.Models.Requests;
using System.Collections.Generic;

namespace FBC.EntityFrameworkCore.APIQueryHelper
{
    public class ACGetListRequest
    {
        public List<ACGetListFilterItem> Filters { get; set; }
        public List<ACGetListOrderItem> Orders { get; set; }

        public int Offset { get; set; }
        public int Count { get; set; }

        public int Echo { get; set; }

        public ACGetListRequest()
        {
            Filters = new List<ACGetListFilterItem>();
            Orders = new List<ACGetListOrderItem>();
        }
    }

}
