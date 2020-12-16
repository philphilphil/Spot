package main

import (
	"fmt"
	"math/bits"

	"github.com/dylhunn/dragontoothmg"
)

var nodesSearcht uint64

func main() {
	board := dragontoothmg.ParseFen("r1b1k2r/pppp1pp1/2nbqn1p/3Pp3/4P2P/2N2N2/PPP2PP1/R1BQKB1R w KQkq - 1 8")

	move := calculateBestMove(&board)
	fmt.Println(move.String())

	// val := getBoardValue(&board)
	// fmt.Println(val)
}

func calculateBestMove(b *dragontoothmg.Board) dragontoothmg.Move {

	moves := b.GenerateLegalMoves()
	bestBoardVal := 0
	var bestMove = moves[0]
	fmt.Printf("White Move: %t\r\n", b.Wtomove)

	for _, move := range moves {
		unapply := b.Apply(move)
		nodesSearcht++
		boardVal := maxi(b, 0)
		unapply()

		// fmt.Printf("White Move: %t Move: %v Eval: %v\r\n", b.Wtomove, move.String(), boardVal)

		if b.Wtomove {
			if boardVal >= bestBoardVal {
				bestMove = move
				bestBoardVal = boardVal
			}
		} else {
			if boardVal <= bestBoardVal {
				bestBoardVal = boardVal
				bestMove = move
			}
		}
	}
	fmt.Println(nodesSearcht)
	return bestMove
}

func maxi(b *dragontoothmg.Board, depth int) int {
	if depth == 0 {
		//fmt.Println(b.ToFen())
		return getBoardValue(b)
	}

	max := 0

	moves := b.GenerateLegalMoves()
	for _, move := range moves {
		unapply := b.Apply(move)
		nodesSearcht++
		score := maxi(b, depth-1)
		unapply()
		if score >= max {
			max = score
			//fmt.Println(b.ToFen())
		}
	}
	return max
}

func mini(b *dragontoothmg.Board, depth int) int {
	if depth == 0 {
		debugVal := getBoardValue(b)
		//fmt.Println(b.ToFen(), " ", debugVal)
		return debugVal
	}

	min := 0

	moves := b.GenerateLegalMoves()
	for _, move := range moves {
		unapply := b.Apply(move)
		nodesSearcht++
		score := maxi(b, depth-1)
		unapply()
		if score <= min {
			min = score
		}
	}
	return min
}

func getBoardValue(b *dragontoothmg.Board) int {

	boardValueWhite := getBoardValueForOneSide(&b.White)
	boardValueBlack := -getBoardValueForOneSide(&b.Black)

	return boardValueWhite + boardValueBlack
}

func getBoardValueForOneSide(bb *dragontoothmg.Bitboards) int {

	pawns := bits.OnesCount64(bb.Pawns)
	kinghts := bits.OnesCount64(bb.Knights)
	bishops := bits.OnesCount64(bb.Bishops)
	rooks := bits.OnesCount64(bb.Rooks)
	queens := bits.OnesCount64(bb.Queens)
	king := bits.OnesCount64(bb.Kings)

	//fmt.Printf("Pawns (%064b) amount %d\r\n", bb.Pawns, bits.OnesCount64(bb.Pawns))
	return (pawns * 10) + (kinghts * 30) + (bishops * 30) + (rooks * 50) + (queens * 90) + (king * 900)
}
