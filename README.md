# <b>Engineering Calculator</b><br /><br />
<strong>To use properly insure that:</strong><br />
1.After each number there is a sign of operation.<br />
2.After each word of operation there is opened bracket symbol '('.<br />
3.Numbers of opened and closed parentheses are equal.<br />
4.The input expression is not starting from a sign operation, exept '-'.<br />
5.The input expression is not ending with any sign operation.<br />
5.There are no empty parentheses.<br />
6.Expression in parentheses cannot start from a sign operation exept '-'.<br />
7.There are no more than one sign operation between math functions or numbers.<br />
8.Before opened bracket sign '(' only a sign operation can appear (exept when parentheses are opened right from the start of expression).<br />

<strong>Keyboard input</strong><br />
You can type text to calulator's input with any button, if its KeyDown() event was implemented.<br />
Any letters, numbers, or operation symbols. Backspace deletes one symbol, DEL clears input field.<br />
If any exeption message, or "Invalid expression" message appears, proceed to clear input field by<br />
DEL button on your keyboard or 'C' button on GUI.<br/ >
<strong>Exeption types</strong><br />
1.<code style="color : darkorange">Invalid input</code> - input expression does not correspond to rules described above.<br />
2.<code style="color : darkorange">Wrong operands format</code> - means that on or more operands in expression acquired a sientific notation in process of calculation (double operand has more than fifteen digits).Note that expression after calculation has only obe such number it will be displayed as result.<br />
3.<code style="color : darkorange">NaN value</code> - calculated expression contains NaN - on of operands, or whole result is undefined (https://learn.microsoft.com/en-us/dotnet/api/system.double.nan).<br />
4.<code style="color : darkorange">Infinity</code> - calculated expression contains Infinity (positive or negative one).<br />
5.<code style="color : darkorange">Failed calculating expression</code> - exeded recurent entrance limit(either fails to shorten expression, passing it again or amount parentesis is too high. MAX_RECURSIVE_CALLS = 250).<br />
