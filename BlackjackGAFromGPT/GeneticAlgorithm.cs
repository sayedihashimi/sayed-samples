using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackGAFromGPT {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class GeneticAlgorithm {
        private readonly int populationSize;
        private readonly int numGenerations;
        private readonly double mutationRate;
        private readonly Random random;

        public GeneticAlgorithm(int populationSize, int numGenerations, double mutationRate) {
            this.populationSize = populationSize;
            this.numGenerations = numGenerations;
            this.mutationRate = mutationRate;
            this.random = new Random();
        }

        public List<int> Run() {
            List<List<int>> population = GenerateInitialPopulation();
            for (int generation = 1; generation <= numGenerations; generation++) {
                List<List<int>> nextPopulation = new List<List<int>>();
                for (int i = 0; i < populationSize; i++) {
                    List<int> parent1 = SelectParent(population);
                    List<int> parent2 = SelectParent(population);
                    List<int> child = Crossover(parent1, parent2);
                    Mutate(child);
                    nextPopulation.Add(child);
                }
                population = nextPopulation;
            }
            return SelectBestIndividual(population);
        }

        private List<List<int>> GenerateInitialPopulation() {
            List<List<int>> population = new List<List<int>>();
            for (int i = 0; i < populationSize; i++) {
                List<int> individual = Enumerable.Range(0, 10).Select(x => random.Next(2, 22)).ToList();
                population.Add(individual);
            }
            return population;
        }

        private List<int> SelectParent(List<List<int>> population) {
            List<int> parent = null;
            double bestFitness = double.MinValue;
            for (int i = 0; i < 5; i++) {
                List<int> candidateParent = population[random.Next(populationSize)];
                double candidateFitness = EvaluateFitness(candidateParent);
                if (candidateFitness > bestFitness) {
                    parent = candidateParent;
                    bestFitness = candidateFitness;
                }
            }
            return parent;
        }

        private List<int> Crossover(List<int> parent1, List<int> parent2) {
            int crossoverPoint = random.Next(parent1.Count);
            List<int> child = parent1.Take(crossoverPoint).Concat(parent2.Skip(crossoverPoint)).ToList();
            return child;
        }

        private void Mutate(List<int> individual) {
            for (int i = 0; i < individual.Count; i++) {
                if (random.NextDouble() < mutationRate) {
                    individual[i] = random.Next(2, 22);
                }
            }
        }

        public double EvaluateFitness(List<int> individual) {
            int numGames = 1000;
            int totalScore = 0;
            for (int i = 0; i < numGames; i++) {
                int score = PlayBlackjack(individual);
                totalScore += score;
            }
            double averageScore = (double)totalScore / numGames;
            return averageScore;
        }

        private int PlayBlackjack(List<int> strategy) {
            int dealerCard = random.Next(2, 12);
            int playerScore = 0;
            bool hasAce = false;
            foreach (int card in strategy) {
                if (card == 11) {
                    hasAce = true;
                }
                playerScore += card;
                if (playerScore > 21 && hasAce) {
                    playerScore -= 10;
                    hasAce = false;
                }
                if (playerScore > 21) {
                    return -1;
                }
                if (playerScore == 21) {
                    return 1;
                }
                if (dealerCard >= 17) {
                    if (dealerCard > 21 || playerScore > dealerCard) {
                        return 1;
                    }
                    if (playerScore < dealerCard) {
                        return -1;
                    }
                    return 0;
                }
                while (dealerCard < 17) {
                    dealerCard += random.Next(2, 12);
                    if (dealerCard > 21) {
                        return 1;
                    }
                }
                if (playerScore > dealerCard) {
                    return 1;
                }
                if (playerScore < dealerCard) {
                    return -1;
                }
                return 0;
            }
            return playerScore;
        }
        private List<int> SelectBestIndividual(List<List<int>> population) {
            List<int> bestIndividual = null;
            double bestFitness = double.MinValue;
            foreach (List<int> individual in population) {
                double fitness = EvaluateFitness(individual);
                if (fitness > bestFitness) {
                    bestIndividual = individual;
                    bestFitness = fitness;
                }
            }
            return bestIndividual;
        }
    }
}
