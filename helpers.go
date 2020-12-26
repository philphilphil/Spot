package main

import "github.com/dylhunn/dragontoothmg"
import "fmt"
import "sort"

func Max(x, y int) int {
	if x > y {
		return x
	}
	return y
}

func Min(x, y int) int {
	if x < y {
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

type ttSLice []Hashtable

func printLogTop100OfTT() {
	printLog(fmt.Sprintf("TT-Size: %v", len(transpoTable)))

	s := make(ttSLice, 0, len(transpoTable))

	for _, d := range transpoTable {
		s = append(s, d)
	}

	sort.Sort(s)

	for _, d := range s {
		printLog(fmt.Sprintf("%+v\n", d))
	}

}

// Len is part of sort.Interface.
func (d ttSLice) Len() int {
	return len(d)
}

// Swap is part of sort.Interface.
func (d ttSLice) Swap(i, j int) {
	d[i], d[j] = d[j], d[i]
}

// Less is part of sort.Interface. We use count as the value to sort by
func (d ttSLice) Less(i, j int) bool {
	return d[i].score > d[j].score
}
