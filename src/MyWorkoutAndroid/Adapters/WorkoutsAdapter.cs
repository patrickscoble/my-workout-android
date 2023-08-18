using System.Collections.Generic;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using MyWorkoutAndroid.Fragments;
using MyWorkoutAndroid.Models;
using Newtonsoft.Json;

namespace MyWorkoutAndroid.Adapters
{
    public class WorkoutsAdapter : SportAdapter<Workout>
    {
        private WorkoutsFragment _workoutsFragment;

        public WorkoutsAdapter(WorkoutsFragment workoutsFragment, List<Workout> workouts)
            : base(workouts)
        {
            _workoutsFragment = workoutsFragment;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            LayoutInflater inflater = (LayoutInflater)_workoutsFragment.Activity.GetSystemService(Context.LayoutInflaterService);
            View view = inflater.Inflate(Resource.Layout.workout_list_item, null);

            Workout workout = Items[position];

            view.FindViewById<TextView>(Resource.Id.workout_date).Text = workout.Date;

            view.Click += delegate
            {
                WorkoutExercisesFragment fragment = new WorkoutExercisesFragment();
                Bundle bundle = new Bundle();
                bundle.PutString("@string/workout", JsonConvert.SerializeObject(workout));
                fragment.Arguments = bundle;

                _workoutsFragment.Activity.SupportFragmentManager.BeginTransaction().Replace(Resource.Id.container, fragment, "workoutExercisesFragment").AddToBackStack("programFragment").Commit();
            };

            return view;
        }
    }
}
