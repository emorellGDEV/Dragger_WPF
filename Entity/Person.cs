using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dragger_WPF.Entity
{
    public class Person
    {
        public ObjectId _id { get; set; }
        public int id_person { get; set; }
        public string name { get; set; }
    }
}
