using FinancePlatform.API.Domain.Entities;

namespace FinancePlatform.API.Application.Interfaces.Services
{
    public interface IGenericService<TViewModel,TInputModel, TEntity>
    {
        Task<TViewModel?> FindByIdAsync(Guid id);
        Task<List<TViewModel>?> FindAllAsync();
        Task<TEntity?> AddAsync(TInputModel entity);
        Task<TEntity?> UpdateAsync(Guid id, Dictionary<string, object> updateRequest);
        Task<bool> DeleteAsync(Guid id);
    }
}
