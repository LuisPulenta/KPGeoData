using KPGeoData.API.Helpers;
using KPGeoData.Shared.Entities;
using KPGeoData.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using static System.Net.WebRequestMethods;

namespace KPGeoData.API.Data
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
            await CheckEventTypesAsync();
            await CheckCompaniesAsync();
            await CheckItemTypesAsync();
            await CheckStatesAsync();
            await CheckRolesAsync();
            await CheckUserAsync("1010", "Luis", "Núñez", "luis@yopmail.com", "351 681 4963", UserType.Admin);
            await CheckUserAsync("2020", "Pablo", "Lacuadri", "pablo@yopmail.com", "351 300 2255", UserType.Admin);
            await CheckUserAsync("3030", "Andrés", "Oberti", "andres@yopmail.com", "351 000 0000", UserType.UserWeb);
        }

        private async Task<User> CheckUserAsync(string document, string firstName, string lastName, string email, string phone, UserType userType)
        {
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Document = document,
                    Company = _context.Companies.FirstOrDefault(),
                    UserType = userType,
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());
            }

            return user;
        }

        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.UserWeb.ToString());
            await _userHelper.CheckRoleAsync(UserType.UserApp.ToString());
        }



        private async Task CheckCompaniesAsync()
        {
            var eventType = await _context.EventTypes.FirstOrDefaultAsync();

            if (!_context.Companies.Any())
            {
                _context.Companies.Add(new Company { Name = "KeyPress" ,Address="Villa Santa Ana",Phone="156123123",CUIT="20-12345678-0",Contact="Pablo Lacuadri",EMail="pablo@yopmail.com",Active=true});
                _context.Companies.Add(new Company { 
                    Name = "Solflix", 
                    Address = "Avda. Circunvalación 1990", 
                    Phone = "155666666", 
                    CUIT = "20-87654321-0", 
                    Contact = "Andrés Oberti", 
                    EMail = "andres@yopmail.com", 
                    Active = true,
                    Surveys = new List<Survey>
                    { 
                        new Survey{ 
                            Name = "Luminarias Barrio Rosedal",
                            Date = DateTime.Now,
                            Active = true,
                            Items = new List<Item>
                            {
                                new Item
                                {
                                    Name="Item 1",
                                    State="Roto",
                                    ItemType="Luminaria",
                                    Address="Aca 123",
                                    Locality="Córdoba",
                                    Country="Argentina",
                                    Latitude=-34.123,
                                    Longitude=-61.3456,
                                    Date=DateTime.Now,
                                    Remarks="Observaciones Item 1",
                                    Active=true,
                                    ItemPhotos=new List<ItemPhoto>
                                    {
                                        new ItemPhoto
                                        {
                                            EventType=eventType,
                                            Date=DateTime.Now,
                                            Remarks="Obs Foto 1",
                                            Photo="http//foto1.jpg"
                                        },new ItemPhoto
                                        {
                                            EventType=eventType,
                                            Date=DateTime.Now,
                                            Remarks="Obs Foto 2",
                                            Photo="http//foto2.jpg"
                                        }
                                    }
                                },
                                 new Item
                                {
                                    Name="Item 2",
                                    State="Roto",
                                    ItemType="Luminaria",
                                    Address="Allá 666",
                                    Locality="Córdoba",
                                    Country="Argentina",
                                    Latitude=-35.555,
                                    Longitude=-62.2226,
                                    Date=DateTime.Now,
                                    Remarks="Observaciones Item 2",
                                    Active=true,
                                    ItemPhotos=new List<ItemPhoto>
                                    {
                                        new ItemPhoto
                                        {
                                            EventType=eventType,
                                            Date=DateTime.Now,
                                            Remarks="Obs Foto 3",
                                            Photo="http//foto3.jpg"
                                        }
                                    }
                                }
                            }
                        },
                        new Survey{
                            Name = "Semáforos Ruta 20",
                            Date = DateTime.Now.AddDays(-5),
                            Active = true,
                        },
                        new Survey{
                            Name = "Postes EPEC B° Ate",
                            Date = DateTime.Now.AddDays(-15),
                            Active = true,
                        },
                    }
                
                });
                ;
            }
            await _context.SaveChangesAsync();
        }

        private async Task CheckItemTypesAsync()
        {
            if (!_context.ItemTypes.Any())
            {
                _context.ItemTypes.Add(new ItemType { Name = "Poste madera", Active = true });
                _context.ItemTypes.Add(new ItemType { Name = "Poste hormigón", Active = true });
                _context.ItemTypes.Add(new ItemType { Name = "Semáforo", Active = true });
                _context.ItemTypes.Add(new ItemType { Name = "Luminaria", Active = true });
                ;
            }
            await _context.SaveChangesAsync();
        }

        private async Task CheckStatesAsync()
        {
            if (!_context.States.Any())
            {
                _context.States.Add(new State { Name = "Averiado", Active = true });
                _context.States.Add(new State { Name = "Defectuoso", Active = true });
                _context.States.Add(new State { Name = "Nuevo", Active = true });
                _context.States.Add(new State { Name = "Destruído", Active = true });
            }
            await _context.SaveChangesAsync();
        }

        private async Task CheckEventTypesAsync()
        {
            if (!_context.EventTypes.Any())
            {
                _context.EventTypes.Add(new EventType { Name = "Antes", Active = true });
                _context.EventTypes.Add(new EventType { Name = "Después", Active = true });
                _context.EventTypes.Add(new EventType { Name = "Relevamiento", Active = true });
            }
            await _context.SaveChangesAsync();
        }
    }
}
