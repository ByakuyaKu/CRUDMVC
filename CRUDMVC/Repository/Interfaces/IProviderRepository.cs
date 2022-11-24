using CRUDMVC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CRUDMVC.Repository.Interfaces
{
    public interface IProviderRepository : IRepository<Provider>
    {
        public Task<SelectList> FillProviderViewBagAsync();
    }
}
