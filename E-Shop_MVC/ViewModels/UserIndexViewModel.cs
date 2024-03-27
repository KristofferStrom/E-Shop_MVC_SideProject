namespace E_Shop_MVC.ViewModels
{
    public class UserIndexViewModel
    {
        public List<UserItem> Users { get; set; } = new List<UserItem>();
        public class UserItem
        {
            public string Id { get; set; }
            public string UserName { get; set; }
            public IList<string> Roles { get; set; } = new List<string>();
        }
    }
}
