using System.Linq.Expressions;
using System.Reflection;

namespace Cookify.Domain.Common.Entities;

public class PartialEntity<TEntity>  where TEntity : IEntity<Guid>
{
    private readonly Dictionary<PropertyInfo, object?> _properties = new();
    public IReadOnlyDictionary<PropertyInfo, object?> Properties => _properties;
    
    public PartialEntity<TEntity> AddValue<TValue>(
        Expression<Func<TEntity, TValue>> property, 
        TValue? value, 
        bool checkNull = false
    )
    {
        if (checkNull && value is null)
        {
            return this;
        }

        _properties.Add(GetPropertyInfo(property), value);
        
        return this;
    }

    private static PropertyInfo GetPropertyInfo<TProperty>(Expression<Func<TEntity, TProperty>> expression)
    {
        return expression.Body switch
        {
            null => throw new ArgumentNullException(nameof(expression)),
            UnaryExpression { Operand: MemberExpression me } => (PropertyInfo)me.Member,
            MemberExpression memberExpression => (PropertyInfo)memberExpression.Member,
            _ => throw new ArgumentException($"The expression doesn't indicate a valid property. [ {expression} ]")
        };
    }
}