using CRUD_NoSQL.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CRUD_NoSQL.Controllers
{
    public class ProductosController : Controller
    {
        private readonly IMongoCollection<Producto> _productosCollection;

        public ProductosController(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("CRUD_NoSQL");
            _productosCollection = database.GetCollection<Producto>("Productos");
        }

        public async Task<IActionResult> Index()
        {
            var productos = await _productosCollection.Find(_ => true).ToListAsync();
            return View(productos);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre", "Precio")] Producto producto)
        {
            producto.Id = ObjectId.GenerateNewId().ToString();
            try
            {
                await _productosCollection.InsertOneAsync(producto);
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"Error insertando el documento: {ex.Message}");
                ModelState.AddModelError("", "Error insertando el documento");
                return View(producto);
            }
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
                return NotFound();

            var producto = await _productosCollection.Find(p => p.Id == id).FirstOrDefaultAsync();
            if (producto == null)
                return NotFound();

            return View(producto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id", "Nombre", "Precio")] Producto producto)
        {
            if (id != producto.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                await _productosCollection.ReplaceOneAsync(p => p.Id == id, producto);
                return RedirectToAction(nameof(Index));
            }

            return View(producto);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
                return NotFound();

            var producto = await _productosCollection.Find(p => p.Id == id).FirstOrDefaultAsync();
            if (producto == null)
                return NotFound();

            return View(producto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id, [Bind("Id", "Nombre", "Precio")] Producto producto)
        {
            await _productosCollection.DeleteOneAsync(p => p.Id == id);
            return RedirectToAction(nameof(Index));
        }
    }
}