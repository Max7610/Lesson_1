using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Diagnostics;
using System.Threading;



namespace Lesson_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Lesson1();
            Lesson2();
            Console.ReadKey();
        }

        static void Lesson1()
        {// Создание списка классов
            var Students = GenericStudentList(10);
            Console.WriteLine("Печать всего списка:");
            #region
            foreach (var student in Students)
            {
                Console.WriteLine(student.ToString());
            }
            #endregion
            Console.WriteLine("\nЗадание 1:\n");
            #region
            foreach (var student in Students)
            {
                Console.WriteLine(student.ToString());
            }
            foreach (var student in Students.Where(n => n.Age > 18 && n.Grade > 7.5d))
            {
                Console.WriteLine($"--> {student.Name} {student.Grade}");
            }
            #endregion
            Console.WriteLine("\nЗадание 2:\n");
            #region


            foreach (var student in Students.Where(n => n.Subjects.IndexOf("Математика") >= 0).OrderByDescending(n => n.Grade))
            {
                Console.WriteLine($"--> {student.Name} {student.Grade}");
            }
            #endregion
            Console.WriteLine("\nЗадание 3:\n");
            #region
            foreach (var student in Students.Where(n => n.Subjects.Any(m => m == "Математика")).OrderByDescending(n => n.Grade))
            {
                Console.WriteLine($"--> {student.Name} {student.Grade}");
            }

            foreach (var student in Students.OrderByDescending(n => n.Grade).Take(3))
            {
                Console.WriteLine(student.ToString());
            }
            #endregion
            Console.WriteLine("\nЗадание 4:\n");
            #region
            foreach (var i in Students.GroupBy(n => n.Age).OrderBy(n => n.Key))
            {
                Console.WriteLine($"{i.Key}) {i.Count()}");
            }
            #endregion
            Console.WriteLine("\nЗадание 5:\n");
            #region
            foreach (var i in Students.GroupBy(n => n.Age).OrderBy(n => n.Key))
            {
                Console.WriteLine($"{i.Key}: {string.Join(", ", i.ToList().Select(n => n.Name))}");

            }
            #endregion
            Console.WriteLine("\nЗадание 6:\n");
            #region
            Console.WriteLine($" a.Средний возраст {Students.Average(n => n.Age)}");
            Console.WriteLine($" b.Максимум - {Students.Max(n => n.Grade)} Минимум - {Students.Min(n => n.Grade)} Среднее - {Students.Average(n => n.Grade)}");
            Console.WriteLine($" c.Количество студентов {Students.Count} суммарный балл {Students.Sum(n => n.Grade)}");
            #endregion
            Console.WriteLine("\nЗадание 7:\n");
            #region
            var Sort_1 = Students.Where(n => n.Name[0] == 'C' || n.Name[0] == 'B')
                                 .OrderBy(n => n.Age)
                                 .OrderBy(n => n.Grade)
                                 .Select(n => new { n.Name });
            Console.WriteLine($"--> {string.Join(", ", Sort_1.Select(n => n.Name))}");
            #endregion
            Console.WriteLine("\nЗадание 8:\n");
            #region
            foreach (var i in Students.Where(n => n.Subjects.Any(m => m == "Физика"))
                                 .Where(n => n.Subjects.Any(m => m == "Математика"))
                                 .Where(n => n.Grade > 8d))
            {
                Console.WriteLine(i.ToString());
            }
            #endregion
            Console.WriteLine("\nЗадание 9:\n");
            #region

            foreach (var i in Students.Select(n => new { StudentName = n.Name, IsExcellent = n.Grade > 8.0 })
                .Where(n => n.IsExcellent))
            {
                Console.WriteLine($"{i.StudentName} -> {i.IsExcellent}");
            }
            #endregion
            Console.WriteLine($"\nЕсть ли ученики с средним балом больше 6,5 {Students.Any(n => n.Grade > 6.5d)}");
            Console.WriteLine("\nСписок предметов:");
            foreach (var i in Students.SelectMany(n => n.Subjects).GroupBy(n => n))
            {
                Console.WriteLine($"-->{i.Key}");
            }
        }
        static void Lesson2()
        {
            Test t = new Test();
            t.Testing();
            TestingAsync p = new TestingAsync();
            p.ReadTwoDateAsync();
        }
        static List<Student> GenericStudentList(int n)
        {
            List<Student> students = new List<Student>();
            for (int i = 0; i < n; i++)
            {
                Random rand = new Random(i);
                students.Add(new Student(i, GeneratirName(i), rand.Next(15, 26), rand.NextDouble() * 15, GenericSubject(2,i)));
            }

            return students;
        }
        static string GeneratirName(int x)
       
        {
            Random rand = new Random(x);
            string abc = "qwertyuiopasdfghjklzxcvbnm";
            string ABC = "QWERTYUIOPASDFGHJKLZXCVBNM";
            string res = ABC[rand.Next(ABC.Length)].ToString();
            int nameLenght = rand.Next(2,8);
            for (int i = 0; i <nameLenght ; i++)
            {
                res += abc[rand.Next(abc.Length)].ToString();
            }
            return res;
        }
        static List<string> GenericSubject(int n,int k)
        {
            List<string> strings = new List<string> {"Математика","Русский язык","Литература","Физика","Информатика","Обществознание","История","Химия","Астрономия" };
            var list = new List<string>();
            Random rand = new Random(k);
            for (int i =0;i<n;i++)
            {
                var a = strings[rand.Next(strings.Count)];
                list.Add(a);
                strings.Remove(a);
            }
            return list;
        }
    }
    
    class Student 
    {
        public int Id { get; }
        public string Name { get; }
        public int Age { get; }
        public double Grade { get; }
        public List<string> Subjects  = new List<string>();
        public Student(int id, string name, int age, double grade, List<string> subjects)
        {
            Id = id;
            Name = name;
            Age = age;
            Grade = grade;
            Subjects = subjects;    
        }
        public override string ToString()
        {
            return $"id: {Id}, Name: {Name} Age: {Age} Grade: {Grade}  \nSubject: {string.Join(", ",Subjects)}";
        }
        
    }

    class TestingAsync
    {
        SqlConnection sqlConnection = null;


        public async void ReadTwoDateAsync()
        {
            var cts1 = new CancellationTokenSource();
            var cts2 = new CancellationTokenSource();
            TestingAsync test = new TestingAsync();
            var a = test.ReadDataSongListAsync(true, cts1.Token, 2000);//Меняя время задержки управляем выполнением или не выполнением процесса 
            Console.WriteLine(">>1");
            var b = test.ReadDataSongListAsync(false, cts2.Token, 1000);
            Console.WriteLine(">>2");
            await Task.Delay(1000);
            cts1.Cancel();
            var resault = await Task.WhenAll(a, b);
            foreach (var i in resault)
            {
                Print(i);
            }
            Console.WriteLine("##############################");
            Print(a.Result);
            Print(b.Result);

        }
        void Print(List<SingerSong> a)
        {
            foreach (var i in a) Console.WriteLine(i.ToString());
            Console.WriteLine("_______________________________");
        }
        async Task<List<SingerSong>> ReadDataSongListAsync(bool activStatus, CancellationToken cancellationToken, int n)
        {
            List<SingerSong> resault = new List<SingerSong>();
            await Task.Delay(n);//Меняя положение задержки меняю точку срабатывания исключения 
            try
            {
                cancellationToken.ThrowIfCancellationRequested(); // Проверяем запрос на отмену
                sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["KaraokeBd"].ConnectionString);//Соеденяемся 

            }
            catch (Exception ex) { Console.WriteLine(ex.Message.ToString()); }
            //await Task.Delay(n);
            try
            {
                cancellationToken.ThrowIfCancellationRequested(); // Проверяем запрос на отмену
                sqlConnection.Open();//Читаем
                Console.WriteLine(sqlConnection.State.ToString());//Статус чтения

            }
            catch (Exception ex) { Console.WriteLine(ex.Message.ToString()); }
            //await Task.Delay(n);
            SqlDataAdapter command = new SqlDataAdapter();
            try
            {
                cancellationToken.ThrowIfCancellationRequested(); // Проверяем запрос на отмену
                command = new SqlDataAdapter("SELECT Singer ,  Song, Activ " +
                                         "FROM SongList ", sqlConnection);//Команда для запроса 

            }
            catch (Exception ex) { Console.WriteLine(ex.Message.ToString()); }
            try
            {
                cancellationToken.ThrowIfCancellationRequested(); // Проверяем запрос на отмену
                DataSet dataSet = new DataSet();//внутренняя база данных
                command.Fill(dataSet);//записываем из внешней во внутреннюю
                foreach (DataRow i in dataSet.Tables[0].Rows)
                {
                    resault.Add(new SingerSong(i));
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message.ToString()); }

            return resault.Where(x => x.Activ == activStatus).ToList();
        }
    }
    class Test
    {
        SqlConnection sqlConnection = null;
        List<SingerSong> SongList = new List<SingerSong>();
        public void Testing()
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["KaraokeBd"].ConnectionString);//Соеденяемся 
            sqlConnection.Open();//Читаем
            Console.WriteLine(sqlConnection.State.ToString());//Статус чтения

            var command = new SqlDataAdapter("SELECT Singer ,  Song, Activ " +
                                     "FROM SongList ", sqlConnection);//Команда для запроса 
            DataSet dataSet = new DataSet();//внутренняя база данных
            command.Fill(dataSet);//записываем из внешней во внутреннюю
            ReadDateSongList(dataSet);//Читаем в отдельный список
            PrintSongList();
            //Console.WriteLine("__________________"); //Использование Linq запроса
            //foreach(var i in SongList.Where(x => x.Activ).ToList())
            //{
            //    Console.WriteLine(i.ToString());
            //}
            sqlConnection.Close();

        }
        void ReadDateSongList(DataSet data)
        {
            foreach (DataRow i in data.Tables[0].Rows)
            {
                SongList.Add(new SingerSong(i));
            }
        }
        void PrintSongList()
        {
            foreach (var i in SongList)
            {
                Console.WriteLine(i.ToString());
            }
        }


    }
    class SingerSong
    {
        public string Song { get; }
        public string Singer { get; }
        public bool Activ { get; }
        public SingerSong(DataRow data)
        {
            Song = (string)data[0];
            Singer = (string)data[1];
            Activ = (int)data[2] == 1 ? true : false;
        }
        public override string ToString()
        {
            return $"{Song} {Singer} {Activ.ToString()}";
        }
    }


}
