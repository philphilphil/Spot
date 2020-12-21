# Spot 🐱
Work in Progress UCI compliant Chess engine. Move gen via [dragontoothmg](https://pkg.go.dev/github.com/dylhunn/dragontoothmg).
## Testing vs Stockfish
Out of interest how the  engine strength grows, I ran automated tests after development milestones with [c-chess-cli](https://github.com/lucasart/c-chess-cli). 20 games against [Stockfish 12](https://github.com/official-stockfish/Stockfish) on a certain level (20 is max with ELO ~3500). Result is winsSpot-draws-winsSF. Run on M1.

| Commit  | Comment/Added                                | Spot Depth | SF Level | TC (s) | Result |
|---------|----------------------------------------------|------------|----------|--------|--------|
| 8bd1439 | Core Engine with basic eval and minimax      | 4          | 0        | 40+15  | 2-2-16 |
| 8bd1439 | ""                                           | 4          | 10       | 40+15  | 0-0-20 |
| 72f7d33 | Alpha beta pruning and piece position eval   | 4          | 0        | 40+15  | 5-8-7  |
| 72f7d33 | ""                                           | 4          | 10       | 40+15  | 0-0-20 |