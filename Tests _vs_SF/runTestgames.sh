#!/bin/bash
rm c-chess-cli.1.log
rm out.pgn
/Users/phil/Projects/c-chess-cli/c-chess-cli -each tc=40+15  option.Threads=1 -engine cmd=stockfish "option.Skill Level=0" -engine cmd=../Spot -games 2 -log -pgn out.pgn 1