using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/Compiller")]
    public class CompillerController :ControllerBase
    {
        private readonly ICompillerRepository _compillerRepo;
        public CompillerController(ICompillerRepository compillerRepo)
        {
            _compillerRepo=compillerRepo;
        }
        
        [HttpPost]
        public async Task<IActionResult>Compile([FromBody]CompileRequest request)
        {
            var result = await _compillerRepo.CompileAsync(request);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        
    }
}