package ui

import (
	"fmt"

	"github.com/gdamore/tcell/v2"
	"github.com/rivo/tview"

	"av/internal/core"
)

// EvidenceListItem represents an evidence item for display in the list
type EvidenceListItem struct {
	ID       string
	Evidence *core.Evidence
}

// EvidenceList is a UI component that displays a list of evidence records
type EvidenceList struct {
	*tview.List
	items      []EvidenceListItem
	onSelected func(item EvidenceListItem)
}

// NewEvidenceList creates a new evidence list view
func NewEvidenceList(items []EvidenceListItem) *EvidenceList {
	list := &EvidenceList{
		List:  tview.NewList(),
		items: items,
	}

	list.SetBorder(true).
		SetTitle(" Evidence Records ").
		SetTitleAlign(tview.AlignLeft)

	// Populate the list
	for _, item := range items {
		// Create a display name from the evidence source
		displayName := item.Evidence.Source
		if displayName == "" {
			displayName = item.ID
		}

		// Add item to the list
		list.AddItem(displayName, "", 0, nil)
	}

	// Set up selection handler
	list.SetSelectedFunc(func(index int, mainText, secondaryText string, shortcut rune) {
		if list.onSelected != nil && index >= 0 && index < len(list.items) {
			list.onSelected(list.items[index])
		}
	})

	// Add instructions at the bottom
	list.SetInputCapture(func(event *tcell.EventKey) *tcell.EventKey {
		return event
	})

	return list
}

// SetOnSelected sets the callback function when an item is selected
func (e *EvidenceList) SetOnSelected(handler func(item EvidenceListItem)) {
	e.onSelected = handler
}

// GetSelectedItem returns the currently selected evidence item
func (e *EvidenceList) GetSelectedItem() *EvidenceListItem {
	index := e.GetCurrentItem()
	if index >= 0 && index < len(e.items) {
		return &e.items[index]
	}
	return nil
}

// GetItemCount returns the number of items in the list
func (e *EvidenceList) GetItemCount() int {
	return len(e.items)
}

// GetFooterText returns helpful text to display in a footer
func (e *EvidenceList) GetFooterText() string {
	return fmt.Sprintf("Total: %d evidence records | Enter to select | Esc to go back", len(e.items))
}
