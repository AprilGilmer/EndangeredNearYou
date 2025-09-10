using EndangeredNearYou.Domain.Repositories;
using EndangeredNearYou.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace EndangeredNearYou.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        //public IActionResult Index()
        //{
        //    List<ProductViewModel> products = _productRepository.GetAllProducts();
        //    return View(products);
        //}

        //public IActionResult ViewProduct(int id)
        //{
        //    ProductViewModel product = _productRepository.GetProductById(id);
        //    return View(product);
        //}

        //public IActionResult UpdateProduct(int id)
        //{
        //    ProductViewModel prod = _productRepository.GetProductById(id);
        //    if (prod == null)
        //    {
        //        return View("ProductNotFound");
        //    }
        //    return View(prod);
        //}

        //public IActionResult UpdateProductToDatabase(ProductViewModel model)
        //{
        //    _productRepository.UpdateProduct(model);

        //    return RedirectToAction("ViewProduct", new { id = model.ProductId });
        //}

        //public IActionResult InsertProduct()
        //{
        //    var prod = _productRepository.AssignCategory();
        //    return View(prod);
        //}

        //public IActionResult InsertProductToDatabase(ProductViewModel productToInsert)
        //{
        //    _productRepository.InsertProduct(productToInsert);
        //    return RedirectToAction("Index");
        //}

        //public IActionResult DeleteProduct(ProductViewModel product)
        //{
        //    _productRepository.DeleteProduct(product);
        //    return RedirectToAction("Index");
        //}
    }
}