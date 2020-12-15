package main

import "fmt"
import "bufio"
import "os"
import "strings"

type UCI interface { //all functions called by chess gui or engine tester
	uci()
	stop()
	go2([]string)
	isReady()
	sendId()
	sendReady()
	sendOptions()
	perft([]string)
	parseUciCommand([]string)
	validateUciCommand([]string)
}

// http://page.mi.fu-berlin.de/block/uci.htm
type UCIs struct {
}

func (u *UCIs) Start() {
	u.sendId()
	u.sendOptions()

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

func (u *UCIs) validateUciCommand(args []string) bool {
	return true //TODO
}

func (u *UCIs) parseUciCommand(args []string) bool {

	switch args[0] {
	case "quit":
		return false
	case "isready":
		u.isReady()
	}

	return true
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

func (u *UCIs) sendReady() {
	fmt.Println("isready")
}

func (u *UCIs) debug() {
	fmt.Println("debug")
}

func (u *UCIs) uci() {
	fmt.Println("uci")
}

func (u *UCIs) isReady() {
	fmt.Println("readyok")
}
