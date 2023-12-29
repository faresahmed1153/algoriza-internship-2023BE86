using System.Text.Json.Serialization;
using Vezeeta.Core.Models;
using Vezeeta.Core.Utilities;

namespace Vezeeta.Core.Dtos
{
	public class PatientBookingsToReturnDto
	{
		public string PictureUrl { get; set; }

		public string doctorName { get; set; }

		public string Specialize { get; set; }

		[JsonConverter(typeof(JsonStringEnumConverter))]
		public DayOfWeek Day { get; set; }

		public TimeOnly Time { get; set; }

		public decimal Price { get; set; }

		public DiscountCode DiscountCode { get; set; }

		public decimal FinalPrice { get; set; }

		[JsonConverter(typeof(JsonStringEnumConverter))]
		public BookingStatus BookingStatus { get; set; }
	}
}
