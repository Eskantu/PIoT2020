using System;
using System.Collections.Generic;
using System.Text;

namespace PIoT2020.Shared.Modelos
{
   public class ChartModel
    {
        public DateTime year { get; set; }
        public decimal value { get; set; }
        public override string ToString()
        {
            return $"{year}-{value}";
        }
    }
}
