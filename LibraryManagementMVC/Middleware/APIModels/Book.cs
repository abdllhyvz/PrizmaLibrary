namespace LibraryManagementService.DBModels
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsAvailable { get; set; } = true;
        public string Author { get; set; }  
    }
}
