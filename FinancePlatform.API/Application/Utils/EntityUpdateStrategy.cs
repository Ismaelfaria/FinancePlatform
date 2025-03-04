using FinancePlatform.API.Application.Interfaces.Utils;
using System.Reflection;

namespace FinancePlatform.API.Application.Utils
{
    public class EntityUpdateStrategy : IEntityUpdateStrategy
    {
        public bool UpdateEntityFields<T>(T entity, Dictionary<string, object> updatedFields)
        {
            if(updatedFields == null || entity == null)
            {
                return false;
            }

            foreach (var entry in updatedFields)
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

            return true;
        }
    }
}
