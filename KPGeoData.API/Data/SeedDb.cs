using KPGeoData.Shared.Entities;

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
            await CheckCountriesAsync();
            await CheckItemTypesAsync();
            await CheckStatesAsync();
            await CheckEventTypesAsync();
        }

        private async Task CheckCountriesAsync()
        {
            if (!_context.Companies.Any())
            {
                _context.Companies.Add(new Company { Name = "KeyPress" ,Address="Villa Santa Ana",Phone="156123123",CUIT="20-12345678-0",Contact="Pablo Lacuadri",EMail="pablo@yopmail.com",Active=true});
                _context.Companies.Add(new Company { Name = "Solflix", Address = "Avda. Circunvalación 1990", Phone = "155666666", CUIT = "20-87654321-0", Contact = "Andrés Oberti", EMail = "andres@yopmail.com", Active = true });
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
