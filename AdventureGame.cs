using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        Game.Start();
    }
}

class Game
{
    static int playerHealth = 100;
    static List<string> inventory = new List<string>();
    static string currentLocation = "prison_cell";
    static bool hasSolvedPuzzle = false;

    static void SaveGame()
    {
        using (StreamWriter writer = new StreamWriter("savegame.txt"))
        {
            writer.WriteLine(playerHealth);
            writer.WriteLine(string.Join(",", inventory));
            writer.WriteLine(currentLocation);
            writer.WriteLine(hasSolvedPuzzle);
        }
        Console.WriteLine("Game saved!");
    }

    static void LoadGame()
    {
        if (File.Exists("savegame.txt"))
        {
            string[] lines = File.ReadAllLines("savegame.txt");
            playerHealth = int.Parse(lines[0]);
            inventory = new List<string>(lines[1].Split(','));
            currentLocation = lines[2];
            hasSolvedPuzzle = bool.Parse(lines[3]);
            Console.WriteLine("Game loaded!");
        }
        else
        {
            Console.WriteLine("No saved game found.");
        }
    }

    public static void Start()
    {
        Console.WriteLine("Text Adventure: Escape the Abandoned Prison!");
        Console.WriteLine("Commands: /inventory, /save, /load, /quit");

        while (true)
        {
            DisplayCurrentLocation();

            Console.Write("Enter your choice: ");
            string input = Console.ReadLine().ToLower(); // Convert input to lowercase

            if (input.StartsWith("/"))
            {
                ProcessCommand(input);
            }
            else
            {
                ProcessDecision(input);
            }
        }
    }

    static void DisplayCurrentLocation()
    {
        Console.WriteLine($"Current Location: {currentLocation}");

        switch (currentLocation)
        {
            case "prison_cell":
                if (!hasSolvedPuzzle)
                {
                    Console.WriteLine("You wake up in a cold, dark prison cell. There is a rusty door to the north.");
                    Console.WriteLine("Choose a direction: north");
                }
                else
                {
                    Console.WriteLine("You are in a prison cell. There's a rusty door to the north.");
                    Console.WriteLine("Choose a direction: north");
                }
                break;
            case "corridor":
                Console.WriteLine("You are in a dimly lit corridor. To the east, you see a flickering light.");
                Console.WriteLine("Choose a direction: north, south, east");
                break;
            case "guard_room":
                Console.WriteLine("You enter a room filled with broken furniture. There's a guard slumped in a chair, snoring loudly.");
                Console.WriteLine("Choose an action: steal, sneak past");
                break;
            case "storage_room":
                Console.WriteLine("You find yourself in a cluttered storage room. Shelves are filled with various items.");
                Console.WriteLine("Choose an action: search, return to corridor");
                break;
            case "tunnel":
                Console.WriteLine("You discover a dark tunnel with a foot of water. It's cold, and your footsteps echo.");

                if (!hasSolvedPuzzle)
                {
                    Console.WriteLine("Puzzle: Answer the following riddles to reveal the secret passage.");
                    Console.WriteLine("1. What gets wetter the more it dries?");
                    Console.WriteLine("   a) Towel   b) Rain   c) Mountain");
                    string answer1 = Console.ReadLine().ToLower();
                    Console.WriteLine("2. What has a face and two hands, but no arms and legs?");
                    Console.WriteLine("   a) Statue   b) Clock   c) Person");
                    string answer2 = Console.ReadLine().ToLower();

                    if (answer1 == "a" && answer2 == "b")
                    {
                        Console.WriteLine("Congratulations! You solved the puzzle and revealed a secret passage.");
                        hasSolvedPuzzle = true;
                    }
                    else
                    {
                        Console.WriteLine("Incorrect answers! The secret passage remains hidden.");
                    }
                }

                if (hasSolvedPuzzle)
                {
                    Console.WriteLine("Choose an action: use waders, go back");
                }
                else
                {
                    Console.WriteLine("Choose an action: go back");
                }
                break;
            case "evidence_room":
                Console.WriteLine("You stumble upon a room filled with files and documents. There's evidence against the criminals.");
                Console.WriteLine("Choose an action: collect evidence, leave");
                break;
            case "baton_room":
                Console.WriteLine("You find a room with a baton hanging on the wall. It might come in handy to knock out a guard.");
                Console.WriteLine("Choose an action: take baton, leave");
                break;
            case "exit":
                Console.WriteLine("You reach the exit! Freedom is just a few steps away.");
                Console.WriteLine("Choose an action: open the door");
                break;
            default:
                Console.WriteLine("Invalid location.");
                break;
        }
    }

    static void ProcessDecision(string decision)
    {
        switch (currentLocation)
        {
            case "prison_cell":
                if (!hasSolvedPuzzle)
                {
                    ProcessPuzzle(decision);
                }
                else if (decision == "north")
                {
                    Console.WriteLine("You escape the prison cell and enter a dimly lit corridor.");
                    currentLocation = "corridor";
                }
                else
                {
                    Console.WriteLine("Invalid choice.");
                }
                break;
            case "corridor":
                if (decision == "north")
                {
                    Console.WriteLine("You find yourself in a guard room. The guard seems to be asleep.");
                    currentLocation = "guard_room";
                }
                else if (decision == "south")
                {
                    Console.WriteLine("You return to your prison cell. Better find another way.");
                    currentLocation = "prison_cell";
                }
                else if (decision == "east")
                {
                    Console.WriteLine("You see a storage room to the east.");
                    currentLocation = "storage_room";
                }
                else
                {
                    Console.WriteLine("Invalid choice.");
                }
                break;
            case "guard_room":
                if (decision == "steal")
                {
                    Console.WriteLine("You silently steal the guard's keys. Good move!");
                    inventory.Add("prison_keys");
                    Console.WriteLine("Now, choose a direction: south");
                }
                else if (decision == "sneak past")
                {
                    Console.WriteLine("You successfully sneak past the snoring guard.");
                    currentLocation = "corridor";
                }
                else
                {
                    Console.WriteLine("Invalid choice.");
                }
                break;
            case "storage_room":
                if (decision == "search")
                {
                    Console.WriteLine("You find a flashlight in the storage room.");
                    inventory.Add("flashlight");
                    Console.WriteLine("Now, choose a direction: west");
                }
                else if (decision == "return to corridor")
                {
                    Console.WriteLine("You return to the dimly lit corridor.");
                    currentLocation = "corridor";
                }
                else
                {
                    Console.WriteLine("Invalid choice.");
                }
                break;
            case "tunnel":
                if (decision == "use waders" && inventory.Contains("waders"))
                {
                    Console.WriteLine("You put on the waders and successfully navigate the cold tunnel.");
                    currentLocation = "evidence_room";
                }
                else if (decision == "use waders")
                {
                    Console.WriteLine("You don't have waders! The water is freezing, and you get hypothermia in your feet.");
                    playerHealth -= 50;
                    Console.WriteLine($"Health decreased. Current health: {playerHealth}");
                    Console.WriteLine("You must turn back!");
                    currentLocation = "corridor"; // Go back to the corridor
                }
                else if (decision == "go back")
                {
                    Console.WriteLine("You go back to the dimly lit corridor.");
                    currentLocation = "corridor";
                }
                else if (decision == "go forward" && inventory.Contains("waders"))
                {
                    Console.WriteLine("You move forward in the tunnel.");
                    currentLocation = "evidence_room";
                }
                else if (decision == "go forward")
                {
                    Console.WriteLine("You try to move forward, but without waders, the cold water is unbearable.");
                    playerHealth -= 50;
                    Console.WriteLine($"Health decreased. Current health: {playerHealth}");
                    Console.WriteLine("You must turn back!");
                    currentLocation = "corridor"; // Go back to the corridor
                }
                else
                {
                    Console.WriteLine("Invalid choice.");
                }
                break;
            case "evidence_room":
                if (decision == "collect evidence" && inventory.Contains("evidence"))
                {
                    Console.WriteLine("You already collected the evidence. No need to do it again.");
                }
                else if (decision == "collect evidence")
                {
                    Console.WriteLine("You collect the crucial evidence against the criminals.");
                    inventory.Add("evidence");
                    Console.WriteLine("Now, choose a direction: west");
                }
                else if (decision == "leave")
                {
                    Console.WriteLine("You leave the evidence room.");
                    currentLocation = "tunnel";
                }
                else
                {
                    Console.WriteLine("Invalid choice.");
                }
                break;
            case "baton_room":
                if (decision == "take baton" && inventory.Contains("baton"))
                {
                    Console.WriteLine("You already took the baton. No need to do it again.");
                }
                else if (decision == "take baton")
                {
                    Console.WriteLine("You take the baton. It might come in handy to knock out a guard.");
                    inventory.Add("baton");
                    Console.WriteLine("Now, choose a direction: west");
                }
                else if (decision == "leave")
                {
                    Console.WriteLine("You leave the baton room.");
                    currentLocation = "tunnel";
                }
                else
                {
                    Console.WriteLine("Invalid choice.");
                }
                break;
            case "exit":
                if (decision == "open the door" && inventory.Contains("evidence"))
                {
                    Console.WriteLine("You open the exit door and step into the moonlight. Congratulations, you've escaped!");
                    Console.WriteLine("When arriving at the nearest police station, you reported the kidnappers.");
                    Console.WriteLine("A few days later, they all were arrested.");
                    Console.WriteLine("You provided the crucial evidence, ensuring their conviction. Well done!");
                    Environment.Exit(0);
                }
                else if (decision == "open the door" && !inventory.Contains("evidence"))
                {
                    Console.WriteLine("You open the exit door and step into the moonlight. Congratulations, you've escaped!");
                    Console.WriteLine("When arriving at the nearest police station, you reported the kidnappers.");
                    Console.WriteLine("However, without evidence, the criminals are never arrested.");
                    Console.WriteLine("You managed to escape, but justice remains elusive. The end.");
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine("Invalid choice.");
                }
                break;
            default:
                Console.WriteLine("Invalid location.");
                break;
        }
    }

    static void ProcessCommand(string command)
    {
        switch (command)
        {
            case "/inventory":
                Console.WriteLine("Inventory: " + string.Join(", ", inventory));
                break;
            case "/save":
                SaveGame();
                break;
            case "/load":
                LoadGame();
                break;
            case "/quit":
                Console.WriteLine("Goodbye!");
                Environment.Exit(0);
                break;
            default:
                Console.WriteLine("Invalid command.");
                break;
        }
    }
}