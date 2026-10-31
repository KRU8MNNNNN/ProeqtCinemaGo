using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proeqt
{
    internal class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Price { get; set; }

        public string Description { get; set; }

        public override string ToString()
        {
            return $"{Id}|{Name}|{Price}|{Description}";
        }
    }
}
