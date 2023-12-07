namespace vsports.Services
{
    public interface ICommon
    {
        Task<string> UploadImgAvatarAsync(IFormFile file);
        Task<string> UploadImgBackgroudAsync(IFormFile file);
        Task<string> UploadPdfAsync(IFormFile file);
        string RandomString(int length);
        string[] GenerateAlphabetArray(int length);
        List<String> RotateTeams(List<string> teams);
    }
}
