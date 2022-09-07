using AutoMapper;
using CompanyEmployees.Extensions;
using Contracts;
using Entities.Exceptions;
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

            if (company is null)
                throw new CompanyNotFoundException(id);

            var companyDto = _mapper.Map<CompanyDto>(company);
            return companyDto;
        }

        public CompanyDto CreateCompany(CompanyForCreationDto company)
        {
            var companyEntity = _mapper.Map<Company>(company);
            _repository.Company.CreateCompany(companyEntity);
            _repository.Save();

            var companyToReturn = _mapper.Map<CompanyDto>(companyEntity);
            return companyToReturn;
        }
        
        public IEnumerable<CompanyDto> GetByIds(IEnumerable<Guid> ids, bool trackChanges)
        {
            if (ids is null)
                throw new IdParametersBadRequestException();

            var companyEntities = _repository.Company.GetByIds(ids, trackChanges);
            if (ids.Count() != companyEntities.Count())
                throw new CollectionByIdsBadRequesttsException();

            var companiesToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
            return companiesToReturn;
        }

        public (IEnumerable<CompanyDto> companies, string ids) CreateCompanyCollection(IEnumerable<CompanyForCreationDto> companyCollection)
        {
            if (companyCollection is null)
                throw new CompanyCollectionBadRequest();

            var companyEntities = _mapper.Map<IEnumerable<Company>>(companyCollection);
            foreach (var company in companyEntities)
            {
                _repository.Company.CreateCompany(company);
            }

            _repository.Save();

            var companyCollectionToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
            var ids = string.Join(",", companyCollectionToReturn.Select(c => c.Id));
            return (companies: companyCollectionToReturn, ids: ids);


        }

        public void DeleteCompany(Guid companyId, bool trackChanges)
        {
            var company = _repository.Company.GetCompany(companyId, trackChanges);
            if (company is null)
                throw new CompanyNotFoundException(companyId);
            _repository.Company.DeleteCompany(company);
            _repository.Save();

        }

        public void UpdateCompany(Guid companyId, CompanyForUpdateDto companyForUpdate, bool trackChanges)
        {
            var companyEntity = _repository.Company.GetCompany(companyId, trackChanges);
            if (companyEntity is null)
                throw new CompanyNotFoundException(companyId);

            _mapper.Map(companyForUpdate, companyEntity);
            _repository.Save();

        }

    }
}
