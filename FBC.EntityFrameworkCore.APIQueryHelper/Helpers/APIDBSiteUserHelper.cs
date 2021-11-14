using FBC.EntityFrameworkCore.APIQueryHelper.Helpers.Lib;
using FBC.EntityFrameworkCore.APIQueryHelper.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace FBC.EntityFrameworkCore.APIQueryHelper.DBModels
{
    //public class APIDBSiteUserHelper : APIDBContextHelper<SiteUser>
    //{
    //    public APIDBSiteUserHelper(APIDBContext dibi) : base(dibi)
    //    {

    //    }

    //    public SiteUser Login(string userName, string password)
    //    {
    //        password = password.ToMD5();
    //        return (from x in db.SiteUser.AsNoTracking()
    //                where x.UserName == userName &&
    //                x.UserPassword == password
    //                select x).FirstOrDefault();
    //    }

    //    protected override IQueryable<SiteUser> getBaseQuery(VMLoggedUser u)
    //    {
    //        throw new System.NotImplementedException();
    //    }
    //}
}
