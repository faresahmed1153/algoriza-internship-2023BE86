using System.ComponentModel.DataAnnotations;

namespace Vezeeta.Core.Dtos
{
	public class SearchDto
	{

		[Range(1, 5, ErrorMessage = "page size starts from 1 till 5")]
		public int PageSize { get; set; }


		[Range(1, int.MaxValue, ErrorMessage = "page starts from 1")]
		public int Page { get; set; }


		private string criteria;

		public string? Criteria
		{
			get { return criteria; }
			set { criteria = value.ToLower(); }
		}


	}
}
