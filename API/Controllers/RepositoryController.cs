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
using AutoMapper;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RepositoryController : ControllerBase
    {
       private readonly DataContext _context;
       private readonly ITokenService _tokenService;
       private readonly IMapper _mapper;

       public RepositoryController(DataContext context, ITokenService tokenService, IMapper mapper) 
       {
          _context = context;
          _tokenService = tokenService;
          _mapper = mapper;
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

        [AllowAnonymous] 
       // [Authorize]
        [HttpPost("mark")]
        public async Task<IActionResult> MarkItem(MarkItemDto markItemDto)
        {
            var appUser = await _context.Users.FirstOrDefaultAsync(x => x.UserName == markItemDto.UserName);
            var itemRep = await _context.Items.FirstOrDefaultAsync(x => x.Name == markItemDto.ItemName);
            var markItem = await _context.MarkItems.FirstOrDefaultAsync(x => x.UserName == markItemDto.UserName 
              && x.ItemName == markItemDto.ItemName );

             if (appUser == null)
                return BadRequest("User is not exists");

             if (itemRep == null)
                return BadRequest("Item is not exists");
 
             if (markItem != null)
                return BadRequest("Bookmark is already exists");
            
             var itemMarkForCreate = _mapper.Map<MarkItem>(markItemDto);
            
             await _context.MarkItems.AddAsync(itemMarkForCreate);

             if (await _context.SaveChangesAsync() > 0)
                return StatusCode(201);

             return BadRequest("Could not bookmark item");
        }
    }        
}
