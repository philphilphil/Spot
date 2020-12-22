package main

import "github.com/dylhunn/dragontoothmg"

type Hashtable struct {
	zobrist uint64
	depth   int
	score   int
	move    dragontoothmg.Move
}
