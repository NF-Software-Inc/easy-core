using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;

namespace easy_core;

/// <summary>
/// Provides an attribute that compares one property is less than or equal to another.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class LessThanOrEqualAttribute : ValidationAttribute
{
	[RequiresUnreferencedCode("The property referenced by 'otherProperty' may be trimmed. Ensure it is preserved.")]
	public LessThanOrEqualAttribute(string otherProperty) : base("{0} must be less than or equal to {1}.")
	{
		ArgumentNullException.ThrowIfNull(otherProperty);
		OtherProperty = otherProperty;
	}

	/// <summary>
	/// Specifies whether to skip validation when one or both properties are null.
	/// </summary>
	public bool IgnoreNull { get; set; }

	/// <summary>
	/// Stores the name of the property to compare against.
	/// </summary>
	public string OtherProperty { get; }

	/// <summary>
	/// Stores the display name of the property to compare against.
	/// </summary>
	public string? OtherPropertyDisplayName { get; internal set; }

	/// <inheritdoc />
	public override bool RequiresValidationContext => true;

	/// <inheritdoc />
	public override string FormatErrorMessage(string name) => string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, OtherPropertyDisplayName ?? OtherProperty);

	/// <inheritdoc />
	protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
	{
		var property = validationContext.ObjectType.GetRuntimeProperty(OtherProperty);

		if (property == null)
			return new ValidationResult("Could not find property to compare against.");
		else if (property.GetIndexParameters().Length > 0)
			throw new ArgumentException("Could not find property to compare against.");

		var otherValue = property.GetValue(validationContext.ObjectInstance, null);

		if (IgnoreNull && (value == null || otherValue == null))
			return null;

		if (Comparer<object>.Default.Compare(value, otherValue) > 0)
		{
			OtherPropertyDisplayName ??= property.GetPropertyDisplayName();
			string[]? members = validationContext.MemberName != null ? [validationContext.MemberName] : null;

			return new ValidationResult(FormatErrorMessage(validationContext.DisplayName), members);
		}

		return null;
	}
}
