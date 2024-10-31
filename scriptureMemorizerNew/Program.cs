class Program
{
    static void Main()
    {
        var reference = new Reference("John", 3, 16);
        var scripture = new Scripture(reference, "For God so loved the world that he gave his one and only Son");

        while (true)
        {
            Console.Clear();
            Console.WriteLine(scripture.GetDisplayText());
            Console.WriteLine("\nPress Enter to hide words, or type 'quit' to exit.");
            var input = Console.ReadLine();
            
            if (input.ToLower() == "quit" || scripture.IsCompletelyHidden())
                break;

            scripture.HideRandomWords(3);  // Hide 3 words at a time
        }

        Console.WriteLine("All words are hidden. Program has ended.");
    }
}