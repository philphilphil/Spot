#!/bin/bash
rm c-chess-cli.1.log
/Users/phil/Projects/c-chess-cli/c-chess-cli -each tc=40+15  option.Threads=1 -engine cmd=stockfish -engine cmd=../Spot -games 2 -log -pgn out.pgn 1