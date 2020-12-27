package main

import (
	//	"fmt"
	"github.com/dylhunn/dragontoothmg"
	"strings"
	"testing"
)

//TODO: refactor all calc test functions into one and use engine test epd
func TestCalculationBlack_1(t *testing.T) {
	//debug =  true
	testGame := getGameFromFen(strings.Fields("fen rnbqkbnr/5ppp/4p3/2PN2B1/1P2P3/p4N2/P1P1BPPP/1R1QK2R b Kkq - 0 1"))
	bestMove := calculateBestMove(testGame)

	if bestMove.String() != "f7f6" {
		t.Errorf("Move wrong got: %v, want: %v", bestMove.String(), "f7f6")
	}
}

func TestCalculationBlack_2(t *testing.T) {
	//debug = true
	testGame := getGameFromFen(strings.Fields("fen rnb1kbnr/pppp1ppp/8/4p1q1/4P1Q1/3P4/PPP2PPP/RNB1KBNR b KQkq - 2 3"))
	bestMove := calculateBestMove(testGame)

	if bestMove.String() != "g5g4" {
		t.Errorf("Move wrong got: %v, want: %v", bestMove.String(), "g5g4")
	}
}
func TestCalculationWhite_1(t *testing.T) {
	testGame := getGameFromFen(strings.Fields("fen rn1qk1nr/7p/5pp1/2PP1b2/7B/p2Q1N2/P1P1BPPP/1R2K2R w Kkq - 0 6"))
	bestMove := calculateBestMove(testGame)

	if bestMove.String() != "d3b5" {
		t.Errorf("Move wrong got: %v, want: %v", bestMove.String(), "d3b5")
	}
}

func TestCalculationWhite_2(t *testing.T) {
	testGame := getGameFromFen(strings.Fields("fen rnb1kbnr/pppp1ppp/8/4p1q1/4P3/3P4/PPP2PPP/RNBQKBNR w KQkq - 1 3"))
	bestMove := calculateBestMove(testGame)

	if bestMove.String() != "c1g5" {
		t.Errorf("Move wrong got: %v, want: %v", bestMove.String(), "c1g5")
	}
}

func TestCalculationMateInOnes(t *testing.T) {
	//debug=true
	var mateInOneFens = make(map[string]string)
	//mateInOneFens["a8a5"] = "Q7/7K/5Q1P/2k5/8/8/4Q3/8 w - - 23 137" TODO: mate in one fixes. if matein2 is in moveorder before m1, he does the m2
	mateInOneFens["a1a8"] = "7k/4pppp/8/8/6P1/8/5N2/R2Q1K2 w Q - 0 1"
	mateInOneFens["f1f7"] = "4rk2/4pppp/8/6N1/6P1/8/8/R4Q1K w Q - 0 1"
	mateInOneFens["d5b3"] = "8/8/8/3Q4/7P/k1K4P/8/8 w - - 15 74"

	CheckFenForBestMove(mateInOneFens, t)
}

func TestCalculationMateInTwos(t *testing.T) {
	//debug = true
	var mateInTwoFens = make(map[string]string)
	mateInTwoFens["f5f8"] = "3BB1N1/QKp3pb/7p/5R1R/3br1k1/7N/4P2P/6n1 w - - 0 1"
	mateInTwoFens["d5d7"] = "2rkr3/2ppp3/2n1n3/R2R4/8/8/3K4/8 w - - 0 1"
	mateInTwoFens["a6h6"] = "5Knk/7b/R7/8/7B/8/8/8 w - - 0 1"
	mateInTwoFens["d2c3"] = "8/8/8/3Q4/k6P/7P/3K4/8 w - - 13 73"
	

	CheckFenForBestMove(mateInTwoFens, t)
}

func CheckFenForBestMove(movePos map[string]string, t *testing.T) {
	for move, fen := range movePos {
		board := dragontoothmg.ParseFen(fen)
		bestMove := calculateBestMove(board)
	
		if bestMove.String() != move {
			t.Errorf("Move wrong got: %v, want: %v", bestMove.String(), move)
		}
	}
}

func TestCalculationPuzzles(t *testing.T) {
	//debug = true
	testGame := getGameFromFen(strings.Fields("fen 1b1B1rBN/1P1ppqR1/KPpk1p2/1RN4Q/5p2/1n3P2/2P2n2/8 w - - 0 1"))
	bestMove := calculateBestMove(testGame)

	if bestMove.String() != "h5f5" {
		t.Errorf("Move wrong got: %v, want: %v", bestMove.String(), "h5f5")
	}

	testGame = getGameFromFen(strings.Fields("fen 2q3k1/pp1n1ppp/2pQ1b2/5N2/1P6/2P5/P4PPP/5RK1 w - - 0 1"))
	bestMove = calculateBestMove(testGame)

	if bestMove.String() != "d6f6" {
		t.Errorf("Move wrong got: %v, want: %v", bestMove.String(), "d6f6")
	}
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

func TestMoveOrdering(t *testing.T) {
	// https://lichess.org/editor/1qq4k/8/3r2p1/N7/2B1N3/8/8/K4QQ1_b_-_-_0_1
	testGame := getGameFromFen(strings.Fields("fen 1qq4k/8/3r2p1/N7/2B1N3/8/8/K4QQ1 b - - 0 1"))

	moves := testGame.GenerateLegalMoves()
	var bestMove dragontoothmg.Move = 4087
	orderedMoves := generateAndOrderMoves(moves, bestMove)

	// fmt.Println(moves)
	// fmt.Println(orderedMoves)

	if orderedMoves[0] != bestMove {
		t.Errorf("Move sort wrong got: %d, want: %d", orderedMoves[0], bestMove)
	}
}
