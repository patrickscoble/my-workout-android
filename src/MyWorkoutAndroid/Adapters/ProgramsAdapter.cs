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
    public class ProgramsAdapter : SportAdapter<Program>
    {
        private ProgramsFragment _programsFragment;

        public ProgramsAdapter(ProgramsFragment programsFragment, List<Program> programs)
            : base(programs)
        {
            _programsFragment = programsFragment;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            LayoutInflater inflater = (LayoutInflater)_programsFragment.Activity.GetSystemService(Context.LayoutInflaterService);
            View view = inflater.Inflate(Resource.Layout.program_list_item, null);

            Program program = Items[position];

            view.FindViewById<TextView>(Resource.Id.program_name).Text = program.Name;

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
