using vsports.Models;

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
        Dictionary<string, List<string>> CreateGroups(List<string> teams, int groupCount);
        Dictionary<string, List<Tuple<string, string>>> GenerateGroupFixtures(Dictionary<string, List<string>> groups);
        Dictionary<string, List<string>> SimulateGroupStage(Dictionary<string, List<Tuple<string, string>>> groupFixtures);
        List<string> GetTopTeams(Dictionary<string, List<string>> groupResults);
        List<List<Tuple<string, string>>> GenerateKnockoutFixtures(List<string> topTeams);
        List<string> SimulateKnockoutStage(List<List<Tuple<string, string>>> knockoutFixtures);


        List<List<string>> GenerateGroupFixtures(List<string> teams, string groupName); // chuyền danh sách đội và bảng

        //tạo lịch thi đấu theo loại
        JsonResultVM CreateSchedule(int numTeams, int numBoard, int numRound, int seasionId, string typle);


    }
}
