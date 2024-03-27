namespace E_Shop_MVC.ViewModels
{
    public class ReviewIndexViewModel
    {
        public List<ReviewItem> Reviews { get; set; } = new List<ReviewItem>();
        public ProductItem Product { get; set; }
        public SubCategoryItem SubCategory { get; set; }
        public CategoryItem Category { get; set; }
        public class SubCategoryItem
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public int NumberOfProducts { get; set; }
        }
        public class CategoryItem
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public int NumberOfProducts { get; set; }
        }

        public class ProductItem
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string ImgTitle { get; set; }
        }
        public class ReviewItem
        {
            public string CustomerName { get; set; }
            public int Rate { get; set; }
            public string Review { get; set; }
            public DateTime Date { get; set; }
        }

    }
}
