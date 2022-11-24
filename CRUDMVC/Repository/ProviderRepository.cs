using CRUDMVC.Data;
using CRUDMVC.Models;
using CRUDMVC.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CRUDMVC.Repository
{
    public class ProviderRepository : Repository<Provider>, IProviderRepository
    {
        public ProviderRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public async Task<SelectList> FillProviderViewBagAsync()
        {
            var providers = await _appDbContext.Providers.ToListAsync();

            if (providers == null)
                providers = new List<Provider>();

            return new SelectList(providers, "Id", "Name");
        }
    }
}
