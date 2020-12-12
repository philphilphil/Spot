import engine


class UCI:

    def __init__(self):
        self.send_id()
        self.send_ready()
        pass

    def send_id(self):
        print("id name Spot 0.1")
        print("id author Phil Baum")

    def send_ready(self):
        print("uciok")

    def is_ready(self):
        print("readyok")

    def validate_uci_command(self, uci_command):
        # TODO
        return True

    def perft(self, depth):
        e = engine.Engine()
        e.start_perft(depth)

    def parse_uci_command(self, uci_command):
        cmd_parts = uci_command.split()

        if cmd_parts[0] == "quit":
            return False
        elif cmd_parts[0] == "isready":
            self.is_ready()
        elif cmd_parts[0] == "uci":
            self.is_ready()
        elif cmd_parts[0] == "position":
            pass # TODO
        elif cmd_parts[0] == "go":
            pass # TODO
        elif cmd_parts[0] == "debug":
            pass # TODO
        elif cmd_parts[0] == "perft":
            self.perft(int(cmd_parts[1]))

        return True

    def start(self):

        while True:

            uci_command = input().strip()

            if self.validate_uci_command(uci_command):
                if not self.parse_uci_command(uci_command):
                    print("Exiting.")
                    break
