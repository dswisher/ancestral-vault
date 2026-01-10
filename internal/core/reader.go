package core

import (
	"encoding/json"
	"fmt"
	"os"
	"path/filepath"
)

// Reader handles loading and parsing evidence files
type Reader struct {
	basePath string
}

// NewReader creates a new Reader with the specified base path
func NewReader(basePath string) *Reader {
	return &Reader{
		basePath: basePath,
	}
}

// LoadEvidence loads an evidence record from a JSON file
func (r *Reader) LoadEvidence(filename string) (*Evidence, error) {
	fullPath := filepath.Join(r.basePath, filename)

	data, err := os.ReadFile(fullPath)
	if err != nil {
		return nil, fmt.Errorf("failed to read evidence file %s: %w", filename, err)
	}

	var evidence Evidence
	if err := json.Unmarshal(data, &evidence); err != nil {
		return nil, fmt.Errorf("failed to parse evidence file %s: %w", filename, err)
	}

	return &evidence, nil
}

// LoadAllEvidence loads all evidence records from a directory and its subdirectories
func (r *Reader) LoadAllEvidence(dir string) ([]*Evidence, error) {
	fullDir := filepath.Join(r.basePath, dir)

	var records []*Evidence

	// Walk through directory tree to find all JSON files
	err := filepath.WalkDir(fullDir, func(path string, d os.DirEntry, err error) error {
		if err != nil {
			return err
		}

		// Skip directories
		if d.IsDir() {
			return nil
		}

		// Only process JSON files
		if filepath.Ext(d.Name()) != ".json" {
			return nil
		}

		// Get relative path from basePath for loading
		relPath, err := filepath.Rel(r.basePath, path)
		if err != nil {
			return fmt.Errorf("failed to get relative path for %s: %w", path, err)
		}

		evidence, err := r.LoadEvidence(relPath)
		if err != nil {
			return err
		}

		records = append(records, evidence)
		return nil
	})

	if err != nil {
		return nil, fmt.Errorf("failed to walk directory %s: %w", dir, err)
	}

	return records, nil
}
