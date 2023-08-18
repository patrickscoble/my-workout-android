namespace MySports.Models.Gym
{
    public class WorkoutExercise
    {
        public int Id { get; set; }

        public int WorkoutId { get; set; }

        public int ProgramExerciseId { get; set; }

        public string Weight { get; set; }

        public bool MaxedOut { get; set; }
    }
}
