namespace FBC.EntityFrameworkCore.APIQueryHelper.Models.Requests
{
    public class ACGetListOrderItem
    {
        public ACGetListOrderDirection Direction { get; set; }
        public string FieldName { get; set; }
        public ACGetListOrderItem()
        {

        }

        public ACGetListOrderItem(ACGetListOrderDirection direction, string fieldName)
        {
            Direction = direction;
            FieldName = fieldName;
        }
    }

}
