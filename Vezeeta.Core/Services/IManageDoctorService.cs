using Vezeeta.Core.Dtos;

namespace Vezeeta.Core.Services
{
	public interface IManageDoctorService
	{

		Task<IReadOnlyList<DoctorToReturnDto>> GetAllDoctors(SearchDto searchDto);

		Task<object> GetDoctorById(int id);

		Task<string> AddDoctor(DoctorDto doctor);

		Task<string> EditDoctor(DoctorUpdateDto doctor);

		Task<string> DeleteDoctor(int id);
	}
}
