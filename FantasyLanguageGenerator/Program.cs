namespace FantasyLanguageGenerator;

class Program
{
    static void Main(string[] args)
    {
        //Default output file path
        string outputFilePath = "language.json";

        ShowTitle();
        if(args.Length == 0)
        {            
            Generator.GenerateLanguage(outputFilePath);
            Console.WriteLine("No input parameters specified; using defaults.");
            Console.WriteLine("Fantasy translations saved successfully.");
            Console.WriteLine();            
            return;
        }

        for (int i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "--generate":
                case "-g":
                    if (i + 1 < args.Length)
                    {
                        outputFilePath = args[i + 1];
                        i++;
                    }
                    Generator.GenerateLanguage(outputFilePath);    
                    Console.WriteLine("Fantasy translations saved successfully.");
                    Console.WriteLine();           
                    break;

                case "--translate":
                case "-t":
                    if (i + 3 < args.Length)
                    {
                        string languagefile = args[i + 1];
                        i++;
                        string sourceTextfile = args[i + 1];
                        i++;
                        string outputTextfile = args[i + 1];
                        i++;

                        Language.LoadLanguage(languagefile);
                        string translation = Translator.Translate(File.ReadAllText(sourceTextfile));
                        Console.WriteLine(string.Format("Translation:{0}{1}{0}", Environment.NewLine, translation));
                        File.WriteAllText(outputTextfile, translation);

                        if(Language.UntranslatedWords.Count > 0)
                        {
                            File.AppendAllText(outputTextfile, Environment.NewLine + "Untranslated Words:" + Environment.NewLine);
                            File.AppendAllText(outputTextfile, Environment.NewLine + "[");                            
                            File.AppendAllText(outputTextfile, string.Join(",", Language.UntranslatedWords.Select(word => string.Format("\"{0}\"", word))));
                            File.AppendAllText(outputTextfile, "]");
                        }

                        Console.WriteLine(string.Format("Translation saved to {0}", outputTextfile));
                    }
                    else
                        Console.WriteLine("Error: No text file provided for translation.");
                        Environment.Exit(1); 
                    break; 

                case "--help":
                case "-h":
                    ShowUsage();
                    break;

                default:
                    Console.WriteLine(string.Format("Unknown command: {0}", args[i]));
                    ShowUsage();
                    break;
            }
        }  
    }

    static void ShowUsage()
    {        
        Console.WriteLine("Usage:");
        Console.WriteLine("  [no arguments]                     Generate a fantasy language JSON file named 'language.json'");
        Console.WriteLine("  -g, --generate <file>              Generate a fantasy language JSON file at the specified path");
        Console.WriteLine("  -t, --translate <input> <output>   Translate an English text file and save to the output file");
        Console.WriteLine("  -h, --help                         Show this help information");

        Console.WriteLine();
        Console.WriteLine("Examples:");
        Console.WriteLine("  FantasyLanguageGenerator                        Generate 'language.json' in the current directory");
        Console.WriteLine("  FantasyLanguageGenerator -g output.json         Generate language file as 'output.json'");
        Console.WriteLine("  FantasyLanguageGenerator -t language.json input.txt output.txt");
        Console.WriteLine("                                                  Translate input.txt and save the result to output.txt");
        Console.WriteLine("  FantasyLanguageGenerator -g output.json -t output.json input.txt output.txt");
        Console.WriteLine("                                                  Generate 'output.json' and translate input.txt to output.txt");
        Console.WriteLine(); 
    }

    static void ShowTitle()
    {
        Console.WriteLine();
        Console.WriteLine("Fantasy Language Generator v1.0.0 - April 2025");
        Console.WriteLine();
    }
}