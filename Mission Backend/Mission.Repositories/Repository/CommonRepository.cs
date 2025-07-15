using Mission.Entities;
using Mission.Entities.ViewModels;
using Mission.Repositories.IRepository;

namespace Mission.Repositories.Repository
{
    public class CommonRepository(MissionDbContext dbContext) : ICommonRepository
    {
        private readonly MissionDbContext _dbContext = dbContext;

        public List<DropDownResponseModel> CountryList()
        {
            var countries = _dbContext.Countries
                .OrderBy(c => c.CountryName)
                .Select(c => new DropDownResponseModel(c.Id, c.CountryName))
                .ToList();

            return countries;
        }

        public List<DropDownResponseModel> CityList(int countryId)
        {
            var cities = _dbContext.Cities
                .Where(c => c.CountryId == countryId)
                .OrderBy(c => c.CityName)
                .Select(c => new DropDownResponseModel(c.Id, c.CityName))
                .ToList();

            return cities;
        }
    }
}
