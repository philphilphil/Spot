import chess
import random
import time

class Engine:

    def get_piece_value(self, piece):
        value = 0

        if piece == chess.PAWN:
            value = 1
        elif piece == chess.KING:
            value = 900
        elif piece == chess.QUEEN:
            value = 90
        elif piece == chess.ROOK:
            value = 50
        elif piece == chess.BISHOP:
            value = 30
        elif piece == chess.KNIGHT:
            value = 30

        return value  # if piece.color == chess.WHITE else -value

    def get_board_value(self, board, color):
        pieces = [(chess.PAWN), (chess.BISHOP), (chess.KING),
                  (chess.QUEEN), (chess.KNIGHT), (chess.ROOK)]
        value = 0

        for (piece) in pieces:
            value += len(board.pieces(piece, color)) * \
                self.get_piece_value(piece)
            value -= len(board.pieces(piece, not color)) * \
                self.get_piece_value(piece)

        return value

    def get_best_move(self, board, player):
        bestBoardValue = self.get_board_value(board, player)
        bestMove = ""

        for m in board.legal_moves:
            board.push(m)
            boardValue = self.get_board_value(board, player)
            board.pop()

            if player == chess.BLACK and bestBoardValue <= boardValue:
                bestBoardValue = boardValue
                bestMove = m
            elif player == chess.WHITE and bestBoardValue >= boardValue:
                bestBoardValue = boardValue
                bestMove = m
            pass

        return bestMove

    def run_some_random_game(self):
        board = chess.Board()

        while not board.is_game_over():
            m = self.get_best_move(board.copy(), board.turn)
            print(self.get_piece_value(board.piece_at(m.from_square)))
            board.push(m)
            print(self.get_board_value(board, board.turn))
            print(board)
            print("")
            pass

        print("game end")
        print(board)

    def start_perft(self, depth):
        start = time.time()
        board = chess.Board()
        count = self.perft(depth, board)
        end = time.time()
        print("Perft Depth: "+ str(depth) + " found moves: " + str(count) + " time: " + str(end-start) + "sec")

    def perft(self, depth, board):
        if depth == 0:
            return 1

        count = 0

        for move in board.legal_moves:
            board.push(move)
            count += self.perft(depth - 1, board)
            board.pop()

        return count

    def __init__(self):
        pass
