using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;

namespace easy_core;

/// <summary>
/// Provides an attribute that compares two properties are inequal.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class CompareAgainstAttribute : ValidationAttribute
{
	[RequiresUnreferencedCode("The property referenced by 'otherProperty' may be trimmed. Ensure it is preserved.")]
	public CompareAgainstAttribute(string otherProperty) : base("{0} and {1} cannot be the same.")
	{
		ArgumentNullException.ThrowIfNull(otherProperty);
		OtherProperty = otherProperty;
	}

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

		if (EqualityComparer<object>.Default.Equals(value, otherValue))
		{
			OtherPropertyDisplayName ??= property.GetPropertyDisplayName();
			string[]? members = validationContext.MemberName != null ? [validationContext.MemberName] : null;

			return new ValidationResult(FormatErrorMessage(validationContext.DisplayName), members);
		}

		return null;
	}
}
