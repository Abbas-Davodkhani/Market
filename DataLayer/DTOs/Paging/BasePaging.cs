namespace DataLayer.DTOs.Paging
{
    public class BasePaging
    {
        public BasePaging()
        {
            PageId = 1;
            HomManyPageAfterAndBefore = 3;
            TakeEntity = 10;
        }
        public int PageId { get; set; }
        public int PageCount { get; set; }
        public int AllEntitiesCount { get; set; }
        public int StartPage { get; set; }
        public int EndPage { get; set; }
        public int HomManyPageAfterAndBefore { get; set; }
        public int TakeEntity { get; set; }
        public int SkipEntity { get; set; }
    }
}
