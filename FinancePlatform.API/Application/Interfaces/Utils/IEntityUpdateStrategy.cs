namespace FinancePlatform.API.Application.Interfaces.Utils
{
    public interface IEntityUpdateStrategy
    {
        void UpdateEntityFields<T>(T entity, Dictionary<string, object> updateRequest);
    }
}
