# WeatherForecast CRUD Implementation Guide

> **Repository:** [BlazorCleanArchitectureTemplate](https://github.com/rZulan/BlazorCleanArchitectureTemplate)  
> This guide documents the complete CRUD implementation using WeatherForecast as a working example.

## Overview

A complete CRUD (Create, Read, Update, Delete) system for WeatherForecast has been implemented across all clean architecture layers with full Blazor UI integration.

## Architecture Implementation

### 1. **Domain Layer** - Core Business Entity
**File:** `Domain/Entities/WeatherForecast.cs`

```csharp
public class WeatherForecast : BaseEntity
{
	public DateTime Date { get; set; }
	public int TemperatureCelsius { get; set; }
	public int TemperatureFahrenheit { get; } // Read-only calculated property
	public string Summary { get; set; }
	public string Condition { get; set; }
	public double Humidity { get; set; }
	public double WindSpeed { get; set; }
	public string Location { get; set; }
}
```

### 2. **Application Layer** - Business Logic & DTOs

#### DTO Definition
**File:** `Application/DTO/WeatherForecast/WeatherForecastDto.cs`

Data Transfer Object with all properties for API responses.

#### Query Handlers (Read Operations)

**GetAllWeatherForecastsQuery** - `Application/Features/WeatherForecasts/Queries/GetAllWeatherForecastsQuery.cs`
- Retrieves all weather forecasts
- Returns forecasts sorted by date (descending)
- Maps entities to DTOs

**GetWeatherForecastByIdQuery** - `Application/Features/WeatherForecasts/Queries/GetWeatherForecastByIdQuery.cs`
- Retrieves a single forecast by ID
- Returns 404 if not found
- Includes proper error handling

#### Command Handlers (Write Operations)

**CreateWeatherForecastCommand** - `Application/Features/WeatherForecasts/Commands/CreateWeatherForecastCommand.cs`
- Validates input (Summary, Location, Date required)
- Creates new entity
- Returns created forecast as DTO

**UpdateWeatherForecastCommand** - `Application/Features/WeatherForecasts/Commands/UpdateWeatherForecastCommand.cs`
- Validates all required fields
- Updates existing forecast
- Sets UpdatedAt timestamp automatically
- Returns 404 if not found

**DeleteWeatherForecastCommand** - `Application/Features/WeatherForecasts/Commands/DeleteWeatherForecastCommand.cs`
- Permanently deletes a forecast
- Returns 404 if not found
- Returns success message on completion

### 3. **Infrastructure Layer** - Data Access

#### Database Context
**File:** `Infrastructure/Data/ApplicationDbContext.cs`

Configured with:
- WeatherForecast DbSet
- Proper entity constraints (max lengths, precisions)
- Default values (CreatedAt, IsActive)
- Indexes on Date, Location, IsActive for query performance

#### Generic Repository Implementation
**File:** `Infrastructure/Repositories/Repository.cs`

Provides data access for any entity inheriting from BaseEntity:
```csharp
Repository<WeatherForecast> operations include:
- GetByIdAsync(id)
- GetAllAsync()
- AddAsync(entity)
- UpdateAsync(entity) - sets UpdatedAt
- DeleteAsync(entity)
- SaveChangesAsync()
```

### 4. **Presentation Layer** - Blazor Components

#### List Component
**File:** `IMS/Components/WeatherForecasts/WeatherForecastList.razor`

Features:
- ✅ Displays all forecasts in a responsive table
- ✅ Shows temperature in both Celsius and Fahrenheit
- ✅ Edit button for each forecast
- ✅ Delete confirmation modal
- ✅ "Create New" button linking to create page
- ✅ Loading indicator
- ✅ Success/Error message alerts
- ✅ Empty state messaging
- ✅ Route: `/weather-forecasts`

#### Create Component
**File:** `IMS/Components/WeatherForecasts/WeatherForecastCreate.razor`

Features:
- ✅ Form with validation
- ✅ Date picker
- ✅ Location field (required)
- ✅ Temperature input (Celsius)
- ✅ Condition dropdown (Sunny, Cloudy, Rainy, Snowy, Stormy, Foggy)
- ✅ Summary textarea (required)
- ✅ Humidity percentage field (0-100)
- ✅ Wind speed input
- ✅ Submit/Cancel buttons
- ✅ Loading state feedback
- ✅ Error display
- ✅ Redirect to list on success
- ✅ Route: `/weather-forecasts/create`

#### Edit Component
**File:** `IMS/Components/WeatherForecasts/WeatherForecastEdit.razor`

Features:
- ✅ Loads existing forecast data
- ✅ Pre-populates all form fields
- ✅ Same form validation as Create
- ✅ Shows CreatedAt and UpdatedAt timestamps
- ✅ Updates forecast on submit
- ✅ Error handling for not found
- ✅ Loading indicator
- ✅ Route: `/weather-forecasts/edit/{id}`

### 5. **Navigation Integration**

**File:** `IMS/Components/Layout/NavMenu.razor`

Added navigation link:
```razor
<NavLink class="nav-link" href="weather-forecasts">
	<span class="bi bi-cloud-sun-nav-menu"></span> Weather Forecasts
</NavLink>
```

## Database Setup

### Initial Setup Required

1. **Create appsettings.json** in IMS project:
```json
{
  "ConnectionStrings": {
	"DefaultConnection": "Server=your_server;Database=IMSDatabase;Trusted_Connection=true;"
  }
}
```

2. **Run EF Core Migrations:**
```powershell
cd C:\Users\analuz\source\repos\Testing\

# Add initial migration
dotnet ef migrations add InitialCreate `
  --project Infrastructure `
  --startup-project IMS

# Apply migration to database
dotnet ef database update `
  --project Infrastructure `
  --startup-project IMS
```

3. **Connection String Formats:**
   - **Local SQL Server:** `Server=(localdb)\\mssqllocaldb;Database=IMSDatabase;Trusted_Connection=true;`
   - **Named Instance:** `Server=.\SQLEXPRESS;Database=IMSDatabase;Trusted_Connection=true;`
   - **Remote Server:** `Server=your_server;Database=IMSDatabase;User Id=sa;Password=your_password;`

## File Structure Created

```
Domain/
├── Entities/
│   └── WeatherForecast.cs

Application/
├── DTO/
│   └── WeatherForecast/
│       └── WeatherForecastDto.cs
├── Features/
│   └── WeatherForecasts/
│       ├── Queries/
│       │   ├── GetAllWeatherForecastsQuery.cs
│       │   └── GetWeatherForecastByIdQuery.cs
│       └── Commands/
│           ├── CreateWeatherForecastCommand.cs
│           ├── UpdateWeatherForecastCommand.cs
│           └── DeleteWeatherForecastCommand.cs
└── Interfaces/
	└── IRepository.cs

Infrastructure/
├── Data/
│   └── ApplicationDbContext.cs
└── Repositories/
	└── Repository.cs

IMS/
├── Components/
│   ├── WeatherForecasts/
│   │   ├── WeatherForecastList.razor
│   │   ├── WeatherForecastCreate.razor
│   │   └── WeatherForecastEdit.razor
│   └── Layout/
│       └── NavMenu.razor (updated)
```

## Usage Routes

| Route | Component | Purpose |
|-------|-----------|---------|
| `/weather-forecasts` | WeatherForecastList | Display all forecasts |
| `/weather-forecasts/create` | WeatherForecastCreate | Create new forecast |
| `/weather-forecasts/edit/{id}` | WeatherForecastEdit | Edit existing forecast |

## Build Status

✅ **Build Success** - All 4 projects compile without errors
- Domain.dll ✓
- Application.dll ✓
- Infrastructure.dll ✓
- IMS.dll ✓

## Architecture Compliance

This implementation strictly follows clean architecture principles:

✅ **Domain Layer** - Contains only entity with no external dependencies
✅ **Application Layer** - Contains business logic, interfaces, and handlers; depends only on Domain
✅ **Infrastructure Layer** - Contains data access; depends on Application and Domain
✅ **Presentation Layer** - Contains UI; depends on all layers through DI

## Next Steps

1. **Set up database connection** in `appsettings.json`
2. **Create and apply migrations** using EF Core commands
3. **Test the application** by running IMS project
4. **Add more features** following the same pattern
5. **Implement validation** using FluentValidation if needed
6. **Add logging** using Serilog

## Testing the CRUD

### Via Blazor UI:
1. Navigate to `/weather-forecasts`
2. Click "New Forecast"
3. Fill in the form and submit
4. Click Edit to modify
5. Click Delete to remove

### Via MediatR Directly (in unit tests):
```csharp
// Create
var createCmd = new CreateWeatherForecastCommand 
{ 
	Date = DateTime.Now,
	Location = "New York",
	Summary = "Sunny and warm",
	TemperatureCelsius = 25
};
var result = await mediator.Send(createCmd);

// Read All
var allQuery = new GetAllWeatherForecastsQuery();
var forecasts = await mediator.Send(allQuery);

// Read One
var getQuery = new GetWeatherForecastByIdQuery(1);
var forecast = await mediator.Send(getQuery);

// Update
var updateCmd = new UpdateWeatherForecastCommand
{
	Id = 1,
	Location = "Los Angeles",
	// ... other properties
};
var updated = await mediator.Send(updateCmd);

// Delete
var deleteCmd = new DeleteWeatherForecastCommand(1);
var deleted = await mediator.Send(deleteCmd);
```

## Common Issues & Solutions

### Issue: "No EF migrations found"
**Solution:** Run `dotnet ef migrations add InitialCreate --project Infrastructure --startup-project IMS`

### Issue: "Connection string not found"
**Solution:** Add ConnectionStrings section to appsettings.json in IMS project

### Issue: "MediatR handlers not registered"
**Solution:** Ensure `AddApplicationServices()` is called in Program.cs

### Issue: "Database tables don't exist"
**Solution:** Run `dotnet ef database update --project Infrastructure --startup-project IMS`

## Performance Considerations

- Forecasts are **sorted by date descending** for latest first
- **Indexes** on Date, Location, IsActive for fast queries
- **AsNoTracking()** on read queries for better performance
- **UpdatedAt timestamp** automatically set on updates

## Future Enhancements

Potential features to add:
- [ ] Search/filter by location or date range
- [ ] Export to CSV/Excel
- [ ] Weather forecast analytics/charts
- [ ] API endpoint for mobile apps
- [ ] Real-time forecast updates
- [ ] User permissions/authorization
- [ ] Forecast attachments (images)
- [ ] Notifications for weather alerts
