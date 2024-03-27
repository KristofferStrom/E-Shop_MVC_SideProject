namespace E_Shop_MVC.ViewModels
{
    public class SubCategoryIndexViewModel
    {
        public List<SubCategoryItem> SubCategories { get; set; } = new List<SubCategoryItem>();
        public bool IsDescId { get; set; }
        public bool IsDescTitle { get; set; }
        public bool IsDescCategory { get; set; }
        public double TotalSubCategoryAmount { get; set; }
        public double SubCategoryAmountPerPage { get; set; }
        public int TotalNumberOfPages { get; set; }
        public int SelectedPageNumber { get; set; }
        public string SelectedColumn { get; set; }
        public bool IsDesc { get; set; }
        public string q { get; set; }
        public class SubCategoryItem
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string CategoryTitle { get; set; }
        }
    }
}
