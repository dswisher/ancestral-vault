package ui

import (
	"fmt"
	"time"

	"av/internal/core"
)

// NewFamilyFromEvidence converts evidence into a Family view model.
// It intelligently analyzes the evidence structure to determine the best family view:
// - For census records (spouse/child relationships): shows nuclear family
// - For marriage records (parent/child relationships): shows family of origin
func NewFamilyFromEvidence(evidence *core.Evidence) (*Family, error) {
	if len(evidence.Personas) == 0 {
		return nil, fmt.Errorf("no personas found in evidence")
	}

	// Analyze the relationships to determine the type of family structure
	hasSpouseRelationship := hasRelationshipType(evidence, "wife", "husband")
	hasChildRelationship := hasRelationshipType(evidence, "son", "daughter")
	hasParentRelationship := hasRelationshipType(evidence, "father", "mother")

	if hasSpouseRelationship || hasChildRelationship {
		// This is likely a census record showing a nuclear family
		// (spouse + children, or just single parent + children)
		return buildNuclearFamily(evidence)
	} else if hasParentRelationship {
		// This is likely a marriage or birth record showing family of origin
		return buildFamilyOfOrigin(evidence)
	}

	// No clear family structure, just show what we have
	return buildFamilyOfOrigin(evidence)
}

// hasRelationshipType checks if the evidence contains any of the given relationship types
func hasRelationshipType(evidence *core.Evidence, types ...string) bool {
	for _, rel := range evidence.Relationships {
		for _, t := range types {
			if rel.Relationship == t {
				return true
			}
		}
	}
	return false
}

// buildNuclearFamily creates a family view for census-type records
// showing a head of household, spouse, and children
func buildNuclearFamily(evidence *core.Evidence) (*Family, error) {
	family := &Family{}

	// Find the head of household (person who has spouse/children relationships pointing TO them)
	headID := findHeadOfHousehold(evidence)
	if headID == "" {
		return nil, fmt.Errorf("could not identify head of household")
	}

	headPersona, exists := evidence.Personas[headID]
	if !exists {
		return nil, fmt.Errorf("head of household persona not found")
	}

	// Determine the head's sex to assign as father or mother
	headSex := getCharacteristic(&headPersona, "sex")
	headPerson, err := personFromPersona(&headPersona)
	if err != nil {
		return nil, fmt.Errorf("error converting head of household: %w", err)
	}

	// Find spouse
	spouseID := findRelatedPersonaReverse(evidence, headID, "wife", "husband")
	if spouseID != "" {
		if spousePersona, exists := evidence.Personas[spouseID]; exists {
			spousePerson, err := personFromPersona(&spousePersona)
			if err != nil {
				return nil, fmt.Errorf("error converting spouse: %w", err)
			}

			// Assign father/mother based on sex
			if headSex == "M" {
				family.Father = headPerson
				family.Mother = spousePerson
			} else {
				family.Mother = headPerson
				family.Father = spousePerson
			}
		}
	} else {
		// Single parent household
		if headSex == "M" {
			family.Father = headPerson
		} else {
			family.Mother = headPerson
		}
	}

	// Find children
	childIDs := findAllRelatedPersonasReverse(evidence, headID, "son", "daughter")
	for _, childID := range childIDs {
		if childPersona, exists := evidence.Personas[childID]; exists {
			childPerson, err := personFromPersona(&childPersona)
			if err != nil {
				return nil, fmt.Errorf("error converting child: %w", err)
			}
			family.Children = append(family.Children, childPerson)
		}
	}

	return family, nil
}

// buildFamilyOfOrigin creates a family view for marriage/birth records
// showing parents and their children
func buildFamilyOfOrigin(evidence *core.Evidence) (*Family, error) {
	family := &Family{}

	// Pick a focus person (first one we find, or one with parent relationships)
	var focusPersonaID string
	for id := range evidence.Personas {
		// Prefer someone who has parents defined
		if findRelatedPersona(evidence, id, "father") != "" || findRelatedPersona(evidence, id, "mother") != "" {
			focusPersonaID = id
			break
		}
	}

	// If no one has parents, just pick the first persona
	if focusPersonaID == "" {
		for id := range evidence.Personas {
			focusPersonaID = id
			break
		}
	}

	focusPersona, exists := evidence.Personas[focusPersonaID]
	if !exists {
		return nil, fmt.Errorf("focus persona not found")
	}

	// Find parents of the focus persona
	fatherID := findRelatedPersona(evidence, focusPersonaID, "father")
	motherID := findRelatedPersona(evidence, focusPersonaID, "mother")

	// Convert parents to UI Person models
	if fatherID != "" {
		if fatherPersona, exists := evidence.Personas[fatherID]; exists {
			person, err := personFromPersona(&fatherPersona)
			if err != nil {
				return nil, fmt.Errorf("error converting father persona: %w", err)
			}
			family.Father = person
		}
	}

	if motherID != "" {
		if motherPersona, exists := evidence.Personas[motherID]; exists {
			person, err := personFromPersona(&motherPersona)
			if err != nil {
				return nil, fmt.Errorf("error converting mother persona: %w", err)
			}
			family.Mother = person
		}
	}

	// Find siblings (anyone who shares the same parents)
	// Include the focus persona as a "child" in the family
	children := []*Person{}

	// Add the focus persona first
	focusPerson, err := personFromPersona(&focusPersona)
	if err != nil {
		return nil, fmt.Errorf("error converting focus persona: %w", err)
	}
	children = append(children, focusPerson)

	// Find other children of the same parents
	for personaID, persona := range evidence.Personas {
		if personaID == focusPersonaID {
			continue // Skip the focus persona, already added
		}

		// Check if this persona has the same parents
		personaFatherID := findRelatedPersona(evidence, personaID, "father")
		personaMotherID := findRelatedPersona(evidence, personaID, "mother")

		if (fatherID != "" && personaFatherID == fatherID) || (motherID != "" && personaMotherID == motherID) {
			person, err := personFromPersona(&persona)
			if err != nil {
				return nil, fmt.Errorf("error converting sibling persona: %w", err)
			}
			children = append(children, person)
		}
	}

	family.Children = children

	return family, nil
}

// findHeadOfHousehold finds the persona who has spouse/children relationships pointing to them
func findHeadOfHousehold(evidence *core.Evidence) string {
	// Count incoming family relationships for each persona
	relationshipCounts := make(map[string]int)

	for _, rel := range evidence.Relationships {
		if rel.Relationship == "wife" || rel.Relationship == "husband" ||
			rel.Relationship == "son" || rel.Relationship == "daughter" {
			relationshipCounts[rel.Persona2]++
		}
	}

	// Find the persona with the most incoming relationships
	var headID string
	maxCount := 0
	for personaID, count := range relationshipCounts {
		if count > maxCount {
			maxCount = count
			headID = personaID
		}
	}

	return headID
}

// getCharacteristic returns the value of a characteristic by type
func getCharacteristic(persona *core.Persona, charType string) string {
	for _, c := range persona.Characteristics {
		if c.Type == charType {
			return c.Value
		}
	}
	return ""
}

// personFromPersona converts a core.Persona to a ui.Person
func personFromPersona(persona *core.Persona) (*Person, error) {
	person := &Person{}

	// Extract name from characteristics
	person.Name = persona.GetName()

	// Extract birth and death information from events
	for _, event := range persona.Events {
		switch event.Type {
		case "birth":
			if event.Date != "" {
				date, err := parseDate(event.Date)
				if err != nil {
					// Log but don't fail - dates might be partial or unparseable
					// In a real app, you might want to handle this differently
					continue
				}
				person.BirthDate = date
			}
			if event.Place != "" {
				person.BirthPlace = event.Place
			}

		case "death":
			if event.Date != "" {
				date, err := parseDate(event.Date)
				if err != nil {
					continue
				}
				person.DeathDate = date
			}
			if event.Place != "" {
				person.DeathPlace = event.Place
			}
		}
	}

	return person, nil
}

// findRelatedPersona finds the first persona related to the given persona with the specified relationship.
// For example, findRelatedPersona(evidence, "john", "father") returns the ID of john's father.
// This looks for relationships where personaID is Persona2 (relationships pointing TO personaID).
// Returns empty string if no such relationship exists.
func findRelatedPersona(evidence *core.Evidence, personaID, relationship string) string {
	for _, rel := range evidence.Relationships {
		if rel.Persona2 == personaID && rel.Relationship == relationship {
			return rel.Persona1
		}
	}
	return ""
}

// findRelatedPersonaReverse finds personas where the given persona has the specified relationship TO them.
// For example, findRelatedPersonaReverse(evidence, "john", "wife") returns the ID of john's wife.
// This looks for relationships where personaID is Persona2 in a reverse relationship.
// Returns empty string if no such relationship exists.
func findRelatedPersonaReverse(evidence *core.Evidence, personaID string, relationships ...string) string {
	for _, rel := range evidence.Relationships {
		if rel.Persona2 == personaID {
			for _, relationship := range relationships {
				if rel.Relationship == relationship {
					return rel.Persona1
				}
			}
		}
	}
	return ""
}

// findAllRelatedPersonasReverse finds all personas where the given persona has the specified relationship TO them.
// For example, findAllRelatedPersonasReverse(evidence, "john", "son", "daughter") returns IDs of all john's children.
func findAllRelatedPersonasReverse(evidence *core.Evidence, personaID string, relationships ...string) []string {
	var result []string
	for _, rel := range evidence.Relationships {
		if rel.Persona2 == personaID {
			for _, relationship := range relationships {
				if rel.Relationship == relationship {
					result = append(result, rel.Persona1)
					break
				}
			}
		}
	}
	return result
}

// parseDate attempts to parse a date string in various common formats
func parseDate(dateStr string) (*time.Time, error) {
	if dateStr == "" {
		return nil, nil
	}

	// Common date formats found in genealogical records
	formats := []string{
		"2-Jan-2006",   // 14-Oct-1909
		"02-Jan-2006",  // 14-Oct-1909 with leading zero
		"2006-01-02",   // ISO format
		"Jan 2, 2006",  // Oct 14, 1909
		"January 2, 2006",
		"2 Jan 2006",
		"2006",         // Year only
	}

	for _, format := range formats {
		if t, err := time.Parse(format, dateStr); err == nil {
			return &t, nil
		}
	}

	return nil, fmt.Errorf("unable to parse date: %s", dateStr)
}
