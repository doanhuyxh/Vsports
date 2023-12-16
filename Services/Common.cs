using Bogus;
using System.Text;
using vsports.Data;
using vsports.Models;

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

            //string temp = teams[1];

            //for (int i = 1; i < teams.Count - 1; i++)
            //{
            //    teams[i] = teams[i + 1];
            //}

            //teams[teams.Count - 1] = temp;

            //return teams;
            string temp = teams[teams.Count - 1]; // Lưu giữ phần tử cuối cùng trong danh sách

            for (int i = teams.Count - 1; i > 0; i--)
            {
                teams[i] = teams[i - 1]; // Di chuyển từ phần tử cuối về đầu danh sách
            }

            teams[0] = temp; // Gán giá trị cuối cùng vào phần tử đầu tiên

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

        public List<List<string>> GenerateGroupFixtures(List<string> teams, string groupName)
        {
            List<List<string>> fixtures = new List<List<string>>();

            for (int i = 0; i < teams.Count - 1; i++)
            {
                List<string> roundFixtures = new List<string>();

                for (int j = i + 1; j < teams.Count; j++)
                {
                    string fixture = $"{teams[i]} vs {teams[j]}";
                    roundFixtures.Add(fixture);
                }

                fixtures.Add(roundFixtures);
            }

            return fixtures;

        }

        public string GetRoundName(int round)
        {
            switch (round)
            {
                case 1:
                    return "Loại";
                case 2:
                    return "Tứ Kết";
                case 3:
                    return "Bán Kết";
                case 4:
                    return "Chung Kết";
                case 5:
                    return "Đặc biệt";
                default:
                    return $"Vòng {round}";
            }
        }

        public async Task<JsonResultVM> CreateSchedule(int numTeams, int numBoard, int numRound, int seasionId, string typle)
        {
            JsonResultVM json = new JsonResultVM();
            json.StatusCode = 200;
            json.Message = "";
            switch (typle)
            {
                #region TheoVong
                case "Theo vòng":
                    List<MatchScheduleAndResults> MatchScheduleAndResults = new List<MatchScheduleAndResults>();
                    int TongVong = numTeams - 1;
                    List<String> name = new List<string>();
                    for (int i = 1; i <= numTeams; i++)
                    {
                        name.Add("Team " + i);
                    }

                    //số lượt gặp nhập
                    for (int i = 1; i <= numRound; i++)
                    {
                        // số vòng đấu
                        for (int j = 1; j <= TongVong * i; j++)
                        {
                            Round round = new Round();
                            round.SeasonOnTournamentId = seasionId;
                            round.RoundName = $"Vòng {j}";
                            round.Created = DateTime.Now;
                            round.IsDelete = false;
                            _context.Add(round);
                            await _context.SaveChangesAsync();

                            Board board3 = new Board();
                            board3.Id = 0;
                            board3.RoundId = round.Id;
                            board3.Name = "A";
                            board3.Created = DateTime.Now;
                            board3.IsDelete = false;
                            _context.Add(board3);
                            await _context.SaveChangesAsync();


                            // thêm lịch
                            for (int k = 0; k < numTeams / 2; k++)
                            {
                                MatchScheduleAndResults.Add(new MatchScheduleAndResults()
                                {
                                    RoundId = round.Id,
                                    BoardId = board3.Id,
                                    Created = DateTime.Now,
                                    SportClub1_Name = name[k],
                                    SportClub2_Name = name[name.Count - 1 - k],
                                    SportClubId_1 = 0,
                                    SportClubId_2 = 0,
                                    SeasonOnTournamentId = seasionId,
                                    Schedule = DateTime.Now,
                                    Status = "Pennding",
                                    Winner = "",
                                    IsDelete = false,
                                });

                            }
                            name = RotateTeams(name);
                        }
                    }

                    _context.MatchScheduleAndResults.AddRange(MatchScheduleAndResults);
                    await _context.SaveChangesAsync();

                    break;
                #endregion

                #region ChiaBang
                case "Chia bảng":
                    string[] board = GenerateAlphabetArray(numBoard);
                    List<MatchScheduleAndResults> schedules = new List<MatchScheduleAndResults>();
                    int totalRound = numTeams / 2 - 1;

                    if (totalRound <= 0)
                    {
                        totalRound = 1;
                    }

                    //lặp qua các bảng
                    for (int a = 0; a < numBoard; a++)
                    {

                        // tạo tên
                        List<String> name2 = new List<String>();
                        for (int j = 1; j < board.Length; j++)
                        {
                            name2.Add($"Team {board[a]}_{j}");
                        }

                        //lặp qua các phần tử của bảng
                        for (int i = 0; i < totalRound / board.Length; i++)
                        {
                            Round r = new Round();
                            r.Created = DateTime.Now;
                            r.IsDelete = false;
                            r.RoundName = "Vòng " + i;
                            r.SeasonOnTournamentId = seasionId;

                            _context.Add(r);
                            await _context.SaveChangesAsync();


                            Board b = new Board();
                            b.Name = board[a];
                            b.IsDelete = false;
                            b.Created = DateTime.Now;
                            b.RoundId = r.Id;
                            _context.Add(b);
                            await _context.SaveChangesAsync();



                            for (int k = 0; k < board.Length; k++)
                            {
                                schedules.Add(new MatchScheduleAndResults
                                {
                                    Schedule = DateTime.Now,
                                    Status = "Pendding",
                                    IsDelete = false,
                                    BoardId = b.Id,
                                    Winner = "0 - 0",
                                    RoundId = r.Id,
                                    SeasonOnTournamentId = seasionId,
                                    SportClubId_1 = 0,
                                    SportClubId_2 = 0,
                                    SportClub1_Name = board[k],
                                    SportClub2_Name = board[board.Count() - k - 1],
                                });
                            }

                            name2 = RotateTeams(name2);

                        }
                        name2.Clear();
                    }

                    _context.MatchScheduleAndResults.AddRange(schedules);
                    await _context.SaveChangesAsync();


                    break;
                #endregion

                #region VongLoaiTrucTiepTheoBang
                case "Vòng loại trực tiếp theo bảng":


                    //tạo tên team
                    List<string> teams = new List<string>();
                    for (int i = 1; i <= numTeams; i++)
                    {
                        teams.Add("Team " + i);
                    }
                    //tạo bảng
                    Dictionary<string, List<string>> boardV = CreateGroups(teams, numBoard);

                    // tạo trận đấu vòng loại theo bảng
                    Dictionary<string, List<Tuple<string, string>>> boardFixtures = GenerateGroupFixtures(boardV);


                    // chèn dữ liệu vòng loại
                    // tạo vòng loại
                    Round vong1 = new Round();
                    vong1.RoundName = "Vòng 1";
                    vong1.Created = DateTime.Now;
                    vong1.IsDelete = false;
                    vong1.SeasonOnTournamentId = seasionId;

                    _context.Add(vong1);
                    _context.SaveChanges();

                    // lặp qua các bảng
                    foreach (var item in boardFixtures)
                    {
                        // tạo bảng
                        Board board1 = new Board();
                        board1.Name = item.Key;
                        board1.RoundId = vong1.Id;
                        board1.Created = DateTime.Now;
                        board1.IsDelete = false;
                        _context.Add(board1);
                        await _context.SaveChangesAsync();

                        // chèn lịch thi đâu theo bảng
                        foreach (var fixture in item.Value)
                        {
                            MatchScheduleAndResults schedule = new MatchScheduleAndResults();
                            schedule.SportClubId_1 = 0;
                            schedule.SportClubId_2 = 0;
                            schedule.SportClub1_Name = fixture.Item1;
                            schedule.SportClub2_Name = fixture.Item2;
                            schedule.BoardId = board1.Id;
                            schedule.RoundId = vong1.Id;
                            schedule.IsDelete = false;
                            schedule.Winner = "";
                            schedule.Schedule = DateTime.Now;
                            schedule.SeasonOnTournamentId = seasionId;
                            schedule.Status = "Pending";
                            _context.Add(schedule);
                            await _context.SaveChangesAsync();

                        }
                    }

                    //tạo đôi chiến thắng ngẫu nhiên sau vòng loại
                    Dictionary<string, List<string>> boardResults = SimulateGroupStage(boardFixtures);

                    //lấy ngẫu nhiên đội thắng
                    List<string> topTeams = GetTopTeams(boardResults);

                    // tự đánh vòng sau loại
                    //tạo vòng bán kết
                    List<List<Tuple<string, string>>> knockoutFixtures = GenerateKnockoutFixtures(topTeams);



                    // chèn dữ liệu vòng bán kết
                    foreach (var round in knockoutFixtures)
                    {

                        Round round2 = new Round();
                        round2.RoundName = "Chung kết";
                        round2.Created = DateTime.Now;
                        round2.IsDelete = false;
                        round2.SeasonOnTournamentId = seasionId;

                        _context.Add(round2);
                        _context.SaveChanges();
                        Board board2 = new Board();
                        board2.Name = "Sau vòng bảng";
                        board2.IsDelete = false;
                        board2.Created = DateTime.Now;
                        board2.RoundId = round2.Id;
                        _context.Add(board2);
                        await _context.SaveChangesAsync();

                        foreach (var match in round)
                        {
                            MatchScheduleAndResults schedule = new MatchScheduleAndResults();
                            schedule.SportClubId_1 = 0;
                            schedule.SportClubId_2 = 0;
                            schedule.SportClub1_Name = match.Item1;
                            schedule.SportClub2_Name = match.Item2;
                            schedule.BoardId = board2.Id;
                            schedule.RoundId = round2.Id;
                            schedule.IsDelete = false;
                            schedule.Winner = "";
                            schedule.Schedule = DateTime.Now;
                            schedule.SeasonOnTournamentId = seasionId;
                            schedule.Status = "Pending";
                            _context.Add(schedule);
                            await _context.SaveChangesAsync();

                        }
                    }


                    break;
                #endregion

                #region HonHop
                case "Hỗn Hợp":
                    //tạo team
                    List<string> teamsH = new List<string>();
                    for (int i = 1; i <= numTeams; i++)
                    {
                        teamsH.Add($"Team{i}");
                    }

                    // tạo trận đấu loại 2 bảng
                    List<List<string>> groupAFixtures = GenerateGroupFixtures(teamsH.Take(numTeams / 2).ToList(), "Bảng A");
                    List<List<string>> groupBFixtures = GenerateGroupFixtures(teamsH.Skip(numTeams / 2).ToList(), "Bảng B");

                    // chèn lịch sử đấu vòng loại

                    // - tạo số lượng vòng đấu
                    List<Round> rounds = new List<Round>();
                    for (int i = 1; i <= groupAFixtures.Count; i++)
                    {
                        Round round = new Round();
                        round.IsDelete = false;
                        round.RoundName = $"Vòng {i}";
                        round.SeasonOnTournamentId = seasionId;
                        _context.Add(round);
                        _context.SaveChanges();
                        rounds.Add(round);
                    }

                    for (int i = 0; i < rounds.Count - 1; i++)
                     {
                        // chèn lịch đáu vòng loại bảng A
                        Board a = new Board();
                        a.Created = DateTime.Now;
                        a.RoundId = rounds[i].Id;
                        a.Name = "Bảng A";
                        a.IsDelete = false;

                        _context.Add(a);
                        _context.SaveChanges();
                        for (int j = 0; j < groupAFixtures[i].Count; j++)
                        {
                            MatchScheduleAndResults match1 = new MatchScheduleAndResults();
                            match1.BoardId = a.Id;
                            match1.RoundId = rounds[i].Id;
                            match1.Created = DateTime.Now;
                            match1.IsDelete = false;
                            match1.SeasonOnTournamentId = seasionId;
                            match1.SportClubId_2 = 0;
                            match1.SportClubId_1 = 0;
                            match1.SportClub1_Name = groupAFixtures[i][j].Split(' ')[0];
                            match1.SportClub2_Name = groupAFixtures[i][j].Split(' ')[2];
                            match1.Winner = "";
                            match1.Status = "Pending";
                            match1.Schedule = DateTime.Now;
                            _context.Add(match1);
                            _context.SaveChanges();


                        }

                        // chèn lịch sử đâu vòng loại bảng B

                        Board b = new Board();
                        b.Created = DateTime.Now;
                        b.RoundId = rounds[i].Id;
                        b.Name = "Bảng B";
                        b.IsDelete = false;
                        _context.Add(b);
                         _context.SaveChanges();

                        for (int j = 0; j < groupBFixtures[i].Count; j++)
                        {
                            MatchScheduleAndResults match2 = new MatchScheduleAndResults();
                            match2.BoardId = a.Id;
                            match2.RoundId = rounds[i].Id;
                            match2.Created = DateTime.Now;
                            match2.IsDelete = false;
                            match2.SeasonOnTournamentId = seasionId;
                            match2.SportClubId_2 = 0;
                            match2.SportClubId_1 = 0;
                            match2.SportClub1_Name = groupBFixtures[i][j].Split(' ')[0];
                            match2.SportClub2_Name = groupBFixtures[i][j].Split(' ')[2];
                            match2.Winner = "";
                            match2.Status = "Pending";
                            match2.Schedule = DateTime.Now;
                            _context.Add(match2);
                            _context.SaveChanges();

                        }

                    }

                    // chèn lịch sử đâu vòng bán kết
                    Round roundn = new Round();
                    roundn.Created = DateTime.Now;
                    roundn.IsDelete = false;
                    roundn.RoundName = "Vòng bán kết";
                    roundn.SeasonOnTournamentId = seasionId;
                    _context.Add(roundn);
                    _context.SaveChanges();



                    // tạo bảng ảo cho vòng này
                    Board boardn = new Board();
                    boardn.Created = DateTime.Now;
                    boardn.IsDelete = false;
                    boardn.RoundId = roundn.Id;
                    boardn.Name = "Bảng vòng bán kết";
                    _context.Add(boardn);
                    _context.SaveChanges();



                    // chèn 2 trận cho 2 đội nhất và nhì mỗi bảng gặp nhau
                    for (int i = 0; i < 2; i++)
                    {
                        MatchScheduleAndResults matchScheduleAndResults = new MatchScheduleAndResults();
                        matchScheduleAndResults.BoardId = boardn.Id;
                        matchScheduleAndResults.RoundId = roundn.Id;
                        matchScheduleAndResults.Created = DateTime.Now;
                        matchScheduleAndResults.IsDelete = false;
                        matchScheduleAndResults.SeasonOnTournamentId = seasionId;
                        matchScheduleAndResults.Winner = "";
                        matchScheduleAndResults.Status = "Pending";
                        matchScheduleAndResults.Schedule = DateTime.Now;
                        matchScheduleAndResults.SportClubId_2 = 0;
                        matchScheduleAndResults.SportClubId_1 = 0;
                        if (i == 0)
                        {
                            matchScheduleAndResults.SportClub1_Name = "Đội nhất bảng A";
                            matchScheduleAndResults.SportClub2_Name = "Đội nhì bảng B";
                        }
                        else
                        {
                            matchScheduleAndResults.SportClub1_Name = "Đội nhì bảng A";
                            matchScheduleAndResults.SportClub2_Name = "Đội nhất bảng B";
                        }
                        _context.Add(matchScheduleAndResults);
                        _context.SaveChanges();


                    }

                    // chèn lịch sử đâu vòng bán kết
                    Round ck = new Round();
                    ck.Created = DateTime.Now;
                    ck.IsDelete = false;
                    ck.RoundName = "Vòng chung kết";
                    ck.SeasonOnTournamentId = seasionId;
                    _context.Add(ck);
                    _context.SaveChanges();



                    // tạo bảng ảo cho vòng này
                    Board ckboard = new Board();
                    ckboard.Created = DateTime.Now;
                    ckboard.IsDelete = false;
                    ckboard.RoundId = ck.Id;
                    ckboard.Name = "Bảng chung kết";
                    _context.Add(ckboard);
                    _context.SaveChanges();

                    // tạo trận chung kết
                    MatchScheduleAndResults matchSchedule = new MatchScheduleAndResults();
                    matchSchedule.BoardId = ckboard.Id;
                    matchSchedule.RoundId = ck.Id;
                    matchSchedule.Created = DateTime.Now;
                    matchSchedule.IsDelete = false;
                    matchSchedule.SeasonOnTournamentId = seasionId;
                    matchSchedule.SportClubId_2 = 0;
                    matchSchedule.SportClubId_1 = 0;
                    matchSchedule.SportClub1_Name = "Đội thắng trận bán kết 1";
                    matchSchedule.SportClub2_Name = "Đội thắng trận bán kết 2";
                    matchSchedule.Winner = "";
                    matchSchedule.Status = "Pending";
                    matchSchedule.Schedule = DateTime.Now;
                    _context.Add(matchSchedule);
                    _context.SaveChanges();


                    break;
                #endregion

                #region VongLoaiTrucTiep
                case "Vòng loại trực tiếp":


                    List<String> nameVL = new List<string>();
                    for (int i = 1; i <= numTeams; i++)
                    {
                        nameVL.Add("Team " + i);
                    }

                    // vòng loại 1/8
                    

                    // lặp qua từng vòng
                    int rount = 1;
                    while (nameVL.Count > 1)
                    {

                        Round vong18 = new Round();
                        vong18.Created = DateTime.Now;
                        vong18.IsDelete = false;
                        vong18.SeasonOnTournamentId = seasionId;
                        vong18.RoundName = GetRoundName(rount);
                        _context.Add(vong18);
                        _context.SaveChanges();

                        Board bangAo1 = new Board();
                        bangAo1.Created = DateTime.Now;
                        bangAo1.IsDelete = false;
                        bangAo1.RoundId = vong18.Id;
                        bangAo1.Name = "A";
                        _context.Add(bangAo1);
                        _context.SaveChanges();

                        // thêm lịch thi đấu
                        int matchCount = nameVL.Count / 2;
                        for (int i = 0; i < matchCount; i++)
                        {
                            Console.WriteLine($"{nameVL[i]} vs {nameVL[nameVL.Count - i - 1]}");

                            MatchScheduleAndResults match2 = new MatchScheduleAndResults();
                            match2.BoardId = bangAo1.Id;
                            match2.RoundId = vong18.Id;
                            match2.Created = DateTime.Now;
                            match2.IsDelete = false;
                            match2.SeasonOnTournamentId = seasionId;
                            match2.SportClubId_2 = 0;
                            match2.SportClubId_1 = 0;
                            match2.SportClub1_Name = nameVL[i];
                            match2.SportClub2_Name = nameVL[nameVL.Count - i - 1];
                            match2.Winner = "";
                            match2.Status = "Pending";
                            match2.Schedule = DateTime.Now;
                            _context.Add(match2);
                            await _context.SaveChangesAsync();
                        }

                        //Cập nhật danh sách đội cho vòng kế tiếp
                        List<string> winners = new List<string>();
                        int matchCountNextRound = nameVL.Count / 2;
                        for (int i = 0; i < matchCountNextRound; i++)
                        {
                            winners.Add((string.Compare(nameVL[i], nameVL[nameVL.Count - i - 1]) < 0) ? nameVL[i] : nameVL[nameVL.Count - i - 1]);
                        }

                        // Kiểm tra số lẻ và tự động cho một đội vào vòng tiếp theo mà không cần thi đấu
                        if (nameVL.Count % 2 != 0)
                        {
                            Console.WriteLine($"{nameVL[nameVL.Count / 2]} sẽ đi tiếp mà không cần thi đấu");
                            winners.Add(nameVL[nameVL.Count / 2]);
                        }

                        nameVL = winners;
                        rount++;
                    }



                    break;
                    #endregion


            }

            return json;
        }
    }
}
