using E_Shop_MVC.Models.Data;
using E_Shop_MVC.Models.Data.Interfaces;
using E_Shop_MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace E_Shop_MVC.Controllers
{
    public class SubCategoryController : Controller
    {
        private readonly ISubCategoryRepository _subCategoryRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly int _subCategoriesPerPage = 10;

        public SubCategoryController(
            ISubCategoryRepository subCategoryRepository,
            ICategoryRepository categoryRepository)
        {
            _subCategoryRepository = subCategoryRepository;
            _categoryRepository = categoryRepository;
        }


        [Authorize]
        public IActionResult Index(string q, bool isDesc, string col, int pageNr = 1)
        {
            var viewModel = new SubCategoryIndexViewModel();

            var subCategorySearchResult = _subCategoryRepository.GetSearchResult(q).ToList();
            var sortedSubCategories = GetSortedSubCategoryList(subCategorySearchResult, isDesc, col, ref viewModel);

            viewModel.SubCategories = GetSubCategoriesByPageNr(pageNr, sortedSubCategories);
            viewModel.TotalSubCategoryAmount = subCategorySearchResult.Count;
            viewModel.SubCategoryAmountPerPage = _subCategoriesPerPage;
            viewModel.TotalNumberOfPages = (int)Math.Ceiling(viewModel.TotalSubCategoryAmount / viewModel.SubCategoryAmountPerPage);
            viewModel.SelectedPageNumber = pageNr;
            viewModel.q = q;

            return View(viewModel);
        }


        [Authorize]
        public IActionResult Edit(int id)
        {
            var viewModel = new SubCategoryEditViewModel();

            var subCategoryToEdit = _subCategoryRepository.GetById(id);

            viewModel.Id = subCategoryToEdit.Id;
            viewModel.Title = subCategoryToEdit.Title;
            viewModel.SelectedCategoryId = _categoryRepository.GetBySubCategoryId(id).Id;
            viewModel.Categories = EditGetCategoryListItems();

            return View(viewModel);
        }


        [Authorize]
        [HttpPost]
        public IActionResult Edit(SubCategoryEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var updatedSubCategory = CreateSubCategory(viewModel);
                _subCategoryRepository.Update(updatedSubCategory);

                return RedirectToAction("Index");
            }

            var subCategoryToEdit = _subCategoryRepository.GetById(viewModel.Id);

            viewModel.Id = subCategoryToEdit.Id;
            viewModel.Title = subCategoryToEdit.Title;
            viewModel.SelectedCategoryId = _categoryRepository.GetBySubCategoryId(viewModel.Id).Id;
            viewModel.Categories = EditGetCategoryListItems();

            return View(viewModel);
        }


        [Authorize]
        public IActionResult New()
        {
            var viewModel = new SubCategoryNewViewModel();

            viewModel.Categories = NewGetCategoryListItems();

            return View(viewModel);
        }


        [Authorize]
        [HttpPost]
        public IActionResult New(SubCategoryNewViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var newSubCategory = CreateSubCategory(viewModel);
                _subCategoryRepository.Add(newSubCategory);

                return RedirectToAction("Index");
            }

            viewModel.Categories = NewGetCategoryListItems();

            return View(viewModel);
        }


        private List<SelectListItem> NewGetCategoryListItems()
        {
            var categories = new List<SelectListItem>();
            categories.Add(new SelectListItem { Value = "0", Text = "...Välj Kategori..." });
            categories.AddRange(_categoryRepository.GetAll().Select(cat => new SelectListItem
            {
                Text = cat.Title,
                Value = cat.Id.ToString()
            }));
            return categories;
        }
        private List<SelectListItem> EditGetCategoryListItems()
        {
            var categories = new List<SelectListItem>();
            categories.AddRange(_categoryRepository.GetAll().Select(cat => new SelectListItem
            {
                Text = cat.Title,
                Value = cat.Id.ToString()
            }));
            return categories;
        }
        private List<SubCategoryIndexViewModel.SubCategoryItem> GetSubCategoriesByPageNr(int pageNr, IEnumerable<SubProductCategory> sortedSubCategories)
        {
            return sortedSubCategories.Skip((pageNr - 1) * _subCategoriesPerPage).Take(_subCategoriesPerPage).Select(subCat =>
                new SubCategoryIndexViewModel.SubCategoryItem
                {
                    Id = subCat.Id,
                    Title = subCat.Title,
                    CategoryTitle = subCat.Category.Title
                }).ToList();
        }
        private IEnumerable<SubProductCategory> GetSortedSubCategoryList(IEnumerable<SubProductCategory> subCategorySearchResult, bool isDesc, string col, ref SubCategoryIndexViewModel viewModel)
        {
            viewModel.IsDescCategory = false;
            viewModel.IsDescTitle = false;
            viewModel.IsDescId = false;
            viewModel.SelectedColumn = col;
            viewModel.IsDesc = isDesc;

            if (isDesc && col == "title")
            {
                subCategorySearchResult = subCategorySearchResult.OrderByDescending(subCat => subCat.Title);

            }
            else if (!isDesc && col == "title")
            {
                subCategorySearchResult = subCategorySearchResult.OrderBy(subCat => subCat.Title);
                viewModel.IsDescTitle = true;
            }
            else if (isDesc && col == "id")
            {
                subCategorySearchResult = subCategorySearchResult.OrderByDescending(subCat => subCat.Id);
            }
            else if (!isDesc && col == "id")
            {
                subCategorySearchResult = subCategorySearchResult.OrderBy(subCat => subCat.Id);
                viewModel.IsDescId = true;
            }

            else if (isDesc && col == "category")
            {
                subCategorySearchResult = subCategorySearchResult.OrderByDescending(subCat => subCat.Category.Title);
            }
            else if (!isDesc && col == "category")
            {
                subCategorySearchResult = subCategorySearchResult.OrderBy(subCat => subCat.Category.Title);
                viewModel.IsDescCategory = true;
            }

            return subCategorySearchResult;
        }
        private SubProductCategory CreateSubCategory(SubCategoryEditViewModel viewModel)
        {
            return new SubProductCategory
            {
                Id = viewModel.Id,
                Title = viewModel.Title,
                Category = _categoryRepository.GetById(viewModel.SelectedCategoryId)
            };
        }
        private SubProductCategory CreateSubCategory(SubCategoryNewViewModel viewModel)
        {
            return new SubProductCategory
            {
                Title = viewModel.Title,
                Category = _categoryRepository.GetById(viewModel.SelectedCategoryId)
            };
        }
    }
}
