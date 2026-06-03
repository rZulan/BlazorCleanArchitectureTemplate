# Quick Start Guide - Adding Features to Clean Architecture

## File Template: Query Handler

Create a new file: `Application/Features/{FeatureName}/Queries/Get{FeatureName}Query.cs`

```csharp
using Application.DTO.{FeatureName};
using Application.Common.Results;
using Domain.Entities;
using Infrastructure.Repositories;
using MediatR;

namespace Application.Features.{FeatureName}.Queries;

public class Get{FeatureName}Query : IRequest<Result<List<{FeatureName}Dto>>>
{
}

public class Get{FeatureName}QueryHandler : IRequestHandler<Get{FeatureName}Query, Result<List<{FeatureName}Dto>>>
{
	private readonly IRepository<{FeatureName}> _repository;

	public Get{FeatureName}QueryHandler(IRepository<{FeatureName}> repository)
	{
		_repository = repository;
	}

	public async Task<Result<List<{FeatureName}Dto>>> Handle(Get{FeatureName}Query request, CancellationToken cancellationToken)
	{
		try
		{
			var items = await _repository.GetAllAsync();
			var dtos = items.Select(item => new {FeatureName}Dto
			{
				Id = item.Id,
				// Map properties here
			}).ToList();

			return Result<List<{FeatureName}Dto>>.Success(dtos);
		}
		catch (Exception ex)
		{
			return Result<List<{FeatureName}Dto>>.InternalServerError($"Error retrieving {FeatureName}: {{ex.Message}}");
		}
	}
}
```

## File Template: Command Handler

Create a new file: `Application/Features/{FeatureName}/Commands/Create{FeatureName}Command.cs`

```csharp
using Application.DTO.{FeatureName};
using Application.Common.Results;
using Domain.Entities;
using Infrastructure.Repositories;
using MediatR;

namespace Application.Features.{FeatureName}.Commands;

public class Create{FeatureName}Command : IRequest<Result<{FeatureName}Dto>>
{
	public string Name { get; set; } = string.Empty;
	// Add other properties
}

public class Create{FeatureName}CommandHandler : IRequestHandler<Create{FeatureName}Command, Result<{FeatureName}Dto>>
{
	private readonly IRepository<{FeatureName}> _repository;

	public Create{FeatureName}CommandHandler(IRepository<{FeatureName}> repository)
	{
		_repository = repository;
	}

	public async Task<Result<{FeatureName}Dto>> Handle(Create{FeatureName}Command request, CancellationToken cancellationToken)
	{
		try
		{
			// Validate
			if (string.IsNullOrEmpty(request.Name))
				return Result<{FeatureName}Dto>.BadRequest("Name is required");

			// Create entity
			var entity = new {FeatureName}
			{
				Name = request.Name,
				// Set other properties
			};

			// Save
			await _repository.AddAsync(entity);
			await _repository.SaveChangesAsync();

			var dto = new {FeatureName}Dto
			{
				Id = entity.Id,
				Name = entity.Name,
				// Map other properties
			};

			return Result<{FeatureName}Dto>.Success(dto, "{FeatureName} created successfully");
		}
		catch (Exception ex)
		{
			return Result<{FeatureName}Dto>.InternalServerError($"Error creating {FeatureName}: {{ex.Message}}");
		}
	}
}
```

## File Template: DTO

Create a new file: `Application/DTO/{FeatureName}/{FeatureName}Dto.cs`

```csharp
namespace Application.DTO.{FeatureName};

public class {FeatureName}Dto
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public DateTime CreatedAt { get; set; }
	public DateTime? UpdatedAt { get; set; }
	public bool IsActive { get; set; }
	// Add other properties
}
```

## File Template: Entity

Create a new file: `Domain/Entities/{FeatureName}.cs`

```csharp
using Domain.Common;

namespace Domain.Entities;

public class {FeatureName} : BaseEntity
{
	public string Name { get; set; } = string.Empty;
	// Add other properties
}
```

## File Template: Blazor Component

Create a new file: `IMS/Components/{FeatureName}/{FeatureName}List.razor`

```razor
@page "/{featurename}"
@inject IMediator Mediator

<PageTitle>{FeatureName}</PageTitle>

<h3>{FeatureName}</h3>

@if (items == null)
{
	<p>Loading...</p>
}
else if (items.Count == 0)
{
	<p>No {FeatureName} found.</p>
}
else
{
	<table class="table">
		<thead>
			<tr>
				<th>Name</th>
				<th>Created</th>
				<th>Actions</th>
			</tr>
		</thead>
		<tbody>
			@foreach (var item in items)
			{
				<tr>
					<td>@item.Name</td>
					<td>@item.CreatedAt</td>
					<td>
						<button class="btn btn-primary btn-sm">Edit</button>
						<button class="btn btn-danger btn-sm">Delete</button>
					</td>
				</tr>
			}
		</tbody>
	</table>
}

@code {
	private List<{FeatureName}Dto>? items;

	protected override async Task OnInitializedAsync()
	{
		var result = await Mediator.Send(new Get{FeatureName}Query());
		if (result.IsSuccess)
		{
			items = result.Data ?? [];
		}
	}
}
```

## Common Commands

### Add a new feature project to solution
```powershell
cd C:\Users\analuz\source\repos\Testing\
dotnet build IMS.slnx
```

### Restore packages
```powershell
dotnet restore IMS.slnx
```

### Run migrations (when DbContext is set up)
```powershell
dotnet ef migrations add InitialCreate --project Infrastructure --startup-project IMS
dotnet ef database update --project Infrastructure --startup-project IMS
```

### Run the application
```powershell
dotnet run --project IMS
```

## File Checklist for New Feature

- [ ] Entity in `Domain/Entities/{FeatureName}.cs`
- [ ] DTO in `Application/DTO/{FeatureName}/{FeatureName}Dto.cs`
- [ ] Query in `Application/Features/{FeatureName}/Queries/Get{FeatureName}Query.cs`
- [ ] Create Command in `Application/Features/{FeatureName}/Commands/Create{FeatureName}Command.cs`
- [ ] Update Command in `Application/Features/{FeatureName}/Commands/Update{FeatureName}Command.cs`
- [ ] Delete Command in `Application/Features/{FeatureName}/Commands/Delete{FeatureName}Command.cs`
- [ ] Blazor List Component in `IMS/Components/{FeatureName}/{FeatureName}List.razor`
- [ ] Blazor Create Component in `IMS/Components/{FeatureName}/{FeatureName}Create.razor`
- [ ] Blazor Edit Component in `IMS/Components/{FeatureName}/{FeatureName}Edit.razor`
