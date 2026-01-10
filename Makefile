# Binary name
BINARY_NAME=av
BUILD_DIR=build

# Go parameters
GOCMD=go
GOBUILD=$(GOCMD) build
GOCLEAN=$(GOCMD) clean
GOTEST=$(GOCMD) test
GOINSTALL=$(GOCMD) install

# Find all Go source files
SOURCES=$(shell find . -name '*.go' -type f)

.PHONY: all build install clean test release help

# Default target
all: build

# Build for current platform - only rebuilds if source files changed
$(BUILD_DIR)/$(BINARY_NAME): $(SOURCES)
	@mkdir -p $(BUILD_DIR)
	$(GOBUILD) -o $(BUILD_DIR)/$(BINARY_NAME) -v

# Convenience target
build: $(BUILD_DIR)/$(BINARY_NAME)

# Install to GOPATH/bin (depends on build)
install: build
	$(GOINSTALL)

# Run tests
test:
	$(GOTEST) -v ./...

# Clean build artifacts
clean:
	$(GOCLEAN)
	rm -rf $(BUILD_DIR)

# Build for all platforms (use this for releases)
release: clean
	@mkdir -p $(BUILD_DIR)
	GOOS=darwin GOARCH=amd64 $(GOBUILD) -o $(BUILD_DIR)/$(BINARY_NAME)-darwin-amd64
	GOOS=darwin GOARCH=arm64 $(GOBUILD) -o $(BUILD_DIR)/$(BINARY_NAME)-darwin-arm64
	GOOS=linux GOARCH=amd64 $(GOBUILD) -o $(BUILD_DIR)/$(BINARY_NAME)-linux-amd64
	GOOS=windows GOARCH=amd64 $(GOBUILD) -o $(BUILD_DIR)/$(BINARY_NAME)-windows-amd64.exe

# Show available targets
help:
	@echo "Available targets:"
	@echo "  make build   - Build for current platform (only if sources changed)"
	@echo "  make install - Build and install to GOPATH/bin"
	@echo "  make test    - Run tests"
	@echo "  make clean   - Remove build artifacts"
	@echo "  make release - Build for all platforms (macOS, Linux, Windows)"
	@echo "  make help    - Show this help message"
