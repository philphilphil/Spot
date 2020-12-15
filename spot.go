package main

import "github.com/dylhunn/dragontoothmg"
import "fmt"
import "math/bits"

func main() {
	board := dragontoothmg.ParseFen("rnbqk1nr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")
	val := getBoardValue(&board)
	fmt.Println(val)
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
