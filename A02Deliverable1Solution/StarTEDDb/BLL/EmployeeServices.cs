using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StarTEDDb.DAL;
using StarTEDDb.Entities;

namespace StarTEDDb.BLL
{
	public class EmployeeServices
	{
		private readonly StarTEDContext _context;

		internal EmployeeServices(StarTEDContext context)
		{
			_context = context;
		}

		#region Queries

		public List<Employee> Employee_Club_List()
		{
			return _context.Employees.Include(e => e.Clubs).Where(e => e.Clubs.Any()).OrderBy(e => e.LastName).ToList();
		}

		public List<Employee> Available_Staff_for_Clubs()
		{
			return _context.Employees.Include(e => e.Clubs).Where(e => e.PositionID == 4 || e.PositionID == 5 || e.PositionID == 6).OrderBy(e => e.LastName).ToList();
		}

		#endregion
	}
}
