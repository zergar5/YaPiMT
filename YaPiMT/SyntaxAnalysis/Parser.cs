using YaPiMT.Core;
using YaPiMT.Core.Errors;
using YaPiMT.Core.Errors.SyntaxErrors;
using YaPiMT.Core.Tables;
using YaPiMT.Core.Tables.ConstTables;

namespace YaPiMT.SyntaxAnalysis;

public class Parser
{
    public readonly CharConstTable AllowedCharactersTable;
    public readonly CharConstTable NumbersTable;
    public readonly CharConstTable SeparatorsTable;
    public readonly CharConstTable OperationsTable1;
    public readonly StringConstTable OperationsTable2;
    public readonly StringConstTable KeywordsTable;
    public readonly LLTable LlTable;
    public readonly TerminalsHandler TerminalsHandler;

    public Parser(
        CharConstTable allowedCharactersTable,
        CharConstTable numbersTable,
        CharConstTable separatorsTable,
        CharConstTable operationsTable1,
        StringConstTable operationsTable2,
        StringConstTable keywordsTable,
        LLTable llTable
    )
    {
        AllowedCharactersTable = allowedCharactersTable;
        NumbersTable = numbersTable;
        SeparatorsTable = separatorsTable;
        OperationsTable1 = operationsTable1;
        OperationsTable2 = operationsTable2;
        KeywordsTable = keywordsTable;
        LlTable = llTable;
        TerminalsHandler = new TerminalsHandler();
    }

    public string Parse(List<Token> tokens, VariableTable identifiersTable,
        VariableTable constantsTable, out List<IError> errors)
    {
        errors = new List<IError>();

        var rpnBuilder = new RpnBuilder();

        var currentState = LlTable.CurrentState;

        var type = DataType.Undefined;
        var init = false;

        var token = tokens[0];
        var prevToken = tokens[0];

        for (var i = 0; i < tokens.Count && LlTable.CurrentStateNumber != -1;)
        {
            var terminals = currentState.Terminals.Split(' ');

            var containedInTerminals = TerminalsHandler.HandleTerminals(token, terminals);

            if (containedInTerminals)
            {
                if (TerminalsHandler.IsAssign(token)) init = true;

                switch (init)
                {
                    case true when prevToken.TableName == "IdentifiersTable":
                        UpdateIdentifierIsInit(identifiersTable, prevToken, errors);
                        init = false;
                        if (token.Name == ";") type = DataType.Undefined;
                        break;
                    case true when prevToken.TableName == "ConstantsTable":
                        errors.Add(new CannotAssignToRvalueError(token.Name));
                        init = false;
                        break;
                }

                if (TerminalsHandler.IsType(token))
                {
                    type = token.Name switch
                    {
                        "int" => DataType.Int,
                        "float" => DataType.Float,
                        _ => DataType.Char
                    };
                }
                else if (token.Name == ";") type = DataType.Undefined;

                if (token.TableName == "IdentifiersTable" && type != DataType.Undefined)
                    UpdateIdentifierType(identifiersTable, token, type, errors);

                if (TerminalsHandler.IsIdentifier(token) &&
                    identifiersTable.GetLexemeType(token.IndexInTable) == DataType.Undefined &&
                    currentState.Accept &&
                    (TerminalsHandler.IsOperator(prevToken) || TerminalsHandler.IsAssign(prevToken) ||
                     prevToken.Name == "("))
                    errors.Add(new UndeclaredIdentifierError(token.Name));

                if (token.TableName != "KeywordsTable" && currentState.Accept)
                {
                    try
                    {
                        rpnBuilder.Append(token.Name);
                    }
                    catch (Exception)
                    {
                        errors.Add(new ExpectedBracketError(token.Name));
                        break;
                    }
                }
            }
            else if(currentState.Error)
            {
                errors.Add(new CannotResolveSymbolError(token.Name));
                errors.Add(new ExpectedError(currentState.Terminals));
                break;
            }

            if (currentState.Accept)
            {
                prevToken = token;
                i++;
                if (i < tokens.Count) token = tokens[i];
            }

            currentState = LlTable.MoveNext(containedInTerminals);
        }

        return rpnBuilder.ToString();
    }

    private void UpdateIdentifierType(VariableTable identifiersTable, Token token, DataType type, List<IError> errors)
    {
        if ((identifiersTable.GetLexemeType(token.IndexInTable) == DataType.Undefined ||
             identifiersTable.GetLexemeType(token.IndexInTable) == type))
        {
            identifiersTable.SetLexemeType(token.IndexInTable, type);
        }
        else errors.Add(new RedefinitionError(token.Name));
    }

    private void UpdateIdentifierIsInit(VariableTable identifiersTable, Token token, List<IError> errors)
    {
        if (identifiersTable.GetLexemeType(token.IndexInTable) != DataType.Undefined)
        {
            identifiersTable.SetLexemeIsInitialized(token.IndexInTable, true);
        }
        else errors.Add(new UndeclaredIdentifierError(token.Name));
    }
}