using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MongodbRepository
{
    public class MdbModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.Int32)]
        public int Id { get; set; }
        public string name { get; set; }
        public string food { get; set; }
        public DateTime birdate { get; set; }
        public int age { get; set; }
     
    }
}
