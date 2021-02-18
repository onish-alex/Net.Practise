namespace ConsoleFileManager
{
    using System;
    using System.Collections.Generic;

    public class CommandHandler
    {
        private Dictionary<string, Action> commands;
        private IEnumerable<string> currentContent;
        private bool isExit;
        private IManager manager;

        public CommandHandler(IManager manager)
        {
            this.manager = manager;
            this.commands = new Dictionary<string, Action>
            {
                { ":back", () => this.currentContent = manager.GetPreviousData() },
                { ":exit", () => this.isExit = true },
            };
        }

        public void Run()
        {
            this.currentContent = this.manager.GetInitialData();
            bool isError = false;
            var input = string.Empty;
            while (!this.isExit)
            {
                Console.Clear();

                // error output
                if (isError)
                {
                    Console.WriteLine("<Error: there's no such file or directory \"{0}\">\n", input);
                    isError = false;
                }

                // content output
                foreach (var item in this.currentContent)
                {
                    Console.WriteLine(item);
                }

                // delimiter
                for (int i = 0; i < 20; i++)
                {
                    Console.Write('-');
                }

                Console.Write("\n>");
                input = Console.ReadLine();

                if (this.commands.ContainsKey(input))
                {
                    this.commands[input].Invoke();
                }
                else
                {
                    var updatedContent = this.manager.GetData(input);
                    if (updatedContent != null)
                    {
                        this.currentContent = updatedContent;
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
