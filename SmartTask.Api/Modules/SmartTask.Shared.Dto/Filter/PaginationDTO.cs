namespace SmartTask.Shared.DTO.Filter
{
    public class PaginationDTO
    {
        public int Current { get; set; } = 1;

        public int PageSize { get; set; } = 25;
    }
}
