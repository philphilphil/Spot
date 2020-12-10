package main

import (
	"fmt"
	"math/rand"
	"github.com/notnil/chess"
)

func main() {
	game := chess.NewGame()
	// generate moves until game is over
	// for game.Outcome() == chess.NoOutcome {
	// 	// select a random move
	// 	moves := game.ValidMoves()
	// 	move := moves[rand.Intn(len(moves))]
	// 	game.Move(move)
	// }
	// print outcome and game PGN
	fmt.Println(game.Position().Board().Draw())
	fmt.Printf("Game completed. %s by %s.\n", game.Outcome(), game.Method())
}