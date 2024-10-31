class Program
{
    static void Main()
    {
        var scriptureLibrary = new ScriptureLibrary();
        scriptureLibrary.LoadFromFile("scriptures.txt");

        var scripture = scriptureLibrary.GetRandomScripture();
        bool partialHiding = true;

        while (true)
        {
            Console.Clear();
            Console.WriteLine(scripture.GetDisplayText(partialHiding));
            Console.WriteLine("\nPress Enter to hide words, type 'hint' for a hint, or type 'quit' to exit.");
            var input = Console.ReadLine()?.ToLower();

            if (input == "quit" || scripture.IsCompletelyHidden())
                break;
            
            else if (input == "hint")
            {
                scripture.RevealAllWords();
                Console.Clear();
                Console.WriteLine(scripture.GetDisplayText());
                Console.WriteLine("\nPress Enter to continue hiding words.");
                Console.ReadLine();
                partialHiding = false;
            }
            else
            {
                scripture.HideRandomWords(3);
                partialHiding = !partialHiding;
            }
        }

        Console.WriteLine("All words are hidden. Program has ended.");
    }
}