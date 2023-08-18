using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Core.View;
using MyWorkoutAndroid.Adapters.Gym;
using MyWorkoutAndroid.Models.Gym;
using Newtonsoft.Json;

namespace MyWorkoutAndroid.Fragments.Gym
{
    public class WorkoutExercisesFragment : SportFragment, IMenuProvider
    {
        private Workout _workout;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            if (Arguments != null)
            {
                _workout = JsonConvert.DeserializeObject<Workout>(Arguments.GetString("@string/workout"));
            }

			Activity.AddMenuProvider(this);

			return inflater.Inflate(Resource.Layout.workout_exercises, container, false);
        }

		public override void OnDestroyView()
		{
			Activity.RemoveMenuProvider(this);
			base.OnDestroyView();
		}

		public void OnCreateMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.workout_exercises_menu, menu);
        }

        public bool OnMenuItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_copy:
                {
                    AlertDialog.Builder builder = new AlertDialog.Builder(Activity);
                    builder.SetTitle("Copy Workout");
                    builder.SetPositiveButton("Copy", CopyWorkoutAction);
                    builder.SetNegativeButton("Cancel", CancelAction);

                    builder.Show();
                    return true;
                }
                case Resource.Id.action_edit:
                {
                    LayoutInflater layoutInflater = LayoutInflater.From(Activity);
                    View view = layoutInflater.Inflate(Resource.Layout.create_update_workout, null);

                    DateEditText = view.FindViewById<EditText>(Resource.Id.create_update_workout_date);
                    DateEditText.Text = _workout.Date;
                    DateEditText.Click += delegate
                    {
                        OnClickDateEditText();
                    };

                    AlertDialog.Builder builder = new AlertDialog.Builder(Activity);
                    builder.SetTitle("Update Workout");
                    builder.SetView(view);
                    builder.SetPositiveButton("Update", UpdateWorkoutAction);
                    builder.SetNeutralButton("Delete", DeleteWorkoutAction);
                    builder.SetNegativeButton("Cancel", CancelAction);

                    // Prepopulate the fields.
                    view.FindViewById<TextView>(Resource.Id.create_update_workout_id).Text = _workout.Id.ToString();
                    view.FindViewById<EditText>(Resource.Id.create_update_workout_date).Text = _workout.Date;

                    builder.Show();
                    return true;
                }
            }
            return false;
        }

        private void CopyWorkoutAction(object sender, DialogClickEventArgs e)
        {
            DbHelper.CopyWorkout(_workout);
            LoadData();
        }

        private void UpdateWorkoutAction(object sender, DialogClickEventArgs e)
        {
            _workout.Date = DateEditText.Text;

            DbHelper.UpdateWorkout(_workout);
            LoadData();
        }

        private void DeleteWorkoutAction(object sender, DialogClickEventArgs e)
        {
            DbHelper.DeleteWorkout(_workout.Id);
            LoadData();

            string text = $"{_workout.Date} has been deleted";
            Toast.MakeText(Activity.Application, text, ToastLength.Short).Show();

            Program program = DbHelper.GetPrograms().Where(program => program.Id == _workout.ProgramId).FirstOrDefault();

            ProgramFragment fragment = new ProgramFragment();
            Bundle bundle = new Bundle();
            bundle.PutString("@string/program", JsonConvert.SerializeObject(program));
            fragment.Arguments = bundle;

            Activity.SupportFragmentManager.BeginTransaction().Replace(Resource.Id.container, fragment, "programFragment").Commit();
        }

        public void UpdateWorkoutExerciseAction(object sender, DialogClickEventArgs e)
        {
            AlertDialog alertDialog = (AlertDialog)sender;

            string id = alertDialog.FindViewById<TextView>(Resource.Id.update_workout_exercise_id).Text;
            string weight = alertDialog.FindViewById<EditText>(Resource.Id.update_workout_exercise_weight).Text;
            bool maxedOut = alertDialog.FindViewById<Switch>(Resource.Id.update_workout_exercise_maxed_out).Checked;

            WorkoutExercise workoutExercise = new WorkoutExercise()
            {
                Id = Convert.ToInt32(id),
                Weight = weight,
                MaxedOut = maxedOut
            };

            DbHelper.UpdateWorkoutExercise(workoutExercise);
            LoadData();
        }

        public override void LoadData()
        {
            Activity.Title = _workout.Date;

            List<WorkoutExercise> workoutExercises = DbHelper.GetWorkoutExercisesByWorkoutId(_workout.Id);
            WorkoutExercisesAdapter workoutExercisesAdapter = new WorkoutExercisesAdapter(this, workoutExercises, _workout, DbHelper);
            ListView.Adapter = workoutExercisesAdapter;
        }
    }
}
