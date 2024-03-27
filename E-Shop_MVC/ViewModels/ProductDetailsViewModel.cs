using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace E_Shop_MVC.ViewModels
{
    public class ProductDetailsViewModel
    {
        public ProductDetailsItem? Product { get; set; }
        public List<ReviewItem> Reviews { get; set; } = new List<ReviewItem>();
        public List<ProductItem> RelatedProducts { get; set; } = new List<ProductItem>();
        public CategoryItem? Category { get; set; }
        public SubCategoryItem? SubCategory { get; set; }
        public PostReviewItem Review { get; set; }
        public int ReviewAmount { get; set; }
        [HiddenInput]
        public int ProductId { get; set; }
        [HiddenInput]
        public int ReviewCategoryId { get; set; }
        [HiddenInput]
        public int ReviewSubCategoryId { get; set; }
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
        public class PostReviewItem
        {
            [MaxLength(50, ErrorMessage = "Namnet får max innehålla 50 tecken")]
            [Required(ErrorMessage = "Fyll i fältet")]
            public string CustomerName { get; set; }
            [Range(1, 5, ErrorMessage = "Du måste betygsätta produkten")]
            [Required(ErrorMessage = "Du måste betygsätta produkten")]
            public int Rate { get; set; }
            [MaxLength(500, ErrorMessage = "Recensionen får max innehålla 500 tecken")]
            public string ReviewText { get; set; }

        }
        public class ReviewItem
        {
            public string CustomerName { get; set; }
            public int Rate { get; set; }
            public string Review { get; set; }
            public DateTime Date { get; set; }
        }
        public class ProductItem
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public decimal Price { get; set; }
            public int AvgRate { get; set; }
            public string ImgTitle { get; set; }
            public int CompanyId { get; set; }
            public string CompanyTitle { get; set; }
            public bool IsTopRated { get; set; }
        }
        public class ProductDetailsItem
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public decimal Price { get; set; }
            public string LongDescription { get; set; }
            public string ShortDescription { get; set; }
            public int AvgRate { get; set; }
            public string ImgTitle { get; set; }
            public int CompanyId { get; set; }
            public string CompanyTitle { get; set; }
            public bool InStock { get; set; }
            public string Color { get; set; }
            public int NumberOfReviews { get; set; }
            public int WarrantyYears { get; set; }
        }
    }
}
