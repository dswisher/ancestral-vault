package core

import "fmt"

// Index provides search and retrieval capabilities for evidence
type Index interface {
	// IndexEvidence adds an evidence record to the index
	IndexEvidence(id string, evidence *Evidence) error

	// SearchPersonas searches for personas by name
	SearchPersonas(query string) ([]PersonaSearchResult, error)

	// GetEvidence retrieves an evidence record by ID
	GetEvidence(id string) (*Evidence, error)

	// Close closes the index and releases resources
	Close() error
}

// PersonaSearchResult represents a search result for a persona
type PersonaSearchResult struct {
	PersonaID  string
	Name       string
	EvidenceID string
	Persona    Persona
}

// MemoryIndex is a simple in-memory implementation of Index for testing
type MemoryIndex struct {
	evidenceRecords map[string]*Evidence
	personas        []PersonaSearchResult
}

// NewMemoryIndex creates a new in-memory index
func NewMemoryIndex() *MemoryIndex {
	return &MemoryIndex{
		evidenceRecords: make(map[string]*Evidence),
		personas:        make([]PersonaSearchResult, 0),
	}
}

// IndexEvidence adds an evidence record to the in-memory index
func (m *MemoryIndex) IndexEvidence(id string, evidence *Evidence) error {
	m.evidenceRecords[id] = evidence

	// Index all personas for searching
	for personaID, persona := range evidence.Personas {
		m.personas = append(m.personas, PersonaSearchResult{
			PersonaID:  personaID,
			Name:       persona.GetName(),
			EvidenceID: id,
			Persona:    persona,
		})
	}

	return nil
}

// SearchPersonas performs a simple substring search on persona names
func (m *MemoryIndex) SearchPersonas(query string) ([]PersonaSearchResult, error) {
	// TODO: Replace with proper full-text search (Bleve)
	results := make([]PersonaSearchResult, 0)

	for _, persona := range m.personas {
		// Simple case-insensitive substring match
		// This is just a placeholder until proper indexing is implemented
		if contains(persona.Name, query) {
			results = append(results, persona)
		}
	}

	return results, nil
}

// GetEvidence retrieves an evidence record by ID
func (m *MemoryIndex) GetEvidence(id string) (*Evidence, error) {
	evidence, exists := m.evidenceRecords[id]
	if !exists {
		return nil, fmt.Errorf("evidence record not found: %s", id)
	}
	return evidence, nil
}

// Close is a no-op for MemoryIndex
func (m *MemoryIndex) Close() error {
	return nil
}

// contains performs a simple case-insensitive substring search
// TODO: Use proper full-text search
func contains(s, substr string) bool {
	// Simple implementation - would be better with proper normalization
	return len(s) >= len(substr) && simpleContains(s, substr)
}

func simpleContains(s, substr string) bool {
	if len(substr) == 0 {
		return true
	}
	if len(s) < len(substr) {
		return false
	}
	for i := 0; i <= len(s)-len(substr); i++ {
		match := true
		for j := 0; j < len(substr); j++ {
			if toLower(s[i+j]) != toLower(substr[j]) {
				match = false
				break
			}
		}
		if match {
			return true
		}
	}
	return false
}

func toLower(b byte) byte {
	if b >= 'A' && b <= 'Z' {
		return b + ('a' - 'A')
	}
	return b
}
