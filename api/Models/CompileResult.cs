using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class CompileResult
    {
        public bool Success { get; set; }
        public string Output { get; set; }=string.Empty;
        public string Error { get; set; }=string.Empty;
    }
}