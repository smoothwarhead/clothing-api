using KerryCoAdmin.Api.Entities.Dtos.Requests;
using KerryCoAdmin.Api.Entities.Dtos.Responses;
using KerryCoAdmin.Api.Entities.Models;
using KerryCoAdmin.Api.Interfaces;
using KerryCoAdmin.Api.Modules;
using KerryCoAdmin.Entities.Dtos.Response;
using KerryCoAdmin.Interfaces;
using KerryCoAdmin.Models;
using KerryCoAdmin.Modules;
using KerryCoAdmin.Repositories;
using KerryCoAdmint.Modules.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Data;
using System.Text;

namespace KerryCoAdmin.Api.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IAdminRepository _adminRepository;
        private readonly IConfiguration _configuration;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly Byte[] _key;
        private readonly DateTime _expirationDate;
       // private readonly EmailSettings _mailSettings;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ISecretService _secretService;
        private readonly List<VaultSecret> _secrets;
        private readonly string _apiKey;
        private readonly string _note;
        private readonly string _email;









        public AuthController(RoleManager<IdentityRole> roleManager, ISecretService secretService, IAdminRepository adminRepository, IUserRepository userRepository, ITokenGenerator tokenGenerator, IConfiguration configuration, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _adminRepository = adminRepository;
            _tokenGenerator = tokenGenerator;
            _configuration = configuration;
            _secretService = secretService;
            _secrets = _secretService.GetSecrets();
            _key = Encoding.UTF8.GetBytes(GetInfo.GetASecret("Jwt-secret-dev-001", _secrets));
            _expirationDate = DateTime.UtcNow.Add(TimeSpan.Parse(GetInfo.GetASecret("Jwt-expiryTimeFrame", _secrets)));
            //_mailSettings = _configuration.GetSection("SendGrid").Get<EmailSettings>();
            _roleManager = roleManager;
           
            _note = GetInfo.GetASecret("Sendgrid-note", _secrets);
            _apiKey = GetInfo.GetASecret("Sendgrid-key", _secrets);
            _email = GetInfo.GetASecret("Sendgrid-senderEmail", _secrets);




        }




        [HttpPost("admin/add-staff")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin")]

        public async Task<IActionResult> AddStaff([FromBody] AuthRequest request)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                            new InitialAuthResponse
                            {
                                IsAuth = false,
                                StatusType = "error",
                                Message = "invalid credential"
                            });
            }

            // check if this user exists
            var user = await _userRepository.GetUserByEmail(request.Email);

            if (user != null)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                            new InitialAuthResponse
                            {
                                IsAuth = false,
                                StatusType = "error",
                                Message = "This user already exists"
                            });
            }

            else
            {
                //get the username
                string userName = Generate.GenerateUsername(request.FirstName, request.LastName, request.Email);

                //get the password
                string Password = Generate.GeneratePassword(8);

                //Add the user to the database
                IdentityUser new_user = new()
                {
                    Email = request.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = userName,
                    PhoneNumber = request.PhoneNumber,
                    TwoFactorEnabled = true

                };

                //check if role exists
                var roleExists = await _userRepository.FindRole(request.Role);

                if (roleExists)
                {
                    //Console.WriteLine(password);

                    var userIsSaved = await _userRepository.CreateUser(new_user, Password);

                    if (!userIsSaved)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError,
                            new InitialAuthResponse
                            {
                                IsAuth = false,
                                StatusType = "error",
                                Message = "User not created1"
                            });
                    }

                    // add user to role

                    var roleAdded = await _userRepository.AddRole(new_user, request.Role);

                    if (!roleAdded)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError,
                            new InitialAuthResponse
                            {
                                IsAuth = false,
                                StatusType = "error",
                                Message = "User not created2"
                            });
                    }





                    var savedUser = await _userRepository.GetUserByEmail(request.Email.Trim());

                    if (savedUser != null)
                    {

                        // add as an admin

                        var new_admin = new Admin()
                        {
                            FirstName = request.FirstName,
                            LastName = request.LastName,
                            ModifiedAt = DateTime.Now,
                            User = savedUser

                        };

                        var adminSaved = await _adminRepository.CreateAdmin(new_admin);

                        if (adminSaved)
                        {
                            string content = $@"Your profile has been created. You can change your password after login<br/>
                                        Your credentials are below<br/>
                                        Email: <strong>{savedUser.Email}</strong><br/>
                                        UserName: <strong>{userName}</strong><br/>
                                        Password: <strong>{Password}</strong>   ";

                            string plainText = "Your credentials";




                            EmailSettings _mailSettings = new EmailSettings()
                            {
                                Key = _apiKey,
                                Note = _note,
                                SenderEmail = _email

                            };


                            var emailSent = EmailService.SendEmail(request.Email, content, plainText, "Account Created", _mailSettings);

                            if (emailSent != null)
                            {
                                var response = new InitialAuthResponse
                                {
                                    IsAuth = true,
                                    StatusType = "success",
                                    Message = "An admin is successfully created. A confirmation email has been sent to their email address",

                                };



                                return Ok(response);



                            }

                            return StatusCode(StatusCodes.Status500InternalServerError,
                            new InitialAuthResponse
                            {
                                IsAuth = false,
                                StatusType = "error",
                                Message = "Email not sent"
                            });

                        }

                        return StatusCode(StatusCodes.Status500InternalServerError,
                           new InitialAuthResponse
                           {
                               IsAuth = false,
                               StatusType = "error",
                               Message = "Admin not saved"
                           });



                    }

                    return StatusCode(StatusCodes.Status500InternalServerError,
                           new InitialAuthResponse
                           {
                               IsAuth = false,
                               StatusType = "error",
                               Message = "User not created"
                           });






                }



                return StatusCode(StatusCodes.Status500InternalServerError,
                            new InitialAuthResponse
                            {
                                IsAuth = false,
                                StatusType = "error",
                                Message = "User not created"
                            });



            }





        }



        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                            new InitialAuthResponse
                            {
                                IsAuth = false,
                                StatusType = "error",
                                Message = "invalid credential"
                            });
            }

            var user = await _userRepository.GetUserByEmail(request.Email);


            var password = await _userRepository.CheckPassword(user, request.Password.Trim());

            if (user != null && password)
            {
                if (user.TwoFactorEnabled)
                {

                    // await _signInManager.SignOutAsync();
                    //await _signInManager.PasswordSignInAsync(user, request.Password.Trim(), false, true);

                    //generate a 2FA token
                    var twoFactorToken = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");



                    string content = $@"Your authorization OTP is below<br/>
                                        {twoFactorToken}";

                    string plainText = "Your OTP";


                    EmailSettings _mailSettings = new EmailSettings()
                    {
                        Key = _apiKey,
                        Note = _note,
                        SenderEmail = _email

                    };


                    var emailSent = EmailService.SendEmail(user.Email, content, plainText, "OTP Confirmation", _mailSettings);


                    if (emailSent != null)
                    {
                        //return ok to client
                        return Ok(new LoginResponse
                        {
                            Is2fa = true,
                            Email = user.Email,
                            StatusType = "success",
                            Message = $"We have sent an OTP to your Email at {user.Email}"
                        });
                    }

                    return StatusCode(StatusCodes.Status500InternalServerError,
                    new InitialAuthResponse
                    {
                        IsAuth = false,
                        StatusType = "error",
                        Message = "Email not sent"
                    });



                }


                var userRoles = await _userRepository.GetRoles(user);

                //generate a token and send to client
                var token = _tokenGenerator.GenerateJwtToken(user, _key, _expirationDate, user.UserName, user.Id, userRoles.ToList());

                if (token != null)
                {
                    var response = new FinalAuthResponse
                    {
                        IsAuth = true,
                        Is2fa = false,
                        Token = token,
                        StatusType = "success",
                        Message = "User succesfully logged in"
                    };



                    var admin = await _adminRepository.GetAdminByUser(user.Id);

                    if (admin != null)
                    {

                        //admin response
                        var adminUser = new AdminResponse
                        {
                            UserId = user.Id,
                            AdminId = admin.AdminId.ToString(),
                            FirstName = admin.FirstName,
                            LastName = admin.LastName
                        };

                        response.User = adminUser;


                        return Ok(response);
                    }

                }

                return Unauthorized(new InitialAuthResponse
                {
                    IsAuth = false,
                    StatusType = "error",
                    Message = "invalid credential"
                });

            }
            return Unauthorized(new InitialAuthResponse
            {
                IsAuth = false,
                StatusType = "error",
                Message = "invalid credential"
            });





        }



        [HttpPost("login-2FA")]
        public async Task<IActionResult> LoginWithOTP([FromBody] OtpRequest req)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(new InitialAuthResponse
                {
                    IsAuth = false,
                    StatusType = "error",
                    Message = "Bad request1"
                });
            }



            Console.WriteLine(req.Code);

            var user = await _userRepository.GetUserByEmail(req.Email);
            var signIn = await _userManager.VerifyTwoFactorTokenAsync(user, "Email", req.Code);



            if (signIn)
            {
                if (user != null)
                {
                    var userRoles = await _userRepository.GetRoles(user);

                    //generate a token and send to client
                    var result = _tokenGenerator.GenerateJwtToken(user, _key, _expirationDate, user.UserName, user.Id, userRoles.ToList());

                    if (result != null)
                    {
                        var response = new FinalAuthResponse
                        {
                            IsAuth = true,
                            Is2fa = false,
                            Token = result,
                            StatusType = "success",
                            Message = "User succesfully logged in",
                            Role = userRoles

                        };

                        var admin = await _adminRepository.GetAdminByUser(user.Id);



                        if (admin != null)
                        {

                            //admin response
                            var adminUser = new AdminResponse
                            {
                                UserId = user.Id,
                                AdminId = admin.AdminId.ToString(),
                                FirstName = admin.FirstName,
                                LastName = admin.LastName,
                            };

                            response.User = adminUser;


                            return Ok(response);
                        }

                

                    }

                    return Unauthorized(new InitialAuthResponse
                    {
                        IsAuth = false,
                        StatusType = "error",
                        Message = "invalid credential"
                    });

                }
                return Unauthorized(new InitialAuthResponse
                {
                    IsAuth = false,
                    StatusType = "error",
                    Message = "invalid credential"
                });

            }
            return Unauthorized(new InitialAuthResponse
            {
                IsAuth = false,
                StatusType = "error",
                Message = "invalid credential"
            });


        }



        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest req)
        {

            if (!ModelState.IsValid)
                return BadRequest();


            var user = await _userRepository.GetUserByEmail(req.Email);

            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);



                string plainText = "Your OTP";


                EmailSettings _mailSettings = new EmailSettings()
                {
                    Key = _apiKey,
                    Note = _note,
                    SenderEmail = _email

                };


                var emailSent = EmailService.SendEmail(user.Email, token, plainText, "OTP Confirmation", _mailSettings);

                if (emailSent != null)
                {
                    //return ok to client
                    return Ok(new InitialAuthResponse
                    {
                        IsAuth = true,
                        StatusType = "success",
                        Message = $"We have sent a password change OTP to your Email at {user.Email}"
                    });
                }

                return StatusCode(StatusCodes.Status500InternalServerError,
                new InitialAuthResponse
                {
                    IsAuth = false,
                    StatusType = "error",
                    Message = "Email not sent"
                });
            }

            return BadRequest("Invalid Request");
        }





        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest req)
        {

            if (!ModelState.IsValid)
                return BadRequest();

            var user = await _userRepository.GetUserByEmail(req.Email);

            if (user == null)
                return BadRequest("Invalid Request");

            var token = req.Token;
            var resetPassResult = await _userManager.ResetPasswordAsync(user, token, req.NewPassword);

            if (!resetPassResult.Succeeded)
            {
                var errors = resetPassResult.Errors.Select(e => e.Description);
                return StatusCode(StatusCodes.Status500InternalServerError, new InitialAuthResponse
                {
                    IsAuth = true,
                    StatusType = "error",
                    Message = String.Join(", ", errors)
                });
            }

            return Ok(new InitialAuthResponse
            {
                IsAuth = true,
                StatusType = "success",
                Message = "Password has been changed successfully"
            });
        }





        [HttpPost("admin/profile/{Id}/change-password")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin, Staff")]

        public async Task<IActionResult> ChangePassword(string Id, [FromBody] ChangePasswordRequest req)
        {


            var user = await _userRepository.GetUserById(Id.Trim());

            if (user == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new InitialAuthResponse
                {
                    IsAuth = true,
                    StatusType = "error",
                    Message = "user not found"
                });
            }

            if (string.Compare(req.NewPassword?.Trim(), req.ConfirmNewPassword?.Trim()) != 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new InitialAuthResponse
                {
                    IsAuth = true,
                    StatusType = "error",
                    Message = "The new password and confirm new password does not match"
                });
            }

            var changedResult = await _userManager.ChangePasswordAsync(user, req.CurrentPassword?.Trim(), req.NewPassword?.Trim());

            if (changedResult.Succeeded)
            {

                var response = new InitialAuthResponse
                {
                    IsAuth = true,
                    StatusType = "success",
                    Message = "Your Password has been successfully changed. Please try to sign back into your account",

                };



                return Ok(response);


            }

            return BadRequest(new InitialAuthResponse
            {
                IsAuth = true,
                StatusType = "success",
                Message = "Password change was unsuccessful"
            });




        }




        [HttpGet("logout")]
        public IActionResult Logout()
        {

            return Ok(new InitialAuthResponse()
            {
                IsAuth = false,
                StatusType = "error",
                Message = "User logged out"

            });
        }


        [HttpGet("roles")]
        public IActionResult GetRoles() { 
            var roles = _roleManager.Roles;
            return Ok(roles);
        }


        [HttpGet("secrets")]
        public IActionResult GetSecrets()
        {
            var secs = _secretService.GetSecrets();

            var cloud = GetInfo.GetASecret("Cloudinary-apiKey", secs);

            
            return Ok(cloud);

        }






    }
}
