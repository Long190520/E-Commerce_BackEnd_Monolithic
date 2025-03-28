namespace E_Commerce_BackEnd.ViewModels
{
    public class ListDTOModel<T>
    {
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public int totalCount { get; set; }
        public int totalNotify { get; set; }
        public double calValue1 { get; set; }
        public double calValue2 { get; set; }
        public double calValue3 { get; set; }
        public List<T> source { get; set; }
    }
    public class AddedParam
    {
        public string key { get; set; }
        public object value { get; set; }
    }
}
