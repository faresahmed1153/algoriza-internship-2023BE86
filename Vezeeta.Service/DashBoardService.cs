using Microsoft.AspNetCore.Identity;
using Vezeeta.Core;
using Vezeeta.Core.Models;
using Vezeeta.Core.Models.Identity;
using Vezeeta.Core.Services;
using Vezeeta.Core.Utilities;

namespace Vezeeta.Service
{
	public class DashBoardService : IDashBoardService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly UserManager<ApplicationUser> _userManager;

		public DashBoardService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
		{
			_unitOfWork = unitOfWork;
			_userManager = userManager;

		}


		public async Task<object> NumOfBookings()
		{

			var TotalOfBookings = await _unitOfWork.DashBoardRepo.GetNumOfBookings();


			var PendingBookings = await _unitOfWork.DashBoardRepo.GetNumOfBookings((Booking b)
				=> b.BookingStatus == BookingStatus.Pending);

			var ConfirmedBookings = await _unitOfWork.DashBoardRepo.GetNumOfBookings((Booking b)
				=> b.BookingStatus == BookingStatus.Completed);

			var CancelledBookings = await _unitOfWork.DashBoardRepo.GetNumOfBookings((Booking b)
				=> b.BookingStatus == BookingStatus.Cancelled);


			var numberOfBookings = new
			{
				TotalOfBookings,
				PendingBookings,
				ConfirmedBookings,
				CancelledBookings
			};
			return numberOfBookings;
		}

		public async Task<object> NumOfBookings(DateOnly? dateTime)
		{

			var TotalOfBookings = await _unitOfWork.DashBoardRepo.GetNumOfBookings((Booking b)
				=> b.CreatedAt >= dateTime && b.CreatedAt <= dateTime);

			var PendingBookings = await _unitOfWork.DashBoardRepo.GetNumOfBookings((Booking b)
				=> b.BookingStatus == BookingStatus.Pending && b.CreatedAt >= dateTime && b.CreatedAt <= dateTime);

			var ConfirmedBookings = await _unitOfWork.DashBoardRepo.GetNumOfBookings((Booking b)
				=> b.BookingStatus == BookingStatus.Completed && b.CreatedAt >= dateTime && b.CreatedAt <= dateTime);

			var CancelledBookings = await _unitOfWork.DashBoardRepo.GetNumOfBookings((Booking b)
				=> b.BookingStatus == BookingStatus.Cancelled && b.CreatedAt >= dateTime && b.CreatedAt <= dateTime);


			var numberOfBookings = new
			{
				TotalOfBookings,
				PendingBookings,
				ConfirmedBookings,
				CancelledBookings
			};
			return numberOfBookings;
		}



		public async Task<int> NumOfDoctors()

			=> await _unitOfWork.DashBoardRepo.GetNumOfDoctors();


		public async Task<int> NumOfDoctors(DateOnly? dateTime)

			=> await _unitOfWork.DashBoardRepo.GetNumOfDoctors((Doctor d)
				=> d.CreatedAt >= dateTime && d.CreatedAt <= dateTime);


		public async Task<int> NumOfPatients()
		{
			var patients = await _userManager.GetUsersInRoleAsync(Role.Patient);

			return patients.Count;
		}


		public async Task<int> NumOfPatients(DateOnly? dateTime)
		{
			var patients = await _userManager.GetUsersInRoleAsync(Role.Patient);

			return patients.Count(p => p.CreatedAt >= dateTime && p.CreatedAt <= dateTime);
		}


		public async Task<IReadOnlyList<object>> TopFiveSpecializations(int top)

			 => await _unitOfWork.DashBoardRepo.GetTopFiveSpecializations(top);



		public async Task<IReadOnlyList<object>> TopTenDoctors(int top)

			=> await _unitOfWork.DashBoardRepo.GetTopTenDoctors(top);



	}
}
