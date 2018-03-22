namespace Genetic
{
    public enum SelectionType
    {
        TournamentSelection,
        RouletteSelection,
        OptimizedRouletteSelection
    }

    public class GeneticParameters
    {
        private int genCount;
        private int popSize;
        private float Px;
        private float Pm;
        public SelectionType selectionType = SelectionType.TournamentSelection;
        private int tour;

        public int generationsCount
        {
            get { return genCount; }
            set { if (value > 0) genCount = value; else genCount = 1; }
        }

        public int populationSize
        {
            get { return popSize; }
            set { if (value > 0) popSize = value; else popSize = 1; }
        }

        public float crossoverProb
        {
            get { return Px; }
            set { if (value >= 0 && value <= 1) Px = value; else Px = 0; }
        }

        public float mutationProb
        {
            get { return Pm; }
            set { if (value >= 0 && value <= 1) Pm = value; else Pm = 0; }
        }


        public int tournamentSize
        {
            get { return tour; }
            set { if (value > 0) tour = value; else tour = 1; }
        }
    }
}