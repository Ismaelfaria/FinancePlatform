using FinancePlatform.API.Domain.Entities;
using FinancePlatform.API.Presentation.DTOs.InputModel;
using FinancePlatform.API.Presentation.DTOs.ViewModel;

namespace FinancePlatform.API.Application.Interfaces.Services
{
    public interface IReconciliationService : IGenericService<ReconciliationViewModel, ReconciliationInputModel, Reconciliation>
    {}
}
