using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleFileManager
{
    public class CommandHandler
    {
        private Dictionary<string, Action> commands;
        private IEnumerable<string> currentContent;
        private bool isExit;
        private IManager manager;

        public CommandHandler(IManager manager)
        {
            this.manager = manager;
            commands = new Dictionary<string, Action>();
            commands.Add(":back", () => currentContent = manager.GetPreviousData());
            commands.Add(":exit", () => isExit = true);
        }

        public void Run()
        {
            currentContent = manager.GetInitialData();
            bool isError = false;
            var input = string.Empty;
            while (!isExit)
            {
                Console.Clear();

                //error output
                if (isError)
                {
                    Console.WriteLine("<Error: there's no such file or directory \"{0}\">\n", input);
                    isError = false;
                }

                //content output
                foreach (var item in currentContent)
                {
                    Console.WriteLine(item);
                }

                //delimiter
                for (int i = 0; i < 20; i++)
                {
                    Console.Write('-');
                }

                Console.Write("\n>");
                input = Console.ReadLine();

                if (commands.ContainsKey(input))
                {
                    commands[input].Invoke();
                }
                else
                {
                    var updatedContent = manager.GetData(input);
                    if (updatedContent != null)
                    {
                        currentContent = updatedContent;
                    }
                    else
                    {
                        isError = true;
                    }
                }
            }
        }
    }
}
