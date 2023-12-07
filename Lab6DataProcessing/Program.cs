using CsvHelper.Configuration;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting.Messaging;

namespace Lab6DataProcessing
{
    public class CsvClass
    {
        public DateTime DateTime { get; set; }
        public double Value { get; set; }
    }

    internal class Program
    {

        static void SeparateDateAndCurrencyRate()
        {
            string csvFilePath = @"C:\Users\newac\OneDrive\Desktop\PAD - Programarea aplicațiilor distribuite\lab5\values.csv";

            string DateFilePath = @"C:\Users\newac\OneDrive\Desktop\PAD - Programarea aplicațiilor distribuite\lab6\task1\dates.csv";
            string RateFilePath = @"C:\Users\newac\OneDrive\Desktop\PAD - Programarea aplicațiilor distribuite\lab6\task1\rates.csv";

            using (var reader = new StreamReader(csvFilePath))
            {
                using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
                {
                    // Читаем записи из CSV файла
                    var records = csv.GetRecords<CsvClass>().ToList();

                    // Разделяем записи на две группы
                    var field1Records = records.Select(r => new { Dates = r.DateTime }).ToList();
                    var field2Records = records.Select(r => new { Values = r.Value }).ToList();

                    // Записываем первую группу в первый файл
                    using (var writer = new StreamWriter(DateFilePath))
                    using (var csvWriter = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
                    {
                        csvWriter.WriteRecords(field1Records);
                    }

                    // Записываем вторую группу во второй файл
                    using (var writer = new StreamWriter(RateFilePath))
                    using (var csvWriter = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
                    {
                        csvWriter.WriteRecords(field2Records);
                    }
                }
            }
        }

        static void SeparateByWeek()
        {
            string csvFilePath = @"C:\Users\newac\OneDrive\Desktop\PAD - Programarea aplicațiilor distribuite\lab5\values.csv";

            using (var reader = new StreamReader(csvFilePath))
            {
                using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
                {
                    // Читаем записи из CSV файла
                    var records = csv.GetRecords<CsvClass>().ToList();

                    var day = records[0].DateTime.DayOfWeek;

                    List<CsvClass> selectedRecords = new List<CsvClass>();

                    for (int i = 0; i < records.Count; i++)
                    {
                        if (records[i].DateTime.DayOfWeek <= day)
                        {
                            selectedRecords.Add(records[i]);
                            day = records[i].DateTime.DayOfWeek;
                        }
                        if (records[i].DateTime.DayOfWeek > day || i == records.Count - 1)
                        {
                            using (var writer = new StreamWriter($@"C:\Users\newac\OneDrive\Desktop\PAD - Programarea aplicațiilor distribuite\lab6\task3\{selectedRecords[0].DateTime.Year}{selectedRecords[0].DateTime.Month.ToString().PadLeft(2, '0')}{selectedRecords[0].DateTime.Day.ToString().PadLeft(2, '0')}-{selectedRecords[selectedRecords.Count - 1].DateTime.Year}{selectedRecords[selectedRecords.Count - 1].DateTime.Month.ToString().PadLeft(2, '0')}{selectedRecords[selectedRecords.Count - 1].DateTime.Day.ToString().PadLeft(2, '0')}.csv"))
                            using (var csvWriter = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
                            {
                                csvWriter.WriteRecords(selectedRecords);
                            }
                            day = records[i].DateTime.DayOfWeek;
                            selectedRecords.Clear();
                            selectedRecords.Add(records[i]);
                        }
                    }
                }
            }
        }

        static void SeparateByYear()
        {
            string csvFilePath = @"C:\Users\newac\OneDrive\Desktop\PAD - Programarea aplicațiilor distribuite\lab5\values.csv";

            using (var reader = new StreamReader(csvFilePath))
            {
                using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
                {
                    // Читаем записи из CSV файла
                    var records = csv.GetRecords<CsvClass>().ToList();
                    var year = 2023;

                    List<CsvClass> selectedRecords = new List<CsvClass>();

                    for (int i = 0; i < records.Count; i++)
                    {
                        if (records[i].DateTime.Year == year)
                        {
                            selectedRecords.Add(records[i]);
                        }
                        if (records[i].DateTime.Year != year || i == records.Count - 1)
                        {
                            using (var writer = new StreamWriter($@"C:\Users\newac\OneDrive\Desktop\PAD - Programarea aplicațiilor distribuite\lab6\task2\{selectedRecords[0].DateTime.Year}{selectedRecords[0].DateTime.Month}{selectedRecords[0].DateTime.Day}-{selectedRecords[selectedRecords.Count - 1].DateTime.Year}{selectedRecords[selectedRecords.Count - 1].DateTime.Month.ToString().PadLeft(2, '0')}{selectedRecords[selectedRecords.Count - 1].DateTime.Day.ToString().PadLeft(2, '0')}.csv"))
                            using (var csvWriter = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
                            {
                                csvWriter.WriteRecords(selectedRecords);
                            }
                            year--;
                            selectedRecords.Clear();
                        }
                    }
                }
            }
        }

        static void GetRateByDate(DateTime dateTime)
        {
            string csvFilePath = @"C:\Users\newac\OneDrive\Desktop\PAD - Programarea aplicațiilor distribuite\lab5\values.csv";

            using (var reader = new StreamReader(csvFilePath))
            {
                using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
                {
                    var records = csv.GetRecords<CsvClass>().ToList();
                    var record = records.FirstOrDefault(r => r.DateTime.Date == dateTime);
                    if(record == null || record.Value == 0)
                        Console.WriteLine("null\n");
                    else
                    {
                        Console.WriteLine(record.DateTime + " | " + record.Value + "\n" );
                    }
   
                }
            }


        }


        static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;

            while (true)
            {
                Console.WriteLine("Выберите действие:");
                Console.WriteLine("1) Разбить values.csv на 2 файла, один с датами, другой с курсом валют");
                Console.WriteLine("2) Разбить values.csv на N файлов по году");
                Console.WriteLine("3) Разбить values.csv на N файлов по неделям:");
                Console.WriteLine("4) Вернуть значение за конкретную дату (дату вводите как yy-mm-dd)");

                string selected = Console.ReadLine().Trim(' ');
                if (selected == "1")
                {
                    SeparateDateAndCurrencyRate();
                }
                else if (selected == "2")
                {
                    SeparateByYear();
                }
                else if (selected == "3")
                {
                    SeparateByWeek();
                }
                else if(selected == "4")
                {
                    Console.WriteLine("---дату вводите как yy-mm-dd");
                    DateTime dateTime;
                    try
                    {
                        selected = Console.ReadLine();
                        string[] yymmdd = selected.Split('-');
                        dateTime = new DateTime(Convert.ToInt32(yymmdd[0]), Convert.ToInt32(yymmdd[1]), Convert.ToInt32(yymmdd[2]));
                        GetRateByDate(dateTime);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Неверный ввод даты\n");
                    }
                }
            }
        }
    }
}
