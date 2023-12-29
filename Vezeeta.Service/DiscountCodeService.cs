using Vezeeta.Core;
using Vezeeta.Core.Models;
using Vezeeta.Core.Services;

namespace Vezeeta.Service
{
	public class DiscountCodeService : IDiscountCodeService
	{
		private readonly IUnitOfWork _unitOfWork;

		public DiscountCodeService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}


		public async Task<string> AddDiscountCode(DiscountCode discountCode)
		{

			var isExists = await _unitOfWork.DiscountCodeRepo.GetByNameAsync((DiscountCode d)
				=> d.Code == discountCode.Code);

			if (isExists)
				return "There is already a discount code with the same name!";

			await _unitOfWork.DiscountCodeRepo.AddAsync(discountCode);

			await _unitOfWork.Complete();

			return "";


		}

		public async Task<string> UpdateDiscountCode(DiscountCode discountCode)
		{

			var isUsed = await _unitOfWork.DiscountCodeRepo.AppliedInBookingAsync((Booking b)
				=> b.DiscountCodeId == discountCode.Id);

			if (isUsed)
				return "This code has already been used in booking you can't update it";

			var isExists = await _unitOfWork.DiscountCodeRepo.GetByNameAsync((DiscountCode d)
				=> d.Code == discountCode.Code);

			if (isExists)
				return "There is already a discount code with the same name !";

			_unitOfWork.DiscountCodeRepo.Update(discountCode);

			await _unitOfWork.Complete();


			return "";



		}


		public async Task<string> DeleteDiscountCode(int id)
		{

			var discountCode = await FindDiscountCode(id);

			if (discountCode is null)
				return "Invalid Id!";

			var isUsed = await _unitOfWork.DiscountCodeRepo.AppliedInBookingAsync((Booking b)
				=> b.DiscountCodeId == discountCode.Id);

			if (isUsed)
				return "This code has already been used in booking you can't update it";

			_unitOfWork.DiscountCodeRepo.Delete(discountCode);

			await _unitOfWork.Complete();


			return "";


		}


		public async Task<string> DeactivateDiscountCode(int id)
		{
			var discountCode = await FindDiscountCode(id);

			if (discountCode is null)
				return "Invalid Id!";

			discountCode.IsActive = false;

			await _unitOfWork.Complete();


			return "";


		}


		private async Task<DiscountCode> FindDiscountCode(int id)

			=> await _unitOfWork.DiscountCodeRepo.GetByIdAsync(id);


	}
}
