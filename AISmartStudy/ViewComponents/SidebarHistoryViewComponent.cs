using AISmartStudy.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AISmartStudy.ViewComponents
{
    public class SidebarHistoryViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public SidebarHistoryViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Vào Database lấy ra 5 lộ trình mới nhất (sắp xếp theo ngày tạo giảm dần)
            var history = await _context.StudyPlans // Fixed: Use StudyPlans DbSet property
                .OrderByDescending(s => s.CreatedAt)
                .Take(5)
                .ToListAsync();

            return View(history);
        }
    }
}