using System;
using System.Collections.Generic;

// Process class to represent a process
class Process
{
    public string ProcessId { get; set; }     // Process ID
    public int ArrivalTime { get; set; }   // Arrival time of the process
    public int BurstTime { get; set; }     // Burst time of the process

    public int Priority { get; set; }     // priority of the process

    public Process(string processId, int arrivalTime, int burstTime, int priority = 0)
    {
        ProcessId = processId;
        ArrivalTime = arrivalTime;
        BurstTime = burstTime;
        Priority = priority;
    }
}

// FCFS scheduling algorithm class
class FCFS
{
    public static void Execute(Queue<Process> readyQueue)
    {
        int time = 0; // Current time
        List<Process> grantchart = new List<Process>(); // List to store completed processes
        Dictionary<int, string> ganttChart = new Dictionary<int, string>(); // Dictionary to store Gantt Chart
        Console.WriteLine("First Come First Served Algo");
        Console.WriteLine("--------------------------------");

        while (readyQueue.Count > 0)
        {
            Process currentProcess = readyQueue.Dequeue(); // Get the front process from the ready queue

            Console.WriteLine("Executing Process ID:\t" + currentProcess.ProcessId + "\nArrival Time:\t" + currentProcess.ArrivalTime + "\nBurst Time:\t" + currentProcess.BurstTime + "\nExecution Time:\t" + time);
            ganttChart.Add(time + currentProcess.ArrivalTime, currentProcess.ProcessId); // Add the process ID to the Gantt Chart

            time += currentProcess.BurstTime; // Update the current time
            grantchart.Add(currentProcess); // Add the completed process to the list
                                            //Console.WriteLine($"|{currentProcess.ProcessId,-12}|{currentProcess.BurstTime,-12}|{(currentProcess.ArrivalTime - currentProcess.BurstTime),-15}|{(currentProcess.ArrivalTime),-18}|");
        }

        Console.WriteLine("--------------------------------");
        //foreach (var process in grantchart)
        //{
        //    Console.WriteLine("Process ID: " + process.ProcessId + ", Arrival Time: " + process.ArrivalTime + ", Burst Time: " + process.BurstTime);
        //}

        Console.WriteLine("Gantt Chart:");
        Console.WriteLine("--------------------------------");
        foreach (var entry in ganttChart)
        {
            Console.Write("[" + entry.Key + "-" + (entry.Key + 1) + "]: " + entry.Value + " ");
        }
        Console.WriteLine();
    }
}
// SJF scheduling algorithm class
class SJF
{
    public static void Execute(Queue<Process> readyQueue)
    {
        // Convert the ready queue to a list
        List<Process> sortedProcesses = readyQueue.ToList();

        // Sort the processes in the list based on burst time in ascending order
        sortedProcesses.Sort((p1, p2) => p1.BurstTime.CompareTo(p2.BurstTime));

        // Create a new queue to store the sorted processes
        Queue<Process> sortedQueue = new Queue<Process>(sortedProcesses);

        int currentTime = 0;  // Current time
        Console.WriteLine("\nSJF Scheduling Algorithm");
        Console.WriteLine("----------------------------");

        // Print Gantt Chart Header
        Console.WriteLine($"|{"Process ID\n",-12}|{"Burst Time\n",-12}|{"Waiting Time\n",-15}|{"Turnaround Time\n",-18}|");

        // Loop until all processes are executed
        while (sortedQueue.Count > 0)
        {
            Process currentProcess = sortedQueue.Dequeue(); // Get the next process from the sorted queue

            // Calculate waiting time for the executed process
            int waitingTime = currentTime - currentProcess.ArrivalTime;

            // Calculate turnaround time for the executed process
            int turnaroundTime = waitingTime + currentProcess.BurstTime;

            // Print Gantt Chart entry
            Console.WriteLine($"|{currentProcess.ProcessId,-12}|{currentProcess.BurstTime,-12}|{(currentTime - currentProcess.ArrivalTime - currentProcess.BurstTime),-15}|{(currentTime - currentProcess.ArrivalTime),-18}|");

            currentTime += currentProcess.BurstTime; // Update current time
        }

        Console.WriteLine("--------------------------");
    }
}
// Round Robin scheduling algorithm class
class RoundRobin
{
    public static void Execute(Queue<Process> readyQueue, int timeQuantum)
    {
        int currentTime = 0;  // Current time
        Console.WriteLine("\nRound Robin Scheduling Algorithm (Time Quantum: " + timeQuantum + ")");
        Console.WriteLine("----------------------------");

        // Print Gantt Chart Header
        Console.WriteLine($"|{"Process ID",-12}|{"Burst Time",-12}|{"Waiting Time",-15}|{"Turnaround Time",-18}|");

        Queue<Process> processQueue = new Queue<Process>(readyQueue); // Create a new queue to store the processes
        Queue<Process> executionQueue = new Queue<Process>(); // Create a new queue to store the executing processes

        while (processQueue.Count > 0 || executionQueue.Count > 0)
        {
            if (executionQueue.Count == 0)
            {
                Process nextProcess = processQueue.Dequeue(); // Get the next process from the process queue
                executionQueue.Enqueue(nextProcess); // Add the process to the execution queue
            }

            Process currentProcess = executionQueue.Dequeue(); // Get the next process from the execution queue

            int remainingBurstTime = currentProcess.BurstTime - timeQuantum; // Calculate remaining burst time

            if (remainingBurstTime > 0)
            {
                currentProcess.BurstTime = remainingBurstTime;
                currentTime += timeQuantum; // Update current time

                // Print Gantt Chart entry
                Console.WriteLine($"|{currentProcess.ProcessId,-12}|{timeQuantum,-12}|{(currentTime - currentProcess.ArrivalTime - timeQuantum),-15}|{(currentTime - currentProcess.ArrivalTime),-18}|");

                executionQueue.Enqueue(currentProcess); // Add the process back to the execution queue
            }
            else
            {
                currentTime += currentProcess.BurstTime; // Update current time

                // Print Gantt Chart entry
                Console.WriteLine($"|{currentProcess.ProcessId,-12}|{currentProcess.BurstTime,-12}|{(currentTime - currentProcess.ArrivalTime - currentProcess.BurstTime),-15}|{(currentTime - currentProcess.ArrivalTime),-18}|");
            }

            // Move processes from the process queue to the execution queue if their arrival time is less than or equal to the current time
            while (processQueue.Count > 0 && processQueue.Peek().ArrivalTime <= currentTime)
            {
                Process nextProcess = processQueue.Dequeue();
                executionQueue.Enqueue(nextProcess);
            }
        }

        Console.WriteLine("--------------------------");
    }
}
// Priority scheduling algorithm class
class PriorityScheduling
{
    public static void Execute(Queue<Process> readyQueue)
    {
        int currentTime = 0;  // Current time
        Console.WriteLine("\nPriority Scheduling Algorithm");
        Console.WriteLine("-----------------------------");

        // Print Gantt Chart Header
        Console.WriteLine($"|{"Process ID",-12}|{"Burst Time",-12}|{"Priority",-12}|{"Waiting Time",-15}|{"Turnaround Time",-18}|");

        Queue<Process> processQueue = new Queue<Process>(readyQueue); // Create a new queue to store the processes
        Queue<Process> executionQueue = new Queue<Process>(); // Create a new queue to store the executing processes

        while (processQueue.Count > 0 || executionQueue.Count > 0)
        {
            if (executionQueue.Count == 0)
            {
                Process nextProcess = processQueue.Dequeue(); // Get the next process from the process queue
                executionQueue.Enqueue(nextProcess); // Add the process to the execution queue
            }

            Process currentProcess = executionQueue.Dequeue(); // Get the next process from the execution queue

            int remainingBurstTime = currentProcess.BurstTime - 1; // Reduce burst time by 1

            if (remainingBurstTime > 0)
            {
                currentProcess.BurstTime = remainingBurstTime;
                currentTime += 1; // Update current time

                // Print Gantt Chart entry
                Console.WriteLine("|{currentProcess.ProcessId,-12}|{1,-12}|{currentProcess.Priority,-12}|{(currentTime - currentProcess.ArrivalTime - 1),-15}|{(currentTime - currentProcess.ArrivalTime),-18}|");

                executionQueue.Enqueue(currentProcess); // Add the process back to the execution queue
            }
            else
            {
                currentTime += currentProcess.BurstTime; // Update current time

                // Print Gantt Chart entry
                Console.WriteLine($"|{currentProcess.ProcessId,-12}|{currentProcess.BurstTime,-12}|{(currentTime - currentProcess.ArrivalTime - currentProcess.BurstTime),-15}|{(currentTime - currentProcess.ArrivalTime),-18}|");
            }

            // Move processes from the process queue to the execution queue if their arrival time is less than or equal to the current time
            while (processQueue.Count > 0 && processQueue.Peek().ArrivalTime <= currentTime)
            {
                Process nextProcess = processQueue.Dequeue();
                executionQueue.Enqueue(nextProcess);
            }

            // Sort the execution queue by priority after each time quantum
            executionQueue = new Queue<Process>(executionQueue.OrderBy(p => p.Priority));
        }

        Console.WriteLine("--------------------------");
    }


    static List<Process> processes = new List<Process>(); // List to store processes

    static void Main(string[] args)
    {
        while (true)
        {
            Console.ReadKey();
            Console.Clear();
            Console.WriteLine("1. Add Process");
            Console.WriteLine("2. Execute FCFS Scheduling");
            Console.WriteLine("3. Execute SJF Scheduling");
            Console.WriteLine("4. Execute Round Robin Scheduling");
            Console.WriteLine("5. Execute Priority Scheduling");
            Console.WriteLine("6. Exit");
            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddProcess();
                    break;
                case "2":
                    FCFS.Execute(new Queue<Process>(processes));
                    break;
                case "3":
                    SJF.Execute(new Queue<Process>(processes));
                    break;
                case "4":
                    Console.Write("Enter time quantum for Round Robin: ");
                    int timeQuantum = Convert.ToInt32(Console.ReadLine());
                    RoundRobin.Execute(new Queue<Process>(processes), timeQuantum);
                    break;
                case "5":
                    PriorityScheduling.Execute(new Queue<Process>(processes));
                    break;
                case "6":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid choice! Please try again.");
                    break;
            }
        }
    }

    static void AddProcess()
    {
        Console.Write("Enter Process ID: ");
        string processId = Console.ReadLine();
        Console.Write("Enter Arrival Time: ");
        int arrivalTime = Convert.ToInt32(Console.ReadLine());
        Console.Write("Enter Burst Time: ");
        int burstTime = Convert.ToInt32(Console.ReadLine());

        Process process = new Process(processId, arrivalTime, burstTime, 0); // Priority is set to 0 for simplicity
        processes.Add(process);
        Console.WriteLine("--------------------------------");
        Console.WriteLine($"Process {processId} added successfully!");
        Console.WriteLine("--------------------------------");
        Console.WriteLine("Press any key to continue");
        Console.ReadKey();
        Console.Clear();
    }

}