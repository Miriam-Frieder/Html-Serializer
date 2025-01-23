
using Html_Serializer;
using System.Security;
using System.Text.RegularExpressions;



string s = "<div id=\"a\" \nclass=\"as av an\" src=\"#\">      <p>Hello World</p>    <a href=\"#\"       >Link</a></div>";
var htmlSerializer= new HtmlSerializer();
var html = await htmlSerializer.Load("https://hebrewbooks.org/beis");
var dom=htmlSerializer.Serialize(html);

var result = dom.Query(Selector.Parse("div div"));
foreach (var item in result)
    Console.WriteLine(item);