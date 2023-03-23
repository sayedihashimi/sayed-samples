namespace BlackjackGAFromGPT {
    internal class Program {
        public static void Main() {
            GeneticAlgorithm ga = new GeneticAlgorithm(100, 50, 0.01);
            List<int> strategy = ga.Run();
            Console.WriteLine($"Best strategy: {string.Join(", ", strategy)}");
            Console.WriteLine($"Average score: {ga.EvaluateFitness(strategy)}");
        }
    }
}