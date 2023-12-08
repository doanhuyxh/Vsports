using System.Text;
using vsports.Data;

namespace vsports.Services
{
    public class Common : ICommon
    {
        private readonly IWebHostEnvironment _iHostingEnvironment;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public Common(IWebHostEnvironment iHostingEnvironment, ApplicationDbContext context, IConfiguration configuration)
        {
            _iHostingEnvironment = iHostingEnvironment;
            _context = context;
            _configuration = configuration;
        }

        public string RandomString(int length)
        {
            Random random = new Random();
            string CharSet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                int index = random.Next(CharSet.Length);
                result.Append(CharSet[index]);
            }
            return result.ToString();
        }

        public async Task<string> UploadImgAvatarAsync(IFormFile file)
        {
            string path = string.Empty;
            if (file != null)
            {
                string uploadsFolder = Path.Combine(_iHostingEnvironment.ContentRootPath, "wwwroot/upload/img_avatar");

                if (file.FileName == null)
                    path = "icon.png";
                else
                    path = DateTime.Now.Ticks.ToString() + Path.GetExtension(file.FileName);
                string filePath = Path.Combine(uploadsFolder, path);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
            return $"/upload/img_avatar/{path}";
        }

        public async Task<string> UploadImgBackgroudAsync(IFormFile file)
        {
            string path = string.Empty;
            if (file != null)
            {
                string uploadsFolder = Path.Combine(_iHostingEnvironment.ContentRootPath, "wwwroot/upload/img_backgroud");

                if (file.FileName == null)
                    path = "icon.png";
                else
                    path = DateTime.Now.Ticks.ToString() + Path.GetExtension(file.FileName);
                string filePath = Path.Combine(uploadsFolder, path);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
            return $"/upload/img_backgroud/{path}";
        }

        public async Task<string> UploadPdfAsync(IFormFile file)
        {
            string path = string.Empty;
            if (file != null)
            {
                string uploadsFolder = Path.Combine(_iHostingEnvironment.ContentRootPath, "wwwroot/upload/pdf");

                if (file.FileName == null)
                    path = "file.pdf";
                else
                    path = DateTime.Now.Ticks.ToString() + Path.GetExtension(file.FileName);
                string filePath = Path.Combine(uploadsFolder, path);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
            return $"/upload/pdf/{path}";
        }

        public string[] GenerateAlphabetArray(int n)
        {
            if (n < 1 || n > 26)
            {
                n = 1;
            }

            char startChar = 'A';
            string[] result = new string[n];

            for (int i = 0; i < n; i++)
            {
                result[i] = ((char)(startChar + i)).ToString();
            }

            return result;
        }

        public List<String> RotateTeams(List<string> teams)
        {

            string temp = teams[1];

            for (int i = 1; i < teams.Count - 1; i++)
            {
                teams[i] = teams[i + 1];
            }

            teams[teams.Count - 1] = temp;

            return teams;
        }

        public Dictionary<string, List<string>> CreateGroups(List<string> teams, int groupCount)
        {
            Dictionary<string, List<string>> groups = new Dictionary<string, List<string>>();

            int teamsPerGroup = teams.Count / groupCount;
            for (int i = 0; i < groupCount; i++)
            {
                string groupName = $"Group {Convert.ToChar('A' + i)}";
                groups[groupName] = new List<string>(teams.GetRange(i * teamsPerGroup, teamsPerGroup));
            }

            return groups;
        }

        public Dictionary<string, List<Tuple<string, string>>> GenerateGroupFixtures(Dictionary<string, List<string>> groups)
        {
            Dictionary<string, List<Tuple<string, string>>> groupFixtures = new Dictionary<string, List<Tuple<string, string>>>();

            foreach (var group in groups)
            {
                List<Tuple<string, string>> fixtures = new List<Tuple<string, string>>();
                List<string> teams = group.Value;

                for (int i = 0; i < teams.Count - 1; i++)
                {
                    for (int j = i + 1; j < teams.Count; j++)
                    {
                        fixtures.Add(new Tuple<string, string>(teams[i], teams[j]));
                    }
                }

                groupFixtures[group.Key] = fixtures;
            }

            return groupFixtures;
        }

        public Dictionary<string, List<string>> SimulateGroupStage(Dictionary<string, List<Tuple<string, string>>> groupFixtures)
        {
            Dictionary<string, List<string>> groupResults = new Dictionary<string, List<string>>();

            foreach (var group in groupFixtures)
            {
                List<string> groupTeams = new List<string> { group.Key };
                List<Tuple<string, string>> fixtures = group.Value;

                foreach (var fixture in fixtures)
                {
                    // Simulate match (replace with your logic)
                    string winner = (new Random().Next(2) == 0) ? fixture.Item1 : fixture.Item2;
                    groupTeams.Add(winner);
                }

                groupResults[group.Key] = groupTeams;
            }

            return groupResults;
        }

        public List<string> GetTopTeams(Dictionary<string, List<string>> groupResults)
        {
            List<string> topTeams = new List<string>();

            foreach (var group in groupResults)
            {
                List<string> sortedTeams = new List<string>(group.Value);
                sortedTeams.RemoveAt(0); // Remove group name
                sortedTeams.Sort();

                topTeams.Add(sortedTeams[sortedTeams.Count - 1]); // Get the top team from each group
            }

            return topTeams;
        }

        public List<List<Tuple<string, string>>> GenerateKnockoutFixtures(List<string> topTeams)
        {
            List<List<Tuple<string, string>>> knockoutFixtures = new List<List<Tuple<string, string>>>();

            int teamsCount = topTeams.Count;
            while (teamsCount > 1)
            {
                List<Tuple<string, string>> fixture = new List<Tuple<string, string>>();
                for (int i = 0; i < teamsCount; i += 2)
                {
                    if (i + 1 < teamsCount)
                    {
                        fixture.Add(new Tuple<string, string>(topTeams[i], topTeams[i + 1]));
                    }
                    else
                    {
                        // Handle odd number of teams, e.g., give a bye to one team
                        fixture.Add(new Tuple<string, string>(topTeams[i], "BYE"));
                    }
                }
                knockoutFixtures.Add(fixture);
                topTeams = SimulateKnockoutRound(fixture);
                teamsCount = topTeams.Count;
            }

            return knockoutFixtures;
        }

        public List<string> SimulateKnockoutStage(List<List<Tuple<string, string>>> knockoutFixtures)
        {
            List<string> knockoutResults = new List<string>();

            foreach (var round in knockoutFixtures)
            {
                foreach (var match in round)
                {
                    // Simulate match (replace with your logic)
                    string winner = (new Random().Next(2) == 0) ? match.Item1 : match.Item2;
                    knockoutResults.Add(winner);
                }
            }

            return knockoutResults;
        }

        public List<string> SimulateKnockoutRound(List<Tuple<string, string>> fixture)
        {
            List<string> winners = new List<string>();

            foreach (var match in fixture)
            {
                // Simulate match (replace with your logic)
                string winner = (new Random().Next(2) == 0) ? match.Item1 : match.Item2;
                winners.Add(winner);
            }

            return winners;
        }
    }
}
