using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Threading;




namespace Lesson_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            Lesson2();
            Console.ReadKey();
        }

   
        static void Lesson2()
        {
            Test t = new Test();
            t.Testing();
            TestingAsync p = new TestingAsync();
            p.ReadTwoDateAsync();
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
