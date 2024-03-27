namespace E_Shop_MVC.ViewModels
{
    public class CategorySelectSubCategoryViewModel
    {
        public List<ProductItem> Products { get; set; }
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
    }
}
