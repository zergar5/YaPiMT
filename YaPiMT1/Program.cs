using YaPiMT1.IO;
using YaPiMT1.Models;
using YaPiMT1.Models.Tables;
using YaPiMT1.Models.Tables.ConstTables;

var constTableReader = new ConstTableReader("../YaPiMT1/Input/");

var stringConstTable = new StringConstTable("StringConstTable.txt", constTableReader);
stringConstTable.PrintTable();
var stringElement = stringConstTable.FindElement(1);
Console.WriteLine(stringElement);
stringElement = stringConstTable.FindElement(2);
Console.WriteLine(stringElement);

var charConstTable = new CharConstTable("CharConstTable.txt", constTableReader);
charConstTable.PrintTable();
var charElement = charConstTable.FindElement('+');
Console.WriteLine(charElement);
charElement = charConstTable.FindElement('d');
Console.WriteLine(charElement);

var variableTable = new VariableTable();

var index = variableTable.AddLexeme(new Lexeme("item", DataType.Undefined, false));
Console.WriteLine($"{index}");
index = variableTable.AddLexeme(new Lexeme("25.5", DataType.Float, true));
Console.WriteLine($"{index}");
index = variableTable.AddLexeme(new Lexeme("size", DataType.Int, true));
Console.WriteLine($"{index}");
index = variableTable.AddLexeme(new Lexeme("symbol", DataType.Char, true));
Console.WriteLine($"{index}");

variableTable.PrintTable();

var lexeme = variableTable.FindLexeme(0);
Console.WriteLine($"{lexeme.Name} {lexeme.Type} {lexeme.IsInitialized}");
lexeme = variableTable.FindLexeme(1);
Console.WriteLine($"{lexeme.Name} {lexeme.Type} {lexeme.IsInitialized}");
lexeme = variableTable.FindLexeme(2);
Console.WriteLine($"{lexeme.Name} {lexeme.Type} {lexeme.IsInitialized}");
lexeme = variableTable.FindLexeme(3);
Console.WriteLine($"{lexeme.Name} {lexeme.Type} {lexeme.IsInitialized}");
// lexeme = variableTable.FindLexeme(4);
// Console.WriteLine($"{lexeme.Name} {lexeme.Type} {lexeme.IsInitialized}");

lexeme = variableTable.RemoveLexeme("25.5");
variableTable.PrintTable();

// lexeme = variableTable.RemoveLexeme("table");
// variableTable.PrintTable();

variableTable.SetLexemeType(0, DataType.Float);
variableTable.SetLexemeIsInitialized(0, true);

lexeme = variableTable.FindLexeme(0);
Console.WriteLine($"{lexeme.Name} {lexeme.Type} {lexeme.IsInitialized}");

// variableTable.SetLexemeType(4, DataType.Char);

var i = variableTable.FindIndex("size");
var x = variableTable.GetLexemeType(2);
var y = variableTable.GetLexemeIsInitialized(2);

Console.WriteLine($"{i}, {x}, {y}");

variableTable.PrintTable();
