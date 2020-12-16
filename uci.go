package main

import "fmt"
import "bufio"
import "os"
import "strings"
import "github.com/dylhunn/dragontoothmg"

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

	for {
		scanner := bufio.NewScanner(os.Stdin)
		if scanner.Scan() {
			line := scanner.Text()
			args := strings.Fields(line)

			if !u.validateUciCommand(args) {
				panic("Error in UCI command")
			}

			if !u.parseUciCommand(args) {
				u.quit()
				break
			}
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
		//TODO turn debug move on or off
	case "setoption":
		//TODO as soon as options are avaibale
	case "position":
		//TODO: implement better
		setGamePosition(&game, args[1:])
		//set position for engine
	case "go":
		bestMove := calculateBestMove(&game)
		fmt.Println("bestmove ", bestMove.String())
	case "stop":
		//stop engine search, return bestmove
	case "ponderhit":
		//TODO, maybe?
	case "isready":
		u.isReady()
	}

	return true
}

func setGamePosition(g *dragontoothmg.Board, args []string) {

	if args[0] == "fen" {
		//TODO parse fen here if not startpos
	} else if args[0] == "startpos" {
		game = dragontoothmg.ParseFen("rnb1kbnr/pppp2pp/8/4ppq1/3PP3/5N2/PPP2PPP/RNBQKB1R w KQkq - 1 4")
	} else {
		panic("Invalid command, fen or startpos needed.")
	}

	if args[1] != "moves" {
		panic("Invalid command, moves required.") //TODO: maybe check this in validateUciCommand?
	}

	// get moves and apply to board
	for _, m := range args[2:] {
		fmt.Println("debug move string:", m)
		move, err := dragontoothmg.ParseMove(m)
		if err != nil {
			panic(err)
		}
		fmt.Println("debug move:", move)
		game.Apply(move)
		fmt.Println("debug fen:", game.ToFen())
	}

}

func (u *UCIs) quit() {
	fmt.Println("Exiting")
}

func (u *UCIs) sendId() {
	fmt.Println("id name Spot 0.1 alpha")
	fmt.Println("id author Phil Baum")
}

func (u *UCIs) sendOptions() {
	//TODO: sent options which can be changed eg. hashsize
	fmt.Println("uciok")
}

func (u *UCIs) debug() {
	fmt.Println("debug")
}

func (u *UCIs) uci() {
	u.sendId()
	u.sendOptions()
}

func (u *UCIs) isReady() {
	fmt.Println("readyok")
}
