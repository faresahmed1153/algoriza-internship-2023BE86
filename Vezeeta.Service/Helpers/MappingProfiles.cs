using AutoMapper;
using Vezeeta.Core.Dtos;
using Vezeeta.Core.Models;
using Vezeeta.Core.Models.Identity;

namespace Vezeeta.Service.Helpers
{
	public class MappingProfiles : Profile
	{


		public MappingProfiles()
		{


			CreateMap<PatientDto, ApplicationUser>()
				.ForMember(d => d.FullName, o => o.MapFrom(s => s.FirstName.ToLower() + " " + s.LastName.ToLower()));

			CreateMap<DoctorDto, ApplicationUser>()
				.ForMember(d => d.FullName, o => o.MapFrom(s => s.FirstName.ToLower() + " " + s.LastName.ToLower()));

			CreateMap<DiscountCodeDto, DiscountCode>();



		}
	}
}
