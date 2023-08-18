using System.Collections.Generic;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Core.View;
using MySports.Adapters.Gym;
using MySports.Models.Gym;

namespace MySports.Fragments.Gym
{
    public class ProgramsFragment : SportFragment, IMenuProvider
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
			Activity.AddMenuProvider(this);

			return inflater.Inflate(Resource.Layout.programs, container, false);
        }

		public override void OnDestroyView()
		{
			Activity.RemoveMenuProvider(this);
			base.OnDestroyView();
		}

		public void OnCreateMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.programs_menu, menu);
        }

        public bool OnMenuItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_add:
                {
                    LayoutInflater layoutInflater = LayoutInflater.From(this.Activity);
                    View view = layoutInflater.Inflate(Resource.Layout.create_update_program, null);

                    AlertDialog.Builder builder = new AlertDialog.Builder(this.Activity);
                    builder.SetTitle("Create Program");
					builder.SetView(view);
                    builder.SetPositiveButton("Create", CreateProgramAction);
					builder.SetNegativeButton("Cancel", CancelAction);

                    builder.Show();
                    return true;
                }
            }

            return false;
        }

        private void CreateProgramAction(object sender, DialogClickEventArgs e)
        {
            AlertDialog alertDialog = (AlertDialog)sender;

            string name = alertDialog.FindViewById<EditText>(Resource.Id.create_update_program_name).Text;

            Program program = new Program()
            {
                Name = name
            };

            DbHelper.CreateProgram(program);
            LoadData();
        }

        public override void LoadData()
        {
            Activity.Title = Resources.GetString(Resource.String.title_gym);

            List<Program> programs = DbHelper.GetPrograms();
            ProgramsAdapter programsAdapter = new ProgramsAdapter(this, programs);
            ListView.Adapter = programsAdapter;
        }
    }
}
