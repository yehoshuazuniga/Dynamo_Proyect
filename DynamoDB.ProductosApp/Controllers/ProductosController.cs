using DynamoDB.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace DynamoDB.ProductosApp.Controllers
{

    [Route("[controller]")]

    public class ProductosController : Controller
	{
		private IProductosRepository _repository;

		public ProductosController(IProductosRepository repository)
		{
			_repository= repository;
		}


		public async Task<IActionResult> Index( string nombre ="")
		{
			if (!string.IsNullOrEmpty(nombre))
			{
				var productos = await _repository.Find(new SearchRequest { Nombre= nombre});

				return View(new ProductosVeiwModel
				{
					Productos = productos,
					ResultType = ResultType.Search
                });
			}
			else
			{
				var productos = await _repository.All();
				return View(productos);
			}
		}

        [Route("Create")]
        public IActionResult Create()
        {
            return View("~/Views/Productos/CreateOrUpdate.cshtml");
        }
        [HttpPost]
        [Route("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductoImputModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View("~/Views/Productos/CreateOrUpdate.cshtml", model);
                await _repository.Add(model);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("~/Views/Productos/CreateOrUpdate.cshtml", model);
            }
        }

        [HttpGet]
        [Route("Edit/{productoId}")]
        public async Task<IActionResult> Edit(Guid productoId)
        {
            var producto = await _repository.Single(productoId);

            ViewBag.productoId = productoId;
            return View("~/Views/Productos/CreateOrUpdate.cshtml", new ProductoImputModel
            {
                Nombre = producto.Nombre,
                Precio = producto.Precio,
                Stock = producto.Stock,
                inputType = InputType.Update
            });
        }

        [HttpPost]
        [Route("Edit/{productoId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid productoId, ProductoImputModel model)
        {
            try
            {

                if (!ModelState.IsValid)
                    return View("~/Views/Productos/CreateOrUpdate.cshtml", model);
                await _repository.Update(productoId, model);
                return RedirectToAction(nameof(Index));

            }
            catch
            {
                return View("~/Views/Productos/CreateOrUpdate.cshtml", model);
            }
        }

        [HttpGet]
        [Route("Delete/{productoId}")]
        public async Task<IActionResult> Delete(Guid productoId)
        {
            await _repository.Remove(productoId);

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        [Route("AddProvider/{productoId}")]
        public async Task<IActionResult> AddProvider(Guid productoId)
        {
            var producto = await _repository.Single(productoId);

            ViewBag.productoId = productoId;
            return View("~/Views/Productos/CreateOrUpdate.cshtml", new ProductoImputModel
            {
                Nombre = producto.Nombre,
                Precio = producto.Precio,
                Stock = producto.Stock,
                inputType = InputType.addProvider
            });
        }
    }
}
