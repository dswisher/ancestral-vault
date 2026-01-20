
COVERAGE_OPTS = /p:CollectCoverage=true /p:CoverletOutputFormat=opencover

.PHONY: test report open clean

test:
	dotnet test $(COVERAGE_OPTS)

report:
	dotnet reportgenerator -reports:coverage.opencover.xml -targetdir:coveragereport

open:
	open coveragereport/index.html

clean:
	dotnet clean
	rm -rf coveragereport

