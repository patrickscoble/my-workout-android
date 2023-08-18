using System;
using System.Collections.Generic;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using MyWorkoutAndroid.Adapters;
using MyWorkoutAndroid.Models;

namespace MyWorkoutAndroid.Fragments
{
    public class WorkoutsFragment : SportFragment
    {
        private Program _program;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _program = ((ProgramFragment)ParentFragment).Program;

            View view = inflater.Inflate(Resource.Layout.workouts, container, false);

            ImageButton AddButton = view.FindViewById<ImageButton>(Resource.Id.workout_add);
            AddButton.Click += AddButtonClick;

            return view;
        }

        private void AddButtonClick(object sender, EventArgs e)
        {
            LayoutInflater layoutInflater = LayoutInflater.From(Activity);
            View view = layoutInflater.Inflate(Resource.Layout.create_update_workout, null);

            DateEditText = view.FindViewById<EditText>(Resource.Id.create_update_workout_date);
            DateEditText.Text = DateTime.Now.ToShortDateString();
            DateEditText.Click += delegate
            {
                OnClickDateEditText();
            };

            AlertDialog.Builder builder = new AlertDialog.Builder(Activity);
            builder.SetTitle("Create Workout");
            builder.SetView(view);
            builder.SetPositiveButton("Create", CreateWorkoutAction);
            builder.SetNegativeButton("Cancel", CancelAction);

            builder.Show();
        }

        private void CreateWorkoutAction(object sender, DialogClickEventArgs e)
        {
            string date = DateEditText.Text;

            Workout workout = new Workout()
            {
                ProgramId = Convert.ToInt32(_program.Id),
                Date = date
            };

            DbHelper.CreateWorkout(workout);
            LoadData();
        }

        public override void LoadData()
        {
            List<Workout> workouts = DbHelper.GetWorkouts(_program.Id);
            WorkoutsAdapter workoutsAdapter = new WorkoutsAdapter(this, workouts);
            ListView.Adapter = workoutsAdapter;
        }
    }
}
