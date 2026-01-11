# Info for Claude

## Purpose

This is a project to manage family history (genealogy).
The source of truth for all data will be text files (markdown, json, yaml, etc).
At some point, I will need to leverage some sort of data store (perhaps Bleve and/or SQLite) to index all the files and store a "cache".

Note that this is my first project written in go, so this is a learning project, and some amount of coaching is appreciated.
I have almost 40 years experience as a developer, so I just need guidance on the nuances of go, not coding in general.


## Coding Standards

All code should be run through `gofmt` to keep the coding style consistent.

To build the project, use `make clean build`.


## Project Layout

* `main.go` - the main program
   * `app` - the application that leverages the other packages to run the app
   * `ui` - the package to build the TUI. Uses a "view model" type paradigm, where each view has a model specific to that page, distinct from the main data model.
   * `core` - the main data models and code to leverage them


