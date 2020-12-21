package main

import (
	"strings"
	"testing"

	//"github.com/dylhunn/dragontoothmg"
)

// Tests entire board eval process:
// getBoardValue, getBoardValueForWhite, getBoardValueForBlack, getPiecesBaseValue
func TestGetBoardValue(t *testing.T) {

	// https://lichess.org/analysis/standard/rnbqkbnr/5ppp/4p3/2PN2B1/1P2P3/p4N2/P1P1BPPP/1R1QK2R_b_Kkq_-_0_1
	testGame := getGameFromFen(strings.Fields("fen rnb1kb1r/5ppp/8/2PQ2B1/1P2n3/p4N1P/P1P1BPP1/1R2K2R w Kkq - 1 5"))
	boardValue := getBoardValue(&testGame)

	if boardValue != 955 {
		t.Errorf("Board value wrong got: %d, want: %d", boardValue, 955)
	}
}
