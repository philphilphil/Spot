package main

import (
	"math/bits"

	"github.com/dylhunn/dragontoothmg"
	//"fmt"
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
	value += getPiecePositionBonusValue(bb.Pawns, whitePawn)
	value += getPiecePositionBonusValue(bb.Knights, whiteKnight)
	value += getPiecePositionBonusValue(bb.Bishops, whiteBishop)
	value += getPiecePositionBonusValue(bb.Rooks, whiteRook)
	value += getPiecePositionBonusValue(bb.Queens, whiteQueen)
	value += getPiecePositionBonusValue(bb.Kings, whiteKing)

	// fmt.Println("Pieces:")
	// fmt.Printf("Pawns (%064b) amount %d\r\n", bb.Pawns, bits.OnesCount64(bb.Pawns))
	// fmt.Println(getPiecePositionBonusValue(bb.Pawns, whitePawn))
	// fmt.Printf("Knights (%064b) amount %d\r\n", bb.Knights, bits.OnesCount64(bb.Knights))
	// fmt.Println(getPiecePositionBonusValue(bb.Knights, whiteKnight))
	// fmt.Printf("Bishops (%064b) amount %d\r\n", bb.Bishops, bits.OnesCount64(bb.Bishops))
	// fmt.Println(getPiecePositionBonusValue(bb.Bishops, whiteBishop))
	// fmt.Printf("Rooks (%064b) amount %d\r\n", bb.Rooks, bits.OnesCount64(bb.Rooks))
	// fmt.Println(getPiecePositionBonusValue(bb.Rooks, whiteRook))
	// fmt.Printf("Queens (%064b) amount %d\r\n", bb.Queens, bits.OnesCount64(bb.Queens))
	// fmt.Println(getPiecePositionBonusValue(bb.Queens, whiteQueen))
	// fmt.Printf("Kings (%064b) amount %d\r\n", bb.Kings, bits.OnesCount64(bb.Kings))
	// fmt.Println(getPiecePositionBonusValue(bb.Kings, whiteKing))
	// fmt.Println(value)
	//panic("s")
	return value
}

// Calculate the value for one side
func getBoardValueForBlack(bb *dragontoothmg.Bitboards) int {
	value := getPiecesBaseValue(bb)
	value += getPiecePositionBonusValue(bb.Pawns, blackPawn)
	value += getPiecePositionBonusValue(bb.Knights, blackKnight)
	value += getPiecePositionBonusValue(bb.Bishops, blackBishop)
	value += getPiecePositionBonusValue(bb.Rooks, blackRook)
	value += getPiecePositionBonusValue(bb.Queens, blackQueen)
	value += getPiecePositionBonusValue(bb.Kings, blackKing)


	return -value
}

func getPiecesBaseValue(bb *dragontoothmg.Bitboards) int {
	pawns := bits.OnesCount64(bb.Pawns)
	kinghts := bits.OnesCount64(bb.Knights)
	bishops := bits.OnesCount64(bb.Bishops)
	rooks := bits.OnesCount64(bb.Rooks)
	queens := bits.OnesCount64(bb.Queens)
	king := bits.OnesCount64(bb.Kings)
	baseValue := (pawns * 100) + (kinghts * 320) + (bishops * 330) + (rooks * 500) + (queens * 900) + (king * 3000)

	//add bonus for having two bishops
	if bishops == 2 {
		baseValue += 20
	}

	return baseValue
}

// Get value for piece depending on position
// TODO: Add different evals for game status, eg mid and endgame
func getPiecePositionBonusValue(bb uint64, values [64]int) int {

	// fmt.Printf("before bb (%064b)   %v\r\n", *bb, *bb)
	// var arrayPos =  int(bits.LeadingZeros64(*bb))
	// fmt.Printf("bb (%064b) pos 1 %d\r\n", *bb, arrayPos)

	var squares []int

	for bb != 0 {
		arrayPos := 63 - int(bits.LeadingZeros64(bb))
		squares = append(squares, arrayPos)
		bb &= ^(uint64(1) << uint(arrayPos))
	}

	var value int
	//fmt.Println(squares)

	for _, s := range squares {
		value += values[s]
	}
	return value
}

//tables are reversed because of bit board reading. 
// From: https://www.chessprogramming.org/Simplified_Evaluation_Function
// TODO: need to find a way to make it better readable for finetuning
// TODO: different eval for endgame
var blackPawn = [64]int{
	0, 0, 0, 0, 0, 0, 0, 0,
	50, 50, 50, 50, 50, 50, 50, 50,
	10, 10, 20, 30, 30, 20, 10, 10,
	5, 5, 10, 25, 25, 10, 5, 5,
	0, 0, 0, 20, 20, 0, 0, 0,
	5, -5, -10, 0, 0, -10, -5, 5,
	5, 10, 10, -20, -20, 10, 10, 5,
	0, 0, 0, 0, 0, 0, 0, 0}

var whitePawn = [64]int{
	0, 0, 0, 0, 0, 0, 0, 0,
	5, 10, 10, -20, -20, 10, 10, 5,
	5, -5, -10, 0, 0, -10, -5, 5,
	0, 0, 0, 20, 20, 0, 0, 0,
	5, 5, 10, 25, 25, 10, 5, 5,
	10, 10, 20, 30, 30, 20, 10, 10,
	50, 50, 50, 50, 50, 50, 50, 50,
	0, 0, 0, 0, 0, 0, 0, 0}

var blackKnight = [64]int{
	-50, -40, -30, -30, -30, -30, -40, -50,
	-40, -20, 0, 0, 0, 0, -20, -40,
	-30, 0, 10, 15, 15, 10, 0, -30,
	-30, 5, 15, 20, 20, 15, 5, -30,
	-30, 0, 15, 20, 20, 15, 0, -30,
	-30, 5, 10, 15, 15, 10, 5, -30,
	-40, -20, 0, 5, 5, 0, -20, -40,
	-50, -40, -30, -30, -30, -30, -40, -50}

var whiteKnight = [64]int{
	-50, -40, -30, -30, -30, -30, -40, -50,
	-40, -20, 0, 5, 5, 0, -20, -40,
	-30, 5, 10, 15, 15, 10, 5, -30,
	-30, 0, 15, 20, 20, 15, 0, -30,
	-30, 5, 15, 20, 20, 15, 5, -30,
	-30, 0, 10, 15, 15, 10, 0, -30,
	-40, -20, 0, 0, 0, 0, -20, -40,
	-50, -40, -30, -30, -30, -30, -40, -50}

var blackBishop = [64]int{
	-20, -10, -10, -10, -10, -10, -10, -20,
	-10, 0, 0, 0, 0, 0, 0, -10,
	-10, 0, 5, 10, 10, 5, 0, -10,
	-10, 5, 5, 10, 10, 5, 5, -10,
	-10, 0, 10, 10, 10, 10, 0, -10,
	-10, 10, 10, 10, 10, 10, 10, -10,
	-10, 5, 0, 0, 0, 0, 5, -10,
	-20, -10, -10, -10, -10, -10, -10, -20}

var whiteBishop = [64]int{
	-20, -10, -10, -10, -10, -10, -10, -20,
	-10, 5, 0, 0, 0, 0, 5, -10,
	-10, 10, 10, 10, 10, 10, 10, -10,
	-10, 0, 10, 10, 10, 10, 0, -10,
	-10, 5, 5, 10, 10, 5, 5, -10,
	-10, 0, 5, 10, 10, 5, 0, -10,
	-10, 0, 0, 0, 0, 0, 0, -10,
	-20, -10, -10, -10, -10, -10, -10, -20}

var blackRook = [64]int{
	0, 0, 0, 0, 0, 0, 0, 0,
	5, 10, 10, 10, 10, 10, 10, 5,
	-5, 0, 0, 0, 0, 0, 0, -5,
	-5, 0, 0, 0, 0, 0, 0, -5,
	-5, 0, 0, 0, 0, 0, 0, -5,
	-5, 0, 0, 0, 0, 0, 0, -5,
	-5, 0, 0, 0, 0, 0, 0, -5,
	0, 0, 0, 5, 5, 0, 0, 0}

var whiteRook = [64]int{
	0, 0, 0, 5, 5, 0, 0, 0,
	-5, 0, 0, 0, 0, 0, 0, -5,
	-5, 0, 0, 0, 0, 0, 0, -5,
	-5, 0, 0, 0, 0, 0, 0, -5,
	-5, 0, 0, 0, 0, 0, 0, -5,
	-5, 0, 0, 0, 0, 0, 0, -5,
	5, 10, 10, 10, 10, 10, 10, 5,
	0, 0, 0, 0, 0, 0, 0, 0}

var blackQueen = [64]int{
	-20, -10, -10, -5, -5, -10, -10, -20,
	-10, 0, 0, 0, 0, 0, 0, -10,
	-10, 0, 5, 5, 5, 5, 0, -10,
	-5, 0, 5, 5, 5, 5, 0, -5,
	0, 0, 5, 5, 5, 5, 0, -5,
	-10, 5, 5, 5, 5, 5, 0, -10,
	-10, 0, 5, 0, 0, 0, 0, -10,
	-20, -10, -10, -5, -5, -10, -10, -20}

var whiteQueen = [64]int{
	-20, -10, -10, -5, -5, -10, -10, -20,
	-10, 0, 5, 0, 0, 0, 0, -10,
	-10, 5, 5, 5, 5, 5, 0, -10,
	0, 0, 5, 5, 5, 5, 0, -5,
	-5, 0, 5, 5, 5, 5, 0, -5,
	-10, 0, 5, 5, 5, 5, 0, -10,
	-10, 0, 0, 0, 0, 0, 0, -10,
	-20, -10, -10, -5, -5, -10, -10, -20}

var blackKing = [64]int{
	-30, -40, -40, -50, -50, -40, -40, -30,
	-30, -40, -40, -50, -50, -40, -40, -30,
	-30, -40, -40, -50, -50, -40, -40, -30,
	-30, -40, -40, -50, -50, -40, -40, -30,
	-20, -30, -30, -40, -40, -30, -30, -20,
	-10, -20, -20, -20, -20, -20, -20, -10,
	20, 20, 0, 0, 0, 0, 20, 20,
	20, 30, 10, 0, 0, 10, 30, 20}

var whiteKing = [64]int{
	20, 30, 10, 0, 0, 10, 30, 20,
	20, 20, 0, 0, 0, 0, 20, 20,
	-10, -20, -20, -20, -20, -20, -20, -10,
	-20, -30, -30, -40, -40, -30, -30, -20,
	-30, -40, -40, -50, -50, -40, -40, -30,
	-30, -40, -40, -50, -50, -40, -40, -30,
	-30, -40, -40, -50, -50, -40, -40, -30,
	-30, -40, -40, -50, -50, -40, -40, -30}
