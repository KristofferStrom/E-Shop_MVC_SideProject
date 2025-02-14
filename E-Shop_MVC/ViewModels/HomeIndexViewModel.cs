﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace E_Shop_MVC.ViewModels
{
    public class HomeIndexViewModel
    {
        public List<ProductItem> Products { get; set; } = new List<ProductItem>();
        public List<ProductItem> TopRatedProducts { get; set; } = new List<ProductItem>();
        public List<CompanyItem> AllCompanies { get; set; } = new List<CompanyItem>();
        public List<CompanyItem> TopCompanies { get; set; } = new List<CompanyItem>();
        public List<CategoryItem> AllCategories { get; set; } = new List<CategoryItem>();
        public List<CategoryItem> TopCategories { get; set; } = new List<CategoryItem>();
        public List<SelectListItem> SortBy { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> IncludeProductAmount { get; set; } = new List<SelectListItem>();

        [HiddenInput]
        public string? q { get; set; }

        public int? SelectedSortBy { get; set; }
        public int? SelectedIncludeProductAmount { get; set; }


        public double ProductsAmount { get; set; }
        public double ProductAmountPerPage { get; set; }
        public int TotalNumberOfPages { get; set; }
        public int SelectedPageNumber { get; set; }
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
