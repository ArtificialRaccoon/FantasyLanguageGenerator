# Fantasy Language Generator

A very simple fantasy language generator.  Language generation can be configured via "word_list.json".

### Configuration Parameters

- **Prefixes** - A list of prefixes in the source language.  For example, "over", "under", "anti", etc.

- **Suffixes** - A list of suffixes in the source language.  For example, "ing", "ize", "er", etc.

- **Roots** - A list of root words in the source language.  For example, "run", "dragon", "water". etc.

- **Vowels** - A list of characters you wish to define as vowels in your fantasy language.  The first five are considered the base/most common vowels.

- **Consonants** - A list of characters you wish to define as consonants in your fantasy language. 

- **SyllablePatterns** - A list of consonant and vowels patterns defined in your fantasy language.  Each has an associated weight for how common they should occour.

- **ConsonantPairWeights** - A list of consonant pairs you wish to permit in your fantasy language.  Each has a assocaited weight for how common they should occour.

### Usage

```
Usage:
  [no arguments]                     Generate a fantasy language JSON file named 'language.json'
  -g, --generate <file>              Generate a fantasy language JSON file at the specified path
  -t, --translate <input> <output>   Translate an English text file and save to the output file
  -h, --help                         Show this help information

Examples:
  FantasyLanguageGenerator                        Generate 'language.json' in the current directory
  FantasyLanguageGenerator -g output.json         Generate language file as 'output.json'
  FantasyLanguageGenerator -t language.json input.txt output.txt
                                                  Translate input.txt and save the result to output.txt
  FantasyLanguageGenerator -g output.json -t input.txt output.txt
                                                  Generate 'output.json' and translate input.txt to output.txt
```

## Example Input:

The dragon scorched the peoples homes, but the queen stood her ground. 
From the castle she raised her magic staff and a surge of light struck the beast. 
With a roar of pain the dragon fled, banished by the queen.


## Example Output:

Xu ecelef peveni xu mudraguyè emnuè, viwu xu moglxè busez dô senmuqv.
Quj xu uxqudi wëq xiëxso dô zajxé eqlo dé cu wok ke irloxf cîdipguf xu urmar.
Dah cu vùporv ke cavu xu ecelef peya, yexülkïi ut xu moglxè.
