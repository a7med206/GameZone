

using GameZone.Models;
using GameZone.Services;

namespace GameZone.Controllers
{
    public class GamesController : Controller
    {
        private readonly ICategoriesService _categoriesService;
        private readonly IDevicesService _devicesService;
        private readonly IGameService _gamesService;

        public GamesController(ICategoriesService categoriesService,
            IDevicesService devicesService,
            IGameService gamesService)
        {
            _devicesService = devicesService;
            _categoriesService = categoriesService;
            _gamesService = gamesService;
        }




        public IActionResult Index()
        {
            var games = _gamesService.GetAll();
            return View(games);
        }

        public IActionResult Details(int id)
        {
            var game = _gamesService.GetById(id);
            if (game == null)
            {
                return NotFound();
            }
            return View(game);
        }

        [HttpGet]
        public IActionResult Create()
        {
            CreateGameFormViewModel viewModel = new()
            {
                Categories = _categoriesService.GetCategories(),
                Devices = _devicesService.GetDevices()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGameFormViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    model.Categories = _categoriesService.GetCategories();
                    model.Devices = _devicesService.GetDevices();
                    return View(model);

                }

                await _gamesService.Create(model);

                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }
}
