namespace Vezeeta.Core.Dtos
{
	public class DoctorBookingsDto
	{
		public int Page { get; set; }

		public int PageSize { get; set; }

		public int DoctorId { get; set; }

		public TimeOnly? Criteria { get; set; }

	}
}
