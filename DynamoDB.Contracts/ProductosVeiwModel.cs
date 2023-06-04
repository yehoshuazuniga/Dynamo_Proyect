using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamoDB.Contracts
{
    public class ProductosVeiwModel
    {
        public IEnumerable<Producto> Productos { get; set; }
        public ResultType ResultType { get; set; }
        public string PaginationToken { get; set; }

    }
}
