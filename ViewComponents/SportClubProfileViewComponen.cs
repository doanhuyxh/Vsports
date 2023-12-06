using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vsports.Data;
using vsports.Models.SportClubVM;

namespace vsports.ViewComponents
{
    public class SportClubProfileViewComponent: ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public SportClubProfileViewComponent (ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            int idClub = 3;
            SportClubVM sportClubVM = await _context.SportClub.FirstOrDefaultAsync(i => i.Id == idClub);

            return View(sportClubVM);
        }
    }
}
