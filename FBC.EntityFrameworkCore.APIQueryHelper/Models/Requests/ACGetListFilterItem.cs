namespace FBC.EntityFrameworkCore.APIQueryHelper.Models.Requests
{
    public class ACGetListFilterItem
    {
        public ACGetListFilterType FilterType { get; set; }
        public string FieldName { get; set; }
        public string Filter { get; set; }

        public ACGetListFilterItem()
        {

        }

        public ACGetListFilterItem(ACGetListFilterType filterType, string fieldName, string filter)
        {
            FilterType = filterType;
            FieldName = fieldName;
            Filter = filter;
        }
    }

}
