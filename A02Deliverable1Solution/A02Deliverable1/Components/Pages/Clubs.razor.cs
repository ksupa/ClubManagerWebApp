using StarTEDDb.BLL;
using StarTEDDb.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MudBlazor;

namespace A02Deliverable1.Components.Pages
{
	public partial class Clubs
	{
		[Inject] private ClubServices _clubServices { get; set; }
		[Inject] private NavigationManager _navigationManager {  get; set; } 

		private List<Club> clubs = [];
		private List<string> errorMessages = [];
		private bool activeStatus = true;
		private bool loading = false;


		private async void DisplayClubs()
		{
			loading = true;
			errorMessages.Clear();
			clubs.Clear();

			try
			{
				// Simulate loading
				await Task.Delay(1500);

				clubs = await _clubServices.Clubs_by_Status(activeStatus);
			}
			catch (Exception ex)
			{
				errorMessages.Add(GetInnerException(ex).Message);
			}
			finally
			{
                loading = false;
				await InvokeAsync(StateHasChanged);
            }
		}

		private Exception GetInnerException(Exception ex)
		{
			while (ex.InnerException != null)
				ex = ex.InnerException;
			return ex;
		}

		private void EditClub(string clubId)
		{
			_navigationManager.NavigateTo($"/club/{clubId}");
		}
	}
}
