using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EbayApp.Data;
using EbayApp.Models;
using System.Data;

namespace EbayApp.Controllers
{
	public class StudentsController : Controller
	{
		private readonly SchoolContext _context;
		private readonly ILogger _logger;

		public StudentsController(SchoolContext context, ILogger<StudentsController> logger)
		{
			_context = context;
			_logger = logger;
		}

		// GET: Students
		public async Task<IActionResult> Index()
		{
			return View(await _context.Students.Include(c => c.Enrollments).ToListAsync());
		}

		// GET: Students/Details/5
		public async Task<IActionResult> Details(Guid? id)
		{
			if (id == null)
			{
				//return NotFound();
				ModelState.AddModelError("", "Unable to get student details. Try again, and if the problem persists, see your system administrator.");
				return View(new Student()); 
			}

			var student = await _context.Students
				.FirstOrDefaultAsync(m => m.Id == id);
			if (student == null)
			{
				//return NotFound();
				ModelState.AddModelError("", "Unable to get student details. Try again, and if the problem persists, see your system administrator.");
				return View(new Student());
			}

			return View(student);
		}

		// GET: Students/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Students/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("FirstMidName,LastName,EnrollmentDate")] Student student)
		{
			try
			{
				if (ModelState.IsValid)
				{
					//student.Id = Guid.NewGuid();
					_context.Add(student);
					await _context.SaveChangesAsync();
					return RedirectToAction(nameof(Index));
				}
			}
			catch(DataException ex)
			{
				_logger.LogInformation(ex.Message);
				ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
			}


			return View(student);
		}

		// GET: Students/Edit/5
		public async Task<IActionResult> Edit(Guid? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var student = await _context.Students.FindAsync(id);
			if (student == null)
			{
				return NotFound();
			}
			return View(student);
		}

		// POST: Students/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(Guid id, [Bind("Id,FirstMidName,LastName,EnrollmentDate")] Student student) //,Created
		{
			if (id != student.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(student);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (student.Id.HasValue && !StudentExists(student.Id.Value))
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
			return View(student);
		}

		// GET: Students/Delete/5
		public async Task<IActionResult> Delete(Guid? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var student = await _context.Students
				.FirstOrDefaultAsync(m => m.Id == id);
			if (student == null)
			{
				return NotFound();
			}

			return View(student);
		}

		// POST: Students/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(Guid id)
		{
			var student = await _context.Students.FindAsync(id);
			_context.Students.Remove(student);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool StudentExists(Guid id)
		{
			return _context.Students.Any(e => e.Id == id);
		}
	}
}
