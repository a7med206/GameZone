
namespace GameZone.Services
{
    public class GameService : IGameService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _imagesPath;

        public GameService(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _imagesPath = $"{_webHostEnvironment.WebRootPath}{FileSettings.ImagesPath}";
        }

        public IEnumerable<Game> GetAll()
        {
            return _context.Games
                .Include(g => g.Category)
                .Include(g => g.Devices)
                .ThenInclude(gd => gd.Device)
                .AsNoTracking()
                .ToList();
        }

        public Game? GetById(int id)
        {
            return _context.Games
                .Include(g => g.Category)
                .Include(g => g.Devices)
                .ThenInclude(gd => gd.Device)
                .AsNoTracking()
                .SingleOrDefault(g => g.Id == id);
        }

        public async Task Create(CreateGameFormViewModel model)
        {
            var coverName = $"{Guid.NewGuid()}{Path.GetExtension(model.Cover.FileName)}";
            var path = Path.Combine(_imagesPath, coverName);

            using var stream = File.Create(path);
            await model.Cover.CopyToAsync(stream);

            Game game = new()
            {
                Name = model.Name,
                Description = model.Description,
                CategoryId = model.CategoryId,
                Cover = coverName,
                //Devices = model.SelectedDevices
                //.Select(d => new GameDevice { DeviceId = d })
                //.ToList()
            };
            _context.Add(game);
            _context.SaveChanges();

            game.Devices = model.SelectedDevices
                .Select(d => new GameDevice {GameId= game.Id, DeviceId = d })
                .ToList();
            _context.Update(game);
            _context.SaveChanges();
        }
    }
}
