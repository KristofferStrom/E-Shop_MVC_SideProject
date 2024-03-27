namespace E_Shop_MVC.ViewModels
{
    public class CategoryShowTreeViewModel
    {
        public CategoryItem Category { get; set; }
        public SubCategoryItem SubCategory { get; set; }
        public class CategoryItem
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public int NumberOfProducts { get; set; }
        }
        public class SubCategoryItem
        {
            public string Title { get; set; }
            public int NumberOfProducts { get; set; }
        }
    }
}
