namespace E_Commerce_BackEnd.ViewModels
{
    public class WebSocketResponse<T>
    {
        public int EntityId { get; set; }
        public string EntityName { get; set; }
        public int TypeId { get; set; }
        //public string TanentCode { get; set; }
        public T Data { get; set; }
        public List<T> DataList { get; set; }
    }

    public enum TypeEntityAction
    {
        UpdateEntity = 1,
        UpdateCountBadge = 2,
        Message = 3,
        MessageCountBadge = 4,
    }
}
