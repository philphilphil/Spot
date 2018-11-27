# CHEP
Work in Progress Chess engine written in .NET Core.

I decided to not write my own Chess implementation since there is already a nice one: [Chess.NET](https://github.com/ProgramFOX/Chess.NET)
## Testing vs Stockfish
With [cutechess-cli](https://github.com/cutechess/cutechess) I ran automated tests after development milestones with 20 games against [Stockfish 9](https://github.com/official-stockfish/Stockfish) on a certain level. Result is winsChep-winsSF-draws. PGN for the games in Testgames-Folder.

| Commit   | Comment                                       | Depth | SF Level | Moves/Time        | Result |
|----------|-----------------------------------------------|-------|----------|-------------------|--------|
| c45b7eb* | Dept 4, basic minimax with alpha-beta pruning | 4     | 0        | C: inf, SF: 40/60 | 2-14-3 |
| c45b7eb* |                       ""                      | 4     | 5        | C: inf, SF: 40/60 | 0-20-0 |
| 0b42d9f  | Added piece position evals                    | 4     | 0        | C: inf, SF: 40/60 | 10-4-6 |
| 0b42d9f  |                       ""                      | 4     | 5        | C: inf, SF: 40/60 |        |
*with some bugfixes backported
