package app

import (
	"fmt"
	"log"
	"time"

	"github.com/gdamore/tcell/v2"
	"github.com/rivo/tview"

	"av/internal/core"
	"av/internal/ui"
)

// Run initializes and runs the application
func Run() error {
	// Load evidence at startup
	index, evidenceList, err := loadEvidence()
	if err != nil {
		return fmt.Errorf("failed to load evidence: %w", err)
	}
	defer index.Close()

	log.Printf("Loaded %d evidence records", len(evidenceList))

	app := tview.NewApplication()

	// Convert evidenceList to UI items
	var uiEvidenceItems []ui.EvidenceListItem
	for _, item := range evidenceList {
		uiEvidenceItems = append(uiEvidenceItems, ui.EvidenceListItem{
			ID:       item.ID,
			Evidence: item.Evidence,
		})
	}

	// Create the evidence list view
	evidenceListView := ui.NewEvidenceList(uiEvidenceItems)

	// Create sample family data (for now, using hard-coded data)
	sampleFamily := createSampleFamily()

	// Create the family page
	familyPage := ui.NewFamilyPage(sampleFamily)

	// Create a Pages component to switch between views
	pages := tview.NewPages().
		AddPage("evidence_list", evidenceListView, true, true).
		AddPage("family", familyPage, true, false)

	// Set up evidence selection handler
	evidenceListView.SetOnSelected(func(item ui.EvidenceListItem) {
		// For now, just show the hard-coded family page
		// TODO: Convert evidence to family model
		pages.SwitchToPage("family")
	})

	// Set up keyboard handling for navigation between pages
	app.SetInputCapture(func(event *tcell.EventKey) *tcell.EventKey {
		currentPage, _ := pages.GetFrontPage()

		switch currentPage {
		case "evidence_list":
			if event.Rune() == 'q' {
				app.Stop()
				return nil
			}
		case "family":
			if event.Key() == tcell.KeyEscape {
				pages.SwitchToPage("evidence_list")
				return nil
			}
		}

		return event
	})

	// Run the application
	return app.SetRoot(pages, true).Run()
}

// createSampleFamily creates hard-coded sample family data
func createSampleFamily() *ui.Family {
	// Parse some sample dates
	fatherBirth := time.Date(1950, 3, 15, 0, 0, 0, 0, time.UTC)
	motherBirth := time.Date(1952, 7, 22, 0, 0, 0, 0, time.UTC)
	child1Birth := time.Date(1975, 1, 10, 0, 0, 0, 0, time.UTC)
	child2Birth := time.Date(1977, 5, 18, 0, 0, 0, 0, time.UTC)
	child3Birth := time.Date(1980, 11, 3, 0, 0, 0, 0, time.UTC)

	father := &ui.Person{
		Name:       "John Smith",
		BirthDate:  &fatherBirth,
		BirthPlace: "Boston, Massachusetts, USA",
	}

	mother := &ui.Person{
		Name:       "Mary Johnson",
		BirthDate:  &motherBirth,
		BirthPlace: "Portland, Maine, USA",
	}

	children := []*ui.Person{
		{
			Name:       "Sarah Smith",
			BirthDate:  &child1Birth,
			BirthPlace: "Cambridge, Massachusetts, USA",
		},
		{
			Name:       "Michael Smith",
			BirthDate:  &child2Birth,
			BirthPlace: "Cambridge, Massachusetts, USA",
		},
		{
			Name:       "Emily Smith",
			BirthDate:  &child3Birth,
			BirthPlace: "Brookline, Massachusetts, USA",
		},
	}

	return &ui.Family{
		Father:   father,
		Mother:   mother,
		Children: children,
	}
}

// EvidenceItem represents an evidence record with its ID for display
type EvidenceItem struct {
	ID       string
	Evidence *core.Evidence
}

// loadEvidence loads all evidence files from the sample directory
func loadEvidence() (core.Index, []EvidenceItem, error) {
	// Create reader for the sample directory
	reader := core.NewReader("sample")

	// Load all evidence from the evidence subdirectory
	records, err := reader.LoadAllEvidence("evidence")
	if err != nil {
		return nil, nil, fmt.Errorf("failed to load evidence files: %w", err)
	}

	// Create an in-memory index
	index := core.NewMemoryIndex()

	// Index all evidence records
	var evidenceList []EvidenceItem
	for i, evidence := range records {
		// Generate an ID based on the index
		id := fmt.Sprintf("evidence-%d", i)

		// Index the evidence
		if err := index.IndexEvidence(id, evidence); err != nil {
			return nil, nil, fmt.Errorf("failed to index evidence %s: %w", id, err)
		}

		evidenceList = append(evidenceList, EvidenceItem{
			ID:       id,
			Evidence: evidence,
		})
	}

	return index, evidenceList, nil
}
