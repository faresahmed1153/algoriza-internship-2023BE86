using System.ComponentModel.DataAnnotations;

namespace Vezeeta.Core.Dtos
{
	public class TimeDto
	{
		public int Id { get; set; }

		[Required]
		public TimeOnly Time { get; set; }
	}
}
