package main

import (
	"fmt"
	"github.com/dylhunn/dragontoothmg"
	"log"
	"math/bits"
	"os"
)

var nodesSearched uint64

func main() {
	// board := dragontoothmg.ParseFen("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")
	// fmt.Println(getBoardValue(&board))
	// move := calculateBestMove(&board)
	// fmt.Println(move.String())

	// val := getBoardValue(&board)
	// fmt.Println(val)

	file, err := os.OpenFile("info.log", os.O_CREATE|os.O_APPEND|os.O_WRONLY, 0644)
	log.SetOutput(file)

	if err != nil {
		log.Fatal(err)
	}

	defer file.Close()

	uci := UCIs{}
	uci.Start()

}

func calculateBestMove(b *dragontoothmg.Board) dragontoothmg.Move {

	var bestBoardVal int = 0
	moves := b.GenerateLegalMoves()
	var bestMove = moves[0]

	if b.Wtomove {
		bestBoardVal = -9999
	} else {
		bestBoardVal = 9999
	}

	for _, move := range moves {
		unapply := b.Apply(move)
		nodesSearched++
		boardVal := minimax(b, 4, -9999, 9999)
		unapply()

		if debug {
			printLog(fmt.Sprintf("White Move: %t Move: %v Eval: %v Nodes: %v", b.Wtomove, move.String(), boardVal, nodesSearched))
		}

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
	//log.Println(nodesSearched)
	return bestMove
}

func minimax(b *dragontoothmg.Board, depth int, alpha int, beta int) int {
	if depth == 0 {
		return getBoardValue(b)
	} else {
		if b.Wtomove {
			moves := b.GenerateLegalMoves()
			for _, move := range moves {
				unapply := b.Apply(move)
				nodesSearched++
				score := minimax(b, depth-1, alpha, beta)
				unapply()
				if score > alpha {
					alpha = score
					if alpha >= beta {
						//printLog("Breaking here. Move: " + move.String())
						break
					}
				}
			}
			return alpha
		} else {
			moves := b.GenerateLegalMoves()
			for _, move := range moves {
				unapply := b.Apply(move)
				nodesSearched++
				score := minimax(b, depth-1, alpha, beta)
				unapply()
				if score < beta {
					beta = score
					if alpha >= beta {
						//printLog("Breaking here. Move: " + move.String())
						break
					}
				}
			}
			return beta
		}
	}

}

// Get value for entire board
func getBoardValue(b *dragontoothmg.Board) int {

	//fmt.Println("white", b.Wtomove)
	boardValueWhite := getBoardValueForWhite(&b.White)
	boardValueBlack := -getBoardValueForBlack(&b.Black)

	return boardValueWhite + boardValueBlack
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

	return value
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

	squares := getPieceSquareNumbers(bb)

	for _, s := range squares {
		value += values[s]
		//fmt.Println(values[s])
	}
	return value
}

// Get index of pieces starting down left = 0
func getPieceSquareNumbers(bb *uint64) []int {
	//TODO: add check for amount of pieces, if all pieces found stop looking
	var squareNumbers []int
	piecePositionMasks := getPiecePositionMasks()
	//log.Println("maskss",piecePositionMasks)

	//Itterate all bit-masks, check if piece is on square, if yes add index of it
	for i := 0; i <= 63; i++ {
		if *bb&piecePositionMasks[i] != 0 {
			squareNumbers = append(squareNumbers, i)
		}
	}

	//fmt.Println(squareNumbers)
	return squareNumbers
}

// Generate a bit-mask for each square to use for comparison
func getPiecePositionMasks() [64]uint64 {
	// TODO: test if list generated by hand is faster than with the loop
	var positionMasks [64]uint64
	positionMasks[0] = 1
	for i := 1; i <= 63; i++ {
		positionMasks[i] = positionMasks[i-1] * 2
	}
	return positionMasks
}
