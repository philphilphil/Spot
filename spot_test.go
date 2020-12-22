package main

import (
	"strings"
	"testing"
	"time"

	//"github.com/dylhunn/dragontoothmg"
)

func TestCalculation(t *testing.T) {
	debug = false
	start := time.Now()
	testGame := getGameFromFen(strings.Fields("fen rnbqkbnr/5ppp/4p3/2PN2B1/1P2P3/p4N2/P1P1BPPP/1R1QK2R b Kkq - 0 1"))
	bestMove := calculateBestMove(&testGame)
   
	if bestMove.String() != "f7f6" {
		t.Errorf("Move wrong got: %v, want: %v", bestMove.String(), "f7f6")
	}

    t.Log(time.Since(start))
}

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

func TestGetBoardValue2(t *testing.T) {

	// https://lichess.org/editor/7k/3q4/8/1n4r1/1R4N1/8/8/K7_b_-_-_0_1
	testGame := getGameFromFen(strings.Fields("fen 7k/3q4/8/1n4r1/1R4N1/8/8/K7 b - - 0 1"))
	boardValue := getBoardValue(&testGame)

	if boardValue != -900 {
		t.Errorf("Board value wrong got: %d, want: %d", boardValue, -900)
	}
}

func TestGetBoardValue3(t *testing.T) {

	// https://lichess.org/editor/2P2p1k/2Pq1p2/2P2p2/1nP2pr1/1RP2pN1/2P2p2/2P2p2/K1P2p2_b_-_-_0_1
	testGame := getGameFromFen(strings.Fields("fen 2P2p1k/2Pq1p2/2P2p2/1nP2pr1/1RP2pN1/2P2p2/2P2p2/K1P2p2 b - - 0 1"))
	boardValue := getBoardValue(&testGame)

	if boardValue != -900 {
		t.Errorf("Board value wrong got: %d, want: %d", boardValue, -900)
	}
}

func TestGetBoardValue4(t *testing.T) {

	// https://lichess.org/editor/1qq4k/8/3r2p1/N7/2B1N3/8/8/K4QQ1_b_-_-_0_1
	testGame := getGameFromFen(strings.Fields("fen 1qq4k/8/3r2p1/N7/2B1N3/8/8/K4QQ1 b - - 0 1"))
	boardValue := getBoardValue(&testGame)

	if boardValue != 375 {
		t.Errorf("Board value wrong got: %d, want: %d", boardValue, 375)
	}
}