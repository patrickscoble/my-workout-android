using System.Collections.Generic;
using Android.Content;
using Android.Database;
using Android.Database.Sqlite;
using MyWorkoutAndroid.Models;

namespace MyWorkoutAndroid.Helpers
{
    public class DbHelper : SQLiteOpenHelper
    {
        private static string DB_NAME = "MyWorkoutAndroid";
        private static int DB_VERSION = 1;

        private static string DB_TABLE_PROGRAM = "GymProgram";
        private static string DB_PROGRAM_COLUMN_ID = "Id";
        private static string DB_PROGRAM_COLUMN_NAME = "Name";
        private static string DB_PROGRAM_COLUMN_DURATION_IN_WEEKS = "DurationInWeeks";
        private static string DB_PROGRAM_COLUMN_FREQUENCY_PER_WEEK = "FrequencyPerWeek";

        private static string DB_TABLE_PROGRAM_EXERCISE = "GymProgramExercise";
        private static string DB_PROGRAM_EXERCISE_COLUMN_ID = "Id";
        private static string DB_PROGRAM_EXERCISE_COLUMN_PROGRAM_ID = "ProgramId";
        private static string DB_PROGRAM_EXERCISE_COLUMN_NAME = "Name";
        private static string DB_PROGRAM_EXERCISE_COLUMN_SETS = "Sets";
        private static string DB_PROGRAM_EXERCISE_COLUMN_REPETITIONS = "Repetitions";
        private static string DB_PROGRAM_EXERCISE_COLUMN_REST_PERIOD = "RestPeriod";

        private static string DB_TABLE_WORKOUT = "GymWorkout";
        private static string DB_WORKOUT_COLUMN_ID = "Id";
        private static string DB_WORKOUT_COLUMN_PROGRAM_ID = "ProgramId";
        private static string DB_WORKOUT_COLUMN_DATE = "Date";

        private static string DB_TABLE_WORKOUT_EXERCISE = "GymWorkoutExercise";
        private static string DB_WORKOUT_EXERCISE_COLUMN_ID = "Id";
        private static string DB_WORKOUT_EXERCISE_COLUMN_WORKOUT_ID = "WorkoutId";
        private static string DB_WORKOUT_EXERCISE_COLUMN_PROGRAM_EXERCISE_ID = "ProgramExerciseId";
        private static string DB_WORKOUT_EXERCISE_COLUMN_WEIGHT = "Weight";
        private static string DB_WORKOUT_EXERCISE_COLUMN_MAXED_OUT = "MaxedOut";

        public DbHelper(Context context)
            : base(context, DB_NAME, null, DB_VERSION)
        {
        }

        public override void OnCreate(SQLiteDatabase db)
        {
            string query = $"CREATE TABLE {DB_TABLE_PROGRAM} ({DB_PROGRAM_COLUMN_ID} INTEGER PRIMARY KEY AUTOINCREMENT, {DB_PROGRAM_COLUMN_NAME} TEXT NOT NULL, {DB_PROGRAM_COLUMN_DURATION_IN_WEEKS} INTEGER NOT NULL, {DB_PROGRAM_COLUMN_FREQUENCY_PER_WEEK} INTEGER NOT NULL);";
            db.ExecSQL(query);

            query = $"CREATE TABLE {DB_TABLE_PROGRAM_EXERCISE} ({DB_PROGRAM_EXERCISE_COLUMN_ID} INTEGER PRIMARY KEY AUTOINCREMENT, {DB_PROGRAM_EXERCISE_COLUMN_PROGRAM_ID} INTEGER NOT NULL, {DB_PROGRAM_EXERCISE_COLUMN_NAME} TEXT NOT NULL, {DB_PROGRAM_EXERCISE_COLUMN_SETS} INTEGER NOT NULL, {DB_PROGRAM_EXERCISE_COLUMN_REPETITIONS} TEXT NOT NULL, {DB_PROGRAM_EXERCISE_COLUMN_REST_PERIOD} TEXT NOT NULL, CONSTRAINT FK_ProgramId FOREIGN KEY ({DB_PROGRAM_EXERCISE_COLUMN_PROGRAM_ID}) REFERENCES {DB_TABLE_PROGRAM}({DB_PROGRAM_COLUMN_ID}));";
            db.ExecSQL(query);

            query = $"CREATE TABLE {DB_TABLE_WORKOUT} ({DB_WORKOUT_COLUMN_ID} INTEGER PRIMARY KEY AUTOINCREMENT, {DB_WORKOUT_COLUMN_PROGRAM_ID} INTEGER NOT NULL, {DB_WORKOUT_COLUMN_DATE} TEXT NOT NULL, CONSTRAINT FK_ProgramId FOREIGN KEY ({DB_WORKOUT_COLUMN_PROGRAM_ID}) REFERENCES {DB_TABLE_PROGRAM}({DB_PROGRAM_COLUMN_ID}));";
            db.ExecSQL(query);

            query = $"CREATE TABLE {DB_TABLE_WORKOUT_EXERCISE} ({DB_WORKOUT_EXERCISE_COLUMN_ID} INTEGER PRIMARY KEY AUTOINCREMENT, {DB_WORKOUT_EXERCISE_COLUMN_WORKOUT_ID} INTEGER NOT NULL, {DB_WORKOUT_EXERCISE_COLUMN_PROGRAM_EXERCISE_ID} INTEGER NOT NULL, {DB_WORKOUT_EXERCISE_COLUMN_WEIGHT} TEXT NOT NULL, {DB_WORKOUT_EXERCISE_COLUMN_MAXED_OUT} BOOLEAN NOT NULL, CONSTRAINT FK_WorkoutId FOREIGN KEY ({DB_WORKOUT_EXERCISE_COLUMN_WORKOUT_ID}) REFERENCES {DB_TABLE_WORKOUT}({DB_WORKOUT_COLUMN_ID}), CONSTRAINT FK_ProgramExerciseId FOREIGN KEY ({DB_WORKOUT_EXERCISE_COLUMN_PROGRAM_EXERCISE_ID}) REFERENCES {DB_TABLE_PROGRAM_EXERCISE}({DB_PROGRAM_EXERCISE_COLUMN_ID}));";
            db.ExecSQL(query);
        }

        public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
        {
            string query = $"DELETE TABLE IF EXISTS {DB_TABLE_WORKOUT_EXERCISE}";
            db.ExecSQL(query);

            query = $"DELETE TABLE IF EXISTS {DB_TABLE_WORKOUT}";
            db.ExecSQL(query);

            query = $"DELETE TABLE IF EXISTS {DB_TABLE_PROGRAM_EXERCISE}";
            db.ExecSQL(query);

            query = $"DELETE TABLE IF EXISTS {DB_TABLE_PROGRAM}";
            db.ExecSQL(query);

            OnCreate(db);
        }

        public List<Program> GetPrograms()
        {
            List<Program> programs = new List<Program>();
            SQLiteDatabase db = this.ReadableDatabase;
            ICursor cursor = db.Query(DB_TABLE_PROGRAM, new string[] { DB_PROGRAM_COLUMN_ID, DB_PROGRAM_COLUMN_NAME, DB_PROGRAM_COLUMN_DURATION_IN_WEEKS, DB_PROGRAM_COLUMN_FREQUENCY_PER_WEEK }, null, null, null, null, null);

            while (cursor.MoveToNext())
            {
                int idIndex = cursor.GetColumnIndex(DB_PROGRAM_COLUMN_ID);
                int id = cursor.GetInt(idIndex);

                int nameIndex = cursor.GetColumnIndex(DB_PROGRAM_COLUMN_NAME);
                string name = cursor.GetString(nameIndex);

                int durationInWeeksIndex = cursor.GetColumnIndex(DB_PROGRAM_COLUMN_DURATION_IN_WEEKS);
                int durationInWeeks = cursor.GetInt(durationInWeeksIndex);

                int frequencyPerWeekIndex = cursor.GetColumnIndex(DB_PROGRAM_COLUMN_FREQUENCY_PER_WEEK);
                int frequencyPerWeek = cursor.GetInt(frequencyPerWeekIndex);

                programs.Add(new Program()
                {
                    Id = id,
                    Name = name,
                    DurationInWeeks = durationInWeeks,
                    FrequencyPerWeek = frequencyPerWeek,
                });
            }

            return programs;
        }

        public void CreateProgram(Program program)
        {
            SQLiteDatabase db = this.WritableDatabase;
            ContentValues values = new ContentValues();
            values.Put(DB_PROGRAM_COLUMN_NAME, program.Name);
            values.Put(DB_PROGRAM_COLUMN_DURATION_IN_WEEKS, program.DurationInWeeks);
            values.Put(DB_PROGRAM_COLUMN_FREQUENCY_PER_WEEK, program.FrequencyPerWeek);
            db.Insert(DB_TABLE_PROGRAM, null, values);
            db.Close();
        }

        public void CopyProgram(Program program)
        {
            SQLiteDatabase db = this.WritableDatabase;
            ContentValues values = new ContentValues();
            values.Put(DB_PROGRAM_COLUMN_NAME, program.Name);
            values.Put(DB_PROGRAM_COLUMN_DURATION_IN_WEEKS, program.DurationInWeeks);
            values.Put(DB_PROGRAM_COLUMN_FREQUENCY_PER_WEEK, program.FrequencyPerWeek);
            long programId = db.Insert(DB_TABLE_PROGRAM, null, values);
            db.Close();

            List<ProgramExercise> programExercises = GetProgramExercises(program.Id);
            foreach (ProgramExercise programExercise in programExercises)
            {
                ProgramExercise newProgramExercise = new ProgramExercise()
                {
                    ProgramId = (int)programId,
                    Name = programExercise.Name,
                    Sets = programExercise.Sets,
                    Repetitions = programExercise.Repetitions,
                    RestPeriod = programExercise.RestPeriod
                };

                CreateProgramExercise(newProgramExercise);
            }
        }

        public void UpdateProgram(Program program)
        {
            SQLiteDatabase db = this.WritableDatabase;
            ContentValues values = new ContentValues();
            values.Put(DB_PROGRAM_COLUMN_NAME, program.Name);
            values.Put(DB_PROGRAM_COLUMN_DURATION_IN_WEEKS, program.DurationInWeeks);
            values.Put(DB_PROGRAM_COLUMN_FREQUENCY_PER_WEEK, program.FrequencyPerWeek);
            db.Update(DB_TABLE_PROGRAM, values, $"{DB_PROGRAM_COLUMN_ID} = ?", new string[] { program.Id.ToString() });
            db.Close();
        }

        public void DeleteProgram(int programId)
        {
            List<Workout> workouts = GetWorkouts(programId);
            foreach (Workout workout in workouts)
            {
                DeleteWorkout(workout.Id);
            }

            List<ProgramExercise> programExercises = GetProgramExercises(programId);
            foreach(ProgramExercise programExercise in programExercises)
            {
                DeleteProgramExercise(programExercise.Id);
            }

            SQLiteDatabase db = this.WritableDatabase;
            db.Delete(DB_TABLE_PROGRAM, $"{DB_PROGRAM_COLUMN_ID} = ?", new string[] { programId.ToString() });
            db.Close();
        }

        public List<ProgramExercise> GetProgramExercises(int programId)
        {
            List<ProgramExercise> programExercises = new List<ProgramExercise>();
            SQLiteDatabase db = this.ReadableDatabase;
            ICursor cursor = db.Query(DB_TABLE_PROGRAM_EXERCISE, new string[] { DB_PROGRAM_EXERCISE_COLUMN_ID, DB_PROGRAM_COLUMN_NAME, DB_PROGRAM_EXERCISE_COLUMN_SETS, DB_PROGRAM_EXERCISE_COLUMN_REPETITIONS, DB_PROGRAM_EXERCISE_COLUMN_REST_PERIOD }, $"{DB_PROGRAM_EXERCISE_COLUMN_PROGRAM_ID} = ?", new string[] { programId.ToString() }, null, null, null, null);

            while (cursor.MoveToNext())
            {
                int idIndex = cursor.GetColumnIndex(DB_PROGRAM_EXERCISE_COLUMN_ID);
                int id = cursor.GetInt(idIndex);

                int nameIndex = cursor.GetColumnIndex(DB_PROGRAM_EXERCISE_COLUMN_NAME);
                string name = cursor.GetString(nameIndex);

                int setsIndex = cursor.GetColumnIndex(DB_PROGRAM_EXERCISE_COLUMN_SETS);
                int sets = cursor.GetInt(setsIndex);

                int repetitionsIndex = cursor.GetColumnIndex(DB_PROGRAM_EXERCISE_COLUMN_REPETITIONS);
                string repetitions = cursor.GetString(repetitionsIndex);

                int restPeriodIndex = cursor.GetColumnIndex(DB_PROGRAM_EXERCISE_COLUMN_REST_PERIOD);
                string restPeriod = cursor.GetString(restPeriodIndex);

                programExercises.Add(new ProgramExercise()
                {
                    Id = id,
                    ProgramId = programId,
                    Name = name,
                    Sets = sets,
                    Repetitions = repetitions,
                    RestPeriod = restPeriod
                });
            }

            return programExercises;
        }

        public void CreateProgramExercise(ProgramExercise programExercise)
        {
            SQLiteDatabase db = this.WritableDatabase;
            ContentValues values = new ContentValues();
            values.Put(DB_PROGRAM_EXERCISE_COLUMN_PROGRAM_ID, programExercise.ProgramId);
            values.Put(DB_PROGRAM_EXERCISE_COLUMN_NAME, programExercise.Name);
            values.Put(DB_PROGRAM_EXERCISE_COLUMN_SETS, programExercise.Sets);
            values.Put(DB_PROGRAM_EXERCISE_COLUMN_REPETITIONS, programExercise.Repetitions);
            values.Put(DB_PROGRAM_EXERCISE_COLUMN_REST_PERIOD, programExercise.RestPeriod);
            long programExerciseId = db.Insert(DB_TABLE_PROGRAM_EXERCISE, null, values);
            db.Close();

            List<Workout> workouts = GetWorkouts(programExercise.ProgramId);
            foreach (Workout workout in workouts)
            {
                WorkoutExercise workoutExercise = new WorkoutExercise()
                {
                    WorkoutId = workout.Id,
                    ProgramExerciseId = (int)programExerciseId,
                    Weight = string.Empty
                };

                CreateWorkoutExercise(workoutExercise);
            }
        }

        public void UpdateProgramExercise(ProgramExercise programExercise)
        {
            SQLiteDatabase db = this.WritableDatabase;
            ContentValues values = new ContentValues();
            values.Put(DB_PROGRAM_EXERCISE_COLUMN_NAME, programExercise.Name);
            values.Put(DB_PROGRAM_EXERCISE_COLUMN_SETS, programExercise.Sets);
            values.Put(DB_PROGRAM_EXERCISE_COLUMN_REPETITIONS, programExercise.Repetitions);
            values.Put(DB_PROGRAM_EXERCISE_COLUMN_REST_PERIOD, programExercise.RestPeriod);
            db.Update(DB_TABLE_PROGRAM_EXERCISE, values, $"{DB_PROGRAM_EXERCISE_COLUMN_ID} = ?", new string[] { programExercise.Id.ToString() });
            db.Close();
        }

        public void DeleteProgramExercise(int programExerciseId)
        {
            List<WorkoutExercise> workoutExercises = GetWorkoutExercisesByProgramExerciseId(programExerciseId);

            foreach (WorkoutExercise workoutExercise in workoutExercises)
            {
                DeleteWorkoutExercise(workoutExercise.Id);
            }

            SQLiteDatabase db = this.WritableDatabase;
            db.Delete(DB_TABLE_PROGRAM_EXERCISE, $"{DB_PROGRAM_EXERCISE_COLUMN_ID} = ?", new string[] { programExerciseId.ToString() });
            db.Close();
        }

        public List<Workout> GetWorkouts(int programId)
        {
            List<Workout> workouts = new List<Workout>();
            SQLiteDatabase db = this.ReadableDatabase;
            ICursor cursor = db.Query(DB_TABLE_WORKOUT, new string[] { DB_WORKOUT_COLUMN_ID, DB_WORKOUT_COLUMN_DATE }, $"{DB_WORKOUT_COLUMN_PROGRAM_ID} = ?", new string[] { programId.ToString() }, null, null, null, null);

            while (cursor.MoveToNext())
            {
                int idIndex = cursor.GetColumnIndex(DB_WORKOUT_COLUMN_ID);
                int id = cursor.GetInt(idIndex);

                int dateIndex = cursor.GetColumnIndex(DB_WORKOUT_COLUMN_DATE);
                string date = cursor.GetString(dateIndex);

                workouts.Add(new Workout()
                {
                    Id = id,
                    ProgramId = programId,
                    Date = date
                });
            }

            return workouts;
        }

        public void CreateWorkout(Workout workout)
        {
            SQLiteDatabase db = this.WritableDatabase;
            ContentValues values = new ContentValues();
            values.Put(DB_WORKOUT_COLUMN_PROGRAM_ID, workout.ProgramId);
            values.Put(DB_WORKOUT_COLUMN_DATE, workout.Date);
            long workoutId = db.Insert(DB_TABLE_WORKOUT, null, values);
            db.Close();

            List<ProgramExercise> programExercises = GetProgramExercises(workout.ProgramId);
            foreach (ProgramExercise programExercise in programExercises)
            {
                WorkoutExercise workoutExercise = new WorkoutExercise()
                {
                    WorkoutId = (int)workoutId,
                    ProgramExerciseId = programExercise.Id,
                    Weight = string.Empty
                };

                CreateWorkoutExercise(workoutExercise);
            }
        }

        public void CopyWorkout(Workout workout)
        {
            SQLiteDatabase db = this.WritableDatabase;
            ContentValues values = new ContentValues();
            values.Put(DB_WORKOUT_COLUMN_PROGRAM_ID, workout.ProgramId);
            values.Put(DB_WORKOUT_COLUMN_DATE, workout.Date);
            long workoutId = db.Insert(DB_TABLE_WORKOUT, null, values);
            db.Close();

            List<WorkoutExercise> workoutExercises = GetWorkoutExercisesByWorkoutId(workout.Id);
            foreach (WorkoutExercise workoutExercise in workoutExercises)
            {
                WorkoutExercise newWorkoutExercise = new WorkoutExercise()
                {
                    WorkoutId = (int)workoutId,
                    ProgramExerciseId = workoutExercise.ProgramExerciseId,
                    Weight = workoutExercise.Weight,
                    MaxedOut = workoutExercise.MaxedOut
                };

                CreateWorkoutExercise(newWorkoutExercise);
            }
        }

        public void UpdateWorkout(Workout workout)
        {
            SQLiteDatabase db = this.WritableDatabase;
            ContentValues values = new ContentValues();
            values.Put(DB_WORKOUT_COLUMN_DATE, workout.Date);
            db.Update(DB_TABLE_WORKOUT, values, $"{DB_WORKOUT_COLUMN_ID} = ?", new string[] { workout.Id.ToString() });
            db.Close();
        }

        public void DeleteWorkout(int workoutId)
        {
            List<WorkoutExercise> workoutExercises = GetWorkoutExercisesByWorkoutId(workoutId);

            foreach (WorkoutExercise workoutExercise in workoutExercises)
            {
                DeleteWorkoutExercise(workoutExercise.Id);
            }

            SQLiteDatabase db = this.WritableDatabase;
            db.Delete(DB_TABLE_WORKOUT, $"{DB_WORKOUT_COLUMN_ID} = ?", new string[] { workoutId.ToString() });
            db.Close();
        }

        public List<WorkoutExercise> GetWorkoutExercisesByWorkoutId(int workoutId)
        {
            List<WorkoutExercise> workoutExercises = new List<WorkoutExercise>();
            SQLiteDatabase db = this.ReadableDatabase;
            ICursor cursor = db.Query(DB_TABLE_WORKOUT_EXERCISE, new string[] { DB_WORKOUT_EXERCISE_COLUMN_ID, DB_WORKOUT_EXERCISE_COLUMN_PROGRAM_EXERCISE_ID, DB_WORKOUT_EXERCISE_COLUMN_WEIGHT, DB_WORKOUT_EXERCISE_COLUMN_MAXED_OUT }, $"{DB_WORKOUT_EXERCISE_COLUMN_WORKOUT_ID} = ?", new string[] { workoutId.ToString() }, null, null, null, null);

            while (cursor.MoveToNext())
            {
                int idIndex = cursor.GetColumnIndex(DB_WORKOUT_EXERCISE_COLUMN_ID);
                int id = cursor.GetInt(idIndex);

                int programExerciseIdIndex = cursor.GetColumnIndex(DB_WORKOUT_EXERCISE_COLUMN_PROGRAM_EXERCISE_ID);
                int programExerciseId = cursor.GetInt(programExerciseIdIndex);

                int weightIndex = cursor.GetColumnIndex(DB_WORKOUT_EXERCISE_COLUMN_WEIGHT);
                string weight = cursor.GetString(weightIndex);

                int maxedOutIndex = cursor.GetColumnIndex(DB_WORKOUT_EXERCISE_COLUMN_MAXED_OUT);
                bool maxedOut = cursor.GetInt(maxedOutIndex) > 0;

                workoutExercises.Add(new WorkoutExercise()
                {
                    Id = id,
                    WorkoutId = workoutId,
                    ProgramExerciseId = programExerciseId,
                    Weight = weight,
                    MaxedOut = maxedOut
                });
            }

            return workoutExercises;
        }

        public List<WorkoutExercise> GetWorkoutExercisesByProgramExerciseId(int programExerciseId)
        {
            List<WorkoutExercise> workoutExercises = new List<WorkoutExercise>();
            SQLiteDatabase db = this.ReadableDatabase;
            ICursor cursor = db.Query(DB_TABLE_WORKOUT_EXERCISE, new string[] { DB_WORKOUT_EXERCISE_COLUMN_ID, DB_WORKOUT_EXERCISE_COLUMN_WORKOUT_ID, DB_WORKOUT_EXERCISE_COLUMN_WEIGHT, DB_WORKOUT_EXERCISE_COLUMN_MAXED_OUT }, $"{DB_WORKOUT_EXERCISE_COLUMN_PROGRAM_EXERCISE_ID} = ?", new string[] { programExerciseId.ToString() }, null, null, null, null);

            while (cursor.MoveToNext())
            {
                int idIndex = cursor.GetColumnIndex(DB_WORKOUT_EXERCISE_COLUMN_ID);
                int id = cursor.GetInt(idIndex);

                int workoutIdIndex = cursor.GetColumnIndex(DB_WORKOUT_EXERCISE_COLUMN_WORKOUT_ID);
                int workoutId = cursor.GetInt(workoutIdIndex);

                int weightIndex = cursor.GetColumnIndex(DB_WORKOUT_EXERCISE_COLUMN_WEIGHT);
                string weight = cursor.GetString(weightIndex);

                int maxedOutIndex = cursor.GetColumnIndex(DB_WORKOUT_EXERCISE_COLUMN_MAXED_OUT);
                bool maxedOut = cursor.GetInt(maxedOutIndex) > 0;

                workoutExercises.Add(new WorkoutExercise()
                {
                    Id = id,
                    WorkoutId = workoutId,
                    ProgramExerciseId = programExerciseId,
                    Weight = weight,
                    MaxedOut = maxedOut
                });
            }

            return workoutExercises;
        }

        public void CreateWorkoutExercise(WorkoutExercise workoutExercise)
        {
            SQLiteDatabase db = this.WritableDatabase;
            ContentValues values = new ContentValues();
            values.Put(DB_WORKOUT_EXERCISE_COLUMN_WORKOUT_ID, workoutExercise.WorkoutId);
            values.Put(DB_WORKOUT_EXERCISE_COLUMN_PROGRAM_EXERCISE_ID, workoutExercise.ProgramExerciseId);
            values.Put(DB_WORKOUT_EXERCISE_COLUMN_WEIGHT, workoutExercise.Weight);
            values.Put(DB_WORKOUT_EXERCISE_COLUMN_MAXED_OUT, workoutExercise.MaxedOut);
            db.Insert(DB_TABLE_WORKOUT_EXERCISE, null, values);
            db.Close();
        }

        public void UpdateWorkoutExercise(WorkoutExercise workoutExercise)
        {
            SQLiteDatabase db = this.WritableDatabase;
            ContentValues values = new ContentValues();
            values.Put(DB_WORKOUT_EXERCISE_COLUMN_WEIGHT, workoutExercise.Weight);
            values.Put(DB_WORKOUT_EXERCISE_COLUMN_MAXED_OUT, workoutExercise.MaxedOut);
            db.Update(DB_TABLE_WORKOUT_EXERCISE, values, $"{DB_WORKOUT_EXERCISE_COLUMN_ID} = ?", new string[] { workoutExercise.Id.ToString() });
            db.Close();
        }

        public void DeleteWorkoutExercise(int workoutExerciseId)
        {
            SQLiteDatabase db = this.WritableDatabase;
            db.Delete(DB_TABLE_WORKOUT_EXERCISE, $"{DB_WORKOUT_EXERCISE_COLUMN_ID} = ?", new string[] { workoutExerciseId.ToString() });
            db.Close();
        }
    }
}
