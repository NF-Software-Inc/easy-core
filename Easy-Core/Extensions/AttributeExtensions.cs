using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

namespace easy_core;

/// <summary>
/// Extension methods related to attributes.
/// </summary>
public static class AttributeExtensions
{
	/// <summary>
	/// Returns the display name of a provided type if found.
	/// </summary>
	/// <param name="type">The data type to return the name for.</param>
	public static string GetTypeDisplayName(this Type type)
	{
		return type.GetCustomAttribute<DisplayAttribute>()?.GetName() ?? type.Name;
	}

	/// <summary>
	/// Returns the display description of a provided type if found.
	/// </summary>
	/// <param name="type">The data type to return the description for.</param>
	public static string? GetTypeDisplayDescription(this Type type)
	{
		return type.GetCustomAttribute<DisplayAttribute>()?.GetDescription();
	}

	/// <summary>
	/// Returns the <see cref="DisplayAttribute.Name"/> value of a provided property if found.
	/// </summary>
	/// <param name="property">The property to find the name of.</param>
	public static string GetPropertyDisplayName(this PropertyInfo property)
	{
		return property.GetCustomAttribute<DisplayAttribute>()?.GetName() ?? property.Name;
	}

	/// <summary>
	/// Returns the <see cref="DisplayAttribute.Name"/> value of a provided property if found.
	/// </summary>
	/// <typeparam name="TModel">The type of the property.</typeparam>
	/// <param name="property">The property to find the name of.</param>
	public static string GetPropertyDisplayName<TModel>(this Expression<Func<TModel>> property)
	{
		return property.GetPropertyAttribute<TModel, DisplayAttribute>()?.GetName() ?? "";
	}

	/// <summary>
	/// Returns the display name of a provided property if found.
	/// </summary>
	/// <param name="propertyName">The name of the property in the parent class.</param>
	/// <param name="type">The data type of the parent class of the property.</param>
	public static string GetPropertyDisplayName(this Type type, string propertyName)
	{
		var property = type.GetProperty(propertyName);

		if (property == null)
			return propertyName;

		return property.GetPropertyDisplayName();
	}

	/// <summary>
	/// Returns the <see cref="DisplayAttribute.Description"/> value of a provided property if found.
	/// </summary>
	/// <param name="property">The property to find the description of.</param>
	public static string? GetPropertyDisplayDescription(this PropertyInfo property)
	{
		return property.GetCustomAttribute<DisplayAttribute>()?.GetDescription();
	}

	/// <summary>
	/// Returns the <see cref="DisplayAttribute.Description"/> value of a provided property if found.
	/// </summary>
	/// <typeparam name="TModel">The type of the property.</typeparam>
	/// <param name="property">The property to find the description of.</param>
	public static string? GetPropertyDisplayDescription<TModel>(this Expression<Func<TModel>> property)
	{
		return property.GetPropertyAttribute<TModel, DisplayAttribute>()?.GetDescription();
	}

	/// <summary>
	/// Returns the display description of a provided property if found.
	/// </summary>
	/// <param name="propertyName">The description of the property in the parent class.</param>
	/// <param name="type">The data type of the parent class of the property.</param>
	public static string? GetPropertyDisplayDescription(this Type type, string propertyName)
	{
		var property = type.GetProperty(propertyName);

		if (property == null)
			return null;

		return property.GetPropertyDisplayDescription();
	}

	/// <summary>
	/// Returns the <see cref="DisplayAttribute.Name"/> value of a provided property value if found.
	/// </summary>
	/// <typeparam name="TModel">The type of the property.</typeparam>
	/// <param name="value">The value to find the name of.</param>
	public static string GetValueDisplayName<TModel>(this TModel value)
	{
		return value.GetValueAttribute<TModel, DisplayAttribute>()?.GetName() ?? value?.ToString() ?? "";
	}

    /// <summary>
    /// Returns the matching attribute from the specified type if found.
    /// </summary>
    /// <typeparam name="TAttribute">The datatype of the attribute to return.</typeparam>
    /// <param name="type">The type to check.</param>
    public static TAttribute? GetTypeAttribute<TAttribute>(this Type type) where TAttribute : Attribute
	{
		return (TAttribute?)Attribute.GetCustomAttribute(type, typeof(TAttribute));
	}

    /// <summary>
    /// Returns the value of a specified property from the <see cref="DisplayAttribute"/> of a provided type if found.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <param name="propertyAccessor">A function to access the desired property from the <see cref="DisplayAttribute"/>.</param>
    /// <returns>The value of the specified property from the <see cref="DisplayAttribute"/> if found; otherwise, null.</returns>
    public static string? GetTypeAttribute(this Type type, Func<DisplayAttribute, string?> propertyAccessor)
    {
        DisplayAttribute? displayAttribute = type.GetTypeAttribute<DisplayAttribute>();
        return displayAttribute != null ? propertyAccessor(displayAttribute) : null;
    }

    /// <summary>
    /// Returns the matching attribute from the specified property if found.
    /// </summary>
    /// <typeparam name="TModel">The datatype of the property to check.</typeparam>
    /// <typeparam name="TAttribute">The datatype of the attribute to return.</typeparam>
    /// <param name="property">The property to check.</param>
    public static TAttribute? GetPropertyAttribute<TModel, TAttribute>(this Expression<Func<TModel>> property) where TAttribute : Attribute
	{
		var expression = (MemberExpression)property.Body;
		var attribute = expression.Member.GetCustomAttribute(typeof(TAttribute)) as TAttribute;

		return attribute;
	}

    /// <summary>
    /// Returns the value of a specified property from the <see cref="DisplayAttribute"/> of a provided property if found.
    /// </summary>
    /// <typeparam name="TModel">The datatype of the property to check.</typeparam>
    /// <param name="property">The property to check.</param>
    /// <param name="propertyAccessor">A function to access the desired property from the <see cref="DisplayAttribute"/>.</param>
    /// <returns>The value of the specified property from the <see cref="DisplayAttribute"/> if found; otherwise, null.</returns>
    public static string? GetPropertyAttribute<TModel>(this Expression<Func<TModel>> property, Func<DisplayAttribute, string?> propertyAccessor)
    {
        var displayAttribute = property.GetPropertyAttribute<TModel, DisplayAttribute>();
        return displayAttribute != null ? propertyAccessor(displayAttribute) : null;
    }

    /// <summary>
    /// Returns the value of a specified property from the <see cref="DisplayAttribute"/> of a provided property if found.
    /// </summary>
    /// <param name="type">The type of the parent class of the property.</param>
    /// <param name="propertyName">The name of the property in the parent class.</param>
    /// <param name="propertyAccessor">A function to access the desired property from the <see cref="DisplayAttribute"/>.</param>
    /// <returns>The value of the specified property from the <see cref="DisplayAttribute"/> if found; otherwise, null.</returns>
    public static string? GetPropertyAttribute(this Type type, string propertyName, Func<DisplayAttribute, string?> propertyAccessor)
    {
        var property = type.GetProperty(propertyName);

        if (property == null)
            return null;

        var displayAttribute = property.GetCustomAttribute<DisplayAttribute>();
        return displayAttribute != null ? propertyAccessor(displayAttribute) : null;
    }

    /// <summary>
    /// Returns the matching attribute from the specified value if found.
    /// </summary>
    /// <typeparam name="TModel">The datatype of the property to check.</typeparam>
    /// <typeparam name="TAttribute">The datatype of the attribute to return.</typeparam>
    /// <param name="value">The value to check.</param>
    public static TAttribute? GetValueAttribute<TModel, TAttribute>(this TModel value) where TAttribute : Attribute
	{
		var memberName = value?.ToString();

		if (value == null || string.IsNullOrWhiteSpace(memberName))
			return null;

		var attribute = value.GetType()
			.GetMember(memberName)
			.FirstOrDefault()
			?.GetCustomAttribute<TAttribute>();

		return attribute;
	}

    /// <summary>
    /// Returns the value of a specified property from the <see cref="DisplayAttribute"/> of a provided property value if found.
    /// </summary>
    /// <typeparam name="TModel">The type of the property.</typeparam>
    /// <param name="value">The value to find the property of.</param>
    /// <param name="propertyAccessor">A function to access the desired property from the <see cref="DisplayAttribute"/>.</param>
    /// <returns>The value of the specified property from the <see cref="DisplayAttribute"/> if found; otherwise, null.</returns>
    public static string? GetValueAttribute<TModel>(this TModel value, Func<DisplayAttribute, string?> propertyAccessor)
    {
        DisplayAttribute? displayAttribute = value.GetValueAttribute<TModel, DisplayAttribute>();
        return displayAttribute != null ? propertyAccessor(displayAttribute) : null;
    }

    /// <summary>
    /// Returns the value of a specified property from the <see cref="DisplayAttribute"/> of a provided property if found.
    /// </summary>
    /// <param name="type">The type of the parent class of the property.</param>
    /// <param name="memberName">The name of the member in the parent class.</param>
    /// <param name="propertyAccessor">A function to access the desired property from the <see cref="DisplayAttribute"/>.</param>
    /// <returns>The value of the specified property from the <see cref="DisplayAttribute"/> if found; otherwise, null.</returns>
    public static string? GetValueAttribute(this Type type, string memberName, Func<DisplayAttribute, string?> propertyAccessor)
    {
        var member = type.GetMember(memberName).FirstOrDefault();

        if (member == null)
            return null;

        var displayAttribute = member.GetCustomAttribute<DisplayAttribute>();
        return displayAttribute != null ? propertyAccessor(displayAttribute) : null;
    }
}
