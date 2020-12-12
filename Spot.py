import engine
import uci


def main():
    # e = engine.Engine()
    # e.run_some_random_game()
    u = uci.UCI()
    u.start()

if __name__ == '__main__':
    main()
