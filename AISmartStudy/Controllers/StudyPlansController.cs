using AISmartStudy.Data;
using AISmartStudy.Models;
using AISmartStudy.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AISmartStudy.Controllers
{
    // Nhớ thêm dòng using này ở trên cùng file nhé:
    // using AISmartStudy.Services;

    public class StudyPlansController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly GeminiService _geminiService; // 1. Thêm dòng này

        // 2. Thêm GeminiService vào trong ngoặc tròn
        public StudyPlansController(ApplicationDbContext context, GeminiService geminiService)
        {
            _context = context;
            _geminiService = geminiService; // 3. Gán giá trị
        }

        // GET: StudyPlans
        public async Task<IActionResult> Index()
        {
            return View(await _context.StudyPlans.ToListAsync());
        }

        // GET: StudyPlans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studyPlan = await _context.StudyPlans
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studyPlan == null)
            {
                return NotFound();
            }

            return View(studyPlan);
        }

        // GET: StudyPlans/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StudyPlans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Topic,ContentJson,CreatedAt")] StudyPlan studyPlan)
        {
            if (ModelState.IsValid)
            {
                // Gọi AI tạo nội dung dựa vào tên Topic mà bạn vừa nhập trên web
                studyPlan.ContentJson = await _geminiService.GenerateStudyPlanAsync(studyPlan.Topic);
                // -------------------------------

                _context.Add(studyPlan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(studyPlan);
        }

        // GET: StudyPlans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studyPlan = await _context.StudyPlans.FindAsync(id);
            if (studyPlan == null)
            {
                return NotFound();
            }
            return View(studyPlan);
        }

        // POST: StudyPlans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Topic,ContentJson,CreatedAt")] StudyPlan studyPlan)
        {
            if (id != studyPlan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(studyPlan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudyPlanExists(studyPlan.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(studyPlan);
        }

        // GET: StudyPlans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studyPlan = await _context.StudyPlans
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studyPlan == null)
            {
                return NotFound();
            }

            return View(studyPlan);
        }

        // POST: StudyPlans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var studyPlan = await _context.StudyPlans.FindAsync(id);
            if (studyPlan != null)
            {
                _context.StudyPlans.Remove(studyPlan);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudyPlanExists(int id)
        {
            return _context.StudyPlans.Any(e => e.Id == id);
        }
    }
}
