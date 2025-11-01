﻿using System.Globalization;
using System.IO;
using System.Text;

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
        public TimeSpan Duration => Time_end - Time_start; // sugar syntax
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
        static TaskList[] tasks = new TaskList[20]; // Defensive Coding Approach
        static int count = 0;

        static void Main(string[] args)
        {
            tasks[count++] = new TaskList("Test: Task 1", DateTime.Today, new TimeSpan(2, 0, 0), new TimeSpan(5, 0, 0))
            {
                Status = StatusEnum.Pending
            };
            tasks[count++] = new TaskList("Test: Task 2", DateTime.Today.AddDays(2), new TimeSpan(5, 0, 0), new TimeSpan(7, 0, 0))
            {
                Status = StatusEnum.Done
            };
            tasks[count++] = new TaskList("Test: Task 3", DateTime.Today.AddDays(1), new TimeSpan(7, 0, 0), new TimeSpan(8, 0, 0))
            {
                Status = StatusEnum.Pending
            };
            while (true)
            {
                Console.Clear();
                Console.WriteLine("===============================");
                Console.WriteLine("         TO-DO LIST APP");
                Console.WriteLine("===============================");
                Console.WriteLine("1. View Tasks");
                Console.WriteLine("2. Add Task");
                Console.WriteLine("3. Delete Task");
                Console.WriteLine("4. Change Status Task");
                Console.WriteLine("5. Edit Task");
                Console.WriteLine("6. Delete All Task");
                Console.WriteLine("7. Today’s Productivity Report");
                Console.WriteLine("8. Export TimeTable to .txt");
                Console.WriteLine("0. Exit");
                Console.WriteLine("-------------------------------");

                Console.Write("Choose an option: ");
                string menuinput = Console.ReadLine();
                if (!int.TryParse(menuinput, out int choose))
                {
                    Console.WriteLine("Invalid input. Try again.");
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                    continue;
                }

                switch (choose)
                {
                    case 0:
                        return;
                    case 1:
                        Console.Clear();
                        ViewTask();
                        Console.WriteLine("===============================");
                        Console.WriteLine("Press Enter to return Menu.");
                        Console.ReadLine();
                        break;

                    case 2:
                        Console.Clear();
                        AddTask();
                        break;
                    case 3:
                        Console.Clear();
                        DeleteTask();
                        break;
                    case 4:
                        Console.Clear();
                        StatusTask();
                        break;
                    case 5:
                        Console.Clear();
                        EditTask();
                        break;
                    case 6:
                        Console.Clear();
                        DeleteAllTask();
                        break;
                    case 7:
                        Console.Clear();
                        ReportTask();
                        break;
                    case 8:
                        Console.Clear();
                        ExportFileTxt();
                        break;

                    default:
                        Console.WriteLine("Invalid choose. Try again.");
                        Console.WriteLine("Press Enter to continue...");
                        Console.ReadLine();
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
                    Console.ReadLine();
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
                if (!int.TryParse(Console.ReadLine(), out int choosestatus))
                {
                    Console.WriteLine("Invalid Input.");
                    Console.ReadLine();
                    return;
                }
                switch (choosestatus)
                {
                    case 0:
                        tasks[countstatus - 1].Status = StatusEnum.Pending;
                        Console.WriteLine("Set back to Pending.");
                        break;
                    case 1:
                        tasks[countstatus - 1].Status = StatusEnum.Done;
                        Console.WriteLine("Marked as Done.");
                        break;
                    default:
                        Console.WriteLine("Invalid status value!");
                        Console.ReadLine();
                        return;
                }
                Console.Clear();
                ViewTask();
                Console.WriteLine("===============================");
                Console.WriteLine("Update list");
                Console.ReadLine();
                return;
            }

            static void DeleteTask()
            {
                while (true)
                {
                    ViewTask();
                    if (count == 0)
                    {
                        Console.ReadLine();
                        return;
                    }

                    Console.WriteLine($"Enter task number (1...{count}) or press 0 to cancel:");
                    if (!int.TryParse(Console.ReadLine(), out int numberDel))
                    {
                        Console.WriteLine("Invalid input!");
                        Console.ReadLine();
                        return;
                    }
                    if (numberDel == 0) return;
                    if (numberDel < 1 || numberDel > count)
                    {
                        Console.WriteLine("Invalid input!. Press Enter to try again.");
                        Console.ReadLine();
                        continue;
                    }
                    for (int i = numberDel - 1; i < count; i++)
                    {
                        tasks[i] = tasks[i + 1];
                    }
                    tasks[count - 1] = null;
                    count--;
                    Console.Clear();
                    ViewTask();
                    Console.WriteLine("===============================");
                    Console.WriteLine("Task Deleted.");
                    Console.ReadLine();
                    return;
                }
            }
            static void EditTask()
            {
                ViewTask();
                while (true)
                {
                    if (count == 0)
                    {
                        Console.ReadLine();
                        return;
                    }
                    Console.WriteLine($"Enter task number (1...{count}) or press 0 to cancel:");
                    if (!int.TryParse(Console.ReadLine(), out int numberEdit))
                    {
                        Console.WriteLine("Invalid Input. Press Enter to try again");
                        Console.ReadLine();
                        continue;
                    }
                    if (numberEdit == 0) return;
                    if (numberEdit < 1 || numberEdit > count)
                    {
                        Console.WriteLine("Invalid task.");
                        Console.ReadLine();
                        continue;
                    }
                    {
                        Console.WriteLine("Enter new Task");
                        int i = numberEdit - 1;
                        tasks[i].Name = Console.ReadLine();
                        Console.Clear();
                        ViewTask();
                        Console.WriteLine("===============================");
                        Console.WriteLine("Task updated successfully.");
                        Console.ReadLine();
                        return;
                    }
                }
            }

            static void DeleteAllTask()
            {
                for (int i = 0; i < count; i++)
                {
                    tasks[i] = null;
                    count = 0;
                }
                ViewTask();
                Console.WriteLine("===============================");
                Console.WriteLine("Delete All Task successfully.");
                Console.ReadLine();
                return;
            }

            static void ReportTask()
            {
                int completecount = 0;
                for (int i = 0; i < count; i++)
                {
                    if (tasks[i].Status == StatusEnum.Done)
                    {
                        completecount++;
                    }
                }
                double completerate = count == 0 ? 0.00 : (double)completecount * 100.0 / count;

                string evaluation = completecount == count // oán tử ba ngôi (ternary operator)
                    ? "Excellent! Keep the energy going!"
                    : completerate < 60
                        ? "Making progress — stay focused!"
                        : "Good work! Keep pushing forward!";
                ViewTask();
                Console.WriteLine("===============================");
                Console.WriteLine("  TODAY'S PRODUCTIVITY REPORT  ");
                Console.WriteLine("===============================");
                Console.WriteLine($"Total Tasks: {count}");
                Console.WriteLine($"Completed: {completecount}");
                Console.WriteLine($"Completion Rate: {completerate:F2}%");
                Console.WriteLine($"Evaluation: {evaluation}");
                Console.ReadLine();
                return;
            }
            static void ExportFileTxt()
            {

                //sap xep ngay gio bat dau
                // ref: https://toidicodedao.com/2015/02/10/series-c-hay-ho-callback-trong-c-delegate-action-predicate-func/
                Array.Sort(tasks, 0, count, Comparer<TaskList>.Create((a, b) =>
                {
                    int byDate = a.Date.Date.CompareTo(b.Date.Date);
                    if (byDate != 0) 
                    { 
                        return byDate;
                    }
                    else 
                    { 
                        int byTime = a.Time_start.CompareTo(b.Time_start);
                        return byTime; 
                    }
                }));
                ////ViewTask();
                //ReportTask();
                static string ExportTaskDetail()
                {
                    var etd = new StringBuilder();
                    etd.AppendLine("                TIMETABLE");
                    etd.AppendLine("========================================");
                    etd.AppendLine($"Export at: {DateTime.Now}");
                    etd.AppendLine("----------------------------------------");
                    if (count == 0)
                    {
                        etd.AppendLine("No task available.");
                    }
                    else
                    {
                        for(int i = 0; i < count;i++)
                        {
                            string statusview = tasks[i].Status == StatusEnum.Done ? "[x]" : "[ ]";
                            etd.AppendLine($"{tasks[i].Date:yyyy:MM:dd} - {tasks[i].Time_start:hh\\:mm} - {tasks[i].Time_end:hh\\:mm} {statusview} {tasks[i].Name}");
                        }
                        int completecount = 0;
                        for (int i = 0; i < count; i++)
                        {
                            if (tasks[i].Status == StatusEnum.Done)
                            {
                                completecount++;
                            }
                        }
                        double completerate = count == 0 ? 0.00 : (double)completecount * 100.0 / count;
                        etd.AppendLine("========================================");
                        etd.AppendLine($"Total Task = {count}");
                        etd.AppendLine($"Completed: {completecount}");
                        etd.AppendLine($"Completion Rate: {completerate:F2}%");
                    }
                    return etd.ToString();
                }
                //Console.WriteLine("test:");
                Console.WriteLine(ExportTaskDetail());
                string text = ExportTaskDetail();
                string docPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                File.WriteAllText(Path.Combine(docPath, "TimeTable.txt"), text);
                Console.WriteLine("===============================");
                Console.WriteLine($"Exported to: {docPath}");
                Console.WriteLine("Press Enter to return...");
                Console.ReadLine();
                return;

            }
               
        }
    }
}
