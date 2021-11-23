namespace SmartTask.Shared.DTO.Filter
{
    public class BaseFilterDTO<T> where T : class, new()
    {
        public T Filter { get; set; } = new T();

        public PaginationDTO Pagination { get; set; } = new PaginationDTO();

        public SortingDTO Sorting { get; set; } = new SortingDTO();
    }
}
