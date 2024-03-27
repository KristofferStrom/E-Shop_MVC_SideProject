using E_Shop_MVC.Models.Data.Interfaces;
using E_Shop_MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace E_Shop_MVC.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IProductRepository _productRepository;
        private readonly ISubCategoryRepository _subCategoryRepository;
        private readonly ICategoryRepository _categoryRepository;


        public ReviewController(IReviewRepository reviewRepository,
                                IProductRepository productRepository,
                                ISubCategoryRepository subCategoryRepository,
                                ICategoryRepository categoryRepository)
        {
            _reviewRepository = reviewRepository;
            _productRepository = productRepository;
            _subCategoryRepository = subCategoryRepository;
            _categoryRepository = categoryRepository;
        }


        public IActionResult Index(int prodId, int catId, int subCatId)
        {
            var viewModel = new ReviewIndexViewModel();

            viewModel.Reviews = IndexGetReviewsByProductId(prodId);
            viewModel.Product = IndexGetProductById(prodId);
            viewModel.SubCategory = IndexGetSubCategoryById(subCatId);
            viewModel.Category = IndexGetCategoryById(catId);

            return View(viewModel);
        }


        private ReviewIndexViewModel.CategoryItem IndexGetCategoryById(int catId)
        {
            var category = _categoryRepository.GetById(catId);
            return new ReviewIndexViewModel.CategoryItem
            {
                Id = category.Id,
                Title = category.Title,
                NumberOfProducts = _productRepository.CountByCategoryId(category.Id)
            };
        }
        private ReviewIndexViewModel.SubCategoryItem IndexGetSubCategoryById(int subCatId)
        {
            var subCategory = _subCategoryRepository.GetById(subCatId);
            return new ReviewIndexViewModel.SubCategoryItem
            {
                Id = subCategory.Id,
                Title = subCategory.Title,
                NumberOfProducts = _productRepository.CountBySubCategoryId(subCategory.Id)
            };
        }
        private ReviewIndexViewModel.ProductItem IndexGetProductById(int prodId)
        {
            var product = _productRepository.GetById(prodId);
            return new ReviewIndexViewModel.ProductItem
            {
                Id = product.Id,
                Title = product.Title,
                ImgTitle = product.imgTitle
            };
        }
        private List<ReviewIndexViewModel.ReviewItem> IndexGetReviewsByProductId(int prodId)
        {
            return _reviewRepository.GetByProductId(prodId).Select(rev => new ReviewIndexViewModel.ReviewItem
            {
                CustomerName = rev.CustomerName,
                Rate = rev.Rate,
                Review = rev.ReviewText,
                Date = rev.Date
            }).OrderByDescending(rev => rev.Date).ToList();
        }
    }
}
