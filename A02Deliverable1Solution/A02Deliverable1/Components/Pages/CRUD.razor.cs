using Microsoft.AspNetCore.Components;
using MudBlazor;
using StarTEDDb.BLL;
using StarTEDDb.Entities;
using static MudBlazor.Defaults.Classes;

namespace A02Deliverable1.Components.Pages
{
	public partial class CRUD
	{
		[Inject] private IDialogService DialogService { get; set; }
		[Inject] private ISnackbar Snackbar { get; set; }
		[Inject] private NavigationManager _navigationManager { get; set; }
		[Inject] private ClubServices _clubServices { get; set; }
		private Club SelectedClub = new();

		[Inject] private EmployeeServices _employeeServices { get; set; }

		[Parameter]
		public string clubID { get; set; }

		private MudForm clubForm = new();
		private List<string> errorMessages = [];
		private List<Employee> availableStaff = [];
		private string feedbackMessage = string.Empty;
		private bool newClub = false;



		protected override void OnInitialized()
		{
			try
			{
				availableStaff = _employeeServices.Available_Staff_for_Clubs();

				if (!string.IsNullOrEmpty(clubID))
				{
					try
					{
						SelectedClub = _clubServices.Club_by_Id(clubID);
					}
					catch (Exception ex)
					{
						errorMessages.Add(GetInnerException(ex).Message);
					}
				}
				else
				{
					newClub = true;
				}
			}
			catch (Exception ex)
			{
				errorMessages.Add(GetInnerException(ex).Message);	
			}

			base.OnInitialized();
		}

		private void Clear()
		{
			if (!newClub)
			{
				errorMessages.Clear();
				SelectedClub.ClubName = null;
				SelectedClub.Employee = null;
				SelectedClub.Fee = 0;
				SelectedClub.Active = false;
			}
			else
			{
				errorMessages.Clear();
				clubForm.ResetAsync();
			}
		}

		private async void AddClub()
		{
			errorMessages.Clear();
			feedbackMessage = string.Empty;
			clubForm.Validate();

			if (clubForm.IsValid)
			{
				try
				{
					string newClubID = _clubServices.AddClub(SelectedClub);

					feedbackMessage = $"{SelectedClub.ClubID} {SelectedClub.ClubName}, has been added to the club list.";
					SelectedClub.ClubID = newClubID;

					bool? choice = await DialogService.ShowMessageBox("Success!", $"{feedbackMessage}", yesText: "Return to Club List", noText: "Add Another Club", cancelText: "Edit Club");

					if (choice == true)
					{
						_navigationManager.NavigateTo("clubs");
					}

					if (choice == false)
					{
						clubForm.ResetAsync();
					}

					_navigationManager.NavigateTo($"club/{newClubID}");
					newClub = false;
				}
				catch (Exception ex)
				{
					bool? choice = await DialogService.ShowMessageBox("An Error Has Occured:", $"{GetInnerException(ex).Message}", yesText: "Okay");
				}
			}
		}

		private async void UpdateClub()
		{
			errorMessages.Clear();
			feedbackMessage = string.Empty;
			clubForm.Validate();

			if (clubForm.IsValid)
			{
				try
				{
					int rowsAffected = _clubServices.UpdateClub(SelectedClub);

					if (rowsAffected == 0)
					{
						bool? choice = await DialogService.ShowMessageBox("", "No changes have been made.", yesText: "Okay");
					}
					else
					{
						feedbackMessage = $"{SelectedClub.ClubID} {SelectedClub.ClubName}, has been updated.";
						bool? choice = await DialogService.ShowMessageBox("Success!", $"{feedbackMessage}", yesText: "Make Another Change", cancelText: "Return to Club List");

						if (choice == null)
						{
							_navigationManager.NavigateTo("clubs");
						}
					}
				}
				catch (Exception ex)
				{
					bool? choice = await DialogService.ShowMessageBox("An Error Has Occured:", $"{GetInnerException(ex).Message}", yesText: "Okay");
				}
			}
		}

		private void DeactivateClub()
		{
			errorMessages.Clear();
			feedbackMessage = string.Empty;

			try
			{
				int rowsAffected = _clubServices.InactiveClub(SelectedClub);

				if (rowsAffected == 0)
				{
					feedbackMessage = $"{SelectedClub.ClubName} active status has not been changed. ";
					FeedbackBox(feedbackMessage, Defaults.Classes.Position.BottomRight, Variant.Outlined);
				}
				else
				{
					feedbackMessage = $"{SelectedClub.ClubID} {SelectedClub.ClubName}, is now Inactive.";
					FeedbackBox(feedbackMessage, Defaults.Classes.Position.BottomRight, Variant.Outlined);
				}
			}
			catch (Exception ex)
			{
				if (GetInnerException(ex).Message.Length > 50)
				{
					errorMessages.Add(GetInnerException(ex).Message);
				}
				else
				{
					FeedbackBox(GetInnerException(ex).Message, Defaults.Classes.Position.BottomRight, Variant.Filled);
				}
			}

		}

		private void ActivateClub()
		{
			errorMessages.Clear();
			feedbackMessage = string.Empty;

			try
			{
				int rowsAffected = _clubServices.ActiveClub(SelectedClub);

				if (rowsAffected == 0)
				{
					feedbackMessage = $"{SelectedClub.ClubName} active status has not been changed. ";
					FeedbackBox(feedbackMessage, Defaults.Classes.Position.BottomRight, Variant.Outlined);
				}
				else
				{
					feedbackMessage = $"{SelectedClub.ClubID} {SelectedClub.ClubName}, is now Active.";
					FeedbackBox(feedbackMessage, Defaults.Classes.Position.BottomRight, Variant.Outlined);
				}
			}
			catch (Exception ex)
			{
				if (GetInnerException(ex).Message.Length > 50)
				{
					errorMessages.Add(GetInnerException(ex).Message);
				}
				else
				{
					FeedbackBox(GetInnerException(ex).Message, Defaults.Classes.Position.BottomRight, Variant.Filled);
				}
			}
		}

		private void FeedbackBox(string message, string position, Variant variant)
		{
			Snackbar.Clear();
			Snackbar.Configuration.PositionClass = position;
			if (variant == Variant.Filled)
			{
				Snackbar.Add(message, Severity.Error, (c) =>
				{
					c.SnackbarVariant = variant;
					c.RequireInteraction = true;
				});
			}
			else
			{
				Snackbar.Add(message, Severity.Info, c => c.SnackbarVariant = variant);
			}
		}

		private Exception GetInnerException(Exception ex)
		{
			while (ex.InnerException != null)
				ex = ex.InnerException;
			return ex;
		}
	}
}
