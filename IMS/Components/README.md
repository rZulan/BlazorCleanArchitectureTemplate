## IMS Presentation Layer Folder Structure

This folder is the Presentation layer of the clean architecture. It contains:

### /Components
Blazor components organized by feature. Each feature should have its own subfolder:
```
Components/
  ├── Dashboard/
  ├── Products/
  ├── Orders/
  └── ...
```

### /Pages
Blazor pages mapped to routes

### /Layouts
Shared layouts for pages

### /Services
Blazor-specific services (local state management, notification services, etc.)

### /Extensions
Helper methods and extensions specific to the presentation layer

### /wwwroot
Static assets (CSS, JavaScript, images, etc.)

## Important Notes

1. **DO NOT** put business logic in components - always use MediatR handlers from the Application layer
2. **DO NOT** put database code in components - always use repositories from the Infrastructure layer
3. **DO NOT** reference Infrastructure directly from components - always go through Application layer (MediatR)
4. Components should only contain UI logic and call MediatR handlers

## Example Component Pattern

```csharp
@inject IMediator Mediator

<div>
	@if (products != null)
	{
		@foreach (var product in products)
		{
			<div>@product.Name</div>
		}
	}
</div>

@code {
	private IEnumerable<ProductDto>? products;

	protected override async Task OnInitializedAsync()
	{
		var result = await Mediator.Send(new GetAllProductsQuery());
		if (result.IsSuccess)
		{
			products = result.Data;
		}
	}
}
```
