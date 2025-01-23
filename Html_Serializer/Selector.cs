using System.Text.RegularExpressions;

namespace Html_Serializer
{
    internal class Selector
    {
        public string? TagName { get; set; }
        public string? Id { get; set; }
        public List<string> Classes { get; set; }
        public Selector? Parent { get; set; }
        public Selector? Child { get; set; }

        public Selector()
        {
            Classes = new List<string>();
        }
        public static Selector Parse(string query)
        {
            var parts = query.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            Selector root = new Selector();
            Selector current = root;

            foreach (var part in parts)
            {
                var selector = new Selector();
                var tokens = Regex.Split(part, "(?=[.#])")
                    .Where(t => !string.IsNullOrWhiteSpace(t))
                    .ToArray();

                foreach (var token in tokens)
                {
                    if (token.StartsWith("#"))
                        selector.Id = token.Substring(1);

                    else if (token.StartsWith("."))
                        selector.Classes.Add(token.Substring(1));

                    else if (HtmlHelper.Instance.HtmlTags.Contains(token)||HtmlHelper.Instance.HtmlVoidTags.Contains(token))
                        selector.TagName = token;
                }

                current.Child = selector;
                selector.Parent = current;
                current = selector;
            }
            return root.Child;
        }
    }
}
