# Easy Core

[![MIT](https://img.shields.io/github/license/NF-Software-Inc/easy-core)](https://github.com/NF-Software-Inc/easy-core/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/Easy.Base.svg)](https://www.nuget.org/packages/Easy.Base/)
[![Build](https://img.shields.io/github/actions/workflow/status/NF-Software-Inc/easy-core/build.yml)](https://github.com/NF-Software-Inc/easy-core/actions/workflows/build.yml)
[![Publish](https://img.shields.io/github/actions/workflow/status/NF-Software-Inc/easy-core/publish.yml?label=publish)](https://github.com/NF-Software-Inc/easy-core/actions/workflows/publish.yml)

## Table of Contents

- [Overview](#overview)
- [Versioning Notice](#versioning-notice)
- [Quick Start](#quick-start)
- [Features](#features)
- [Development Setup](#development-setup)
- [Supported Frameworks](#supported-frameworks)
- [Contributing](#contributing)
- [Authors](#authors)
- [License](#license)
- [Acknowledgments](#acknowledgments)

## Overview

Easy Core is a .NET utility library that provides reusable components for common application development tasks. It includes extension methods, security services, validation attributes, converters, and utility helpers to simplify and accelerate modern .NET application development.

## Versioning Notice

As of version 10.0.0, Easy Core uses a versioning scheme aligned with the .NET major version it targets:

- **Easy Core 10.x.x** includes a **.NET 10.x** target (and multi-targets **.NET 8/9** for compatibility)
- Major version = .NET major version
- Minor and patch versions for library-specific updates

## Quick Start

### Installation

Install via NuGet:

```
dotnet add package Easy.Base
```

Or manually add to your `.csproj` file:

```xml
<ItemGroup>
  <PackageReference Include="Easy.Base" Version="10.0.*" />
</ItemGroup>
```

### Basic Usage Examples

**String Extensions:**
```csharp
using easy_core;

var encoded = "Hello".Base64Encode();  // Encode to Base64
var decoded = encoded.Base64Decode();   // Decode from Base64
var csv = "Hello, World".ToCsvString(); // Format for CSV
```

**Collection Extensions:**
```csharp
var numbers = new[] { 1, 2, 3, 4, 5 };

// Check if null or empty
if (numbers.IsNotNullOrEmpty())
{
    // Use collection
}

// LINQ operations
var filtered = numbers.ExceptBy(new[] { 2, 4 }, x => x);
```

**Encryption & Hashing:**
```csharp
// Encrypt sensitive data
var key = EncryptionService.NewKey();
var encrypted = EncryptionService.Encrypt("sensitive", key);
var decrypted = EncryptionService.Decrypt(encrypted, key);

// Hash passwords
var hash = HashingService.CreateHash("password");
var isValid = HashingService.VerifyHash("password", hash);
```

**OTP Generation:**
```csharp
// Generate time-based one-time passwords for 2FA
var secret = OtpService.GenerateSecret();
var otp = OtpService.GenerateOtp(secret);
```

## Features

### Extension Methods

Convenient extension methods for common operations:

- **String Extensions** - Base64 encoding/decoding, CSV formatting, string manipulation
- **Collection Extensions** - LINQ operations, null checking, filtering helpers
- **Date & Time Extensions** - Date calculations, time zone handling, ISO week operations
- **Enum Extensions** - Flag checking, enum value retrieval with attributes
- **IP Address Extensions** - IP range calculations, address parsing
- **Stream Extensions** - Stream reading and copying utilities
- **Attribute Extensions** - Reflection-based attribute value retrieval

### Security Tools

Cryptography and security utilities:

- **EncryptionService** - AES encryption/decryption with configurable settings
- **HashingService** - PBKDF2 password hashing with salt and iteration support
- **OtpService** - Time-based one-time password (TOTP) generation and validation
- **CertificateBuilder** - SSL certificate generation and validation

### Utility Tools

Helper classes for common operations:

- **MimeTypeMap** - Comprehensive MIME type lookups from file extensions
- **DriveMapper** - Drive letter and volume management
- **PredicateBuilder** - Dynamic LINQ query construction

### Validation Attributes

Custom data validation attributes:

- **CompareAgainstAttribute** - Compare field values
- **GreaterThanAttribute** - Numeric comparison validation
- **RequiredIfNullAttribute** / **RequiredIfNotNullAttribute** - Conditional field requirements
- **LessThanAttribute** - Maximum value validation

### Converters

Type conversion utilities for common scenarios.

## Development Setup

### Prerequisites

- .NET 8.0 SDK or later
- Visual Studio 2022 or Visual Studio Code (with C# extension)

### Building the Library

```bash
# Clone the repository
git clone https://github.com/NF-Software-Inc/easy-core.git
cd easy-core

# Build the project
dotnet build

# Run tests
dotnet test

# Create NuGet package
dotnet pack --configuration Release
```

### Project Structure

```
Easy-Core/
├── Attributes/      # Custom validation attributes
├── Converters/      # Type conversion utilities
├── Extensions/      # Extension methods for common types
├── Interfaces/      # Service interfaces and abstractions
├── Models/          # Supporting model classes
├── Tools/           # Utility and security service classes
└── Enums/           # Enum definitions
```

## Supported Frameworks

- .NET 8.0
- .NET 9.0
- .NET 10.0

## Contributing

We welcome contributions! To contribute:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## Authors

* **NF Software Inc.**

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details

## Acknowledgments

Thank you to:
* [Kmg Design](https://www.iconfinder.com/kmgdesignid) for the project icon
