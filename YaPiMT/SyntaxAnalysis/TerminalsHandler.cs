using YaPiMT.Core;

namespace YaPiMT.SyntaxAnalysis;

public class TerminalsHandler
{
    public bool HandleTerminals(Token token, string[] terminals)
    {
        if(terminals.Any(t => t == token.Name)) return true;
        if(terminals.Any(t => t == "id") && IsIdentifier(token)) return true;
        if(terminals.Any(t => t == "const") && IsConst(token)) return true;

        return false;
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