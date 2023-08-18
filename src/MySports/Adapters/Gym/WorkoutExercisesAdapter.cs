using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Views;
using Android.Widget;
using MySports.Fragments.Gym;
using MySports.Helpers;
using MySports.Models.Gym;

namespace MySports.Adapters.Gym
{
    public class WorkoutExercisesAdapter : SportAdapter<WorkoutExercise>
    {
        private WorkoutExercisesFragment _workoutExercisesFragment;
        private Workout _workout;
        private DbHelper _dbHelper;

        public WorkoutExercisesAdapter(WorkoutExercisesFragment workoutExercisesFragment, List<WorkoutExercise> workoutExercises, Workout workout, DbHelper dbHelper)
            : base (workoutExercises)
        {
            this._workoutExercisesFragment = workoutExercisesFragment;
            this._workout = workout;
            this._dbHelper = dbHelper;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            LayoutInflater inflater = (LayoutInflater)_workoutExercisesFragment.Activity.GetSystemService(Context.LayoutInflaterService);
            View view = inflater.Inflate(Resource.Layout.workout_exercise_list_item, null);

            WorkoutExercise workoutExercise = Items[position];
            ProgramExercise programExercise = _dbHelper.GetProgramExercises(_workout.ProgramId).Where(programExercise => programExercise.Id == workoutExercise.ProgramExerciseId).FirstOrDefault();

            view.FindViewById<TextView>(Resource.Id.workout_exercise_name).Text = programExercise.Name;
            view.FindViewById<TextView>(Resource.Id.workout_exercise_rest_period).Text = programExercise.RestPeriod;
            view.FindViewById<TextView>(Resource.Id.workout_exercise_sets_repetitions).Text = $"{programExercise.Sets} x {programExercise.Repetitions}";
            view.FindViewById<TextView>(Resource.Id.workout_exercise_weight).Text = workoutExercise.Weight;
            view.FindViewById<ImageView>(Resource.Id.workout_exercise_maxed_out).Visibility = workoutExercise.MaxedOut ? ViewStates.Visible : ViewStates.Gone;

            view.Click += delegate
            {
                LayoutInflater layoutInflater = LayoutInflater.From(_workoutExercisesFragment.Activity);
                View view = layoutInflater.Inflate(Resource.Layout.update_workout_exercise, null);

                AlertDialog.Builder builder = new AlertDialog.Builder(_workoutExercisesFragment.Activity);
				builder.SetTitle("Update Workout Exercise");
				builder.SetView(view);
				builder.SetPositiveButton("Update", _workoutExercisesFragment.UpdateWorkoutExerciseAction);
				builder.SetNegativeButton("Cancel", _workoutExercisesFragment.CancelAction);

                // Prepopulate the fields.
                view.FindViewById<TextView>(Resource.Id.update_workout_exercise_id).Text = workoutExercise.Id.ToString();
                view.FindViewById<TextView>(Resource.Id.update_workout_exercise_name).Text = programExercise.Name;
                view.FindViewById<TextView>(Resource.Id.update_workout_exercise_sets).Text = programExercise.Sets.ToString();
                view.FindViewById<TextView>(Resource.Id.update_workout_exercise_repetitions).Text = programExercise.Repetitions;
                view.FindViewById<TextView>(Resource.Id.update_workout_exercise_rest_period).Text = programExercise.RestPeriod;
                view.FindViewById<EditText>(Resource.Id.update_workout_exercise_weight).Text = workoutExercise.Weight;
                view.FindViewById<Switch>(Resource.Id.update_workout_exercise_maxed_out).Checked = workoutExercise.MaxedOut;

                builder.Show();
            };

            return view;
        }
    }
}
