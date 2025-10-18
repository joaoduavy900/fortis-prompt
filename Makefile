.PHONY: build test clean help

# Default target
help:
	@echo "Available targets:"
	@echo "  build    - Build the Docker image"
	@echo "  test     - Run tests in Docker container"
	@echo "  clean    - Remove Docker image and containers"
	@echo "  help     - Show this help message"

# Build Docker image
build:
	docker build -t random-dates-tests .

# Run tests in Docker container
test: build
	docker run --rm random-dates-tests

# Clean up Docker resources
clean:
	docker rmi random-dates-tests 2>/dev/null || true
	docker system prune -f

# Run tests with verbose output
test-verbose: build
	docker run --rm random-dates-tests dotnet test --logger "console;verbosity=detailed"

# Run tests and show results
test-results: build
	docker run --rm -v $(PWD)/test-results:/app/RandomDates.Playwright/TestResults random-dates-tests
