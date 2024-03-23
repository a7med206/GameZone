
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
            var coverName = await SaveCover(model.Cover);

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
                .Select(d => new GameDevice { GameId = game.Id, DeviceId = d })
                .ToList();
            _context.Update(game);
            _context.SaveChanges();
        }

        public async Task<Game?> Edit(EditGameFormViewModel model)
        {
            var game = _context.Games
                .Include(g => g.Devices)
                .SingleOrDefault(g => g.Id == model.Id);
            if (game == null)
            {
                return null;
            }

            game.Name = model.Name;
            game.Description = model.Description;
            game.CategoryId = model.CategoryId;
            game.Devices = model.SelectedDevices
                .Select(d => new GameDevice { DeviceId = d })
                .ToList();

            var hasNewCover = model.Cover != null;
            var oldCoverPath = game.Cover;

            if (hasNewCover)
            {
                var coverName = await SaveCover(model.Cover);

                game.Cover = coverName;
            }

            var affectedRows = _context.SaveChanges();
            if (affectedRows > 0)
            {
                if (hasNewCover)
                {
                    var oldCoverFullPath = Path.Combine(_imagesPath, oldCoverPath);
                    File.Delete(oldCoverFullPath);
                }
                return game;
            }
            else
            {
                var coverFullPath = Path.Combine(_imagesPath, game.Cover);
                File.Delete(coverFullPath);
                return null;
            }
        }

        private async Task<string> SaveCover(IFormFile cover)
        {
            var coverName = $"{Guid.NewGuid()}{Path.GetExtension(cover.FileName)}";
            var path = Path.Combine(_imagesPath, coverName);

            using var stream = File.Create(path);
            await cover.CopyToAsync(stream);

            return coverName;
        }


        public bool Delete(int id)
        {
            var isDeleted = false;
            var game = _context.Games.Find(id);
            if (game == null)
            {
                return isDeleted;
            }

            _context.Remove(game);
            var effectedRows = _context.SaveChanges();
            if (effectedRows > 0)
            {
                isDeleted = true;
            var coverFullPath = Path.Combine(_imagesPath, game.Cover);
            File.Delete(coverFullPath);
            }

            return isDeleted;
        }
    }
}
