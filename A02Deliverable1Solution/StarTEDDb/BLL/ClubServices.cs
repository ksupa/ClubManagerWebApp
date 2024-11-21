using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.IdentityModel.Tokens;
using StarTEDDb.DAL;
using StarTEDDb.Entities;

namespace StarTEDDb.BLL
{
	public class ClubServices
	{
		private readonly StarTEDContext _context;

		internal ClubServices(StarTEDContext context)
		{
			_context = context;
		}

		#region Queries

		public async Task <List<Club>> Clubs_by_Status(bool status)
		{
			return await _context.Clubs.Include(c => c.Employee).Where(c => c.Active == status).OrderBy(c => c.ClubName).ToListAsync();
		}

		public Club Club_by_Id(string clubId)
		{
			return _context.Clubs.Include(c => c.Employee).Where(c => c.ClubID == clubId).FirstOrDefault();
		}
		#endregion


		#region CRUD 

		public string AddClub(Club club)
		{
			if (club == null)
			{
				throw new ArgumentNullException("No club information has been entered.");
			}

			// Access error messages from the validation attributes
			List<ValidationResult> validation = new List<ValidationResult>();
			ValidationContext validate = new ValidationContext(club);

			bool validClub = Validator.TryValidateObject(club, validate, validation, true);
			bool existingClubId = _context.Clubs.Any(c => c.ClubID.ToLower() == club.ClubID.ToLower());
			bool existingClubName = _context.Clubs.Any(c => c.ClubName.ToLower() == club.ClubName.ToLower());

			if (!validClub)
			{
				string error = validation.Select(c => c.ErrorMessage).FirstOrDefault();
				throw new ValidationException(error);
			}

			if(existingClubId && existingClubName)
			{
				throw new ArgumentException("Club ID and Club Name match an already existing club.");
			}

			if (existingClubId)
			{
				throw new ArgumentException("Club ID matches an already existing club.");
			}

			if (existingClubName)
			{
				throw new ArgumentException("Club Name matches an already existing club.");
			}

			club.ClubID = club.ClubID.ToUpper();
			_context.Clubs.Add(club);
			_context.SaveChanges();

			return club.ClubID;
		}

		public int UpdateClub(Club club)
		{

			if (club == null)
			{
				throw new ArgumentNullException("No club information has been entered.");
			}

			List<ValidationResult> validation = new List<ValidationResult>();
			ValidationContext validate = new ValidationContext(club);

			bool validClub = Validator.TryValidateObject(club, validate, validation, true);
			bool exists = _context.Clubs.Any(c => c.ClubID != club.ClubID && c.ClubName == club.ClubName && c.Employee == club.Employee && c.Fee == club.Fee);
			bool existingClubName = _context.Clubs.Any(c => c.ClubID != club.ClubID && c.ClubName.ToLower() == club.ClubName.ToLower());

			if (!validClub)
			{
				{
					string error = validation.Select(c => c.ErrorMessage).FirstOrDefault();
					throw new ValidationException(error);
				}
			}

			if (exists)
			{
				throw new ArgumentException("There is already a club with this information.");
			}

			if (existingClubName)
			{
				throw new ArgumentException("Club Name matches an already existing club.");
			}


			EntityEntry<Club> updatingClub = _context.Entry(club);
			updatingClub.State = EntityState.Modified;

			return _context.SaveChanges();
		}

		public int InactiveClub(Club club)
		{
			if (club == null)
			{
				throw new ArgumentNullException("No club information has been entered.");
			}

			bool exists = _context.Clubs.Any(c => c.ClubID == club.ClubID);

			if (!exists)
			{
				throw new ArgumentException("Submit a club first to change its Active Status.");
			}

			club.Active = false;

			EntityEntry<Club> updatingClub = _context.Entry(club);
			updatingClub.State = EntityState.Modified;

			return _context.SaveChanges();
		}

		public int ActiveClub(Club club)
		{
			if (club == null)
			{
				throw new ArgumentNullException("No club information has been entered.");
			}

			bool exists = _context.Clubs.Any(c => c.ClubID == club.ClubID);

			if (!exists)
			{
				throw new ArgumentException("Submit a club first to change its Active Status.");
			}

			club.Active = true;

			EntityEntry<Club> updatingClub = _context.Entry(club);
			updatingClub.State = EntityState.Modified;

			return _context.SaveChanges();
		}

		#endregion
	}
}
