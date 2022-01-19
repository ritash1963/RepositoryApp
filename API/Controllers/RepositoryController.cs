using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using API.DTOs;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RepositoryController : ControllerBase
    {
       private readonly DataContext _context;
       private readonly ITokenService _tokenService;
       public RepositoryController(DataContext context, ITokenService tokenService) 
       {
          _context = context;
          _tokenService = tokenService;
       }
       
       // api/repository/mongoid_search
       // [AllowAnonymous] 
       [Authorize]
       [HttpGet("{itemname}")]
        public async Task<ActionResult<IEnumerable<RepositoryDto>>> GetRepositories(string itemname)
       {
           
           return Ok(await _context.Owners.Join(_context.Items, o => o.Id, i => i.OwnerId,
                      (o, i) => new { o, i })
                     .Where(oi => oi.o.Id == oi.i.OwnerId && oi.i.Name.Contains(itemname))
                     .Select(oi => new { oi.i.Name, oi.o.AvatarUrl })
                     .ToListAsync()); 
        
       }

       [Authorize]
       [HttpGet]
        public async Task<ActionResult<IEnumerable<RepositoryDto>>> GetAllRepositories()
       {
           
           return Ok(await _context.Owners.Join(_context.Items, o => o.Id, i => i.OwnerId,
                      (o, i) => new { o, i })
                     .Select(oi => new { oi.i.Name, oi.o.AvatarUrl })
                     .ToListAsync()); 
        
       }
    }        
}
