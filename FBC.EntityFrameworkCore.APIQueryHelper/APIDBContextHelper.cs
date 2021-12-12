using FBC.EntityFrameworkCore.APIQueryHelper.Models;
using FBC.EntityFrameworkCore.APIQueryHelper.Models.Requests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FBC.EntityFrameworkCore.APIQueryHelper
{
    public abstract class APIDBContextHelper<TDataTable, TDatabase> where TDatabase : DbContext, new()
    {
        protected TDatabase db;

        public APIDBContextHelper(TDatabase dibi)
        {
            this.db = dibi;
        }

        public TDatabase getContext()
        {
            return db;
        }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="u"></param>
        ///// <returns>WARNING! it can be null</returns>
        //protected IQueryable<MeterCard> getBaseQueryForMeterCard(VMLoggedUser u)
        //{
        //    //Console.WriteLine(new String('.',500));
        //    var meterCardQuery = from x in db.MeterCard.AsNoTracking() where x.Deleted == false select x;

        //    if (u != null)
        //    {
        //        if (!u.HasAllMeterGroupRights)
        //        {
        //            var rights = new List<string>();
        //            if (u.UserMeterGroupRights != null && u.UserMeterGroupRights.Any())
        //            {
        //                rights.AddRange(u.UserMeterGroupRights);
        //            }

        //            if (u.HasNonGroupedMeterRights)
        //            {
        //                rights.Add(null);
        //                rights.Add("");
        //            }
        //            if (rights.Any())
        //            {
        //                meterCardQuery = meterCardQuery.Where(x => rights.Contains(x.MeterGroupName));
        //            }
        //            else
        //            {
        //                //do not return any meter
        //                //q = q.Where(x => false);
        //                //don't query if user has no rights, just return null
        //                return null;
        //            }
        //        }
        //    }
        //    return meterCardQuery;
        //}

        /// <summary>
        /// Other queries call this method first.If it returns null then the calling query also returns null.
        /// </summary>
        /// <param name="extraParams">For example: UserRights</param>
        /// <param name="noTracking">
        /// true: give a query read only (for performance and keep data read only)<br />
        /// false: give an edit-open query for updating data
        /// </param>
        /// <returns></returns>
        protected abstract IQueryable<TDataTable> getBaseQuery(bool noTracking, object extraParams = null);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aq"></param>
        /// <param name="filteredCount"></param>
        /// <param name="extraParams"></param>
        /// <returns></returns>
        private IQueryable<TDataTable> getBaseToListQuery(ACGetListRequest aq, ref int filteredCount, object extraParams = null)
        {
            var q = getBaseQuery(true, extraParams);
            if (q == null)
            {
                return null;
            }
            else
            {
                foreach (var filter in aq.Filters)
                {
                    q = this.Filter(q, filter);
                }

                foreach (var order in aq.Orders)
                {
                    q = this.Order(q, order);
                }
            }
            filteredCount = q.Count();
            if (aq.Skip > 0)
            {
                q = q.Skip(aq.Skip);
            }
            if (aq.Count > 0)
            {
                q = q.Take(aq.Count);
            }
            //Console.WriteLine(new String('*', 100));
            //Console.WriteLine(q.ToSql());
            return q;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="q"></param>
        /// <param name="aq"></param>
        /// <returns></returns>
        private IQueryable<TDataTable> applyFilter(IQueryable<TDataTable> q, ACGetListRequest aq)
        {
            if (q == null)
            {
                return null;
            }
            else
            {
                foreach (var filter in aq.Filters)
                {
                    q = this.Filter(q, filter);
                }

                foreach (var order in aq.Orders)
                {
                    q = this.Order(q, order);
                }
            }
            //filteredCount = q.Count();
            if (aq.Skip > 0)
            {
                q = q.Skip(aq.Skip);
            }
            if (aq.Count > 0)
            {
                q = q.Take(aq.Count);
            }
            //Console.WriteLine(new String('*', 100));
            //Console.WriteLine(q.ToSql());
            return q;
        }



        private async Task<Tuple<int, IQueryable<TDataTable>>> getBaseToListQueryAsync(ACGetListRequest aq, object extraParams = null)
        {

            var q = getBaseQuery(true, extraParams);
            if (q == null)
            {
                return null;
            }
            else
            {
                foreach (var filter in aq.Filters)
                {
                    q = this.Filter(q, filter);
                }

                foreach (var order in aq.Orders)
                {
                    q = this.Order(q, order);
                }
            }
            int filteredCount = await q.CountAsync();
            if (aq.Skip > 0)
            {
                q = q.Skip(aq.Skip);
            }
            if (aq.Count > 0)
            {
                q = q.Take(aq.Count);
            }
            //Console.WriteLine(new String('*', 100));
            //Console.WriteLine(q.ToSql());
            return new Tuple<int, IQueryable<TDataTable>>(filteredCount, q);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="aq"></param>
        /// <param name="noTracking"></param>
        /// <param name="extraParams"></param>
        /// <returns></returns>
        public TDataTable FirstOrDefault(ACGetListRequest aq, bool noTracking = true, object extraParams = null)
        {
            var q = getBaseQuery(noTracking, extraParams);
            q = applyFilter(q, aq);
            return q.FirstOrDefault();
        }

        //public static IQueryable<TSource> Where<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="noTracking"></param>
        /// <param name="extraParams"></param>
        /// <returns></returns>
        public TDataTable FirstOrDefault(Expression<Func<TDataTable, bool>> predicate, bool noTracking = true, object extraParams = null)
        {
            var q = getBaseQuery(noTracking, extraParams);
            q = q.Where(predicate);
            return q.FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aq"></param>
        /// <param name="noTracking"></param>
        /// <param name="extraParams"></param>
        /// <returns></returns>
        public async Task<TDataTable> FirstOrDefaultAsync(ACGetListRequest aq, bool noTracking = true, object extraParams = null)
        {
            var q = getBaseQuery(noTracking, extraParams);
            q = applyFilter(q, aq);
            return await q.FirstOrDefaultAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="noTracking"></param>
        /// <param name="extraParams"></param>
        /// <returns></returns>
        public async Task<TDataTable> FirstOrDefaultAsync(Expression<Func<TDataTable, bool>> predicate, bool noTracking = true, object extraParams = null)
        {
            var q = getBaseQuery(noTracking, extraParams);
            q = q.Where(predicate);
            return await q.FirstOrDefaultAsync();
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="aq"></param>
        /// <param name="extraParams"></param>
        /// <returns></returns>
        public virtual ACListResponse<TDataTable> ToList(ACGetListRequest aq, object extraParams = null)
        {
            int filteredCount = 0;
            var q = getBaseToListQuery(aq, ref filteredCount, extraParams);
            if (q == null)
            {
                return new ACListResponse<TDataTable>()
                {
                    Echo = aq.Echo,
                    SkippedCount = aq.Skip,
                    Count = 0,
                    TotalFilteredCount = 0,
                    Data = new List<TDataTable>()
                };
            }
            else
            {
                var data = q.ToList();
                return new ACListResponse<TDataTable>()
                {
                    Echo = aq.Echo,
                    SkippedCount = aq.Skip,
                    Count = data.Count,
                    TotalFilteredCount = filteredCount,
                    Data = data
                };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aq"></param>
        /// <param name="extraParams"></param>
        /// <returns></returns>
        public async Task<ACListResponse<TDataTable>> ToListAsync(ACGetListRequest aq, object extraParams = null)
        {
            var q = await getBaseToListQueryAsync(aq, extraParams);
            if (q == null)
            {
                return new ACListResponse<TDataTable>()
                {
                    Echo = aq.Echo,
                    SkippedCount = aq.Skip,
                    Count = 0,
                    TotalFilteredCount = 0,
                    Data = new List<TDataTable>()
                };
            }
            else
            {
                var data = await q.Item2.ToListAsync();
                return new ACListResponse<TDataTable>()
                {
                    Echo = aq.Echo,
                    SkippedCount = aq.Skip,
                    Count = data.Count,
                    TotalFilteredCount = q.Item1,
                    Data = data
                };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="q"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        private IQueryable<T> Order<T>(IQueryable<T> q, ACGetListOrderItem t)
        {
            return q.OrderBy(t.FieldName + (t.Direction == ACGetListOrderDirection.Descending ? " desc" : ""));
        }

        /*
        ("Int32") -> InvariantCulture.LCID: 127
        ("Int32") -> InvariantCulture.KeyboardLayoutId: 127
        ("String") -> InvariantCulture.DisplayName: Invariant Language (Invariant Country)
        ("String") -> InvariantCulture.NativeName: Invariant Language (Invariant Country)
        ("String") -> InvariantCulture.EnglishName: Invariant Language (Invariant Country)
        ("String") -> InvariantCulture.TwoLetterISOLanguageName: iv
        ("String") -> InvariantCulture.ThreeLetterISOLanguageName: ivl
        ("String") -> InvariantCulture.ThreeLetterWindowsLanguageName: IVL
        ("CultureTypes") -> InvariantCulture.CultureTypes: SpecificCultures, InstalledWin32Cultures, FrameworkCultures
        ("Boolean") -> InvariantCulture.UseUserOverride: False
        ("Boolean") -> InvariantCulture.IsReadOnly: True
        ("String") -> InvariantCulture.CompareInfo.Name:
        ("Int32") -> InvariantCulture.CompareInfo.LCID: 127
        ("Int32") -> InvariantCulture.TextInfo.ANSICodePage: 1252
        ("Int32") -> InvariantCulture.TextInfo.OEMCodePage: 437
        ("Int32") -> InvariantCulture.TextInfo.MacCodePage: 10000
        ("Int32") -> InvariantCulture.TextInfo.EBCDICCodePage: 37
        ("Int32") -> InvariantCulture.TextInfo.LCID: 127
        ("String") -> InvariantCulture.TextInfo.CultureName:
        ("Boolean") -> InvariantCulture.TextInfo.IsReadOnly: True
        ("String") -> InvariantCulture.TextInfo.ListSeparator: ,
        ("Boolean") -> InvariantCulture.TextInfo.IsRightToLeft: False
        ("Int32") -> InvariantCulture.NumberFormat.CurrencyDecimalDigits: 2
        ("String") -> InvariantCulture.NumberFormat.CurrencyDecimalSeparator: .
        ("Boolean") -> InvariantCulture.NumberFormat.IsReadOnly: True
        ("Int32[]") -> InvariantCulture.NumberFormat.CurrencyGroupSizes: 3
        ("Int32[]") -> InvariantCulture.NumberFormat.NumberGroupSizes: 3
        ("Int32[]") -> InvariantCulture.NumberFormat.PercentGroupSizes: 3
        ("String") -> InvariantCulture.NumberFormat.CurrencyGroupSeparator: ,
        ("String") -> InvariantCulture.NumberFormat.CurrencySymbol: ☼
        ("String") -> InvariantCulture.NumberFormat.NaNSymbol: NaN
        ("Int32") -> InvariantCulture.NumberFormat.CurrencyNegativePattern: 0
        ("Int32") -> InvariantCulture.NumberFormat.NumberNegativePattern: 1
        ("Int32") -> InvariantCulture.NumberFormat.PercentPositivePattern: 0
        ("Int32") -> InvariantCulture.NumberFormat.PercentNegativePattern: 0
        ("String") -> InvariantCulture.NumberFormat.NegativeInfinitySymbol: -Infinity
        ("String") -> InvariantCulture.NumberFormat.NegativeSign: -
        ("Int32") -> InvariantCulture.NumberFormat.NumberDecimalDigits: 2
        ("String") -> InvariantCulture.NumberFormat.NumberDecimalSeparator: .
        ("String") -> InvariantCulture.NumberFormat.NumberGroupSeparator: ,
        ("Int32") -> InvariantCulture.NumberFormat.CurrencyPositivePattern: 0
        ("String") -> InvariantCulture.NumberFormat.PositiveInfinitySymbol: Infinity
        ("String") -> InvariantCulture.NumberFormat.PositiveSign: +
        ("Int32") -> InvariantCulture.NumberFormat.PercentDecimalDigits: 2
        ("String") -> InvariantCulture.NumberFormat.PercentDecimalSeparator: .
        ("String") -> InvariantCulture.NumberFormat.PercentGroupSeparator: ,
        ("String") -> InvariantCulture.NumberFormat.PercentSymbol: %
        ("String") -> InvariantCulture.NumberFormat.PerMilleSymbol: %
        ("String[]") -> InvariantCulture.NumberFormat.NativeDigits: 0, 1, 2, 3, 4, 5, 6, 7, 8, 9
        ("DigitShapes") -> InvariantCulture.NumberFormat.DigitSubstitution: None
        ("String") -> InvariantCulture.DateTimeFormat.AMDesignator: AM
        ("Calendar") -> InvariantCulture.DateTimeFormat.Calendar: System.Globalization.GregorianCalendar
        ("String") -> InvariantCulture.DateTimeFormat.DateSeparator: /
        ("DayOfWeek") -> InvariantCulture.DateTimeFormat.FirstDayOfWeek: Sunday
        ("CalendarWeekRule") -> InvariantCulture.DateTimeFormat.CalendarWeekRule: FirstDay
        ("String") -> InvariantCulture.DateTimeFormat.FullDateTimePattern: dddd, dd MMMM yyyy HH:mm:ss
        ("String") -> InvariantCulture.DateTimeFormat.LongDatePattern: dddd, dd MMMM yyyy
        ("String") -> InvariantCulture.DateTimeFormat.LongTimePattern: HH:mm:ss
        ("String") -> InvariantCulture.DateTimeFormat.MonthDayPattern: MMMM dd
        ("String") -> InvariantCulture.DateTimeFormat.PMDesignator: PM
        ("String") -> InvariantCulture.DateTimeFormat.RFC1123Pattern: ddd, dd MMM yyyy HH':'mm':'ss 'GMT'
        ("String") -> InvariantCulture.DateTimeFormat.ShortDatePattern: MM/dd/yyyy
        ("String") -> InvariantCulture.DateTimeFormat.ShortTimePattern: HH:mm
        ("String") -> InvariantCulture.DateTimeFormat.SortableDateTimePattern: yyyy'-'MM'-'dd'T'HH':'mm':'ss
        ("String") -> InvariantCulture.DateTimeFormat.TimeSeparator: :
        ("String") -> InvariantCulture.DateTimeFormat.UniversalSortableDateTimePattern: yyyy'-'MM'-'dd HH':'mm':'ss'Z'
        ("String") -> InvariantCulture.DateTimeFormat.YearMonthPattern: yyyy MMMM
        ("String[]") -> InvariantCulture.DateTimeFormat.AbbreviatedDayNames: Sun, Mon, Tue, Wed, Thu, Fri, Sat
        ("String[]") -> InvariantCulture.DateTimeFormat.ShortestDayNames: Su, Mo, Tu, We, Th, Fr, Sa
        ("String[]") -> InvariantCulture.DateTimeFormat.DayNames: Sunday, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday
        ("String[]") -> InvariantCulture.DateTimeFormat.AbbreviatedMonthNames: Jan, Feb, Mar, Apr, May, Jun, Jul, Aug, Sep, Oct, Nov, Dec,
        ("String[]") -> InvariantCulture.DateTimeFormat.MonthNames: January, February, March, April, May, June, July, August, September, October, November, December,
        ("Boolean") -> InvariantCulture.DateTimeFormat.IsReadOnly: True
        ("String") -> InvariantCulture.DateTimeFormat.NativeCalendarName: Gregorian Calendar
        ("String[]") -> InvariantCulture.DateTimeFormat.AbbreviatedMonthGenitiveNames: Jan, Feb, Mar, Apr, May, Jun, Jul, Aug, Sep, Oct, Nov, Dec,
        ("String[]") -> InvariantCulture.DateTimeFormat.MonthGenitiveNames: January, February, March, April, May, June, July, August, September, October, November, December,
        ("DateTime") -> InvariantCulture.Calendar.MinSupportedDateTime: 1/1/0001 12:00:00 AM
        ("DateTime") -> InvariantCulture.Calendar.MaxSupportedDateTime: 12/31/9999 11:59:59 PM
        ("CalendarAlgorithmType") -> InvariantCulture.Calendar.AlgorithmType: SolarCalendar
        ("GregorianCalendarTypes") -> InvariantCulture.Calendar.CalendarType: Localized
        ("Int32[]") -> InvariantCulture.Calendar.Eras: 1
        ("Int32") -> InvariantCulture.Calendar.TwoDigitYearMax: 2049
        ("Boolean") -> InvariantCulture.Calendar.IsReadOnly: True
        ("DateTime") -> InvariantCulture.DateTimeFormat.Calendar.MinSupportedDateTime: 1/1/0001 12:00:00 AM
        ("DateTime") -> InvariantCulture.DateTimeFormat.Calendar.MaxSupportedDateTime: 12/31/9999 11:59:59 PM
        ("CalendarAlgorithmType") -> InvariantCulture.DateTimeFormat.Calendar.AlgorithmType: SolarCalendar
        ("GregorianCalendarTypes") -> InvariantCulture.DateTimeFormat.Calendar.CalendarType: Localized
        ("Int32[]") -> InvariantCulture.DateTimeFormat.Calendar.Eras: 1
        ("Int32") -> InvariantCulture.DateTimeFormat.Calendar.TwoDigitYearMax: 2049
        ("Boolean") -> InvariantCulture.DateTimeFormat.Calendar.IsReadOnly: True

         */
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="q"></param>
        /// <param name="ft"></param>
        /// <returns></returns>
        private IQueryable<T> Filter<T>(IQueryable<T> q, ACGetListFilterItem ft)
        {
            if (string.IsNullOrEmpty(ft.Filter))
            {
                return q.Where(@$"{ft.FieldName} == null || {ft.FieldName} == @0", "");
            }
            string filterString = ft.Filter;
            object filterObject = filterString; // set default raw string
            string fieldName = ft.FieldName;
            if (ft.FilterType == ACGetListFilterType.StringContains ||
                ft.FilterType == ACGetListFilterType.StringEquals ||
                ft.FilterType == ACGetListFilterType.StringStartsWith ||
                ft.FilterType == ACGetListFilterType.StringEndsWith
                )
            {
                filterObject = filterString.ToLower();
                var searchProperty = typeof(T).GetProperty(ft.FieldName);
                if (searchProperty != null)
                {
                    string typeName = searchProperty.PropertyType.Name.ToLowerInvariant();
                    if (!typeName.Contains("string"))
                    {
                        fieldName = $"{fieldName}.ToString()";
                    }
                }
                else //TODO Maybe we should  convert all types to string
                {
                    //fieldName = $"{fieldName}.ToString()";
                }
            }
            else
            {
                var searchProperty = typeof(T).GetProperty(ft.FieldName);
                if (searchProperty != null)
                {
                    /*
                    *   https://stackoverflow.com/a/45591441/4546246
                    *   The difference is that the way you are using it, Dynamic LINQ binds the passed variables by value, which is equivalent of using constant values inside the static query. The equivalent static LINQ would be
                    *
                    *   EntitySet.Where(p => p.Date == new DateTime(2016, 02, 12))
                    *   which will translate the same way as the sample dynamic query.
                    *
                    *   If you wish to let Dynamic LINQ bind the variables as parameters to the resulting query, you can use anonymous type with properties like this:
                    *
                    *   EntitySet.Where($"Date == @0.date", new { date })
                    *   which will be translated the same way as your sample static query.
                    *
                    *   https://stackoverflow.com/a/1389947/4546246
                    *   itmes = items.Where( string.Format( "{0} > @0", searchField ),DateTime.Parse( searchString ) );
                    *
                    *   https://stackoverflow.com/a/48862202/4546246 (Dynamic LINQ date query performance)
                    *   This is not a good idea. EF has no idea how to translate ToFileTime() to SQL query, so it will just query whole table and perform your Where in memory on client. The right way is to use parameters:
                    *   db.EntityName
                    *       .Where($"x => x.StartDate > @0", DateTime.ParseExact("02/19/2018", "MM/dd/yyyy", CultureInfo.InvariantCulture));
                    *   @0 represents first parameter in a list, for which we pass parsed DateTime object.
                    *
                    *   If you are not sure if something is bad for perfomance - enable EF logging and see which SQL queries are generated.
                    */
                    //Where($"x => x.StartDate > @0", DateTime.ParseExact("02/19/2018", "MM/dd/yyyy", CultureInfo.InvariantCulture));
                    string typeName = searchProperty.PropertyType.Name.ToLowerInvariant();
                    if (typeName.Contains("date") || typeName.Contains("time"))
                    {
                        try
                        {
                            filterObject = DateTime.Parse(filterString, CultureInfo.InvariantCulture);
                        }
                        catch /*(Exception exc)*/
                        {
                            //TODO handle exception
                            filterObject = filterString;//.ToLower();
                        }
                    }
                }
            }
            //"".ToLower().ToString().StartsWith("");
            switch (ft.FilterType)
            {
                case ACGetListFilterType.StringEquals:
                    return q.Where(@$"{fieldName}.ToLower() == @0", filterObject);
                case ACGetListFilterType.StringContains:
                    return q.Where(@$"{fieldName}.ToLower().Contains(@0)", filterObject);
                case ACGetListFilterType.StringStartsWith:
                    return q.Where(@$"{fieldName}.ToLower().StartsWith(@0)", filterObject);
                case ACGetListFilterType.StringEndsWith:
                    return q.Where(@$"{fieldName}.ToLower().EndsWith(@0)", filterObject);
                case ACGetListFilterType.LessThan:
                    return q.Where(@$"{fieldName} < @0", filterObject);
                case ACGetListFilterType.GreaterThan:
                    return q.Where(@$"{fieldName} > @0", filterObject);
                case ACGetListFilterType.LessAndEquals:
                    return q.Where(@$"{fieldName} <= @0", filterObject);
                case ACGetListFilterType.GreaterAndEquals:
                    return q.Where(@$"{fieldName} >= @0", filterObject);
                case ACGetListFilterType.Equals:
                default:
                    return q.Where(@$"{fieldName} == @0", filterObject);
            }



        }

        /// <summary>
        /// if user not set, results all meters with conditions.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="u"></param>
        /// <returns></returns>
        public List<TDataTable> ToList(Expression<Func<TDataTable, bool>> e, object extraParams = null)
        {
            var q = getBaseQuery(true, extraParams);
            if (q == null)
            {
                return null;
            }
            else
            {
                if (e != null) q = q.Where(e);
                return q.ToList();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="extraParams"></param>
        /// <returns></returns>
        public async Task<List<TDataTable>> ToListAsync(Expression<Func<TDataTable, bool>> e, object extraParams = null)
        {
            var q = getBaseQuery(true, extraParams);
            if (q == null)
            {
                return null;
            }
            else
            {
                if (e != null) q = q.Where(e);
                return await q.ToListAsync();
            }
        }
    }
}
