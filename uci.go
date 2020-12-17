package main

import "fmt"
import "bufio"
import "os"
import "log"
import "strings"

//import "time"
import "github.com/dylhunn/dragontoothmg"

var game dragontoothmg.Board
var debug bool = false

// http://page.mi.fu-berlin.de/block/uci.htm
// Implements the universal chess interface
type UCI interface { //all functions called by chess gui or engine tester
	uci()
	stop()
	gouci([]string)
	isReady()
	sendId()
	sendOptions()
	perft([]string)
	parseUciCommand([]string)
	validateUciCommand([]string)
}

type UCIs struct {
	//game dragontoothmg.Board
}

// Starts the UCI process, waiting for command line input from other software
func (u *UCIs) Start() {
	u.uci() //TODO maybe not here
	reader := bufio.NewReader(os.Stdin)

	for {

		line, err := reader.ReadString('\n')
		if err != nil {
			printLog("Error in UCI command")
		}

		//printLog("SCANNER:" + line)
		args := strings.Fields(line)

		if !u.validateUciCommand(args) {
			printLog("Error in UCI command")
		}

		if debug {
			log.Println("<- ", args)
		}

		if !u.parseUciCommand(args) {
			u.quit()
			break
		}
	}
}

// Validates input from the CLI
func (u *UCIs) validateUciCommand(args []string) bool {
	return true //TODO
}

// Parses UCI command and excecutes
func (u *UCIs) parseUciCommand(args []string) bool {

	switch args[0] {
	case "quit":
		return false
	case "uci":
		u.sendId()
		u.sendOptions()
	case "debug":
		debug = true
	case "setoption":
		//TODO as soon as options are avaibale
	case "position":
		//TODO: implement better
		printLog("pos set start")
		setGamePosition(&game, args[1:])
		printLog("pos set done")
	case "go":
		printLog("go start")
		bestMove := calculateBestMove(&game)
		printLog("go done bestmove: " + bestMove.String())
		printMessage("bestmove "+bestMove.String())
	case "stop":
		//stop engine search, return bestmove
	case "ponderhit":
		//TODO, maybe?
	case "isready":
		u.isReady()
	}

	return true
}

// GUI/Runner sends information about the position and moves played from that position
// position can be sent via "startpos" or fen string
// for game setup position is sent without moves
func setGamePosition(g *dragontoothmg.Board, args []string) {

	movesLocation := getMovesLocation(args)

	if movesLocation == -1 { //no moves sent just set position
		game = getGameFromFen(args)
	} else {
		game = getGameFromFen(args[:movesLocation])

		// get moves and apply to board
		for _, m := range args[movesLocation+1:] {
			move, err := dragontoothmg.ParseMove(m)
			if err != nil {
				panic(err)
			}
			game.Apply(move)
		}
	}
	//log.Println("debug", game.ToFen())
}

func getGameFromFen(args []string) dragontoothmg.Board {
	if args[0] == "fen" {
		fen := strings.Join(args[1:], " ")
		return dragontoothmg.ParseFen(fen)
	} else if args[0] == "startpos" {
		return dragontoothmg.ParseFen("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")
	} else {
		printMessage("ERROR: Invalid command, fen or startpos needed.") //TODO: maybe check this in validateUciCommand?
	}
	panic("s")
}

func (u *UCIs) quit() {
	printMessage("Exiting")
}

func (u *UCIs) sendId() {
	printMessage("id name Spot 0.1 alpha")
	printMessage("id author Phil Baum")
}

func (u *UCIs) sendOptions() {
	//TODO: sent options which can be changed eg. hashsize
	printMessage("uciok")
}

func (u *UCIs) debug() {
	printMessage("debug")
}

func (u *UCIs) uci() {
	u.sendId()
	u.sendOptions()
}

func (u *UCIs) isReady() {
	printMessage("readyok")
}

func printMessage(cmd string) {
	if debug {
		log.Println("-> ", cmd)
	}
	fmt.Println(cmd)
}

func printLog(msg string) {
	if debug {
		log.Println("::DBG:: ", msg)
	}
}

func getMovesLocation(args []string) int {
	for i, a := range args {
		if a == "moves" {
			return i
		}
	}
	return -1
}
