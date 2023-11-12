using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KerryCoAdmin.Interfaces;
using KerryCoAdmin.Models;
using KerryCoAdmin.Entities.Dtos.Response;
using AutoMapper;

namespace BusinessManagement.Controllers
{

    [Route("admin")]
    [ApiController]
    public class AdminController : Controller
    {

       
        private readonly IMapper _mapper;
        private readonly IAdminRepository _adminRepository;

        public AdminController(IAdminRepository adminRepository, IMapper mapper)
        {
            _adminRepository = adminRepository;
            _mapper = mapper;

        }



        
        [HttpGet("{Id}/staffs")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin")]

        public async Task<IActionResult> GetAdmins(string Id)
        {
            
            var result = await _adminRepository.GetAdmins();

           

            var admins = new List<Admin>();

            foreach (var admin in result)
            {
                if(admin.User?.Id.ToString() != Id)
                {
                    admins.Add(admin);
                }
            }

            

            return Ok(admins);
        }


        [HttpGet("profile/{Id}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin, Staff")]

        public async Task<IActionResult> GetAdminProfile([FromRoute] string Id)
        {
            var adminResponse = await _adminRepository.GetAdminByUser(Id);
            

            if(adminResponse == null)
            {
                return NotFound("This admin cannot be found");
            }

            var admin = _mapper.Map<AdminProfileResponse>(adminResponse);

            return Ok(admin);



        }



       

    }
}
