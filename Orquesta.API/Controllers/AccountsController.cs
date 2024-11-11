using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Orquesta.API.Helpers;
using Orquesta.Shared.DTOs;
using Orquesta.Shared.Enums;
using Orquesta.Shared.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using Orquesta.API.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using System.Diagnostics.Contracts;

namespace Orquesta.API.Controllers

{
    [ApiController]
    [Route("/api/accounts")]
    public class AccountsController : ControllerBase
    {
        private readonly IUserHelper _userHelper;
        private readonly IConfiguration _configuration;
        private readonly IFileStorage _fileStorage;
        private readonly string _container;
        private readonly IMailHelper _mailHelper;
        private readonly DataContext _context;
        private Contratante contratante = new Contratante();
        private Representante representante = new Representante();

        public AccountsController(IUserHelper userHelper, IConfiguration configuration, IFileStorage fileStorage, IMailHelper mailHelper, DataContext context)
        {
            _userHelper = userHelper;
            _configuration = configuration;
            _fileStorage = fileStorage;
            _container = "users";
            _context = context;
            _mailHelper = mailHelper;
        }

        [HttpGet("all")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> GetAll([FromQuery] UserDTO model)
        {
            var queryable = _context.Users
                .AsQueryable();

            return Ok(await queryable
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .ToListAsync());
        }

        [HttpGet("nombre")]
        public async Task<ActionResult> Get()
        {
            return Ok(await _context.Users.ToListAsync());
        }


        [HttpPost("CreateUser")]
        public async Task<ActionResult> CreateUser([FromBody] UserDTO model)
        {
            User user = model;
            if (!string.IsNullOrEmpty(model.Photo))
            {
                var photoUser = Convert.FromBase64String(model.Photo);
                model.Photo = await _fileStorage.SaveFileAsync(photoUser, ".jpg", _container);
            }



            var result = await _userHelper.AdduserAsync(user, model.Password);
            try
            {

                if (result.Succeeded && user.UserType == UserType.UserContratante)
                {
                   
                    contratante.Name = user.FullName;
                    contratante.Document = user.Document;
                    contratante.PhoneNumber = user.PhoneNumber;
                    contratante.Email = user.Email;
                    _context.Add(contratante);
                    await _context.SaveChangesAsync();

                    await _userHelper.AddUserToRoleAsync(user, user.UserType.ToString());
                    var myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                    var tokenLink = Url.Action("ConfirmEmail", "accounts", new
                    {
                        userid = user.Id,
                        token = myToken
                    }, HttpContext.Request.Scheme, _configuration["UrlWEB"]);

                    var response = _mailHelper.SendMail(user.FullName, user.Email!,
                        $"Orquestas- Confirmación de cuenta",
                        $"<h1>Orquesta - Confirmación de cuenta</h1>" +
                        $"<p>Para habilitar el usuario, por favor hacer clic 'Confirmar Email':</p>" +
                        $"<b><a href ={tokenLink}>Confirmar Email</a></b>");

                    if (response.IsSuccess)
                    {
                        return NoContent();
                    }

                    return BadRequest(response.Message);
                }

                else if (result.Succeeded && user.UserType == UserType.UserRepresentante)
                {

                    representante.Name = user.FullName;
                    representante.Document = user.Document;
                    representante.PhoneNumber = user.PhoneNumber;
                    representante.Email = user.Email;
                    _context.Add(representante);
                    await _context.SaveChangesAsync();

                    await _userHelper.AddUserToRoleAsync(user, user.UserType.ToString());
                    var myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                    var tokenLink = Url.Action("ConfirmEmail", "accounts", new
                    {
                        userid = user.Id,
                        token = myToken
                    }, HttpContext.Request.Scheme, _configuration["UrlWEB"]);

                    var response = _mailHelper.SendMail(user.FullName, user.Email!,
                        $"Orquestas- Confirmación de cuenta",
                        $"<h1>Orquesta - Confirmación de cuenta</h1>" +
                        $"<p>Para habilitar el usuario, por favor hacer clic 'Confirmar Email':</p>" +
                        $"<b><a href ={tokenLink}>Confirmar Email</a></b>");

                    if (response.IsSuccess)
                    {
                        return NoContent();
                    }

                    return BadRequest(response.Message);

                }
                    

                if (result.Errors.FirstOrDefault().Code.Contains(("Duplicate")))
                {
                    return BadRequest("There is already an account associated with this email");
                }
                else if (result.Errors.FirstOrDefault().Code.Contains(("Lower")))
                {

                    return BadRequest("Passwords must have at least one lowercase");
                }
                else if (result.Errors.FirstOrDefault().Code.Contains(("Upper")))
                {

                    return BadRequest("Passwords must have at least one uppercase");
                }
                return BadRequest(result.Errors.FirstOrDefault());
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException!.Message.Contains("duplicate"))
                {
                    return BadRequest("An account type with that name already exists.");
                }
                else
                {
                    return BadRequest(dbUpdateException.InnerException.Message);
                }
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }


         [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] LoginDTO model)
        {
            var result = await _userHelper.LoginAsync(model);
            if (result.Succeeded)
            {
                var user = await _userHelper.GetUserAsync(model.Email);
                return Ok(BuildToken(user));
            }


            if (result.IsLockedOut)
            {
                return BadRequest("Ha superado el máximo número de intentos, su cuenta está bloqueada, intente de nuevo en 5 minutos.");
            }

            if (result.IsNotAllowed)
            {
                return BadRequest("El usuario no ha sido habilitado, debes de seguir las instrucciones del correo enviado para poder habilitar el usuario.");
            }


            return BadRequest("Email o contraseña incorrectos.");
        }

		private TokenDTO BuildToken(User user)
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, user.Email!),
				new Claim(ClaimTypes.Role, user.UserType.ToString()),
				new Claim("Document", user.Document),
				new Claim("FirstName", user.FirstName),
				new Claim("LastName", user.LastName),
				new Claim("Photo", user.Photo ?? string.Empty)
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwtKey"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddDays(30);
            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiration,
                signingCredentials: credentials);

            return new TokenDTO
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Put(User user)
        {
            try
            {
                if (!string.IsNullOrEmpty(user.Photo))
                {
                    var photoUser = Convert.FromBase64String(user.Photo);
                    user.Photo = await _fileStorage.SaveFileAsync(photoUser, ".jpg", _container);
                }

                var currentUser = await _userHelper.GetUserAsync(user.Email!);
                if (currentUser == null)
                {
                    return NotFound();
                }

                currentUser.Document = user.Document;
                currentUser.FirstName = user.FirstName;
                currentUser.LastName = user.LastName;
                currentUser.PhoneNumber = user.PhoneNumber;
                currentUser.Photo = !string.IsNullOrEmpty(user.Photo) && user.Photo != currentUser.Photo ? user.Photo : currentUser.Photo;

                var result = await _userHelper.UpdateUserAsync(currentUser);
                if (result.Succeeded)
                {
                    return Ok(BuildToken(currentUser));
                }


                return BadRequest(result.Errors.FirstOrDefault());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> GetA()
        {
            return Ok(await _userHelper.GetUserAsync(User.Identity!.Name!));
        }

        [HttpPost("changePassword")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> ChangePasswordAsync(ChangePasswordDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userHelper.GetUserAsync(User.Identity!.Name!);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userHelper.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.FirstOrDefault()!.Description);
            }

            return NoContent();
        }

        [HttpGet("ConfirmEmail")]
        public async Task<ActionResult> ConfirmEmailAsync(string userId, string token)
        {
            token = token.Replace(" ", "+");
            var user = await _userHelper.GetUserAsync(new Guid(userId));
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userHelper.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.FirstOrDefault());
            }

            return NoContent();
        }
        [HttpPost("ResedToken")]
        public async Task<ActionResult> ResedToken([FromBody] EmailDTO model)
        {
            User user = await _userHelper.GetUserAsync(model.Email);
            if (user == null)
            {
                return NotFound();
            }

            var myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
            var tokenLink = Url.Action("ConfirmEmail", "accounts", new
            {
                userid = user.Id,
                token = myToken
            }, HttpContext.Request.Scheme, _configuration["UrlWEB"]);

            var response = _mailHelper.SendMail(user.FullName, user.Email!,
                $"Orquestas- Confirmación de cuenta",
                $"<h1>Orquesta - Confirmación de cuenta</h1>" +
                $"<p>Para habilitar el usuario, por favor hacer clic 'Confirmar Email':</p>" +
                $"<b><a href ={tokenLink}>Confirmar Email</a></b>");

            if (response.IsSuccess)
            {
                return NoContent();
            }

            return BadRequest(response.Message);
        }

        [HttpPost("RecoverPassword")]
        public async Task<ActionResult> RecoverPassword([FromBody] EmailDTO model)
        {
            User user = await _userHelper.GetUserAsync(model.Email);
            if (user == null)
            {
                return NotFound();
            }

            var myToken = await _userHelper.GeneratePasswordResetTokenAsync(user);
            var tokenLink = Url.Action("ResetPassword", "accounts", new
            {
                userid = user.Id,
                token = myToken
            }, HttpContext.Request.Scheme, _configuration["UrlWEB"]);

            var response = _mailHelper.SendMail(user.FullName, user.Email!,
                $"Orquestas - Recuperación de contraseña",
                $"<h1>Orquesta - Recuperación de contraseña</h1>" +
                $"<p>Para recuperar su contraseña, por favor hacer clic 'Recuperar Contraseña':</p>" +
                $"<b><a href ={tokenLink}>Recuperar Contraseña</a></b>");

            if (response.IsSuccess)
            {
                return NoContent();
            }

            return BadRequest(response.Message);
        }

        [HttpPost("ResetPassword")]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordDTO model)
        {
            User user = await _userHelper.GetUserAsync(model.Email);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userHelper.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                return NoContent();
            }

            return BadRequest(result.Errors.FirstOrDefault()!.Description);
        }

        [HttpPut("User")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Putadmin(User user)
        {
            try
            {
                if (!string.IsNullOrEmpty(user.Photo))
                {
                    var photoUser = Convert.FromBase64String(user.Photo);
                    user.Photo = await _fileStorage.SaveFileAsync(photoUser, ".jpg", _container);
                }

                var currentUser = await _userHelper.GetUserAsync(user.Email!);
                if (currentUser == null)
                {
                    return NotFound();
                }

                currentUser.Document = user.Document;
                currentUser.FirstName = user.FirstName;
                currentUser.LastName = user.LastName;
                currentUser.PhoneNumber = user.PhoneNumber;
                currentUser.Photo = !string.IsNullOrEmpty(user.Photo) && user.Photo != currentUser.Photo ? user.Photo : currentUser.Photo;
                currentUser.UserType = user.UserType;

                var result = await _userHelper.UpdateUserAsync(currentUser);
                if (result.Succeeded)
                {
                    return Ok(BuildToken(currentUser));
                }

                return BadRequest(result.Errors.FirstOrDefault());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}