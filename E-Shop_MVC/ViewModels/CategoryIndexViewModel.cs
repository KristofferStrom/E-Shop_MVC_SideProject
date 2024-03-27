namespace E_Shop_MVC.ViewModels
{
    public class CategoryIndexViewModel
    {
        public List<CategoryItem> Categories { get; set; } = new List<CategoryItem>();
        public bool IsDescId { get; set; }
        public bool IsDescTitle { get; set; }
        public double TotalCategoryAmount { get; set; }
        public double CategoryAmountPerPage { get; set; }
        public int TotalNumberOfPages { get; set; }
        public int SelectedPageNumber { get; set; }
        public string SelectedColumn { get; set; }
        public bool IsDesc { get; set; }
        public string q { get; set; }
        public class CategoryItem
        {
            public int Id { get; set; }
            public string Title { get; set; }
        }
    }
}
