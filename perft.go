package main

import "github.com/dylhunn/dragontoothmg"
import "fmt"
import "time"


func StartPerft(n int) {
	start := time.Now()
	board := dragontoothmg.ParseFen("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")
	a := Perft(&board, n)

	fmt.Printf("Perft Depth: %d found moves: %d time: %s \r\n", n, a, time.Since(start))
}

func Perft(b *dragontoothmg.Board, n int) int64 {
	if n <= 0 {
		return 1
	}
	moves := b.GenerateLegalMoves()
	if n == 1 {
		return int64(len(moves))
	}
	var count int64 = 0
	for _, move := range moves {
		unapply := b.Apply(move)
		count += Perft(b, n-1)
		unapply()
	}
	return int64(count)
}
