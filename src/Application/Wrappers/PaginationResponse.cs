namespace Application.Wrappers
{
    public class PaginationResponse<T> where T : class
    {
        public PaginationResponse(int pageindex,int pageSize,int count,IEnumerable<T> result)
        {
            PageIndex = pageindex;
            PageSize = pageSize;
            Count = count;
            Result = result;
        }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
        public IEnumerable<T> Result { get; set; }
    }
}
