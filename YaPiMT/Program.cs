using YaPiMT.Core.Tables;
using YaPiMT.Core;
using YaPiMT.Core.Tables.ConstTables;
using YaPiMT.IO;
using YaPiMT.LexicalAnalysis;

var constTableI = new ConstTableIO("../YaPiMT/Input/");
var sourceCodeI = new SourceCodeIO("../YaPiMT/Input/");

var allowedCharactersTable = new CharConstTable("AllowedCharactersTable.txt", constTableI);
var numbersTable = new CharConstTable("NumbersTable.txt", constTableI);
var separatorsTable = new CharConstTable("SeparatorsTable.txt", constTableI);
var operationsTable1 = new CharConstTable("OperationsTable1.txt", constTableI);
var operationsTable2 = new StringConstTable("OperationsTable2.txt", constTableI);
var keywordsTable = new StringConstTable("KeywordsTable.txt", constTableI);

var sourceCode = sourceCodeI.ReadFromFile("SourceCode.txt");

var scanner = new Scanner(
    allowedCharactersTable,
    numbersTable,
    separatorsTable,
    operationsTable1,
    operationsTable2,
    keywordsTable
    );

scanner.AnalyzeProgramCode(sourceCode,
    out var tokens, out var errors, out var identifiersTable, out var constantsTable);

var tokenO = new TokenIO("../YaPiMT/Output/");
var errorO = new ErrorIO("../YaPiMT/Output/");

tokenO.WriteToFile(tokens, "Tokens.txt");
errorO.WriteToFile(errors, "Errors.txt");

identifiersTable.PrintTable();
constantsTable.PrintTable();