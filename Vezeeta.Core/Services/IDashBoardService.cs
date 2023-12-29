namespace Vezeeta.Core.Services
{
	public interface IDashBoardService
	{
		Task<int> NumOfDoctors();

		Task<int> NumOfDoctors(DateOnly? date);


		Task<int> NumOfPatients();

		Task<int> NumOfPatients(DateOnly? date);


		Task<object> NumOfBookings();

		Task<object> NumOfBookings(DateOnly? date);



		Task<IReadOnlyList<object>> TopFiveSpecializations(int top);


		Task<IReadOnlyList<object>> TopTenDoctors(int top);
	}
}
