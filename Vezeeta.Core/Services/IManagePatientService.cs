using Vezeeta.Core.Dtos;

namespace Vezeeta.Core.Services
{
	public interface IManagePatientService
	{
		Task<IReadOnlyList<PatientToReturnDto>> GetAllPatients(SearchDto searchDto);

		Task<IReadOnlyList<object>> GetPatientById(string id);
	}
}
