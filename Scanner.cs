namespace LoxInterperter1
{
    public class Scanner
    {
        private string source;
        private List<Token> tokens = new();
        private int start = 0;
        private int current = 0;
        private int line = 1;
        public Scanner(string source)
        {
            this.source = source;
        }

        internal List<Token> ScanTokens()
        {
            while (!IsAtEnd())
            {
                start = current;
                ScanToken();
            }

            tokens.Add(new Token(TokenType.EOF, string.Empty, null, line));
            return tokens;
        }

        private void ScanToken()
        {
            char c = Advance();
            switch (c)
            {
                case '(': AddToken(TokenType.LEFT_PAREN); break;
                case ')': AddToken(TokenType.RIGHT_PAREN); break;
                case '{': AddToken(TokenType.LEFT_BRACE); break;
                case '}': AddToken(TokenType.RIGHT_BRACE); break;
                case ',': AddToken(TokenType.COMMA); break;
                case '.': AddToken(TokenType.DOT); break;
                case '-': AddToken(TokenType.MINUS); break;
                case '+': AddToken(TokenType.PLUS); break;
                case ';': AddToken(TokenType.SEMICOLON); break;
                case '*': AddToken(TokenType.STAR); break;

                case '!':
                    {
                        AddToken(Match('=') ? TokenType.BANG_EQUAL : TokenType.BANG);
                        break;
                    }
                case '=':
                    {
                        AddToken(Match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL);
                        break;
                    }
                case '<':
                    {
                        AddToken(Match('=') ? TokenType.LESS_EQUAL : TokenType.LESS);
                        break;
                    }
                case '>':
                    {
                        AddToken(Match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER);
                        break;
                    }
                case '/':
                    {
                        if (Match('/'))
                        {
                            while (Peek() != '\n' && !IsAtEnd())
                                Advance();
                        }
                        else
                        {
                            AddToken(TokenType.SLASH);
                        }
                        break;
                    }

                case ' ':
                case '\r':
                case '\t':
                    // Ignore whitespace.
                    break;

                case '\n':
                    line++;
                    break;
                case '"':
                    {
                        ParseString();
                        break;
                    }
                default:
                    if (IsDigit(c))
                    {
                        ParseNumber();
                    }
                    else
                    {
                        Program.Error(line, "Unexpected character.");
                    }
                    break;
            }
        }

        private void ParseNumber()
        {
            throw new NotImplementedException();
        }

        private bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        private void ParseString()
        {
            while (Peek() != '"' && !IsAtEnd())
            {
                if (Peek() == '\n') line++;
                Advance();
            }

            if (IsAtEnd())
            {
                Program.Error(line, "Unterminated string.");
                return;
            }

            // The Closing ".
            Advance();

            var value = source.Substring(start + 1, current - 1);
            AddToken(TokenType.STRING, value);
        }
        private char Peek()
        {
            if (IsAtEnd())
            {
                return '\0';
            }
            return source.ElementAt(current);
        }

        private bool Match(char expected)
        {
            if (IsAtEnd())
                return false;

            if (expected != source.ElementAt(current))
            {
                return false;
            }

            current++;
            return true;
        }

        private void AddToken(TokenType type, object? literal = null)
        {
            string text = source.Substring(start, current);
            tokens.Add(new Token(type, text, literal, line));
        }

        private char Advance()
        {
            return source.ElementAt(current++);
        }

        private bool IsAtEnd()
        {
            return current >= source.Length;
        }
    }
}