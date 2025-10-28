using System;
using System.Globalization;

namespace Todolist
{

    class InputParse
    {
        public static string GetName(string prompt)
        {
            Console.Write(prompt);
            string name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Task name cannot be empty!");
                return GetName(prompt);
            }
            return name;
        }
        public static DateTime GetDate(string prompt)
        {
            while (true)
            {
                Console.WriteLine(prompt);
                string dateinput = Console.ReadLine();
                if (string.IsNullOrEmpty(dateinput))
                {
                    return DateTime.Now;
                }
                if (DateTime.TryParseExact(
                    dateinput,
                    new[] { "yyyy-MM-dd" },
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime date))
                {
                    return date;
                }
                Console.WriteLine("Invalid date! Please use yyyy-MM-dd");
            }
        }
        public static TimeSpan GetTime(string prompt)
        {
            while (true)
            {
                Console.WriteLine(prompt);
                string timeinput = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(timeinput))
                {
                    return TimeSpan.Zero;
                }
                if (TimeSpan.TryParseExact(
                    timeinput,
                    new[] { "hh\\:mm" },
                    CultureInfo.InvariantCulture,
                    TimeSpanStyles.None,
                    out TimeSpan time))
                {
                    return time;
                }
                Console.WriteLine("Invalid time! Plese use HH:mm");
            }
        }
    }
    public enum StatusEnum
    {
        Pending,
        Done
    }
    class TaskList
    {
        public string Name;
        public StatusEnum Status;
        public DateTime Date;
        public TimeSpan Time_start;
        public TimeSpan Time_end;
        public TimeSpan Duration => Time_end - Time_start;
        public TaskList(string name, DateTime date, TimeSpan time_start, TimeSpan time_end)
        {
            Name = name;
            Status = StatusEnum.Pending;
            Date = date;
            Time_start = time_start;
            Time_end = time_end;

        }
    }

    class Program
    {
        static TaskList[] tasks = new TaskList[20];
        static int count = 0;
        
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("===============================");
                Console.WriteLine("         TO-DO LIST APP");
                Console.WriteLine("===============================");
                Console.WriteLine("1. View Tasks");
                Console.WriteLine("2. Add Task");
                Console.WriteLine("3. Waiting....");
                Console.WriteLine("4. Mark Task as Done");
                Console.WriteLine("0. Exit");
                Console.WriteLine("-------------------------------");

                Console.Write("Choose an option: ");
                int choose = Convert.ToInt32(Console.ReadLine());
                switch (choose)
                {
                    case 0:
                        return;
                    case 1:
                        Console.Clear();
                        ViewTask();
                        Console.WriteLine("Press Enter to return Menu.");
                        Console.ReadLine();
                        break;

                    case 2:
                        Console.Clear();
                        AddTask();
                        break;
                    //case 3:
                    //    Console.Clear();
                       
                    //    break;
                    case 4:
                        Console.Clear();
                        StatusTask();
                        break;

                    default:
                        Console.WriteLine("Invalid input. Try again.");
                        break;

                }

            }

            static void ViewTask()
            {
                Console.WriteLine("===============================");
                Console.WriteLine("       TASK LIST");
                Console.WriteLine("===============================");
                if (count == 0)
                {
                    Console.WriteLine("No tasks available.");
                }
                for (int i = 0; i < count; i++)
                {
                    string statusview = tasks[i].Status == StatusEnum.Done ? "[x]" : "[ ]";
                    Console.WriteLine($"{i + 1}. {statusview} {tasks[i].Name} ({tasks[i].Date:yyyy-MM-dd} {tasks[i].Time_start:hh\\:mm} - {tasks[i].Time_end:hh\\:mm}) - {tasks[i].Duration.TotalMinutes} min");
                }
                Console.WriteLine("===============================");
                Console.WriteLine($"Total: {count} task.");

            }

            static void AddTask()
            {
                string name = InputParse.GetName("Enter task name: ");
                DateTime date = InputParse.GetDate("Enter date (yyyy-MM-dd) or press Enter to skip: ");
                TimeSpan time_start = InputParse.GetTime("Enter start time (HH:mm) or press Enter to skip: ");
                TimeSpan time_end = InputParse.GetTime("Enter end time (HH:mm) or press Enter to skip: ");
                if (time_end < time_start && time_end != TimeSpan.Zero && time_start != TimeSpan.Zero)
                {
                    Console.WriteLine("End time > Start time");
                    Console.ReadLine();
                    return;
                }

                tasks[count] = new TaskList(name, date, time_start, time_end);
                tasks[count].Name = name;
                tasks[count].Date = date;
                tasks[count].Time_start = time_start;
                tasks[count].Time_end = time_end;
                count++;
                Console.WriteLine("Task added successfully!");
                Console.ReadLine();
            }

            static void StatusTask()
            {
                ViewTask();
                if (count == 0)
                {                    
                    return;
                }                
                Console.WriteLine($"Enter task number (1..{count}) or press 0 to cancel: ");
                if (!int.TryParse(Console.ReadLine(), out int countstatus))
                {
                    Console.WriteLine("Invalid input!");
                    Console.ReadLine();
                    return;
                }
                if (countstatus == 0) return;
                if (countstatus < 0 || countstatus > count)
                {
                    Console.WriteLine("Invalid task number!");
                    Console.ReadLine();
                    return;
                }
                
                Console.WriteLine("Enter status: press 1 = Done, press 0 = Pending");
                int chosestatus = Convert.ToInt32(Console.ReadLine());
                switch (chosestatus)
                {
                    case 0:
                        tasks[countstatus - 1].Status = StatusEnum.Pending;
                        Console.WriteLine("Set back to Pending.");
                        break;
                    case 1:
                        tasks[countstatus-1].Status = StatusEnum.Done;
                        Console.WriteLine("Marked as Done.");
                        break;
                    default:
                        Console.WriteLine("Invalid status value!");
                        Console.ReadLine();
                        return;
                }
                Console.WriteLine("===============================");
                Console.WriteLine("Update list");
            }
        }
    }
}
