using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppThi.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;

namespace AppThi.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly AppThiContext _context;

        private const string URL = "https://localhost:44303/api/employees";

        public EmployeesController(AppThiContext context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index(string searching=null)
        {
            if(searching != null)
            {
                var client1 = new HttpClient();
                var result1 = client1.GetAsync(URL + "/search?q=" + searching).Result;
                List<Employee> employees1 = JsonConvert.DeserializeObject<List<Employee>>(result1.Content.ReadAsStringAsync().Result);
                return View(employees1);
            }
            var client = new HttpClient();
            var result = client.GetAsync(URL).Result;
            List<Employee> employees = JsonConvert.DeserializeObject<List<Employee>>(result.Content.ReadAsStringAsync().Result);
            return View(employees);
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .FirstOrDefaultAsync(m => m.ID == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create([Bind("ID,Name,Salary,Department")] Employee employee)
        {                              
            
            string jsonInString = JsonConvert.SerializeObject(employee);
            var client = new HttpClient();
            HttpResponseMessage result = await client.PostAsync(URL, new StringContent(jsonInString, Encoding.UTF8, "application/json"));
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            return BadRequest();                   
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ID,Name,Salary,Department")] Employee employee)
        {
            if (id != employee.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.ID))
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
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .FirstOrDefaultAsync(m => m.ID == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var employee = await _context.Employee.FindAsync(id);
            _context.Employee.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(long id)
        {
            return _context.Employee.Any(e => e.ID == id);
        }
    }
}
