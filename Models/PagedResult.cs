namespace RestaurantAPI.Models
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; }
        public int TotalPages { get; set; }
        public int ItemForm { get; set; }

        public int ItemsTo { get; set; }
        public int TotalItemsCount { get; set; }

        public PagedResult(List<T> items, int totalCount, int pageSize, int pageNumber)
        {
            Items = items;
            TotalItemsCount = totalCount;
            ItemForm = pageSize*(pageNumber-1)+1;
            ItemsTo = ItemForm + pageSize-1;
            TotalItemsCount = (int)Math.Ceiling(totalCount / (double)pageSize); 
        }
    }
}
