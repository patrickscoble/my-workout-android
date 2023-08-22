using System.Collections.Generic;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using MyWorkoutAndroid.Fragments;
using MyWorkoutAndroid.Helpers;
using MyWorkoutAndroid.Models;
using Newtonsoft.Json;

namespace MyWorkoutAndroid.Adapters
{
    public class ProgramsAdapter : SportAdapter<Program>
    {
        private ProgramsFragment _programsFragment;
        private DbHelper _dbHelper;

        public ProgramsAdapter(ProgramsFragment programsFragment, List<Program> programs, DbHelper dbHelper)
            : base(programs)
        {
            _programsFragment = programsFragment;
            _dbHelper = dbHelper;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            LayoutInflater inflater = (LayoutInflater)_programsFragment.Activity.GetSystemService(Context.LayoutInflaterService);
            View view = inflater.Inflate(Resource.Layout.program_list_item, null);

            Program program = Items[position];
            List<Workout> workouts = _dbHelper.GetWorkouts(program.Id);

            view.FindViewById<TextView>(Resource.Id.program_name).Text = program.Name;
            view.FindViewById<TextView>(Resource.Id.program_duration_in_weeks).Text = $"{program.DurationInWeeks} weeks";

            ProgressBar progressBar = view.FindViewById<ProgressBar>(Resource.Id.program_progress_bar);
            progressBar.Max = program.DurationInWeeks * program.FrequencyPerWeek;
            progressBar.Progress = workouts.Count;

            view.FindViewById<TextView>(Resource.Id.program_progress).Text = $"{workouts.Count} / {program.DurationInWeeks * program.FrequencyPerWeek}";

            view.Click += delegate
            {
                ProgramFragment fragment = new ProgramFragment();
                Bundle bundle = new Bundle();
                bundle.PutString("@string/program", JsonConvert.SerializeObject(program));
                fragment.Arguments = bundle;

                _programsFragment.Activity.SupportFragmentManager.BeginTransaction().Replace(Resource.Id.container, fragment, "programFragment").AddToBackStack("programsFragment").Commit();
            };

            return view;
        }
    }
}
