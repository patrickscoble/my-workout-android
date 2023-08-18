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
    public class ProgramExercisesFragment : SportFragment
    {
        private ProgramFragment _programFragment;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _programFragment = (ProgramFragment)ParentFragment;

            View view = inflater.Inflate(Resource.Layout.program_exercises, container, false);

            ImageButton AddButton = view.FindViewById<ImageButton>(Resource.Id.program_exercise_add);
            AddButton.Click += AddButtonClick;

            return view;
        }

        private void AddButtonClick(object sender, EventArgs e)
        {
            LayoutInflater layoutInflater = LayoutInflater.From(Activity);
            View view = layoutInflater.Inflate(Resource.Layout.create_update_program_exercise, null);

            AlertDialog.Builder builder = new AlertDialog.Builder(Activity);
            builder.SetTitle("Create Program Exercise");
            builder.SetView(view);
            builder.SetPositiveButton("Create", CreateProgramExerciseAction);
            builder.SetNegativeButton("Cancel", CancelAction);

            builder.Show();
        }

        private void CreateProgramExerciseAction(object sender, DialogClickEventArgs e)
        {
            AlertDialog alertDialog = (AlertDialog)sender;

            string name = alertDialog.FindViewById<EditText>(Resource.Id.create_update_program_exercise_name).Text;
            string sets = alertDialog.FindViewById<EditText>(Resource.Id.create_update_program_exercise_sets).Text;
            string repetitions = alertDialog.FindViewById<EditText>(Resource.Id.create_update_program_exercise_repetitions).Text;
            string restPeriod = alertDialog.FindViewById<EditText>(Resource.Id.create_update_program_exercise_rest_period).Text;

            ProgramExercise programExercise = new ProgramExercise()
            {
                ProgramId = Convert.ToInt32(_programFragment.Program.Id),
                Name = name,
                Sets = Convert.ToInt32(sets),
                Repetitions = repetitions,
                RestPeriod = restPeriod
            };

            DbHelper.CreateProgramExercise(programExercise);
            LoadData();

            _programFragment.LoadData();
        }

        public void UpdateProgramExerciseAction(object sender, DialogClickEventArgs e)
        {
            AlertDialog alertDialog = (AlertDialog)sender;

            string id = alertDialog.FindViewById<TextView>(Resource.Id.create_update_program_exercise_id).Text;
            string name = alertDialog.FindViewById<EditText>(Resource.Id.create_update_program_exercise_name).Text;
            string sets = alertDialog.FindViewById<EditText>(Resource.Id.create_update_program_exercise_sets).Text;
            string repetitions = alertDialog.FindViewById<EditText>(Resource.Id.create_update_program_exercise_repetitions).Text;
            string restPeriod = alertDialog.FindViewById<EditText>(Resource.Id.create_update_program_exercise_rest_period).Text;

            ProgramExercise programExercise = new ProgramExercise()
            {
                Id = Convert.ToInt32(id),
                Name = name,
                Sets = Convert.ToInt32(sets),
                Repetitions = repetitions,
                RestPeriod = restPeriod
            };

            DbHelper.UpdateProgramExercise(programExercise);
            LoadData();

            _programFragment.LoadData();
        }

        public void DeleteProgramExerciseAction(object sender, DialogClickEventArgs e)
        {
            AlertDialog alertDialog = (AlertDialog)sender;

            string id = alertDialog.FindViewById<TextView>(Resource.Id.create_update_program_exercise_id).Text;
            string name = alertDialog.FindViewById<TextView>(Resource.Id.create_update_program_exercise_name).Text;

            DbHelper.DeleteProgramExercise(Convert.ToInt32(id));
            LoadData();

            string text = $"{name} has been deleted";
            Toast.MakeText(Activity.Application, text, ToastLength.Short).Show();

            _programFragment.LoadData();
        }

        public override void LoadData()
        {
            List<ProgramExercise> programExercises = DbHelper.GetProgramExercises(_programFragment.Program.Id);
            ProgramExercisesAdapter programExercisesAdapter = new ProgramExercisesAdapter(this, programExercises);
            ListView.Adapter = programExercisesAdapter;
        }
    }
}
