using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace Html_Serializer
{
    internal class HtmlElement
    {
        public string? Id { get; set; }
        public string Name { get; set; }
        public Dictionary<string, string> Attributes { get; set; }
        public List<string> Classes { get; set; }
        public string InnerHtml { get; set; }
        public HtmlElement? Parent { get; set; }
        public List<HtmlElement> Children { get; set; }

        public HtmlElement(string name)
        {
            Children = new List<HtmlElement>();
            Attributes = new Dictionary<string, string>();
            InnerHtml = "";
            Name = name;
            Classes = new List<string>();
        }

        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> queue = new Queue<HtmlElement>();
            foreach (var child in this.Children)
            {
                queue.Enqueue(child);
            }
            while (queue.Count > 0)
            {
                HtmlElement current = queue.Dequeue();

                yield return current;

                foreach (var child in current.Children)
                {
                    queue.Enqueue(child);
                }
            }
        }

        public IEnumerable<HtmlElement> Ancestors()
        {
            HtmlElement? current = this.Parent;

            while (current != null)
            {
                yield return current;
                current = current.Parent;
            }
        }

        public IEnumerable<HtmlElement> Query(Selector selector)
        {
            var results = new HashSet<HtmlElement>();
            var tmpElement = new HtmlElement("tmp");
            tmpElement.Children.Add(this);
            FindElementsBySelectorRecursive(tmpElement, selector, results);
            return results;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"<{Name}");

            foreach (var attribute in Attributes)
            {
                sb.Append($" {attribute.Key}=\"{attribute.Value}\"");
            }

            sb.Append(">");

            return sb.ToString();
        }


        private void FindElementsBySelectorRecursive(HtmlElement element, Selector selector, HashSet<HtmlElement> results)
        {
            var descendants = element.Descendants();

            var filtered = descendants.Where(d => MatchesSelector(d, selector));

            if (selector.Child == null)
            {
                foreach (var item in filtered)
                {
                    results.Add(item);
                }
            }
            else
            {
                foreach (var descendant in filtered)
                {
                    FindElementsBySelectorRecursive(descendant, selector.Child, results);
                }
            }
        }

        private bool MatchesSelector(HtmlElement element, Selector selector)
        {
            if (!string.IsNullOrEmpty(selector.TagName) && element.Name != selector.TagName)
                return false;

            if (!string.IsNullOrEmpty(selector.Id) && element.Id != selector.Id)
                return false;

            foreach (var className in selector.Classes)
            {
                if (!element.Classes.Contains(className))
                    return false;
            }

            return true;
        }
    }

}

