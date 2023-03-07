using System.Text;
using System.Text.RegularExpressions;

namespace gamification_backend.Stub;

public class StubParser
{
    public enum TokenType
    {
        String,
        Separator,
        Newline,
        Number,
        Undefined
    }

    private readonly Queue<Token> _tokensList;

    public StubParser()
    {
        _tokensList = new Queue<Token>();
    }

    public void Parse(string input)
    {
        foreach (var line in input.Split("\n"))
        {
            foreach (var word in line.Split(" "))
            {
                var type = GetTokenType(word);
                if (type == TokenType.Separator)
                {
                    var parts = word.Split(":");
                    _tokensList.Enqueue(new Token(GetTokenType(parts[0]), parts[0]));
                    _tokensList.Enqueue(new Token(TokenType.Separator, ":"));
                    _tokensList.Enqueue(new Token(GetTokenType(parts[1]), parts[1]));
                    continue;
                }

                _tokensList.Enqueue(new Token(GetTokenType(word), word));
            }

            _tokensList.Enqueue(new Token(TokenType.Newline, "\n"));
        }
    }

    private static TokenType GetTokenType(string word)
    {
        return word switch
        {
            _ when new Regex("^[a-zA-ZæøåÆØÅ]*$").IsMatch(word) => TokenType.String,
            _ when new Regex("^[\\d]*$").IsMatch(word) => TokenType.Number,
            _ when new Regex("^[\\w*:\\w*]*$").IsMatch(word) => TokenType.Separator,
            _ => throw new Exception($"Can't parse {word}")
        };
    }

    public void ConsumeOne()
    {
        if (Done())
            throw new IndexOutOfRangeException("The token list is empty");
        _tokensList.Dequeue();
    }

    public Token Next()
    {
        if (Done())
            throw new IndexOutOfRangeException("The token list is empty");
        return _tokensList.Dequeue();
    }

    public bool Done()
    {
        return _tokensList.Count == 0;
    }

    public bool NextIsSeparator()
    {
        if (Done())
            return false;
        return _tokensList.Peek().Type == TokenType.Separator;
    }

    public bool NextIsNewLine()
    {
        if (Done())
            return false;
        return _tokensList.Peek().Type == TokenType.Newline;
    }

    public bool NextIsNumber()
    {
        if (Done())
            return false;
        return _tokensList.Peek().Type == TokenType.Number;
    }

    public string RestOfLineAsString()
    {
        StringBuilder sb = new();
        while (!_tokensList.Peek().IsNewLine())
        {
            sb.Append(Next().Value);
            if (!_tokensList.Peek().IsNewLine())
                sb.Append(' ');
        }

        ConsumeOne();
        return sb.ToString();
    }

    public readonly struct Token
    {
        public TokenType Type { get; }
        public string Value { get; }

        public Token(TokenType type, string value)
        {
            Type = type;
            Value = value;
        }

        public bool IsNewLine()
        {
            return Type == TokenType.Newline;
        }

        public bool IsSeparator()
        {
            return Type == TokenType.Separator;
        }

        public bool IsString()
        {
            return Type == TokenType.String;
        }

        public bool IsNumber()
        {
            return Type == TokenType.Number;
        }
    }
}