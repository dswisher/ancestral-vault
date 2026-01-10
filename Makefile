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

.PHONY: all build install clean test release samples

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


# Copy select sample files over from my real vault
samples:
	cp ../family/evidence/census/1930-mahaska-dunwoody-walter.json sample/evidence/census
	cp ../family/evidence/census/1940-mahaska-dunwoody-walter.json sample/evidence/census
	cp ../family/evidence/census/1950-mahaska-dunwoody-emma.json sample/evidence/census
	cp ../family/evidence/marriages/1910-mahaska-walter-dunwoody.json sample/evidence/marriages

