package main

import (
	"av/internal/app"
)

func main() {
	if err := app.Run(); err != nil {
		panic(err)
	}
}
