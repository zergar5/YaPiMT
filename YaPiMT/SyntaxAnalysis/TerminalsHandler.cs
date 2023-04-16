using YaPiMT.Core;

namespace YaPiMT.SyntaxAnalysis;

public class TerminalsHandler
{
    private readonly List<string> _notTerminals;
    private int _ruleNumber;

    public TerminalsHandler()
    {
        _notTerminals = new List<string>
        {
            "program",
            "eps",
            "initialize",
            "assignment",
            "type",
            "id",
            "equals",
            "nextId",
            "expr",
            "operation",
            "nextOperation",
            "operator",
            "assign"
        };
    }

    public bool HandleNonterminal(Token token, string nonterminal)
    {
        if (nonterminal == "program")
        {
            if (_ruleNumber == 0)
            {
                _ruleNumber = 1;
                return true;
            }
            if (_ruleNumber == 1 && IsType(token))
            {
                _ruleNumber = 0;
                return true;
            }
            if (_ruleNumber == 2 && IsIdentifier(token))
            {
                _ruleNumber = 0;
                return true;
            }
            if (_ruleNumber == 3)
            {
                _ruleNumber = 0;
                return true;
            }

            _ruleNumber++;
        }
        else if (nonterminal == "initialize" && IsType(token)) return true;
        else if (nonterminal == "assignment" && IsIdentifier(token)) return true;
        else if (nonterminal == "type" && IsType(token)) return true;
        else if (nonterminal == "id" && IsIdentifier(token)) return true;
        else if (nonterminal == "nextId")
        {
            if (_ruleNumber == 0)
            {
                _ruleNumber = 1;
                return true;
            }
            if (_ruleNumber == 1 && token.Name == ",")
            {
                _ruleNumber = 0;
                return true;
            }
            if (_ruleNumber == 2)
            {
                _ruleNumber = 0;
                return true;
            }

            _ruleNumber++;
        }
        else if (nonterminal == "equals")
        {
            if (_ruleNumber == 0)
            {
                _ruleNumber = 1;
                return true;
            }
            if (_ruleNumber == 1 && token.Name == "=")
            {
                _ruleNumber = 0;
                return true;
            }
            if (_ruleNumber == 2)
            {
                _ruleNumber = 0;
                return true;
            }

            _ruleNumber++;
        }
        else if (nonterminal == "expr" && (IsIdentifier(token) || IsConst(token))) return true;
        else if (nonterminal == "operation")
        {
            if (_ruleNumber == 0)
            {
                _ruleNumber = 1;
                return true;
            }
            if (_ruleNumber == 1 && IsOperator(token))
            {
                _ruleNumber = 0;
                return true;
            }

            if (_ruleNumber == 2)
            {
                _ruleNumber = 0;
                return true;
            }

            _ruleNumber++;
        }
        else if (nonterminal == "operator" && IsOperator(token)) return true;
        else if (nonterminal == "nextOperation")
        {
            if (_ruleNumber == 0)
            {
                _ruleNumber = 1;
                return true;
            }
            if (_ruleNumber == 1 && (IsIdentifier(token) || IsConst(token)))
            {
                _ruleNumber = 0;
                return true;
            }

            if (_ruleNumber == 2 && token.Name == "(")
            {
                _ruleNumber = 0;
                return true;
            }

            _ruleNumber++;
        }
        else if (nonterminal == "assign" && IsAssign(token)) return true;
        else if (nonterminal == "eps") return true;

        return false;
    }

    public bool HandleTerminal(Token token, string terminal)
    {
        if (terminal == "identifier" && IsIdentifier(token)) return true;
        if (terminal == "const" && IsConst(token)) return true;
        if (terminal == "operator" && IsOperator(token)) return true;
        if (terminal == "assign" && IsAssign(token)) return true;
        if (token.Name == terminal) return true;

        return false;
    }

    public bool IsNonterminal(string terminal)
    {
        return _notTerminals.Contains(terminal);
    }

    public bool IsType(Token token)
    {
        return token.Name is "int" or "float" or "char";
    }

    public bool IsIdentifier(Token token)
    {
        return token.TableName is "IdentifiersTable";
    }

    public bool IsConst(Token token)
    {
        return token.TableName is "ConstantsTable";
    }

    public bool IsOperator(Token token)
    {
        return (token.TableName == "OperationsTable1" && token.Name != "=") ||
               token is { TableName: "OperationsTable2", Name: "==" or "!=" };
    }

    public bool IsAssign(Token token)
    {
        return token.Name == "=" || token is { TableName: "OperationsTable2", Name: not ("==" and "!=") };
    }
}