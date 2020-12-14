import engine
import uci


def main():
    e = engine.Engine()
    # e.run_some_random_game()
    # u = uci.UCI()
    # u.start()
    e.start_perft(2);
    e.start_perft(3);
    #e.start_perft(4);
    #e.start_perft(5);
    #e.start_perft(6);

if __name__ == '__main__':
    main()
