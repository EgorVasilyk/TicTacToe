using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TicTacToe {
    public partial class MainWindow : Window {
        private const string X = "X";
        private const string O = "O";

        private List<(int, int)[]> victoryCombinations = new List<(int, int)[]> {
            new (int, int)[] { (0, 0), (1, 0), (2, 0) },
            new (int, int)[] { (0, 1), (1, 1), (2, 1) },
            new (int, int)[] { (0, 2), (1, 2), (2, 2) },

            new (int, int)[] { (0, 0), (0, 1), (0, 2) },
            new (int, int)[] { (1, 0), (1, 1), (1, 2) },
            new (int, int)[] { (2, 0), (2, 1), (2, 2) },

            new (int, int)[] { (0, 0), (1, 1), (2, 2) },
            new (int, int)[] { (0, 2), (1, 1), (2, 0) },
        };

        private Button[,] field;

        private string currentPlayerSign = "";

        public MainWindow() {
            InitializeComponent();

            field = new Button[3, 3] {
                { cell11, cell12, cell13 },
                { cell21, cell22, cell23 },
                { cell31, cell32, cell33 },
            };
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            Start();
        }

        private bool Move(int r, int c, string s) {
            Button targetCell = field[r, c];

            if ((string)targetCell.Content != "") return false;

            targetCell.Content = s;

            string winner = FindWinner();

            if (winner != "") {
                Finish(winner);
                return false;
            }

            return true;
        }
        private void PlayerMove(int r, int c) {
            bool shouldMove = Move(r, c, currentPlayerSign);
            if (shouldMove) BotMove();
        }

        private void BotMove() {
            List<(int, int)> candidates = GetBotMoveCandidates();
            if (candidates.Count == 0) {
                Finish("");
                return;
            }
            int i = (int)new Random().NextInt64(candidates.Count);
            var (r, c) = candidates[i];
            Move(r, c, currentPlayerSign == X ? O : X);
        }

        private List<(int, int)> GetBotMoveCandidates() {
            List<(int, int)> result = new List<(int, int)>();
            for (int r = 0; r < 3; r++) {
                for (int c = 0; c < 3; c++) {
                    Button cell = field[r, c];
                    if ((string)cell.Content == "") result.Add((r, c));
                }
            }
            return result;
        }
        private string FindWinner() {
            foreach ((int, int)[] combination in victoryCombinations) {
                string result = VerifyVictoryCombination(combination);
                if (result != "") return result;
            }
            return "";
        }
        private string VerifyVictoryCombination((int, int)[] combination) {
            string previous = "";
            foreach (var (r, c) in combination) {
                Button cell = field[r, c];
                string current = (string)cell.Content;
                if (current == "") return "";
                if (previous == "") {
                    previous = current;
                    continue;
                }
                if (previous != current) return "";
                previous = current;
            }
            return previous;
        }
        private void PickPlayerSign() {
            if (currentPlayerSign == "") {
                long v = new Random().NextInt64(2);
                currentPlayerSign = v == 0 ? X : O;
                return;
            }
            currentPlayerSign = currentPlayerSign == X ? O : X;
        }
        private void Start() {
            foreach (Button cell in field) {
                cell.Content = "";
                cell.IsEnabled = true;
            }
            PickPlayerSign();
            statusLabel.Content = "";
        }
        private void Finish(string winner) {
            foreach (Button cell in field) {
                cell.Content = "";
                cell.IsEnabled = false;
            }
            if (winner == ""){
                statusLabel.Content = "Ничья";
                return;
            }
            statusLabel.Content = $"Победили { (winner == X ? "крестики" : "нолики") }";
        }

        private void cell11_Click(object sender, RoutedEventArgs e) {
            PlayerMove(0, 0);
        }

        private void cell12_Click(object sender, RoutedEventArgs e) {
            PlayerMove(0, 1);
        }

        private void cell13_Click(object sender, RoutedEventArgs e) {
            PlayerMove(0, 2);
        }

        private void cell21_Click(object sender, RoutedEventArgs e) {
            PlayerMove(1, 0);
        }

        private void cell22_Click(object sender, RoutedEventArgs e) {
            PlayerMove(1, 1);
        }

        private void cell23_Click(object sender, RoutedEventArgs e) {
            PlayerMove(1, 2);
        }

        private void cell31_Click(object sender, RoutedEventArgs e) {
            PlayerMove(2, 0);
        }

        private void cell32_Click(object sender, RoutedEventArgs e) {
            PlayerMove(2, 1);
        }

        private void cell33_Click(object sender, RoutedEventArgs e) {
            PlayerMove(2, 2);
        }
    }
}
