import chess
import random

def get_piece_value(piece):
    value = 0

    if piece.piece_type == chess.PAWN:
        value = 1
    elif piece.piece_type == chess.KING:
        value = 900
    elif piece.piece_type == chess.QUEEN:
        value = 90
    elif piece.piece_type == chess.ROOK:
        value = 50
    elif piece == chess.BISHOP:
        value = 30
    elif piece.piece_type == chess.KNIGHT:
        value = 30

    return value

def get_board_value(board):
    return 0

def get_random_move(board):
    i = 0
    r = random.randint(0,board.legal_moves.count()-1)
    for move in board.legal_moves:
        if i == r:
            return move
        pass

        i += 1

def main():
    board = chess.Board()

    while not board.is_game_over():
        m = get_random_move(board)
        print(get_piece_value(board.piece_at(m.from_square)))
        board.push(m)
        print(board)
        print("")
        pass
    
    print("game end")
    print(board)

if __name__ == '__main__':
    main()