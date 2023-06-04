namespace DynamoDB.Contracts
{
    public class ProductoImputModel
    {
        public string Nombre { get; set; }
        public string Precio { get; set; }
        public string Stock { get; set; }
        public List<string>? Proveedores { get; set; }

        public InputType inputType { get; set; }

    }
}
