# IMS Blazor Clean Architecture Documentation

## Overview

The IMS Blazor project has been restructured following **Uncle Bob's Clean Architecture** principles with four distinct layers:

```
┌─────────────────────────────────────────────────────────────┐
│                    IMS (Presentation)                       │
│              Blazor Components & Pages                      │
└────────────────────────────┬────────────────────────────────┘
							 │
							 ▼
┌─────────────────────────────────────────────────────────────┐
│                  Application Layer                          │
│   DTOs, MediatR Handlers, Business Logic & Interfaces      │
└────────────────────────────┬────────────────────────────────┘
							 │
							 ▼
┌─────────────────────────────────────────────────────────────┐
│                Infrastructure Layer                         │
│   Repositories, DbContext, External Services               │
└────────────────────────────┬────────────────────────────────┘
							 │
							 ▼
┌─────────────────────────────────────────────────────────────┐
│                     Domain Layer                            │
│         Core Entities, Enums, Business Rules               │
└─────────────────────────────────────────────────────────────┘
```

## Layer Responsibilities

### 1. **Domain Layer** (`Domain/`)
The innermost layer - contains only core business entities and enums.

**Files:**
- `Common/BaseEntity.cs` - Base class for all domain entities
- `Enums/ResultStatus.cs` - Operation result status enum

**Characteristics:**
- ✅ No external dependencies
- ✅ Pure business rules
- ✅ Framework agnostic

**Use for:**
- Entity definitions
- Business enums
- Value objects
- Domain exceptions

---

### 2. **Application Layer** (`Application/`)
Contains application services, DTOs, MediatR handlers, and business logic orchestration.

**Files:**
- `Common/Results/Result.cs` - Generic result wrapper for operations
- `Common/Behaviors/LoggingBehavior.cs` - MediatR pipeline behavior
- `Extensions/ServiceCollectionExtensions.cs` - DI configuration

**Project Structure:**
```
Application/
├── Common/
│   ├── Behaviors/
│   └── Results/
├── DTO/
│   ├── Category/
│   ├── Product/
│   └── ...
├── Features/
│   ├── Categories/
│   │   ├── Queries/
│   │   └── Commands/
│   ├── Products/
│   └── ...
└── Interfaces/
	└── IRepository.cs
```

**Characteristics:**
- ✅ Depends on Domain only
- ✅ Contains CQRS pattern (Queries & Commands)
- ✅ Orchestrates business operations

**Use for:**
- CQRS Query handlers
- CQRS Command handlers
- DTOs (Data Transfer Objects)
- Application interfaces
- MediatR pipeline behaviors

---

### 3. **Infrastructure Layer** (`Infrastructure/`)
Handles data access, external services, and technical implementation details.

**Files:**
- `Repositories/IRepository.cs` - Generic repository interface
- `Repositories/Repository.cs` - Generic repository implementation
- `Extensions/ServiceCollectionExtensions.cs` - DI configuration

**Project Structure:**
```
Infrastructure/
├── Data/
│   └── ApplicationDbContext.cs
├── Migrations/
├── Repositories/
│   ├── IRepository.cs
│   ├── Repository.cs
│   └── [Feature]Repository.cs
├── Services/
│   └── [ExternalService].cs
└── Extensions/
	└── ServiceCollectionExtensions.cs
```

**Characteristics:**
- ✅ Depends on Domain and Application
- ✅ Implements data access patterns
- ✅ Integrates external services

**Use for:**
- DbContext configuration
- Repository implementations
- External service integrations
- Entity Framework migrations
- Background services

---

### 4. **IMS Presentation Layer** (`IMS/`)
Blazor UI components and pages that consume the Application layer through MediatR.

**Files:**
- `Program.cs` - Startup configuration
- `Extensions/PresentationExtensions.cs` - Blazor-specific DI
- `Components/` - Blazor components by feature
- `wwwroot/` - Static assets

**Project Structure:**
```
IMS/
├── Components/
│   ├── Dashboard/
│   ├── Products/
│   ├── Categories/
│   └── [Feature]/
├── Pages/
├── Layouts/
├── Services/
├── Extensions/
├── wwwroot/
└── Program.cs
```

**Characteristics:**
- ✅ Depends on all layers
- ✅ Contains UI logic only
- ✅ Communicates via MediatR

**Use for:**
- Blazor component definitions
- Page layouts
- UI services (state management, notifications)
- Static assets styling

---

## Key Patterns

### 1. CQRS Pattern (Command Query Responsibility Segregation)

**Queries** - Read operations
```csharp
// Application/Features/Products/Queries/GetAllProductsQuery.cs
public class GetAllProductsQuery : IRequest<Result<List<ProductDto>>>
{
}

public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, Result<List<ProductDto>>>
{
	private readonly IRepository<Product> _repository;

	public GetAllProductsQueryHandler(IRepository<Product> repository)
	{
		_repository = repository;
	}

	public async Task<Result<List<ProductDto>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
	{
		var products = await _repository.GetAllAsync();
		return Result<List<ProductDto>>.Success(/* map to DTO */);
	}
}
```

**Commands** - Write operations
```csharp
// Application/Features/Products/Commands/CreateProductCommand.cs
public class CreateProductCommand : IRequest<Result<ProductDto>>
{
	public string Name { get; set; } = string.Empty;
}

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<ProductDto>>
{
	private readonly IRepository<Product> _repository;

	public CreateProductCommandHandler(IRepository<Product> repository)
	{
		_repository = repository;
	}

	public async Task<Result<ProductDto>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
	{
		// Validate, create entity, save
		await _repository.AddAsync(product);
		await _repository.SaveChangesAsync();
		return Result<ProductDto>.Success(/* mapped DTO */);
	}
}
```

### 2. Result Pattern for Consistent Error Handling

```csharp
// Generic result with data
var result = await mediator.Send(new GetProductQuery(id));

if (result.IsSuccess)
{
	var product = result.Data;
}
else if (result.Status == ResultStatus.NotFound)
{
	// Handle not found
}
else
{
	// Handle error
}

// Non-generic result
var deleteResult = Result.Success("Product deleted successfully");
```

### 3. Repository Pattern for Data Access

```csharp
// Use generic repository
var product = await _repository.GetByIdAsync(id);
await _repository.UpdateAsync(product);
await _repository.SaveChangesAsync();
```

---

## Dependency Injection Setup

The DI container is configured in `IMS/Program.cs`:

```csharp
// Application layer services
builder.Services.AddApplicationServices();

// Infrastructure layer services
builder.Services.AddInfrastructureServices(connectionString);

// Presentation layer services
builder.Services.AddPresentationServices();
```

Each layer has its own `ServiceCollectionExtensions.cs` file for registering services:

- **Domain**: No DI (pure entities)
- **Application**: `AddApplicationServices()` - Registers MediatR
- **Infrastructure**: `AddInfrastructureServices()` - Registers DbContext and repositories
- **IMS**: `AddPresentationServices()` - Registers Blazor components

---

## Creating a New Feature

### Step 1: Define Entity in Domain Layer
```csharp
// Domain/Entities/Product.cs
public class Product : BaseEntity
{
	public string Name { get; set; } = string.Empty;
	public decimal Price { get; set; }
}
```

### Step 2: Create DTO in Application Layer
```csharp
// Application/DTO/Product/ProductDto.cs
public class ProductDto
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public decimal Price { get; set; }
}
```

### Step 3: Create Query Handler in Application Layer
```csharp
// Application/Features/Products/Queries/GetAllProductsQuery.cs
public class GetAllProductsQuery : IRequest<Result<List<ProductDto>>> { }

public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, Result<List<ProductDto>>>
{
	private readonly IRepository<Product> _repository;

	public GetAllProductsQueryHandler(IRepository<Product> repository)
	{
		_repository = repository;
	}

	public async Task<Result<List<ProductDto>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
	{
		var products = await _repository.GetAllAsync();
		var dtos = products.Select(p => new ProductDto 
		{ 
			Id = p.Id, 
			Name = p.Name, 
			Price = p.Price 
		}).ToList();

		return Result<List<ProductDto>>.Success(dtos);
	}
}
```

### Step 4: Create Blazor Component in IMS Layer
```razor
@* IMS/Components/Products/ProductList.razor *@
@inject IMediator Mediator

<h3>Products</h3>

@if (products == null)
{
	<p>Loading...</p>
}
else if (products.Count == 0)
{
	<p>No products found.</p>
}
else
{
	<table>
		<thead>
			<tr>
				<th>Name</th>
				<th>Price</th>
			</tr>
		</thead>
		<tbody>
			@foreach (var product in products)
			{
				<tr>
					<td>@product.Name</td>
					<td>@product.Price.ToString("C")</td>
				</tr>
			}
		</tbody>
	</table>
}

@code {
	private List<ProductDto>? products;

	protected override async Task OnInitializedAsync()
	{
		var result = await Mediator.Send(new GetAllProductsQuery());
		if (result.IsSuccess)
		{
			products = result.Data ?? [];
		}
	}
}
```

---

## Best Practices

### ✅ DO:
- ✅ Keep Domain layer pure (no external dependencies)
- ✅ Use MediatR for all business operations
- ✅ Return `Result<T>` for consistent error handling
- ✅ Use repositories for all data access
- ✅ Organize features by domain entity in Application layer
- ✅ Keep components focused on UI logic
- ✅ Inject MediatR into Blazor components

### ❌ DON'T:
- ❌ Add external packages to Domain layer
- ❌ Access DbContext directly from components
- ❌ Skip the Application layer handlers
- ❌ Reference Infrastructure directly from components
- ❌ Put business logic in Blazor components
- ❌ Create tight coupling between layers

---

## NuGet Packages Used

| Package | Layer | Version | Purpose |
|---------|-------|---------|---------|
| MediatR | Application, IMS | 14.1.0 | CQRS implementation |
| EntityFrameworkCore | Infrastructure | 10.0.0 | ORM |
| EntityFrameworkCore.SqlServer | Infrastructure | 10.0.0 | SQL Server provider |
| Microsoft.Extensions.DependencyInjection | All | 10.0.0 | Dependency injection |

---

## Next Steps

1. **Create your DbContext** in Infrastructure/Data/
2. **Add Entity Models** to Domain/Entities/
3. **Create DTOs** in Application/DTO/
4. **Implement Features** with Query/Command handlers in Application/Features/
5. **Build Blazor Components** in IMS/Components/
6. **Run Migrations** to set up your database

---

## Troubleshooting

### Build errors about missing projects?
- Run: `dotnet restore IMS.slnx`
- Reload the solution in Visual Studio

### Circular dependencies?
- Check that layers only depend on inner layers (not outward)
- Domain → should depend on nothing
- Application → should depend only on Domain
- Infrastructure → should depend only on Domain
- IMS → can depend on all

### MediatR handlers not found?
- Ensure handlers are in the correct assembly (Application)
- Verify `AddApplicationServices()` is called in Program.cs

---

## References

- [Clean Architecture by Uncle Bob](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [MediatR Documentation](https://github.com/jbogard/MediatR)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [Blazor Documentation](https://docs.microsoft.com/en-us/aspnet/core/blazor/)
