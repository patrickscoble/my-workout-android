using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Widget;
using MyWorkoutAndroid.Fragments.Gym;
using MyWorkoutAndroid.Models.Gym;

namespace MyWorkoutAndroid.Adapters.Gym
{
    public class ProgramExercisesAdapter : SportAdapter<ProgramExercise>
    {
        private ProgramExercisesFragment _programExercisesFragment;

        public ProgramExercisesAdapter(ProgramExercisesFragment programExercisesFragment, List<ProgramExercise> programExercises)
            : base (programExercises)
        {
            this._programExercisesFragment = programExercisesFragment;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            LayoutInflater inflater = (LayoutInflater)_programExercisesFragment.Activity.GetSystemService(Context.LayoutInflaterService);
            View view = inflater.Inflate(Resource.Layout.program_exercise_list_item, null);

            ProgramExercise programExercise = Items[position];

            view.FindViewById<TextView>(Resource.Id.program_exercise_name).Text = programExercise.Name;
            view.FindViewById<TextView>(Resource.Id.program_exercise_rest_period).Text = programExercise.RestPeriod;
            view.FindViewById<TextView>(Resource.Id.program_exercise_sets_repetitions).Text = $"{programExercise.Sets} x {programExercise.Repetitions}";

            view.Click += delegate
            {
                LayoutInflater layoutInflater = LayoutInflater.From(_programExercisesFragment.Activity);
                View view = layoutInflater.Inflate(Resource.Layout.create_update_program_exercise, null);

				AlertDialog.Builder builder = new AlertDialog.Builder(_programExercisesFragment.Activity);
				builder.SetTitle("Update Program Exercise");
				builder.SetView(view);
				builder.SetPositiveButton("Update", _programExercisesFragment.UpdateProgramExerciseAction);
				builder.SetNeutralButton("Delete", _programExercisesFragment.DeleteProgramExerciseAction);
				builder.SetNegativeButton("Cancel", _programExercisesFragment.CancelAction);

                // Prepopulate the fields.
                view.FindViewById<TextView>(Resource.Id.create_update_program_exercise_id).Text = programExercise.Id.ToString();
                view.FindViewById<EditText>(Resource.Id.create_update_program_exercise_name).Text = programExercise.Name;
                view.FindViewById<EditText>(Resource.Id.create_update_program_exercise_sets).Text = programExercise.Sets.ToString();
                view.FindViewById<EditText>(Resource.Id.create_update_program_exercise_repetitions).Text = programExercise.Repetitions;
                view.FindViewById<EditText>(Resource.Id.create_update_program_exercise_rest_period).Text = programExercise.RestPeriod;

                builder.Show();
            };

            return view;
        }
    }
}
