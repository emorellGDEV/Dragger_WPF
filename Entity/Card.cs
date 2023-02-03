using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Dragger_WPF.Entity
{
    public class Card
    {
        public ObjectId _id { get; set; }
        public int fk_id_responsable { get; set; }

        public int id_card { get; set; }

        public string description { get; set; }

        public int priority{ get; set; }

        public DateTime goalDate { get; set; }

        public DateTime creationDate { get; set; }

        public int position { get; set; }
    }
}
