namespace MyWorkoutAndroid.Models.Gym
{
    public class ProgramExercise
    {
        public int Id { get; set; }

        public int ProgramId { get; set; }

        public string Name { get; set; }

        public int Sets { get; set; }

        public string Repetitions { get; set; }

        public string RestPeriod { get; set; }
    }
}
