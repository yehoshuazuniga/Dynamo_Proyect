using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using DynamoDB.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace DynamoDB.Core
{
    public class ProductosRepository : IProductosRepository
    {

        private readonly AmazonDynamoDBClient _client;
        private readonly DynamoDBContext _context;

        public ProductosRepository()
        {
            _client= new AmazonDynamoDBClient();
            _context = new DynamoDBContext(_client);
        }

        public async Task Add(ProductoImputModel entity)
        {
            var producto = new Producto()
            {
                Id =Guid.NewGuid(),
                Nombre = entity.Nombre,
                Precio = entity.Precio,
                Stock = entity.Stock,
                Proveedores = entity.Proveedores,
            };

             await _context.SaveAsync(producto);

        }

        public async Task<ProductosVeiwModel> All(string paginationToken = "")
        {
            var table = _context.GetTargetTable<Producto>();
            var scanOps = new ScanOperationConfig();
            if (!string.IsNullOrEmpty(paginationToken))
            {
                scanOps.PaginationToken= paginationToken;
            }
            var results= table.Scan(scanOps);
            List<Document>data = await results.GetNextSetAsync();
            IEnumerable<Producto> productos= _context.FromDocuments<Producto>(data);

            return new ProductosVeiwModel
            {
                PaginationToken = results.PaginationToken,
                Productos = productos,
                ResultType = ResultType.List

            };
        }

        public async Task<IEnumerable<Producto>> Find(SearchRequest searchReq)
        {
            var scanConditions = new List<ScanCondition>();
            if (!string.IsNullOrEmpty(searchReq.Nombre))
                scanConditions.Add(new ScanCondition("Nombre", ScanOperator.Equal, searchReq.Nombre));
            if (!string.IsNullOrEmpty(searchReq.Precio))
                scanConditions.Add(new ScanCondition("Precio", ScanOperator.Equal, searchReq.Precio));
            if (!string.IsNullOrEmpty(searchReq.Stock))
                scanConditions.Add(new ScanCondition("Stock", ScanOperator.Equal, searchReq.Stock));
            return await _context.ScanAsync<Producto>(scanConditions, null).GetRemainingAsync();

        }
        //quita productos
        public async Task Remove(Guid prductoId)
        {
            await _context.DeleteAsync<Producto>(prductoId);
        }

        public async Task<Producto> Single(Guid prductoId)
        {
            return await _context.LoadAsync<Producto>(prductoId);
        }

        public async Task Update(Guid prductoId, ProductoImputModel entity)
        {
             var producto = await Single(prductoId);

            producto.Nombre = entity.Nombre;
            producto.Precio = entity.Precio;
            producto.Stock = entity.Stock;
            if(entity.inputType== DynamoDB.Contracts.InputType.addProvider)
            {
                producto.Proveedores.Add(entity.Proveedores.First());
            }
            if (entity.inputType == DynamoDB.Contracts.InputType.removeProvider)
            {
                producto.Proveedores.Remove(entity.Proveedores.First());

            }

            await _context.SaveAsync(producto);
            
        }
    }
}
