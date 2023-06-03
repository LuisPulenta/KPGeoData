using KPGeoData.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using static System.Net.WebRequestMethods;

namespace KPGeoData.API.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;

        public SeedDb(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckEventTypesAsync();
            await CheckCompaniesAsync();
            await CheckItemTypesAsync();
            await CheckStatesAsync();
            
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
