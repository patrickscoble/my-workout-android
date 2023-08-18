using System.Collections.Generic;
using Android.Content;
using Android.Database;
using Android.Database.Sqlite;
using MyWorkoutAndroid.Models.Gym;
using MyWorkoutAndroid.Models.MiniGolf;
using MyWorkoutAndroid.Models.Tennis;

namespace MyWorkoutAndroid.Helpers
{
    public class DbHelper : SQLiteOpenHelper
    {
        private static string DB_NAME = "MyWorkoutAndroid";
        private static int DB_VERSION = 1;

        private static string DB_TABLE_ROUND = "MiniGolfRound";
        private static string DB_ROUND_COLUMN_ID = "Id";
        private static string DB_ROUND_COLUMN_DESCRIPTION = "Description";
        private static string DB_ROUND_COLUMN_DATE = "Date";
        private static string DB_ROUND_COLUMN_PLAYER_ONE = "PlayerOne";
        private static string DB_ROUND_COLUMN_PLAYER_TWO = "PlayerTwo";

        private static string DB_TABLE_HOLE = "MiniGolfHole";
        private static string DB_HOLE_COLUMN_ID = "Id";
        private static string DB_HOLE_COLUMN_ROUND_ID = "RoundId";
        private static string DB_HOLE_COLUMN_NUMBER = "Number";
        private static string DB_HOLE_COLUMN_DESCRIPTION = "Description";
        private static string DB_HOLE_COLUMN_PAR = "Par";
        private static string DB_HOLE_COLUMN_PLAYER_ONE_SCORE = "PlayerOneScore";
        private static string DB_HOLE_COLUMN_PLAYER_TWO_SCORE = "PlayerTwoScore";

        private static string DB_TABLE_MATCH = "TennisMatch";
        private static string DB_MATCH_COLUMN_ID = "Id";
        private static string DB_MATCH_COLUMN_DESCRIPTION = "Description";
        private static string DB_MATCH_COLUMN_DATE = "Date";
        private static string DB_MATCH_COLUMN_PLAYER_ONE = "PlayerOne";
        private static string DB_MATCH_COLUMN_PLAYER_TWO = "PlayerTwo";

        private static string DB_TABLE_SET = "TennisSet";
        private static string DB_SET_COLUMN_ID = "Id";
        private static string DB_SET_COLUMN_MATCH_ID = "MatchId";
        private static string DB_SET_COLUMN_NUMBER = "Number";
        private static string DB_SET_COLUMN_PLAYER_ONE_SCORE = "PlayerOneScore";
        private static string DB_SET_COLUMN_PLAYER_TWO_SCORE = "PlayerTwoScore";

        private static string DB_TABLE_PROGRAM = "GymProgram";
        private static string DB_PROGRAM_COLUMN_ID = "Id";
        private static string DB_PROGRAM_COLUMN_NAME = "Name";

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
            string query = $@"CREATE TABLE {DB_TABLE_ROUND} ({DB_ROUND_COLUMN_ID} INTEGER PRIMARY KEY AUTOINCREMENT, {DB_ROUND_COLUMN_DESCRIPTION} TEXT NOT NULL, {DB_ROUND_COLUMN_DATE} TEXT NOT NULL, {DB_ROUND_COLUMN_PLAYER_ONE} TEXT NOT NULL, {DB_ROUND_COLUMN_PLAYER_TWO} TEXT NOT NULL);";
            db.ExecSQL(query);

            query = $"CREATE TABLE {DB_TABLE_HOLE} ({DB_HOLE_COLUMN_ID} INTEGER PRIMARY KEY AUTOINCREMENT, {DB_HOLE_COLUMN_ROUND_ID} INTEGER NOT NULL, {DB_HOLE_COLUMN_NUMBER} INTEGER NOT NULL, {DB_HOLE_COLUMN_DESCRIPTION} TEXT NOT NULL, {DB_HOLE_COLUMN_PAR} INTEGER NOT NULL, {DB_HOLE_COLUMN_PLAYER_ONE_SCORE} INTEGER NOT NULL, {DB_HOLE_COLUMN_PLAYER_TWO_SCORE} INTEGER NOT NULL, CONSTRAINT FK_RoundId FOREIGN KEY ({DB_HOLE_COLUMN_ROUND_ID}) REFERENCES {DB_TABLE_ROUND}({DB_ROUND_COLUMN_ID}));";
            db.ExecSQL(query);

            query = $@"CREATE TABLE {DB_TABLE_MATCH} ({DB_MATCH_COLUMN_ID} INTEGER PRIMARY KEY AUTOINCREMENT, {DB_MATCH_COLUMN_DESCRIPTION} TEXT NOT NULL, {DB_MATCH_COLUMN_DATE} TEXT NOT NULL, {DB_MATCH_COLUMN_PLAYER_ONE} TEXT NOT NULL, {DB_MATCH_COLUMN_PLAYER_TWO} TEXT NOT NULL);";
            db.ExecSQL(query);

            query = $"CREATE TABLE {DB_TABLE_SET} ({DB_SET_COLUMN_ID} INTEGER PRIMARY KEY AUTOINCREMENT, {DB_SET_COLUMN_MATCH_ID} INTEGER NOT NULL, {DB_SET_COLUMN_NUMBER} INTEGER NOT NULL, {DB_SET_COLUMN_PLAYER_ONE_SCORE} INTEGER NOT NULL, {DB_SET_COLUMN_PLAYER_TWO_SCORE} INTEGER NOT NULL, CONSTRAINT FK_MatchId FOREIGN KEY ({DB_SET_COLUMN_MATCH_ID}) REFERENCES {DB_TABLE_MATCH}({DB_MATCH_COLUMN_ID}));";
            db.ExecSQL(query);

            query = $"CREATE TABLE {DB_TABLE_PROGRAM} ({DB_PROGRAM_COLUMN_ID} INTEGER PRIMARY KEY AUTOINCREMENT, {DB_PROGRAM_COLUMN_NAME} TEXT NOT NULL);";
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
            string query = $"DELETE TABLE IF EXISTS {DB_TABLE_SET}";
            db.ExecSQL(query);

            query = $"DELETE TABLE IF EXISTS {DB_TABLE_MATCH}";
            db.ExecSQL(query);

            query = $"DELETE TABLE IF EXISTS {DB_TABLE_HOLE}";
            db.ExecSQL(query);

            query = $"DELETE TABLE IF EXISTS {DB_TABLE_ROUND}";
            db.ExecSQL(query);

            query = $"DELETE TABLE IF EXISTS {DB_TABLE_WORKOUT_EXERCISE}";
            db.ExecSQL(query);

            query = $"DELETE TABLE IF EXISTS {DB_TABLE_WORKOUT}";
            db.ExecSQL(query);

            query = $"DELETE TABLE IF EXISTS {DB_TABLE_PROGRAM_EXERCISE}";
            db.ExecSQL(query);

            query = $"DELETE TABLE IF EXISTS {DB_TABLE_PROGRAM}";
            db.ExecSQL(query);

            OnCreate(db);
        }

        public List<Round> GetRounds()
        {
            List<Round> rounds = new List<Round>();
            SQLiteDatabase db = this.ReadableDatabase;
            ICursor cursor = db.Query(DB_TABLE_ROUND, new string[] { DB_ROUND_COLUMN_ID, DB_ROUND_COLUMN_DESCRIPTION, DB_ROUND_COLUMN_DATE, DB_ROUND_COLUMN_PLAYER_ONE, DB_ROUND_COLUMN_PLAYER_TWO }, null, null, null, null, null);

            while (cursor.MoveToNext())
            {
                int idIndex = cursor.GetColumnIndex(DB_ROUND_COLUMN_ID);
                int id = cursor.GetInt(idIndex);

                int descriptionIndex = cursor.GetColumnIndex(DB_ROUND_COLUMN_DESCRIPTION);
                string description = cursor.GetString(descriptionIndex);

                int dateIndex = cursor.GetColumnIndex(DB_ROUND_COLUMN_DATE);
                string date = cursor.GetString(dateIndex);

                int playerOneIndex = cursor.GetColumnIndex(DB_ROUND_COLUMN_PLAYER_ONE);
                string playerOne = cursor.GetString(playerOneIndex);

                int playerTwoIndex = cursor.GetColumnIndex(DB_ROUND_COLUMN_PLAYER_TWO);
                string playerTwo = cursor.GetString(playerTwoIndex);

                rounds.Add(new Round()
                {
                    Id = id,
                    Description = description,
                    Date = date,
                    PlayerOne = playerOne,
                    PlayerTwo = playerTwo
                });
            }

            return rounds;
        }

        public void CreateRound(Round round)
        {
            SQLiteDatabase db = this.WritableDatabase;
            ContentValues values = new ContentValues();
            values.Put(DB_ROUND_COLUMN_DESCRIPTION, round.Description);
            values.Put(DB_ROUND_COLUMN_DATE, round.Date);
            values.Put(DB_ROUND_COLUMN_PLAYER_ONE, round.PlayerOne);
            values.Put(DB_ROUND_COLUMN_PLAYER_TWO, round.PlayerTwo);
            db.Insert(DB_TABLE_ROUND, null, values);
            db.Close();
        }

        public void UpdateRound(Round round)
        {
            SQLiteDatabase db = this.WritableDatabase;
            ContentValues values = new ContentValues();
            values.Put(DB_ROUND_COLUMN_DESCRIPTION, round.Description);
            values.Put(DB_ROUND_COLUMN_DATE, round.Date);
            values.Put(DB_ROUND_COLUMN_PLAYER_ONE, round.PlayerOne);
            values.Put(DB_ROUND_COLUMN_PLAYER_TWO, round.PlayerTwo);
            db.Update(DB_TABLE_ROUND, values, $"{DB_ROUND_COLUMN_ID} = ?", new string[] { round.Id.ToString() });
            db.Close();
        }

        public void DeleteRound(int id)
        {
            List<Hole> holes = GetHoles(id);

            foreach (Hole hole in holes)
            {
                DeleteHole(hole.Id);
            }

            SQLiteDatabase db = this.WritableDatabase;
            db.Delete(DB_TABLE_ROUND, $"{DB_ROUND_COLUMN_ID} = ?", new string[] { id.ToString() });
            db.Close();
        }

        public List<Hole> GetHoles(int roundId)
        {
            List<Hole> holes = new List<Hole>();
            SQLiteDatabase db = this.ReadableDatabase;
            ICursor cursor = db.Query(DB_TABLE_HOLE, new string[] { DB_HOLE_COLUMN_ID, DB_HOLE_COLUMN_NUMBER, DB_HOLE_COLUMN_DESCRIPTION, DB_HOLE_COLUMN_PAR, DB_HOLE_COLUMN_PLAYER_ONE_SCORE, DB_HOLE_COLUMN_PLAYER_TWO_SCORE }, $"{DB_HOLE_COLUMN_ROUND_ID} = ?", new string[] { roundId.ToString() }, null, null, null, null);

            while (cursor.MoveToNext())
            {
                int idIndex = cursor.GetColumnIndex(DB_HOLE_COLUMN_ID);
                int id = cursor.GetInt(idIndex);

                int numberIndex = cursor.GetColumnIndex(DB_HOLE_COLUMN_NUMBER);
                int number = cursor.GetInt(numberIndex);

                int descriptionIndex = cursor.GetColumnIndex(DB_HOLE_COLUMN_DESCRIPTION);
                string description = cursor.GetString(descriptionIndex);

                int parIndex = cursor.GetColumnIndex(DB_HOLE_COLUMN_PAR);
                int par = cursor.GetInt(parIndex);

                int playerOneScoreIndex = cursor.GetColumnIndex(DB_HOLE_COLUMN_PLAYER_ONE_SCORE);
                int playerOneScore = cursor.GetInt(playerOneScoreIndex);

                int playerTwoScoreIndex = cursor.GetColumnIndex(DB_HOLE_COLUMN_PLAYER_TWO_SCORE);
                int playerTwoScore = cursor.GetInt(playerTwoScoreIndex);

                holes.Add(new Hole()
                {
                    Id = id,
                    Number = number,
                    Description = description,
                    Par = par,
                    PlayerOneScore = playerOneScore,
                    PlayerTwoScore = playerTwoScore
                });
            }

            return holes;
        }

        public void CreateHole(Hole hole)
        {
            SQLiteDatabase db = this.WritableDatabase;
            ContentValues values = new ContentValues();
            values.Put(DB_HOLE_COLUMN_ROUND_ID, hole.RoundId);
            values.Put(DB_HOLE_COLUMN_NUMBER, hole.Number);
            values.Put(DB_HOLE_COLUMN_DESCRIPTION, hole.Description);
            values.Put(DB_HOLE_COLUMN_PAR, hole.Par);
            values.Put(DB_HOLE_COLUMN_PLAYER_ONE_SCORE, hole.PlayerOneScore);
            values.Put(DB_HOLE_COLUMN_PLAYER_TWO_SCORE, hole.PlayerTwoScore);
            db.Insert(DB_TABLE_HOLE, null, values);
            db.Close();
        }

        public void UpdateHole(Hole hole)
        {
            SQLiteDatabase db = this.WritableDatabase;
            ContentValues values = new ContentValues();
            values.Put(DB_HOLE_COLUMN_NUMBER, hole.Number);
            values.Put(DB_HOLE_COLUMN_DESCRIPTION, hole.Description);
            values.Put(DB_HOLE_COLUMN_PAR, hole.Par);
            values.Put(DB_HOLE_COLUMN_PLAYER_ONE_SCORE, hole.PlayerOneScore);
            values.Put(DB_HOLE_COLUMN_PLAYER_TWO_SCORE, hole.PlayerTwoScore);
            db.Update(DB_TABLE_HOLE, values, $"{DB_HOLE_COLUMN_ID} = ?", new string[] { hole.Id.ToString() });
            db.Close();
        }

        public void DeleteHole(int id)
        {
            SQLiteDatabase db = this.WritableDatabase;
            db.Delete(DB_TABLE_HOLE, $"{DB_HOLE_COLUMN_ID} = ?", new string[] { id.ToString() });
            db.Close();
        }

        public List<Match> GetMatches()
        {
            List<Match> matches = new List<Match>();
            SQLiteDatabase db = this.ReadableDatabase;
            ICursor cursor = db.Query(DB_TABLE_MATCH, new string[] { DB_MATCH_COLUMN_ID, DB_MATCH_COLUMN_DESCRIPTION, DB_MATCH_COLUMN_DATE, DB_MATCH_COLUMN_PLAYER_ONE, DB_MATCH_COLUMN_PLAYER_TWO }, null, null, null, null, null);

            while (cursor.MoveToNext())
            {
                int idIndex = cursor.GetColumnIndex(DB_MATCH_COLUMN_ID);
                int id = cursor.GetInt(idIndex);

                int descriptionIndex = cursor.GetColumnIndex(DB_MATCH_COLUMN_DESCRIPTION);
                string description = cursor.GetString(descriptionIndex);

                int dateIndex = cursor.GetColumnIndex(DB_MATCH_COLUMN_DATE);
                string date = cursor.GetString(dateIndex);

                int playerOneIndex = cursor.GetColumnIndex(DB_MATCH_COLUMN_PLAYER_ONE);
                string playerOne = cursor.GetString(playerOneIndex);

                int playerTwoIndex = cursor.GetColumnIndex(DB_MATCH_COLUMN_PLAYER_TWO);
                string playerTwo = cursor.GetString(playerTwoIndex);

                matches.Add(new Match()
                {
                    Id = id,
                    Description = description,
                    Date = date,
                    PlayerOne = playerOne,
                    PlayerTwo = playerTwo
                });
            }

            return matches;
        }

        public void CreateMatch(Match match)
        {
            SQLiteDatabase db = this.WritableDatabase;
            ContentValues values = new ContentValues();
            values.Put(DB_MATCH_COLUMN_DESCRIPTION, match.Description);
            values.Put(DB_MATCH_COLUMN_DATE, match.Date);
            values.Put(DB_MATCH_COLUMN_PLAYER_ONE, match.PlayerOne);
            values.Put(DB_MATCH_COLUMN_PLAYER_TWO, match.PlayerTwo);
            db.Insert(DB_TABLE_MATCH, null, values);
            db.Close();
        }

        public void UpdateMatch(Match match)
        {
            SQLiteDatabase db = this.WritableDatabase;
            ContentValues values = new ContentValues();
            values.Put(DB_MATCH_COLUMN_DESCRIPTION, match.Description);
            values.Put(DB_MATCH_COLUMN_DATE, match.Date);
            values.Put(DB_MATCH_COLUMN_PLAYER_ONE, match.PlayerOne);
            values.Put(DB_MATCH_COLUMN_PLAYER_TWO, match.PlayerTwo);
            db.Update(DB_TABLE_MATCH, values, $"{DB_MATCH_COLUMN_ID} = ?", new string[] { match.Id.ToString() });
            db.Close();
        }

        public void DeleteMatch(int id)
        {
            List<Set> sets = GetSets(id);

            foreach (Set set in sets)
            {
                DeleteSet(set.Id);
            }

            SQLiteDatabase db = this.WritableDatabase;
            db.Delete(DB_TABLE_MATCH, $"{DB_MATCH_COLUMN_ID} = ?", new string[] { id.ToString() });
            db.Close();
        }

        public List<Set> GetSets(int matchId)
        {
            List<Set> sets = new List<Set>();
            SQLiteDatabase db = this.ReadableDatabase;
            ICursor cursor = db.Query(DB_TABLE_SET, new string[] { DB_SET_COLUMN_ID, DB_SET_COLUMN_NUMBER, DB_SET_COLUMN_PLAYER_ONE_SCORE, DB_SET_COLUMN_PLAYER_TWO_SCORE }, $"{DB_SET_COLUMN_MATCH_ID} = ?", new string[] { matchId.ToString() }, null, null, null, null);

            while (cursor.MoveToNext())
            {
                int idIndex = cursor.GetColumnIndex(DB_SET_COLUMN_ID);
                int id = cursor.GetInt(idIndex);

                int numberIndex = cursor.GetColumnIndex(DB_SET_COLUMN_NUMBER);
                int number = cursor.GetInt(numberIndex);

                int playerOneScoreIndex = cursor.GetColumnIndex(DB_SET_COLUMN_PLAYER_ONE_SCORE);
                int playerOneScore = cursor.GetInt(playerOneScoreIndex);

                int playerTwoScoreIndex = cursor.GetColumnIndex(DB_SET_COLUMN_PLAYER_TWO_SCORE);
                int playerTwoScore = cursor.GetInt(playerTwoScoreIndex);

                sets.Add(new Set()
                {
                    Id = id,
                    Number = number,
                    PlayerOneScore = playerOneScore,
                    PlayerTwoScore = playerTwoScore
                });
            }

            return sets;
        }

        public void CreateSet(Set set)
        {
            SQLiteDatabase db = this.WritableDatabase;
            ContentValues values = new ContentValues();
            values.Put(DB_SET_COLUMN_MATCH_ID, set.MatchId);
            values.Put(DB_SET_COLUMN_NUMBER, set.Number);
            values.Put(DB_SET_COLUMN_PLAYER_ONE_SCORE, set.PlayerOneScore);
            values.Put(DB_SET_COLUMN_PLAYER_TWO_SCORE, set.PlayerTwoScore);
            db.Insert(DB_TABLE_SET, null, values);
            db.Close();
        }

        public void UpdateSet(Set set)
        {
            SQLiteDatabase db = this.WritableDatabase;
            ContentValues values = new ContentValues();
            values.Put(DB_SET_COLUMN_NUMBER, set.Number);
            values.Put(DB_SET_COLUMN_PLAYER_ONE_SCORE, set.PlayerOneScore);
            values.Put(DB_SET_COLUMN_PLAYER_TWO_SCORE, set.PlayerTwoScore);
            db.Update(DB_TABLE_SET, values, $"{DB_SET_COLUMN_ID} = ?", new string[] { set.Id.ToString() });
            db.Close();
        }

        public void DeleteSet(int id)
        {
            SQLiteDatabase db = this.WritableDatabase;
            db.Delete(DB_TABLE_SET, $"{DB_SET_COLUMN_ID} = ?", new string[] { id.ToString() });
            db.Close();
        }

        public List<Program> GetPrograms()
        {
            List<Program> programs = new List<Program>();
            SQLiteDatabase db = this.ReadableDatabase;
            ICursor cursor = db.Query(DB_TABLE_PROGRAM, new string[] { DB_PROGRAM_COLUMN_ID, DB_PROGRAM_COLUMN_NAME }, null, null, null, null, null);

            while (cursor.MoveToNext())
            {
                int idIndex = cursor.GetColumnIndex(DB_PROGRAM_COLUMN_ID);
                int id = cursor.GetInt(idIndex);

                int nameIndex = cursor.GetColumnIndex(DB_PROGRAM_COLUMN_NAME);
                string name = cursor.GetString(nameIndex);

                programs.Add(new Program()
                {
                    Id = id,
                    Name = name
                });
            }

            return programs;
        }

        public void CreateProgram(Program program)
        {
            SQLiteDatabase db = this.WritableDatabase;
            ContentValues values = new ContentValues();
            values.Put(DB_PROGRAM_COLUMN_NAME, program.Name);
            db.Insert(DB_TABLE_PROGRAM, null, values);
            db.Close();
        }

        public void CopyProgram(Program program)
        {
            SQLiteDatabase db = this.WritableDatabase;
            ContentValues values = new ContentValues();
            values.Put(DB_PROGRAM_COLUMN_NAME, program.Name);
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
