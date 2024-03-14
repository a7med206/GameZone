namespace GameZone.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly ApplicationDbContext _context;

        public CategoriesService(ApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<SelectListItem> GetCategories()
        {
            return _context.Categories
                   .Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() })
                   .OrderBy(c => c.Text)
                   .AsNoTracking()
                   .ToList();
        }
    }
}
