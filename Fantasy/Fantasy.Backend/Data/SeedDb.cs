using Fantasy.Backend.Helpers;
using Fantasy.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fantasy.Backend.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IFilesStorage _filesStorage;

        public SeedDb(DataContext context, IFilesStorage filesStorage)
        {
            _context = context;
            _filesStorage = filesStorage;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckCountriesAsync();
            await CheckTeamsAsync();
        }

        private async Task CheckCountriesAsync()
        {
            if (!_context.Countries.Any())
            {
                var countriesSQLScript = File.ReadAllText("Data\\Countries.sql");
                await _context.Database.ExecuteSqlRawAsync(countriesSQLScript);
            }
        }

        private async Task CheckTeamsAsync()
        {
            if (!_context.Teams.Any())
            {
                foreach (var country in _context.Countries)
                {
                    var imagePath = string.Empty;
                    var filePath = $"{Environment.CurrentDirectory}\\Images\\Flags\\{country.Name}.png";
                    if (File.Exists(filePath))
                    {
                        var fileBytes = File.ReadAllBytes(filePath);
                        using var imageStream = new MemoryStream(fileBytes);
                        imagePath = await _filesStorage.SaveFileAsync(imageStream, country.Name);
                    }
                    _context.Teams.Add(new Team { Name = country.Name, Country = country!, Image = imagePath });
                }

                await _context.SaveChangesAsync();
            }
        }
    }
}