using AutoMapper;
using Contracts;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service
{
    internal sealed class CompanyService : ICompanyService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public CompanyService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            this._repository = repository;
            this._logger = logger;
            this._mapper = mapper;
        }

        public IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges)
        {
            var companies = _repository.Company.GetAllCompanies(trackChanges);
            var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);
            //var companiesDto = companies.Select(p => new CompanyDto(p.Id, p.Name ?? "", string.Join(' ', p.Address, p.Country))).ToList();
            return companiesDto;
            
        }

        public CompanyDto GetCompany(Guid id, bool trackChanges)
        {
            var company = _repository.Company.GetCompany(id, trackChanges);

            var companyDto = _mapper.Map<CompanyDto>(company);
            return companyDto;
        }
        

    }
}
