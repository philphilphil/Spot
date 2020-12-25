package main

import "github.com/dylhunn/dragontoothmg"

func Max(x, y int) int {
	if x > y {
		return x
	}
	return y
}

func MovesToString(moves []dragontoothmg.Move) string {
	moveString := ""

	for _, v := range moves {
		moveString = moveString + " " + v.String()
	}

	return moveString
}