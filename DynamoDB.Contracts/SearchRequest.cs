using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamoDB.Contracts
{
    public class SearchRequest
    {

        public string Nombre { get; set; }
        public string Precio { get; set; }
        public string Stock { get; set; }

    }
}
