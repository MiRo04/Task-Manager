namespace TaskManagerAPI.Models.Common
{
    public class QueryObject
    {
        public string? userId { get; set; } = null;
        public string? username { get; set; } = null;
        public string? email { get; set; } = null;
        public string ? sortBy { get; set; } = null;
        public bool sortDescending { get; set; } = false;
        public int pageNumber { get; set; } = 1;
        public int pageSize { get; set; } = 20;
    }
}
