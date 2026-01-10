package main

import (
	"time"

	"github.com/gdamore/tcell/v2"
	"github.com/rivo/tview"

	"github.com/dswisher/ancestral-vault/internal/ui"
)

func main() {
	app := tview.NewApplication()

	// Create the main screen with hints
	mainView := tview.NewTextView().
		SetTextAlign(tview.AlignCenter).
		SetText("\n\n\nAncestral Vault\n\nGenealogy Application\n\n\n[gray]Press 'f' to show family\nPress 'q' to exit[-]").
		SetDynamicColors(true)

	// Create sample family data
	sampleFamily := createSampleFamily()

	// Create the family page
	familyPage := ui.NewFamilyPage(sampleFamily)

	// Create a Pages component to switch between views
	pages := tview.NewPages().
		AddPage("main", mainView, true, true).
		AddPage("family", familyPage, true, false)

	// Set up keyboard handling
	app.SetInputCapture(func(event *tcell.EventKey) *tcell.EventKey {
		currentPage, _ := pages.GetFrontPage()

		switch currentPage {
		case "main":
			if event.Rune() == 'q' {
				app.Stop()
				return nil
			}
			if event.Rune() == 'f' {
				pages.SwitchToPage("family")
				return nil
			}
		case "family":
			if event.Key() == tcell.KeyEscape {
				pages.SwitchToPage("main")
				return nil
			}
		}

		return event
	})

	// Run the application
	if err := app.SetRoot(pages, true).Run(); err != nil {
		panic(err)
	}
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
