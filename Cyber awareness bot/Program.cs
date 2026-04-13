using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Speech.Synthesis;
using static System.Console;

namespace Cyber_awareness_bot
{
    internal class Program
    {
        
        private static readonly string CyberSecurityLogo = @"
   _____           
  / ____|         
 | |    _   _ ___   
 | |   | | | / __|
 | |___| |_| \__ \
  \____|\__, |___/ 
         __/ |
        |___/  AWARENESS  
                            BOT
";

        private static readonly string Border = "=================================================";


            public static void Main(string[] args)
            {
           

                Title = "Cybersecurity Awareness Bot"; 

                
                DisplayAsciiLogo();
                
                PlayVoiceGreeting();

                
                string userName = GetUserName();
                DisplayTextGreeting(userName);

               
                WriteLine("\n Ready to answer your cybersecurity questions, " + userName + ".");
                WriteLine("exit the application.");
                ReadKey();
            }

            private static void DisplayAsciiLogo()
            {
               ForegroundColor = ConsoleColor.Red; 
               WriteLine(CyberSecurityLogo);
               ForegroundColor = ConsoleColor.White; 
               WriteLine("\nLoading Bot...");
            }

            private static void PlayVoiceGreeting()
            {
                string audioFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "welcome greeting");

                if (File.Exists(audioFilePath))
                {
                    try
                    {
                        SoundPlayer player = new SoundPlayer(audioFilePath);
                        player.PlaySync();
                        WriteLine("\n(Voice greeting played)");
                    }
                    catch (Exception ex)
                    {
                        WriteLine($"\nError playing voice greeting: {ex.Message}");
                        WriteLine("Ensure 'welcome greeting' is in the application directory.");
                    }
                }
                else
                {
                    WriteLine("\n(Voice greeting file 'welcome greeting' not found.)");
                    WriteLine("Ensure the WAV file is in the same directory as the executable.");
                }
            }

            private static string GetUserName()
            {
                WriteLine("\n" + Border);
               Write("Before we start, what's your name? ");
                string name = ReadLine();
                
                if (string.IsNullOrWhiteSpace(name))
                {
                    return "User"; 
                }
                return name;
            }

            private static void DisplayTextGreeting(string name)
            {
         
                WriteLine(Border);
                WriteLine($"Hello, {name}! Welcome to the Cybersecurity Awareness Bot.");
               WriteLine("I'm here to help you stay safe online by answering your questions.");
                WriteLine(Border);
            }
        }

    }


