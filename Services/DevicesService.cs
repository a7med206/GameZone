namespace GameZone.Services
{
    public class DevicesService : IDevicesService
    {
        private readonly ApplicationDbContext _context;
        public DevicesService(ApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<SelectListItem> GetDevices()
        {
            return _context.Devices
                   .Select(d => new SelectListItem { Text = d.Name, Value = d.Id.ToString() })
                   .OrderBy(d => d.Text)
                   .AsNoTracking()
                   .ToList();
        }
    }
}
