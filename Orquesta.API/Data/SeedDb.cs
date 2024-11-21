using Orquesta.API.Data;
using Orquesta.API.Helpers;
using Orquesta.Shared.Entities;
using Orquesta.Shared.Enums;
using System;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using Azure.Identity;
using Azure;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace Orquesta.API.Data
{
    public class SeedDb
    {

        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckContratanteAsync();
            await CheckRoleAsync();
            await CheckUserAsync("1010", "Super", "Admin", "saralefay2010@gmail.com", "3015555555", UserType.Admin);
        }

        private async Task CheckRoleAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.UserContratante.ToString());
            await _userHelper.CheckRoleAsync(UserType.UserRepresentante.ToString());
            

        }

        private async Task<User> CheckUserAsync(string document, string firstName, string lastName, string email, string phone, UserType userType)
        {
            var user = await _userHelper.GetUserAsync(email);

            if (user == null)
            {

                user = new User
                {
                    Document = document,
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    PhoneNumber = phone,
                    UserName = email,
                    UserType = userType,
                };

                await _userHelper.AdduserAsync(user, "123456Sara");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());

                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);

            }
            return user;
        }

        private async Task CheckContratanteAsync()
        {
            if (!_context.Contratantes.Any())
            {

                _context.Contratantes.Add(new Contratante { Name = "Kevin", Email = "kevin@gmail.com", Document = "1231321", PhoneNumber = "3104827932" });
                _context.Contratantes.Add(new Contratante { Name = "Sara", Email = "sara@gmail.com", Document = "431231231", PhoneNumber = "3122446060" });
                _context.EstadoReservas.Add(new EstadoReserva { Estado = "En proceso" });
                _context.EstadoReservas.Add(new EstadoReserva { Estado = "Completada" });
                _context.EstadoReservas.Add(new EstadoReserva { Estado = "Cancelada" });

            }



            await _context.SaveChangesAsync();

        }

    }
}