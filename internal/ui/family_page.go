package ui

import (
	"fmt"

	"github.com/rivo/tview"
)

// NewFamilyPage creates a new family page view with the given family data
func NewFamilyPage(family *Family) tview.Primitive {
	// Create the parent views (top half)
	fatherView := createPersonView("Father", family.Father)
	motherView := createPersonView("Mother", family.Mother)

	// Create horizontal layout for parents
	parentsView := tview.NewFlex().
		AddItem(fatherView, 0, 1, false).
		AddItem(motherView, 0, 1, false)

	// Create children view (bottom half)
	childrenView := createChildrenView(family.Children)

	// Create vertical layout: parents on top, children on bottom
	mainView := tview.NewFlex().
		SetDirection(tview.FlexRow).
		AddItem(parentsView, 0, 1, false).
		AddItem(childrenView, 0, 1, false)

	return mainView
}

// createPersonView creates a view for a single person
func createPersonView(title string, person *Person) *tview.TextView {
	view := tview.NewTextView().
		SetDynamicColors(true)
	view.SetBorder(true).SetTitle(title)

	if person == nil {
		fmt.Fprintf(view, "[gray]No data[-]")
		return view
	}

	fmt.Fprintf(view, "[yellow]Name:[-] %s\n\n", person.Name)

	if person.BirthDate != nil {
		fmt.Fprintf(view, "[green]Born:[-] %s\n", person.BirthDate.Format("2 Jan 2006"))
	}
	if person.BirthPlace != "" {
		fmt.Fprintf(view, "[green]Place:[-] %s\n", person.BirthPlace)
	}

	if person.DeathDate != nil {
		fmt.Fprintf(view, "\n[red]Died:[-] %s\n", person.DeathDate.Format("2 Jan 2006"))
	}
	if person.DeathPlace != "" {
		fmt.Fprintf(view, "[red]Place:[-] %s\n", person.DeathPlace)
	}

	return view
}

// createChildrenView creates a view for the list of children
func createChildrenView(children []*Person) *tview.TextView {
	view := tview.NewTextView().
		SetDynamicColors(true)
	view.SetBorder(true).SetTitle("Children")

	if len(children) == 0 {
		fmt.Fprintf(view, "[gray]No children[-]")
		return view
	}

	for i, child := range children {
		fmt.Fprintf(view, "[yellow]%d. %s[-]", i+1, child.Name)
		if child.BirthDate != nil {
			fmt.Fprintf(view, " (b. %s)", child.BirthDate.Format("2006"))
		}
		fmt.Fprintf(view, "\n")
	}

	return view
}
