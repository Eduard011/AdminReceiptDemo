using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AdminReciptsDemo.Context;
using AdminReciptsDemo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AdminReciptsDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController: ControllerBase
    {
        private readonly ApplicationDbContext _appDbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(ApplicationDbContext appDbContext, UserManager<ApplicationUser> userManager)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
        }

        [HttpPost("AssignUserRole")]
        public async Task<ActionResult> AssignRoleUser(Role rol)
        {
            var user = await _userManager.FindByIdAsync(rol.UserId);

            if (user == null)
            {
                return NotFound();
            }

            await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, rol.RoleName));

            await _userManager.AddToRoleAsync(user, rol.RoleName);

            return Ok();
        }

        [HttpPost("RemoveUserRole")]
        public async Task<ActionResult> RemoveRoleUser(Role rol)
        {
            var user = await _userManager.FindByIdAsync(rol.UserId);

            if (user == null)
            {
                return NotFound();
            }

            await _userManager.RemoveClaimAsync(user, new Claim(ClaimTypes.Role, rol.RoleName));

            await _userManager.RemoveFromRoleAsync(user, rol.RoleName);

            return Ok();
        }
    }
}