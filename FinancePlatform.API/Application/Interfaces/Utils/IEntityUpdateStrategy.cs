namespace FinancePlatform.API.Application.Interfaces.Utils
{
    public interface IEntityUpdateStrategy
    {
        bool UpdateEntityFields<T>(T entity, Dictionary<string, object> updatedFields);
    }
}
