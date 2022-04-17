using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    public class Threat
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public string Target { get; set; }
        public bool ConfidentialityBreach { get; set; }
        public bool IntegrityViolation { get; set; }
        public bool AccessViolation { get; set; }
    }
}
