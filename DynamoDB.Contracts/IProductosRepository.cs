namespace DynamoDB.Contracts
{
    public interface IProductosRepository
    {

        Task<Producto> Single(Guid productoId);
        Task<ProductosVeiwModel> All(string paginationToken = "");
        Task<IEnumerable<Producto>> Find(SearchRequest searchReq);
        Task Add(ProductoImputModel entity);
        Task Remove(Guid prductoId);
        Task Update(Guid prductoId, ProductoImputModel entity);
    }
}
