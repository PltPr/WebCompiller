using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class CompileRequest
    {
        public string Language { get; set; }=string.Empty;
        public string FileName { get; set; }=string.Empty;
        public string Code { get; set; }=string.Empty;
    }
}