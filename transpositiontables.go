package main

import "github.com/dylhunn/dragontoothmg"

//var transpoTable map[uint64]Hashtable

var transpoTable = make(map[uint64]Hashtable)

type Hashtable struct {
	zobrist uint64
	depth   int
	score   int
	gamma   int
	move    dragontoothmg.Move
}

func Max(x, y int) int {
	if x > y {
		return x
	}
	return y
}
