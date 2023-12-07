using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using vsports.Models;
using vsports.Models.TournamentsVM;
using vsports.Data;
using Bogus;
using vsports.Models.MatchScheduleAndResultsVM;
using vsports.Models.SeasonOnTournamentsVM;
using vsports.Services;
using Microsoft.EntityFrameworkCore.Metadata;

namespace vTournamentss.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TournamentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _memory;
        private readonly ICommon _icommon;

        public TournamentsController(ApplicationDbContext context, IMemoryCache memory, ICommon common)
        {
            _context = context;
            _memory = memory;
            _icommon = common;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetData()
        {
            JsonResultVM json = new JsonResultVM();
            json.StatusCode = 200;
            List<TournamentsVM> tournaments = _context.Tournaments
                                              .Include(t => t.Organizer)
                                               .Include(t => t.Sport)
                                               .Where(t => t.IsDelete == false)
                                               .Select(t => new TournamentsVM
                                               {
                                                   Id = t.Id,
                                                   SportId = t.SportId,
                                                   UserId = t.UserId,
                                                   Name = t.Name,
                                                   SportName = t.Sport.Name,
                                                   OrganizerName = t.Organizer.FullName,
                                               }).ToList();

            json.Data = tournaments;
            return Ok(json);
        }

        public async Task<IActionResult> AddEditData(int id = 0)
        {
            TournamentsVM TournamentsVM = new TournamentsVM();
            if (id != 0)
            {
                TournamentsVM = await _context.Tournaments.FirstOrDefaultAsync(x => x.Id == id);
            }

            return PartialView("AddEditData", TournamentsVM);
        }

        [HttpPost]
        public async Task<IActionResult> SaveData(TournamentsVM vm)
        {
            JsonResultVM json = new JsonResultVM();
            try
            {
                Tournaments Tournaments = new Tournaments();
                if (vm.Id != 0)
                {
                    Tournaments = await _context.Tournaments.FirstOrDefaultAsync(i => i.Id == vm.Id);
                    Tournaments.Name = vm.Name;
                    _context.Update(Tournaments);
                    _context.SaveChanges();
                }
                else
                {
                    Tournaments = vm;
                    Tournaments.Created = DateTime.Now;
                    Tournaments.IsDelete = false;
                    _context.Add(Tournaments);
                    _context.SaveChanges();

                    //tạo độ mặc định tham gia giải đấu                    
                    SportClub club = _context.SportClub.FirstOrDefault(i => i.Id == 3);
                    SportClubOnTournaments clubOnTournaments = new SportClubOnTournaments();
                    clubOnTournaments.SportClubId = club.Id;
                    clubOnTournaments.TournamentsId = Tournaments.Id;
                    clubOnTournaments.Created = DateTime.Now;
                    clubOnTournaments.Status = "Accepted";
                    clubOnTournaments.Point = 0;
                    clubOnTournaments.IsDelete = false;

                }
                json.Data = Tournaments;
                json.Message = "";
                json.StatusCode = 200;

                return Ok(json);
            }
            catch (Exception ex)
            {
                json.Data = vm;
                json.Message = ex.Message;
                json.StatusCode = 500;
                return Ok(json);
            }
        }

        public async Task<IActionResult> DeleteData(int id)
        {
            JsonResultVM json = new JsonResultVM();
            Tournaments Tournaments = await _context.Tournaments.FirstOrDefaultAsync(i => i.Id == id);
            Tournaments.IsDelete = true;
            _context.Update(Tournaments);
            _context.SaveChanges();
            json.StatusCode = 200;
            return Ok(json);
        }

        public IActionResult Detail(int id)
        {
            return PartialView("Detail");
        }

        public IActionResult AddFakeData()
        {
            List<ApplicationUser> users = _context.Users.ToList();

            foreach (var user in users)
            {
                Tournaments tournaments = new Faker<Tournaments>()
                                               .RuleFor(t => t.IsDelete, f => false)
                                               .RuleFor(t => t.Created, DateTime.Now)
                                               .RuleFor(t => t.Name, f => f.Lorem.Lines(1))
                                               .RuleFor(t => t.UserId, user.Id)
                                               .RuleFor(t => t.AvatarImage, "/upload/img_avatar/blank_avatar.png")
                                               .RuleFor(t => t.BackgroudImage, "/upload/img_backgroud/img_bg_bank.png")
                                               .RuleFor(t => t.SportId, f => f.Random.Int(1, 2))
                                               .Generate();
                _context.Add(tournaments);
                _context.SaveChanges();
            }

            return Ok();
        }
        public IActionResult AddFakeDataMember()
        {
            List<SportClub> SportClubs = _context.SportClub.ToList();

            foreach (var SportClub in SportClubs)
            {
                SportClubOnTournaments clubOnTournaments = new Faker<SportClubOnTournaments>()
                                                               .RuleFor(s => s.IsDelete, f => false)
                                                               .RuleFor(s => s.SportClubId, SportClub.Id)
                                                               .RuleFor(s => s.TournamentsId, f => f.Random.Int(1, 306))
                                                               .RuleFor(s => s.Point, f => f.Random.Int(1, 306))
                                                               .RuleFor(s => s.Created, DateTime.Now)
                                                               .RuleFor(s => s.Status, "Accepted")
                                                               .Generate();

                _context.SportClubOnTournaments.Add(clubOnTournaments);
                _context.SaveChanges();
            }

            return Ok();
        }

        // lưu và cập nhật lịch thi đấu
        [HttpPost]
        public async Task<IActionResult> MatchScheduleAndResults(MatchScheduleAndResultsVM vm)
        {
            JsonResultVM json = new JsonResultVM();
            try
            {
                MatchScheduleAndResults schedu = new MatchScheduleAndResults();
                if (vm.Id != 0)
                {
                    schedu = await _context.MatchScheduleAndResults.FirstOrDefaultAsync(i => i.Id == vm.Id);
                    vm.Created = schedu.Created;
                    vm.IsDelete = schedu.IsDelete;
                    _context.Entry(schedu).CurrentValues.SetValues(vm);

                }
                else
                {
                    schedu = vm;
                    schedu.IsDelete = false;
                    schedu.Created = DateTime.Now;
                    _context.Add(schedu);
                }
                json.Data = schedu;
                json.StatusCode = 200;
                _context.SaveChanges();
                return Ok(json);
            }
            catch (Exception ex)
            {
                json.Message = ex.Message;
                json.StatusCode = 500;
                json.Data = vm;
                return Ok(json);
            }
        }
        //lấy tất cả lịch thi đấu theo điều kiện mùa giải và vồng đấu
        public async Task<IActionResult> GetDataSchedule(int seasionId, int roundId)
        {
            JsonResultVM json = new JsonResultVM();
            try
            {
                List<MatchScheduleAndResults> list = _context.MatchScheduleAndResults.Where(i => i.IsDelete == false && i.SeasonOnTournamentId == seasionId && i.RoundId == roundId).ToList();
                json.Data = list;
                json.StatusCode = 200;
                return Ok(json);
            }
            catch (Exception ex)
            {
                json.Message = ex.Message;
                json.StatusCode = 500;
                return Ok(json);
            }
        }
        //lấy tất cả lịch thi đấu theo ID
        public async Task<IActionResult> GetDataScheduleById(int id)
        {
            JsonResultVM json = new JsonResultVM();
            try
            {
                MatchScheduleAndResults list = await _context.MatchScheduleAndResults.FirstOrDefaultAsync(i => i.IsDelete == false && i.Id == id);
                json.Data = list;
                json.StatusCode = 200;
                return Ok(json);
            }
            catch (Exception ex)
            {
                json.Message = ex.Message;
                json.StatusCode = 500;
                return Ok(json);
            }
        }
        //Xóa lịch thi đấu theo ID
        public async Task<IActionResult> DeleteDataScheduleById(int id)
        {
            JsonResultVM json = new JsonResultVM();
            try
            {
                MatchScheduleAndResults list = await _context.MatchScheduleAndResults.FirstOrDefaultAsync(i => i.IsDelete == false && i.Id == id);
                list.IsDelete = true;
                _context.MatchScheduleAndResults.Update(list);
                json.Data = list;
                json.StatusCode = 200;
                return Ok(json);
            }
            catch (Exception ex)
            {
                json.Message = ex.Message;
                json.StatusCode = 500;
                return Ok(json);
            }
        }

        [AllowAnonymous]
        //Tạo mùa giải
        public async Task<IActionResult> SaveDataSeason(SeasonOnTournamentsVM vm)
        {
            JsonResultVM json = new JsonResultVM();
            SeasonOnTournaments season = new SeasonOnTournaments();
            try
            {
                int numRound = vm.numberOfRounds;
                int numTeams = vm.TeamsNumber;
                int nunBoard = vm.NumberBoard;
                if (vm.AvatarFile != null)
                {
                    vm.AvatarImage = await _icommon.UploadImgAvatarAsync(vm.AvatarFile);
                }
                if(vm.BackgroudFile != null)
                {
                    vm.BackgroudImage = await _icommon.UploadImgBackgroudAsync(vm.BackgroudFile);
                }

                if(vm.Id == 0)
                {
                    // tạo mùa giải
                    season = vm;
                    season.Created = DateTime.Now;
                    season.IsDelete = false;
                    _context.Add(season);
                    _context.SaveChanges();



                    // tạo lịch thi đấu theo vòng
                    if (season.CompetitionForm == "Theo vòng")
                    {
                        List<MatchScheduleAndResults> MatchScheduleAndResults = new List<MatchScheduleAndResults>();
                        int TongVong = numTeams - 1;
                        List<String> name = new List<string>();
                        for (int i = 1; i <= numTeams;  i++)
                        {
                            name.Add("Team "+i);
                        }

                        //số lượt gặp nhập
                        for (int i = 1; i <= numRound; i++)
                        {
                            // số vòng đâu
                            for (int j = 1; j <= TongVong*i; j++)
                            {
                                Round round = new Round();
                                round.SeasonOnTournamentId = season.Id;
                                round.RoundName = $"Vòng {j+1}";
                                round.Created = DateTime.Now;
                                round.IsDelete = false;
                                _context.Add(round);
                                _context.SaveChanges();

                                Board board = new Board();
                                board.Id = 0;
                                board.RoundId = round.Id;
                                board.Name = "A";
                                board.Created = DateTime.Now;
                                board.IsDelete = false;
                                _context.Add(board);
                                _context.SaveChanges();

                                // thêm lịch
                                for (int k = 0; k < numTeams/2; k++)
                                {
                                    MatchScheduleAndResults.Add(new MatchScheduleAndResults()
                                    {
                                        RoundId = round.Id,
                                        BoardId = board.Id,
                                        Created = DateTime.Now,
                                        SportClub1_Name = name[k],
                                        SportClub2_Name = name[name.Count-1-k],
                                        SportClubId_1 = 0,
                                        SportClubId_2 = 0,
                                        SeasonOnTournamentId= season.Id,
                                        Schedule = DateTime.Now,
                                        Status = "Pennding",
                                        Winner = "",
                                        IsDelete = false,
                                    });

                                }
                                name = _icommon.RotateTeams(name);
                            }
                        }

                        _context.MatchScheduleAndResults.AddRange(MatchScheduleAndResults);
                        _context.SaveChanges();
                    }
                    else if(vm.CompetitionForm == "Chia bảng")
                    {

                    }

                }
                else
                {

                }
                return Ok(json);
            }
            catch (Exception ex)
            {
                json.Message = ex.Message;
                json.StatusCode = 500;
                return Ok(json);
            }
        }
    }
}
