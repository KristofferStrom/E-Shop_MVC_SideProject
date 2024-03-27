namespace E_Shop_MVC.ViewModels
{
    public class ProductIndexViewModel
    {
        public List<ProductItem> Products { get; set; } = new List<ProductItem>();
        public bool IsDescId { get; set; }
        public bool IsDescTitle { get; set; }
        public bool IsDescCompany { get; set; }
        public bool IsDescSubCategory { get; set; }
        public bool IsDescPrice { get; set; }
        public double TotalProductsAmount { get; set; }
        public double ProductAmountPerPage { get; set; }
        public int TotalNumberOfPages { get; set; }
        public int SelectedPageNumber { get; set; }
        public string SelectedColumn { get; set; }
        public bool IsDesc { get; set; }
        public string q { get; set; }
        public class ProductItem
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string SubCategoryTitle { get; set; }
            public string CompanyTitle { get; set; }
            public decimal Price { get; set; }
            public string ImgTitle { get; set; }
        }
    }
}
