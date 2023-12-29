using Vezeeta.Core.Models;

namespace Vezeeta.Core.Services
{
	public interface IDiscountCodeService
	{
		Task<string> AddDiscountCode(DiscountCode discountCode);

		Task<string> UpdateDiscountCode(DiscountCode discountCode);

		Task<string> DeleteDiscountCode(int id);

		Task<string> DeactivateDiscountCode(int id);

	}
}
