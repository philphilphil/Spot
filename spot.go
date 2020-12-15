package main

import "github.com/dylhunn/dragontoothmg"
import "fmt"
import "math/bits"

func main() {
	board := dragontoothmg.ParseFen("rnb1kbnr/pppp1ppp/8/P7/8/4qP2/1PPPBPPP/RNBQK2R w KQkq - 3 6")
	
	move :=calculateBestMove(&board)
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
		boardVal := getBoardValue(b)
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

	return bestMove
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
