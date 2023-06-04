using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamoDB.Contracts
{
    [DynamoDBTable("ProductosProveedor")]
    public class Producto
    {

        [DynamoDBProperty("id")]
        [DynamoDBHashKey]
        public Guid Id { get; set; }

        [DynamoDBProperty("nombre")]
        public string Nombre { get; set; }

        [DynamoDBProperty("precio")]
        public string Precio { get; set; }

        [DynamoDBProperty("stock")]
        public string Stock { get; set; }

        [DynamoDBProperty("proveedores")]
        public List<string> Proveedores { get; set; }


    }
}
