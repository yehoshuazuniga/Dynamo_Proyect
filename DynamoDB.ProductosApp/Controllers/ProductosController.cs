using DynamoDB.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

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

            return await RecuperarProducto(productoId, InputType.Update);

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
            return await RecuperarProducto(productoId, InputType.addProvider);
        }

        [HttpPost]
        [Route("AddProvider/{productoId}")]
        public async Task<IActionResult> AddProvider(Guid productoId, ProductoImputModel model)
        {
            try
            {
                model.inputType = InputType.addProvider;
                await _repository.Update(productoId, model);
                return RedirectToAction(nameof(Index));

            }
            catch
            {
                return View("~/Views/Productos/CreateOrUpdate.cshtml", model);
            }
        }

        [HttpGet]
        [Route("removeProvider/{productoId}/{provider}")]
        public async Task<IActionResult> removeProvider(Guid productoId, string provider)
        {
            try
            {
                var producto = await _repository.Single(productoId);

                var model = new ProductoImputModel()
                {
                    Nombre = producto.Nombre,
                    Precio = producto.Precio,
                    Stock = producto.Stock,
                    Proveedores= new List<string>() { provider},
                    inputType = DynamoDB.Contracts.InputType.removeProvider
                };


                await _repository.Update(productoId, model);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Index));
            }

        }


        private async Task<IActionResult> RecuperarProducto(Guid productoId, InputType inputType)
        {
            var producto = await _repository.Single(productoId);

            ViewBag.productoId = productoId;
            ViewData["proveedores"] = producto.Proveedores;
            return View("~/Views/Productos/CreateOrUpdate.cshtml", new ProductoImputModel
            {
                Nombre = producto.Nombre,
                Precio = producto.Precio,
                Stock = producto.Stock,
                inputType = inputType
            });
        }
    }
}
