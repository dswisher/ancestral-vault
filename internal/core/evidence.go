package core

// Evidence represents facts extracted from a single source document or record
type Evidence struct {
	Source        string                `json:"source"`
	Personas      map[string]Persona    `json:"personas"`
	Relationships []PersonaRelationship `json:"relationships"`
}

// Persona represents an individual as they appear in an evidence source
type Persona struct {
	Characteristics []Characteristic `json:"characteristics"`
	Events          []Event          `json:"events"`
}

// GetName returns the persona's name from their characteristics
// Returns empty string if no name characteristic is found
func (p *Persona) GetName() string {
	for _, c := range p.Characteristics {
		if c.Type == "full-name" || c.Type == "name" {
			return c.Value
		}
	}
	return ""
}

// Characteristic represents an attribute of a persona that doesn't involve a date or place
type Characteristic struct {
	Type  string `json:"type"`
	Value string `json:"value"`
	Note  string `json:"note,omitempty"`
}

// Event represents something that happened to a persona with a date and/or place
type Event struct {
	Type  string `json:"type"`
	Date  string `json:"date,omitempty"`
	Place string `json:"place,omitempty"`
	Role  string `json:"role,omitempty"`
	Note  string `json:"note,omitempty"`
}

// PersonaRelationship represents a relationship between two personas in an evidence source
type PersonaRelationship struct {
	Persona1     string `json:"persona1"`
	Persona2     string `json:"persona2"`
	Relationship string `json:"relationship"`
}
