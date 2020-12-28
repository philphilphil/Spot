package main

import 	(
	"github.com/dylhunn/dragontoothmg"
	"math/bits"
)

// Get value for entire board
func getBoardValue(b *dragontoothmg.Board) int {
	boardValue := getBoardValueForWhite(&b.White) + getBoardValueForBlack(&b.Black)
	return boardValue
}

// Calculate the value for one side
// TODO: Refactor getBoardValueForWhite and getBoardValueForBlack into one?
func getBoardValueForWhite(bb *dragontoothmg.Bitboards) int {
	value := getPiecesBaseValue(bb)
	value += getPiecePositionBonusValue(&bb.Pawns, whitePawn)
	value += getPiecePositionBonusValue(&bb.Knights, whiteKnight)
	value += getPiecePositionBonusValue(&bb.Bishops, whiteBishop)
	value += getPiecePositionBonusValue(&bb.Rooks, whiteRook)
	value += getPiecePositionBonusValue(&bb.Queens, whiteQueen)
	value += getPiecePositionBonusValue(&bb.Kings, whiteKing)

	// fmt.Println("Pieces:")
	// fmt.Printf("Pawns (%064b) amount %d\r\n", bb.Pawns, bits.OnesCount64(bb.Pawns))
	// fmt.Println(getPiecePositionBonusValue(&bb.Pawns, whitePawn))
	// fmt.Printf("Knights (%064b) amount %d\r\n", bb.Knights, bits.OnesCount64(bb.Knights))
	// fmt.Println(getPiecePositionBonusValue(&bb.Knights, whiteKnight))
	// fmt.Printf("Bishops (%064b) amount %d\r\n", bb.Bishops, bits.OnesCount64(bb.Bishops))
	// fmt.Println(getPiecePositionBonusValue(&bb.Bishops, whiteBishop))
	// fmt.Printf("Rooks (%064b) amount %d\r\n", bb.Rooks, bits.OnesCount64(bb.Rooks))
	// fmt.Println(getPiecePositionBonusValue(&bb.Rooks, whiteRook))
	// fmt.Printf("Queens (%064b) amount %d\r\n", bb.Queens, bits.OnesCount64(bb.Queens))
	// fmt.Println(getPiecePositionBonusValue(&bb.Queens, whiteQueen))
	// fmt.Printf("Kings (%064b) amount %d\r\n", bb.Kings, bits.OnesCount64(bb.Kings))
	// fmt.Println(getPiecePositionBonusValue(&bb.Kings, whiteKing))
	//fmt.Println(value)

	return value
}

// Calculate the value for one side
func getBoardValueForBlack(bb *dragontoothmg.Bitboards) int {
	value := getPiecesBaseValue(bb)
	value += getPiecePositionBonusValue(&bb.Pawns, blackPawn)
	value += getPiecePositionBonusValue(&bb.Knights, blackKnight)
	value += getPiecePositionBonusValue(&bb.Bishops, blackBishop)
	value += getPiecePositionBonusValue(&bb.Rooks, blackRook)
	value += getPiecePositionBonusValue(&bb.Queens, blackQueen)
	value += getPiecePositionBonusValue(&bb.Kings, blackKing)

	return -value
}

func getPiecesBaseValue(bb *dragontoothmg.Bitboards) int {
	pawns := bits.OnesCount64(bb.Pawns)
	kinghts := bits.OnesCount64(bb.Knights)
	bishops := bits.OnesCount64(bb.Bishops)
	rooks := bits.OnesCount64(bb.Rooks)
	queens := bits.OnesCount64(bb.Queens)
	king := bits.OnesCount64(bb.Kings)
	return (pawns * 100) + (kinghts * 320) + (bishops * 330) + (rooks * 500) + (queens * 900) + (king * 3000)
}

// Get value for piece depending on position
// TODO: Add different evals for game status, eg mid and endgame
func getPiecePositionBonusValue(bb *uint64, values [64]int) int {
	var value int

	// Reverse the piece values. For better human readability they are saved as we see the board
	// TODO: write a script which creates the correct sorted order of piece evals on build,
	// 		 think of a format to save the piece evals
	for i, j := 0, len(values)-1; i < j; i, j = i+1, j-1 {
		values[i], values[j] = values[j], values[i]
	}

	squares := getPieceSquareNumbers(*bb)

	for _, s := range squares {
		value += values[s]
		//fmt.Println(values[s])
	}
	return value
}

// Get index of pieces starting down left = 0
// A first version iterrated over the piecePositionMasks-array until it found the square
// but the other way around is faster.
// TODO: reverse search for black? especially in the beginning 4 rows are for sure empty for black
func getPieceSquareNumbers(bb uint64) []int {
	var squareNumbers []int
	var num uint64 = bb

	for num != 0 {
		m := num & -num

		for i := 0; i <= 63; i++ {
			if piecePositionMasks[i] == m {
				squareNumbers = append(squareNumbers, i)
				break
			}
		}
		num &= num - 1

	}
	return squareNumbers
}

var whitePawn = [64]int{
	0, 0, 0, 0, 0, 0, 0, 0,
	50, 50, 50, 50, 50, 50, 50, 50,
	10, 10, 20, 30, 30, 20, 10, 10,
	5, 5, 10, 25, 25, 10, 5, 5,
	0, 0, 0, 20, 20, 0, 0, 0,
	5, -5, -10, 0, 0, -10, -5, 5,
	5, 10, 10, -20, -20, 10, 10, 5,
	0, 0, 0, 0, 0, 0, 0, 0}

var blackPawn = [64]int{
	0, 0, 0, 0, 0, 0, 0, 0,
	5, 10, 10, -20, -20, 10, 10, 5,
	5, -5, -10, 0, 0, -10, -5, 5,
	0, 0, 0, 20, 20, 0, 0, 0,
	5, 5, 10, 25, 25, 10, 5, 5,
	10, 10, 20, 30, 30, 20, 10, 10,
	50, 50, 50, 50, 50, 50, 50, 50,
	0, 0, 0, 0, 0, 0, 0, 0}

var whiteKnight = [64]int{
	-50, -40, -30, -30, -30, -30, -40, -50,
	-40, -20, 0, 0, 0, 0, -20, -40,
	-30, 0, 10, 15, 15, 10, 0, -30,
	-30, 5, 15, 20, 20, 15, 5, -30,
	-30, 0, 15, 20, 20, 15, 0, -30,
	-30, 5, 10, 15, 15, 10, 5, -30,
	-40, -20, 0, 5, 5, 0, -20, -40,
	-50, -40, -30, -30, -30, -30, -40, -50}

var blackKnight = [64]int{
	-50, -40, -30, -30, -30, -30, -40, -50,
	-40, -20, 0, 5, 5, 0, -20, -40,
	-30, 5, 10, 15, 15, 10, 5, -30,
	-30, 0, 15, 20, 20, 15, 0, -30,
	-30, 5, 15, 20, 20, 15, 5, -30,
	-30, 0, 10, 15, 15, 10, 0, -30,
	-40, -20, 0, 0, 0, 0, -20, -40,
	-50, -40, -30, -30, -30, -30, -40, -50}

var whiteBishop = [64]int{
	-20, -10, -10, -10, -10, -10, -10, -20,
	-10, 0, 0, 0, 0, 0, 0, -10,
	-10, 0, 5, 10, 10, 5, 0, -10,
	-10, 5, 5, 10, 10, 5, 5, -10,
	-10, 0, 10, 10, 10, 10, 0, -10,
	-10, 10, 10, 10, 10, 10, 10, -10,
	-10, 5, 0, 0, 0, 0, 5, -10,
	-20, -10, -10, -10, -10, -10, -10, -20}

var blackBishop = [64]int{
	-20, -10, -10, -10, -10, -10, -10, -20,
	-10, 5, 0, 0, 0, 0, 5, -10,
	-10, 10, 10, 10, 10, 10, 10, -10,
	-10, 0, 10, 10, 10, 10, 0, -10,
	-10, 5, 5, 10, 10, 5, 5, -10,
	-10, 0, 5, 10, 10, 5, 0, -10,
	-10, 0, 0, 0, 0, 0, 0, -10,
	-20, -10, -10, -10, -10, -10, -10, -20}

var whiteRook = [64]int{
	0, 0, 0, 0, 0, 0, 0, 0,
	5, 10, 10, 10, 10, 10, 10, 5,
	-5, 0, 0, 0, 0, 0, 0, -5,
	-5, 0, 0, 0, 0, 0, 0, -5,
	-5, 0, 0, 0, 0, 0, 0, -5,
	-5, 0, 0, 0, 0, 0, 0, -5,
	-5, 0, 0, 0, 0, 0, 0, -5,
	0, 0, 0, 5, 5, 0, 0, 0}

var blackRook = [64]int{
	0, 0, 0, 5, 5, 0, 0, 0,
	-5, 0, 0, 0, 0, 0, 0, -5,
	-5, 0, 0, 0, 0, 0, 0, -5,
	-5, 0, 0, 0, 0, 0, 0, -5,
	-5, 0, 0, 0, 0, 0, 0, -5,
	-5, 0, 0, 0, 0, 0, 0, -5,
	5, 10, 10, 10, 10, 10, 10, 5,
	0, 0, 0, 0, 0, 0, 0, 0}

var whiteQueen = [64]int{
	-20, -10, -10, -5, -5, -10, -10, -20,
	-10, 0, 0, 0, 0, 0, 0, -10,
	-10, 0, 5, 5, 5, 5, 0, -10,
	-5, 0, 5, 5, 5, 5, 0, -5,
	0, 0, 5, 5, 5, 5, 0, -5,
	-10, 5, 5, 5, 5, 5, 0, -10,
	-10, 0, 5, 0, 0, 0, 0, -10,
	-20, -10, -10, -5, -5, -10, -10, -20}

var blackQueen = [64]int{
	-20, -10, -10, -5, -5, -10, -10, -20,
	-10, 0, 5, 0, 0, 0, 0, -10,
	-10, 5, 5, 5, 5, 5, 0, -10,
	0, 0, 5, 5, 5, 5, 0, -5,
	-5, 0, 5, 5, 5, 5, 0, -5,
	-10, 0, 5, 5, 5, 5, 0, -10,
	-10, 0, 0, 0, 0, 0, 0, -10,
	-20, -10, -10, -5, -5, -10, -10, -20}

var whiteKing = [64]int{
	-30, -40, -40, -50, -50, -40, -40, -30,
	-30, -40, -40, -50, -50, -40, -40, -30,
	-30, -40, -40, -50, -50, -40, -40, -30,
	-30, -40, -40, -50, -50, -40, -40, -30,
	-20, -30, -30, -40, -40, -30, -30, -20,
	-10, -20, -20, -20, -20, -20, -20, -10,
	20, 20, 0, 0, 0, 0, 20, 20,
	20, 30, 10, 0, 0, 10, 30, 20}

var blackKing = [64]int{
	20, 30, 10, 0, 0, 10, 30, 20,
	20, 20, 0, 0, 0, 0, 20, 20,
	-10, -20, -20, -20, -20, -20, -20, -10,
	-20, -30, -30, -40, -40, -30, -30, -20,
	-30, -40, -40, -50, -50, -40, -40, -30,
	-30, -40, -40, -50, -50, -40, -40, -30,
	-30, -40, -40, -50, -50, -40, -40, -30,
	-30, -40, -40, -50, -50, -40, -40, -30}
