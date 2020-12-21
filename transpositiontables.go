package main

type Hashtable struct {
	zobrist uint64
	depth   int
	flag    int
	eval    int
	ancient int
	move    uint8
}
