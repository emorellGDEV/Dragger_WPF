using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dragger_WPF.Entity
{
    public class Card
    {
        public int _id_persona { get; set; }

        public int _id_card { get; set; }

        public string _description { get; set; }

        public string _color { get; set; }

        public int _priority { get; set; }

        public DateOnly _goalDate { get; set; }

        public DateOnly _creationDate { get; set; }
    }
}
