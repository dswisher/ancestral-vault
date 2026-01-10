package ui

import "time"

// Person represents an individual in the family tree
type Person struct {
	Name       string
	BirthDate  *time.Time
	BirthPlace string
	DeathDate  *time.Time
	DeathPlace string
}

// Family represents a family unit with parents and children
type Family struct {
	Father   *Person
	Mother   *Person
	Children []*Person
}
