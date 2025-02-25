using FinancePlatform.API.Application.Interfaces.Utils;
using System.Reflection;

namespace FinancePlatform.API.Application.Utils
{
    public class ReflectionUpdateStrategy : IEntityUpdateStrategy
    {
        public void UpdateEntityFields<T>(T entity, Dictionary<string, object> updateRequest)
        {
            foreach (var entry in updateRequest)
            {
                try
                {
                    PropertyInfo property = entity.GetType().GetProperty(entry.Key);
                    if (property != null && property.CanWrite)
                    {
                        property.SetValue(entity, Convert.ChangeType(entry.Value, property.PropertyType));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao atualizar campo {entry.Key}: {ex.Message}");
                }
            }
        }
    }
}
