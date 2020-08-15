namespace boticario.Helpers.Enums
{
    public class LogEventEnum
    {
        public enum Events
        {
            Generic = 1000,

            GetItem = 1001,
            ListItems = 1002,
            InsertItem = 1003,
            UpdateItem = 1004,
            DeleteItem = 1005,

            GetItemNotFound = 2001,
            ListItemsNotFound = 2002,
            UpdateItemNotFound = 2004,
            DeleteItemNotFound = 2005,

            GetItemError = 3001,
            ListItemsError = 3002,
            InsertItemError = 3003,
            UpdateItemError = 3004,
            DeleteItemError = 3005
        }
    }
}
