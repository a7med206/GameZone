


namespace GameZone.ViewModels
{
    public class CreateGameFormViewModel: GameFormViewModel
    {
       
        [AllowedExtentions(FileSettings.AllowedExtensions),
            MaxFileSize(FileSettings.MaxFileSizeInBytes)]
        public IFormFile Cover { get; set; } = default!;
    }
}
