using Vezeeta.Core.Dtos;

namespace Vezeeta.Core.Services
{
	public interface IAdminService
	{



		Task<IReadOnlyList<PatientToReturnDto>> GetAllPatients();

		Task<PatientToReturnDto> GetPatientById(int id);
	}
}
