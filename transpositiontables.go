package main

import "github.com/dylhunn/dragontoothmg"

//var transpoTable map[uint64]Hashtable

var transpoTable = make(map[uint64]Hashtable)
type Bound int

type Hashtable struct {
	zobrist uint64
	depth   int
	score   int
	move    dragontoothmg.Move
	bound Bound
}

const  (
	UpperBound Bound = iota
	LowerBound 
	Exact      
)
