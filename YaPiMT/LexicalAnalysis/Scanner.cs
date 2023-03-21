using YaPiMT.Core;
using YaPiMT.Core.Errors;
using YaPiMT.Core.Tables;
using YaPiMT.Core.Tables.ConstTables;

namespace YaPiMT.LexicalAnalysis;

public class Scanner
{
    public readonly CharConstTable AllowedCharactersTable;
    public readonly CharConstTable NumbersTable;
    public readonly CharConstTable SeparatorsTable;
    public readonly CharConstTable OperationsTable1;
    public readonly StringConstTable OperationsTable2;
    public readonly StringConstTable KeywordsTable;

    public Scanner(
        CharConstTable allowedCharactersTable,
        CharConstTable numbersTable,
        CharConstTable separatorsTable,
        CharConstTable operationsTable1,
        StringConstTable operationsTable2,
        StringConstTable keywordsTable
        )
    {
        AllowedCharactersTable = allowedCharactersTable;
        NumbersTable = numbersTable;
        SeparatorsTable = separatorsTable;
        OperationsTable1 = operationsTable1;
        OperationsTable2 = operationsTable2;
        KeywordsTable = keywordsTable;
    }

    public void AnalyzeProgramCode(string sourceCode, out List<Token> tokens, out List<IError> errors, out VariableTable IdentifiersTable, out VariableTable ConstantsTable)
    {
        IdentifiersTable = new VariableTable();
        ConstantsTable = new VariableTable();
        tokens = new List<Token>();
        errors = new List<IError>();

        var codeLines = sourceCode.Replace("\t", "").Split("\r\n");

        for (var i = 0; i < codeLines.Length; i++)
        {
            var characterSequences = codeLines[i].Split(' ');

            for (var j = 0; j < characterSequences.Length; j++)
            {
                var characterSequence = characterSequences[j];

                while (!string.IsNullOrEmpty(characterSequence))
                {
                    var sequenceHasError = false;

                    int indexInTable;

                    if (AllowedCharactersTable.FindElementIndex(characterSequence[0]) != -1)
                    {
                        characterSequence = CheckSequenceForIdentifier(characterSequence, IdentifiersTable, i + 1,
                            tokens, errors, ref sequenceHasError);

                        if (sequenceHasError) break;
                    }
                    else if (NumbersTable.FindElementIndex(characterSequence[0]) != -1)
                    {
                        characterSequence = CheckSequenceForConstant(characterSequence, ConstantsTable, i + 1, tokens,
                            errors, ref sequenceHasError);

                        if (sequenceHasError) break;
                    }
                    else if (OperationsTable1.FindElementIndex(characterSequence[0]) != -1 || OperationsTable2.FindElementIndex(characterSequence[..1]) != -1)
                    {
                        if ((indexInTable = OperationsTable1.FindElementIndex(characterSequence[0])) != -1)
                        {
                            tokens.Add(new Token(nameof(OperationsTable1), characterSequence[0], indexInTable));
                            characterSequence = characterSequence[1..];
                        }
                        else if ((indexInTable = OperationsTable2.FindElementIndex(characterSequence[..1])) != -1)
                        {
                            tokens.Add(new Token(nameof(OperationsTable2), characterSequence[..1], indexInTable));
                            characterSequence = characterSequence[2..];
                        }
                    }
                    else if (characterSequence[0] == '/')
                    {
                        if (characterSequence.Length > 1 && characterSequence[1] == '*')
                        {
                            var indexInCharacterSequence = characterSequence.IndexOf("*/", 2);

                            if (indexInCharacterSequence == -1)
                            {
                                for (; i < codeLines.Length && codeLines[i].IndexOf("*/") == -1; i++) { }

                                if (i == codeLines.Length)
                                {
                                    j = characterSequences.Length;
                                    break;
                                }

                                j = 0;
                                characterSequences = codeLines[i].Split(' ');
                                for (; j < characterSequences.Length && characterSequences[j].IndexOf("*/") == -1; j++) { }

                                if (j == characterSequences.Length) break;

                                characterSequence = characterSequences[j];
                                indexInCharacterSequence = characterSequence.IndexOf("*/");
                                indexInCharacterSequence+=2;
                                characterSequence = characterSequence[indexInCharacterSequence..];
                            }
                            else
                            {
                                indexInCharacterSequence+=2;
                                characterSequence = characterSequence[indexInCharacterSequence..];
                            }
                        }
                        else if (characterSequence.Length > 1 && characterSequence[1] == '/')
                        {
                            j = characterSequence.Length;
                            break;
                        }
                        else
                        {
                            errors.Add(new UnreachableCodeError(i + 1, characterSequence));
                            break;
                        }
                    }
                    else if (characterSequence[0] == '!')
                    {
                        if (characterSequence[1] == '=')
                        {
                            tokens.Add(new Token(nameof(OperationsTable2), characterSequence[..1],
                                OperationsTable2.FindElementIndex(characterSequence[..1])));
                            characterSequence = characterSequence[2..];
                        }
                        else
                        {
                            errors.Add(new UnexpectedTokenError(i + 1, characterSequence[..1]));
                            characterSequence = characterSequence[1..];
                        }
                    }
                    else if ((indexInTable = SeparatorsTable.FindElementIndex(characterSequence[0])) != -1)
                    {
                        tokens.Add(new Token(nameof(SeparatorsTable), characterSequence[0],
                            indexInTable));
                        characterSequence = characterSequence[1..];
                    }
                    else
                    {
                        errors.Add(new InvalidCharacterError(i + 1, characterSequence[0]));
                        characterSequence = characterSequence[1..];
                    }
                }
            }
        }
    }

    private string CheckSequenceForConstant(string characterSequence, VariableTable ConstantsTable, in int lineNumber, List<Token> tokens, List<IError> errors, ref bool sequenceHasError)
    {
        var constant = "";
        var k = 0;

        for (; k < characterSequence.Length; k++)
        {
            var character = characterSequence[k];
            if (NumbersTable.FindElementIndex(character) != -1 || character == '.' && constant.Count(c => c == '.') < 1)
            {
                constant += character;
            }
            else if (SeparatorsTable.FindElementIndex(character) != -1)
            {
                break;
            }
            else
            {
                errors.Add(new UnreachableCodeError(lineNumber, characterSequence[k..]));
                sequenceHasError = true;
                break;
            }
        }

        var index = ConstantsTable.AddLexeme(constant.Contains('.')
            ? new Lexeme(constant, DataType.Float, true)
            : new Lexeme(constant, DataType.Int, true));

        tokens.Add(new Token(nameof(ConstantsTable), constant, index));

        return characterSequence[k..];
    }

    private string CheckSequenceForIdentifier(string characterSequence, VariableTable IdentifiersTable, in int lineNumber, List<Token> tokens, List<IError> errors, ref bool sequenceHasError)
    {
        var identifier = "";
        var k = 0;

        for (; k < characterSequence.Length; k++)
        {
            var character = characterSequence[k];

            if (AllowedCharactersTable.FindElementIndex(character) != -1 ||
                NumbersTable.FindElementIndex(character) != -1)
            {
                identifier += character;
            }
            else if (SeparatorsTable.FindElementIndex(character) != -1 || OperationsTable1.FindElementIndex(character) != -1)
            {
                break;
            }
            else
            {
                errors.Add(new UnreachableCodeError(lineNumber, characterSequence[k..]));
                sequenceHasError = true;
                break;
            }
        }

        int indexInTable;

        if ((indexInTable = KeywordsTable.FindElementIndex(identifier)) != -1)
        {
            tokens.Add(new Token(nameof(KeywordsTable), identifier, indexInTable));
        }
        else
        {
            indexInTable = IdentifiersTable.AddLexeme(new Lexeme(identifier, DataType.Undefined, false));

            tokens.Add(
                new Token(nameof(IdentifiersTable), identifier, indexInTable));
        }

        return characterSequence[k..];
    }
}