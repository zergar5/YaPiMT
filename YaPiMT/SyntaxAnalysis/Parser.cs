﻿using System.Runtime.CompilerServices;
using YaPiMT.Core.Errors;
using YaPiMT.Core.Tables;
using YaPiMT.Core;
using YaPiMT.Core.AST;
using YaPiMT.Core.Errors.SyntaxErrors;
using YaPiMT.Core.Tables.ConstTables;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Reflection.PortableExecutable;
using YaPiMT.IO;

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
    private int _ruleNumber = 0;

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
            var containedInTerminals = false;

            if (LlTable.IsNonterminal(currentState.Terminals))
            {
                containedInTerminals = HandleNonterminal(token, currentState.Terminals);
            }
            else
            {
                var terminals = currentState.Terminals.Split(' ');

                foreach (var terminal in terminals)
                {
                    containedInTerminals = HandleTerminal(token, terminal);
                    if (containedInTerminals) break;
                }

                if (containedInTerminals)
                {
                    if (IsAssign(token)) init = true;

                    switch (init)
                    {
                        case true when prevToken.TableName == "IdentifiersTable":
                            UpdateIdentifierIsInit(identifiersTable, prevToken, errors);
                            init = false;
                            type = DataType.Undefined;
                            break;
                        case true when prevToken.TableName == "ConstantsTable":
                            errors.Add(new CannotAssignToRvalueError(token.Name));
                            init = false;
                            break;
                    }

                    if (IsType(token))
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

                    if (IsIdentifier(token) &&
                        identifiersTable.GetLexemeType(token.IndexInTable) == DataType.Undefined &&
                        (IsOperator(prevToken) || prevToken.Name == "("))
                        errors.Add(new UndeclaredIdentifierError(token.Name));

                    if (token.TableName != "KeywordsTable")
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
            }

            if (currentState.Accept)
            {
                prevToken = token;
                i++;
                if (i < tokens.Count) token = tokens[i];
            }

            if (currentState.Error && !containedInTerminals)
            {
                if (!LlTable.IsNonterminal(currentState.Terminals))
                {
                    errors.Add(new ExpectedError(currentState.Terminals));
                    break;
                }
                else
                {
                    errors.Add(new CannotResolveSymbolError(token.Name));
                    break;
                }
            }

            currentState = LlTable.MoveNext(containedInTerminals);
        }

        return rpnBuilder.ToString();
    }
    private bool HandleNonterminal(Token token, string nonterminal)
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

    private bool HandleTerminal(Token token, string terminal)
    {
        if (terminal == "identifier" && IsIdentifier(token)) return true;
        if (terminal == "const" && IsConst(token)) return true;
        if (terminal == "operator" && IsOperator(token)) return true;
        if (terminal == "assign" && IsAssign(token)) return true;
        if (token.Name == terminal) return true;

        return false;
    }

    private bool IsType(Token token)
    {
        return token.Name is "int" or "float" or "char";
    }

    private bool IsIdentifier(Token token)
    {
        return token.TableName is "IdentifiersTable";
    }

    private bool IsConst(Token token)
    {
        return token.TableName is "ConstantsTable";
    }

    private bool IsOperator(Token token)
    {
        return (token.TableName == "OperationsTable1" && token.Name != "=") ||
               token is { TableName: "OperationsTable2", Name: "==" or "!=" };
    }

    private bool IsAssign(Token token)
    {
        return token.Name == "=" || token is { TableName: "OperationsTable2", Name: not ("==" and "!=") };
    }

    private void UpdateIdentifierType(VariableTable identifiersTable, Token token, DataType type, List<IError> errors)
    {
        if (identifiersTable.GetLexemeType(token.IndexInTable) == DataType.Undefined &&
            identifiersTable.GetLexemeIsInitialized(token.IndexInTable) == false)
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