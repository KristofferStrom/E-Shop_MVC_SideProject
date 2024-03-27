namespace E_Shop_MVC.ViewModels
{
    public class CompanyIndexViewModel
    {
        public List<CompanyItem> Companies { get; set; } = new List<CompanyItem>();
        public bool IsDescId { get; set; }
        public bool IsDescTitle { get; set; }
        public double TotalCompanyAmount { get; set; }
        public double CompanyAmountPerPage { get; set; }
        public int TotalNumberOfPages { get; set; }
        public int SelectedPageNumber { get; set; }
        public string SelectedColumn { get; set; }
        public bool IsDesc { get; set; }
        public string q { get; set; }
        public class CompanyItem
        {
            public int Id { get; set; }
            public string Title { get; set; }
        }
    }
}
