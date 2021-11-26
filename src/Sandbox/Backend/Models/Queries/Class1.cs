using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Models.Queries
{
    public class ValueTextItem<TValue>
    {
        public TValue Value { get; set; }
        public string Text { get; set; }
    }
}
