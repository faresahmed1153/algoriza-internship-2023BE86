using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Vezeeta.Core.Utilities;

namespace Vezeeta.Core.Dtos
{
	public class DiscountCodeDto
	{
		public int? Id { get; set; }

		public string Code { get; set; }

		[Range(1, double.MaxValue, ErrorMessage = "Value must be greater than zero!")]
		public int Value { get; set; }


		[JsonConverter(typeof(JsonStringEnumConverter))]
		public DiscountType Type { get; set; }

		[Range(0, int.MaxValue, ErrorMessage = "BookingsCompleted can't be a negative value!")]
		public int BookingsCompleted { get; set; }


		public bool IsActive { get; set; } = true;

	}
}
