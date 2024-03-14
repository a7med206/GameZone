using System.Diagnostics;

namespace GameZone.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGameService _gamesService;

        public HomeController(ILogger<HomeController> logger,
            IGameService gameService)
        {
            _logger = logger;
            _gamesService = gameService;
        }

        public IActionResult Index()
        {
            var games = _gamesService.GetAll();
            return View(games);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}