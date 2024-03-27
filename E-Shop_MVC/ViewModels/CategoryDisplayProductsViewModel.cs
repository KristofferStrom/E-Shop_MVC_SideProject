namespace E_Shop_MVC.ViewModels
{
    public class CategoryDisplayProductsViewModel
    {
        public List<ProductItem> Products { get; set; } = new List<ProductItem>();
        public List<ProductItem> TopRatedProducts { get; set; } = new List<ProductItem>();
        public List<CompanyItem> Companies { get; set; } = new List<CompanyItem>();
        public List<SubCategoryItem> SubCategories { get; set; } = new List<SubCategoryItem>();
        public SubCategoryItem SubCategory { get; set; }
        public CategoryItem Category { get; set; }
        public class SubCategoryItem
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public int NumberOfProducts { get; set; }
            public string Active { get; set; }
        }
        public class CompanyItem
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public int NumberOfProducts { get; set; }
        }
        public class ProductItem
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public decimal Price { get; set; }
            public int Rate { get; set; }
            public string ImgTitle { get; set; }
            public int CompanyId { get; set; }
            public string CompanyTitle { get; set; }
            public bool IsTopRated { get; set; }
        }
        public class CategoryItem
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public int NumberOfProducts { get; set; }
        }
    }
}
