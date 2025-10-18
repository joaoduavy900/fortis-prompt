# Random Dates Test

Automated testing for random.org calendar dates generator.

## Requirements

- Generate 4 random dates between specified ranges
- Validate dates are within the correct range
- Assert the correct count of generated dates

## How to run

### Using Docker
```bash
make test
```

### Using .NET directly
```bash
dotnet test
```

## Available commands

- `make test` - Run tests in Docker
- `make clean` - Clean Docker resources
- `make help` - Show all commands