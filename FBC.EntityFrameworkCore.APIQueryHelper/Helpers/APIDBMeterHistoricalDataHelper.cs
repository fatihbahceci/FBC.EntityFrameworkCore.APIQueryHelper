using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FBC.EntityFrameworkCore.APIQueryHelper.Models;
using FBC.EntityFrameworkCore.APIQueryHelper.Models.Requests;
using FBC.EntityFrameworkCore.APIQueryHelper.Models.Responses;
using Microsoft.EntityFrameworkCore;

namespace FBC.EntityFrameworkCore.APIQueryHelper.DBModels
{
    //public class APIDBMeterHistoricalDataHelper : APIDBContextHelper<MeterHistoricalData>
    //{
    //    public APIDBMeterHistoricalDataHelper(APIDBContext dibi) : base(dibi)
    //    {
    //    }

    //    protected override IQueryable<MeterHistoricalData> getBaseQuery(VMLoggedUser u)
    //    {
    //        var meterCardQuery = getBaseQueryForMeterCard(u);
    //        var qr = from x in db.MeterHistoricalData.AsNoTracking()
    //                     //join c in q on new { x.MeterId } equals new { MeterId = c.Id } into mc
    //                 where (from p in meterCardQuery select p.Id).Contains(x.MeterId)
    //                 select x;

    //        return qr;
    //    }

    //    public override ACListResponse<MeterHistoricalData> ToList(ACGetListRequest aq, VMLoggedUser u)
    //    {
    //        var r = base.ToList(aq, u);
    //        if (r.Data?.Any() == true)
    //        {
    //            r.Data.ForEach(x =>
    //            {
    //                var items = db.DBMeterData.ToList(new ACGetListRequest()
    //                {
    //                    Filters = new List<ACGetListFilterItem>()
    //                 {
    //                     new ACGetListFilterItem( ACGetListFilterType.Equals, nameof(MeterData.Source),""+ (int)DBMeterDataSource.DBMeterHistoricalData),
    //                     new ACGetListFilterItem( ACGetListFilterType.Equals, nameof(MeterData.MeterId),"" + x.MeterId),
    //                     new ACGetListFilterItem( ACGetListFilterType.Equals, nameof(MeterData.Mdtime),"" + x.Hdtime.ToString( CultureInfo.InvariantCulture))
    //                 }

    //                }, u);
    //                x.MeterData = items.Data;
    //            });
    //        }

    //        return r;
    //    }
    //}
}