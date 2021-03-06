package main

import (
	"fmt"
	"github.com/dylhunn/dragontoothmg"
	"log"
	"math"
	"os"
	"time"
	//"flag"
	// "runtime"
	// "runtime/pprof"
)

//Version
var BuildVersion string = ""
var CommitHash string = ""

//Debug things
// var cpuprofile = flag.String("cpuprofile", "", "write cpu profile to `file`")
// var memprofile = flag.String("memprofile", "", "write memory profile to `file`")

//Chess things
var piecePositionMasks = [64]uint64{1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192, 16384, 32768, 65536, 131072, 262144, 524288, 1048576, 2097152, 4194304, 8388608, 16777216, 33554432, 67108864, 134217728, 268435456, 536870912, 1073741824, 2147483648, 4294967296, 8589934592, 17179869184, 34359738368, 68719476736, 137438953472, 274877906944, 549755813888, 1099511627776, 2199023255552, 4398046511104, 8796093022208, 17592186044416, 35184372088832, 70368744177664, 140737488355328, 281474976710656, 562949953421312, 1125899906842624, 2251799813685248, 4503599627370496, 9007199254740992, 18014398509481984, 36028797018963968, 72057594037927936, 144115188075855872, 288230376151711744, 576460752303423488, 1152921504606846976, 2305843009213693952, 4611686018427387904, 9223372036854775808}
var nodesSearched uint64
var mateScore = 9999
var opptime, oppinc, mytime, myinc int64 //times in ms
var stopSearch bool = false
var searchStartTime time.Time

func main() {
	///////// LOG OUTPUT, CPU/MEMORY PROFLINIG STUFF ///////
	// flag.Parse()
	// if *cpuprofile != "" {
	// 	f, err := os.Create(*cpuprofile)
	// 	if err != nil {
	// 		log.Fatal("could not create CPU profile: ", err)
	// 	}
	// 	defer f.Close() // error handling omitted for example
	// 	if err := pprof.StartCPUProfile(f); err != nil {
	// 		log.Fatal("could not start CPU profile: ", err)
	// 	}
	// 	defer pprof.StopCPUProfile()
	// }

	// if *memprofile != "" {
	// 	f, err := os.Create(*memprofile)
	// 	if err != nil {
	// 		log.Fatal("could not create memory profile: ", err)
	// 	}
	// 	defer f.Close() // error handling omitted for example
	// 	runtime.GC()    // get up-to-date statistics
	// 	if err := pprof.WriteHeapProfile(f); err != nil {
	// 		log.Fatal("could not write memory profile: ", err)
	// 	}
	// }

	file, err := os.OpenFile("spot_debug.log", os.O_CREATE|os.O_APPEND|os.O_WRONLY, 0644)
	log.SetOutput(file)

	if err != nil {
		fmt.Println(err)
	}

	defer file.Close()
	////////////////////////////////////////////////////////

	//debug = true

	uci := UCIs{}
	uci.Start()
}

func calculateBestMove(b dragontoothmg.Board, searchFixedDepth int) dragontoothmg.Move {

	var bestMove dragontoothmg.Move
	var color int
	var pvline []string
	var currLine []string
	transpoTable = make(map[uint64]Hashtable) // clear tt
	currDepth := 0
	var bestBoardVal int = 9999
	window_size := 1000 // TODO: Tweak window size after null move, killer move and move ordering are implemented
	alpha := -9999
	beta := 9999

	if b.Wtomove { //beaucse of our root node, colors need to be switched here
		color = 1
	} else {
		color = -1
	}

	timeForMove := calculateTimeForMove()
	printLog(fmt.Sprintf("Time to calc move: %v sec", timeForMove/1000))
	searchStartTime = time.Now()

	for {
		printLog(fmt.Sprintf("START: BestBoardVal: %v Alpha/Beta: %v / %v  WindowSize: %v\r\n", bestBoardVal, alpha, beta, window_size))
		currDepth++
		bestBoardVal = -9999

		moves := generateAndOrderMoves(b.GenerateLegalMoves(), bestMove)
		printLog(fmt.Sprintf("Orderd Moves %v", MovesToString(moves)))

		for _, move := range moves {
			nodesSearched = 0
			currLine = nil
			unapply := b.Apply(move)
			boardVal := -negaMaxAlphaBeta(b, currDepth, -beta, -alpha, -color, &currLine)
			unapply()

			printLog(fmt.Sprintf("White Move: %t Color: %v Depth: %v Move: %v Eval: %v CurBestEval: %v Nodes: %v Time: %v sec",
				b.Wtomove, color, currDepth, move.String(), boardVal, bestBoardVal, nodesSearched, time.Since(searchStartTime)))

			//check mate check
			if boardVal >= mateScore-10 { //found a forced mate
				pvline = append(currLine, move.String())
				printUCIInfo("", currDepth, int(time.Since(searchStartTime).Milliseconds()), int(nodesSearched), boardVal, pvline)
				return move
			}

			if boardVal >= bestBoardVal {
				bestMove = move
				pvline = append(currLine, move.String())
				bestBoardVal = boardVal
			}

			//printUCIInfo(move.String(), currDepth, int(time.Since(searchStartTime).Milliseconds()), int(nodesSearched), bestBoardVal, nil)

			if stopSearch { //Stop search if triggerd by uci
				return bestMove
			} else if mytime != 0 { //stop search when time limit reached
				if time.Since(searchStartTime).Milliseconds() >= timeForMove {
					printUCIInfo("", currDepth, int(time.Since(searchStartTime).Milliseconds()), int(nodesSearched), bestBoardVal, pvline)
					return bestMove
				}
			}
		}
		printUCIInfo("", currDepth, int(time.Since(searchStartTime).Milliseconds()), int(nodesSearched), bestBoardVal, pvline)

		if bestBoardVal <= alpha || bestBoardVal >= beta {
			printLog(fmt.Sprintf("Out of aspiration window bounds, doing research Alpha %v Beta: %v BestVal: %v", alpha, beta, bestBoardVal))
			alpha = -9999
			beta = 9999
			currDepth--
		} else {
			//Aspiration Window
			alpha = -bestBoardVal - window_size
			beta = bestBoardVal + window_size
		}

		//Check if search needs to stop
		// Uci sends stop command or fixed depth reached.  times check is done above
		if stopSearch || (searchFixedDepth != 0 && currDepth == searchFixedDepth) {
			return bestMove
		}

		// printLogTop100OfTT()
		// panic("stop")
		printLog(fmt.Sprintf("END: BestBoardVal: %v Alpha/Beta: %v / %v  WindowSize: %v\r\n", bestBoardVal, alpha, beta, window_size))

	}

	return bestMove
}

func negaMaxAlphaBeta(b dragontoothmg.Board, depth int, alpha int, beta int, color int, pvline *[]string) int {

	var line []string
	moves := b.GenerateLegalMoves()

	if !b.OurKingInCheck() && len(moves) == 0 { //stalemate
		return 1 * color
	} else if b.OurKingInCheck() && len(moves) == 0 { //checkmate
		return -(mateScore - depth)
	}

	if depth == 0 {
		val := getBoardValue(&b) //Quiesce(b, alpha, beta)
		return val * color
	}

	//check TT Table
	tt, ok := transpoTable[b.Hash()]
	if ok && tt.depth >= depth {
		if tt.bound == Exact {
			return tt.score
		} else if tt.bound == LowerBound {
			alpha = Max(alpha, tt.score)
		} else if tt.bound == UpperBound {
			beta = Min(beta, tt.score)
		}

		if alpha > beta {
			return tt.score
		}
	}

	alphaOrig := alpha
	bestScore := -9999
	score := 0
	for _, move := range moves {
		unapply := b.Apply(move)
		nodesSearched++
		score = -negaMaxAlphaBeta(b, depth-1, -beta, -alpha, -color, &line)
		unapply()

		if score >= beta {
			*pvline = append(line, move.String())

			return score
		}

		if score > bestScore {
			bestScore = score
			if score > alpha {
				alpha = score
			}
		}

		*pvline = append(line, move.String())
	}

	//write TT
	ht := Hashtable{depth: depth, score: score, zobrist: b.Hash()}
	if score < alphaOrig {
		ht.bound = UpperBound
	} else if score > beta {
		ht.bound = LowerBound
	} else {
		ht.bound = Exact
	}
	transpoTable[b.Hash()] = ht

	return bestScore
}

func generateAndOrderMoves(moves []dragontoothmg.Move, bestMove dragontoothmg.Move) []dragontoothmg.Move {

	var orderedMoves []dragontoothmg.Move
	var bestMoveLocation int

	if bestMove != 0 {
		// Step 1 find previus best move and put on pos 1
		for i, m := range moves {
			if m == bestMove {
				orderedMoves = append(orderedMoves, m)
				bestMoveLocation = i
			}
		}
	}

	//step 2, go over list again and sort other moves
	// TODO: implement sorting, currently random
	for i, m := range moves {
		if bestMoveLocation != 0 && i == bestMoveLocation {
			continue
		}
		orderedMoves = append(orderedMoves, m)
	}

	// fmt.Println(moves)
	// fmt.Println(orderedMoves)

	return orderedMoves
}

func Quiesce(b dragontoothmg.Board, alpha int, beta int) int {

	eval := getBoardValue(&b)

	if eval >= beta {
		return eval
	}
	if alpha < eval {
		alpha = eval
	}

	captureMoves := generateCaptureMoves(&b)

	for _, m := range captureMoves {
		unapply := b.Apply(m)
		score := -Quiesce(b, -beta, -alpha)
		unapply()

		if score >= beta {
			return beta

		}
		if score > alpha {
			alpha = score
		}
	}

	return alpha
}

func generateCaptureMoves(b *dragontoothmg.Board) []dragontoothmg.Move {

	var captures []dragontoothmg.Move

	moves := b.GenerateLegalMoves()

	for _, m := range moves {
		if dragontoothmg.IsCapture(m, b) {
			captures = append(captures, m)
		}
	}

	return captures
}

// ty Jonatan! :)
// https://mediocrechess.blogspot.com/2007/01/guide-time-management.html
func calculateTimeForMove() int64 {
	// Use 2.25% of the time + 80% of the increment
	inc := math.Round(float64(myinc) * 0.8)
	timeForMove := mytime/40 + int64(inc)

	// if on low time use 1 seconds to atleast get some move
	if timeForMove < 0 {
		timeForMove = 1000
	}

	return timeForMove
}
