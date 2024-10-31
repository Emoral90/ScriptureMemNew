using System;
using System.Collections.Generic;

class Word
{
    private string _text;
    private bool _isHidden;

    public Word(string text)
    {
        _text = text;
        _isHidden = false;
    }

    public void Hide() => _isHidden = true;
    public void Show() => _isHidden = false;
    public bool IsHidden() => _isHidden;

    public string GetDisplayText()
    {
        return _isHidden ? new string('_', _text.Length) : _text;
    }

    public string GetPartialDisplayText()
    {
        if (_isHidden)
            return new string('_', _text.Length);
        int visiblePart = Math.Max(1, _text.Length / 2);
        return _text.Substring(0, visiblePart) + new string('_', _text.Length - visiblePart);
    }
}

class Reference
{
    private string _book;
    private int _chapter;
    private int _verse;
    private int _endVerse;

    public Reference(string book, int chapter, int verse)
    {
        _book = book;
        _chapter = chapter;
        _verse = verse;
        _endVerse = verse;
    }

    public Reference(string book, int chapter, int startVerse, int endVerse)
    {
        _book = book;
        _chapter = chapter;
        _verse = startVerse;
        _endVerse = endVerse;
    }

    public string GetDisplayText()
    {
        return _endVerse == _verse
            ? $"{_book} {_chapter}:{_verse}"
            : $"{_book} {_chapter}:{_verse}-{_endVerse}";
    }
}

class Scripture
{
    private Reference _reference;
    private List<Word> _words;

    public Scripture(Reference reference, string text)
{
        _reference = reference;
        _words = text.Split(' ').Select(word => new Word(word)).ToList();
    }

    public void HideRandomWords(int numberToHide)
    {
        var random = new Random();
        for (int i = 0; i < numberToHide; i++)
        {
            int index;
            do
            {
                index = random.Next(_words.Count);
            }
            // Check to hide still visible words
            while (_words[index].IsHidden());

            _words[index].Hide();
        }
    }

    public string GetDisplayText(bool partial = false)
    {
        var wordsText = partial 
            ? _words.Select(word => word.GetPartialDisplayText()) 
            : _words.Select(word => word.GetDisplayText());
        return $"{_reference.GetDisplayText()} " + string.Join(" ", wordsText);    
    }

    public bool IsCompletelyHidden()
    {
        return _words.TrueForAll(word => word.IsHidden());
    }

    public void RevealAllWords() => _words.ForEach(word => word.Show());
}

class ScriptureLibrary
{
    private List<Scripture> _scriptures = new List<Scripture>();

    public void LoadFromFile(string filePath)
    {
        foreach (var line in File.ReadAllLines(filePath))
        {
            var parts = line.Split(" - ");
            var referenceParts = parts[0].Split(new char[] { ' ', ':', '-' });
            string book = referenceParts[0];
            int chapter = int.Parse(referenceParts[1]);
            int startVerse = int.Parse(referenceParts[2]);
            int endVerse = referenceParts.Length > 3 ? int.Parse(referenceParts[3]) : startVerse;

            var reference = endVerse > startVerse 
                ? new Reference(book, chapter, startVerse, endVerse) 
                : new Reference(book, chapter, startVerse);
            _scriptures.Add(new Scripture(reference, parts[1]));
        }
    }

    public Scripture GetRandomScripture()
    {
        var random = new Random();
        return _scriptures[random.Next(_scriptures.Count)];
    }
}