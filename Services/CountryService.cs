using System.Collections.Generic;
using System.Threading.Tasks;
using PoolGameServer.Entities;
using PoolGameServer.Persistence;
using PoolGameServer.Repositories;

namespace PoolGameServer.Services
{
    public interface ICountryService
    {
        Task AddCountry(Country country);
        Task RemoveCountry(Country country);
        Task<IEnumerable<Country>> GetCountries();
        Task<Country>GetCountry(string countryId);

    }
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CountryService(ICountryRepository countryRepository,IUnitOfWork unitOfWork)
        {
            _countryRepository = countryRepository;
        }

        public async Task AddCountry(Country country)
        {
            _countryRepository.Add(country);
            await _unitOfWork.Commit();
        }

        public async Task<IEnumerable<Country>> GetCountries()
        {
            return await _countryRepository.GetAll();
        }

        public async Task<Country> GetCountry(string countryId)
        {
            var country = await _countryRepository.GetById(countryId);
            return country;
        }

        public async Task RemoveCountry(Country country)
        {
            var id = country.Id;
            await _countryRepository.Remove(id);
        }

    }
}