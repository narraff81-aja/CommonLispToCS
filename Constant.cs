using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLispToCS {
    public interface ICSharpable {
        List<string> ToCS();
    }
    public class Constant : ICSharpable {

        public List<string> Comments { get; set; }
        public string Type { get; set; } = "var";//o "dynamic" ???
        public string Name { get; set; }
        public string Value { get; set; }//alcune volte è una formula
        public Constant(string name) {
            Name = name;
        }
        public void AddComment(string text) {
            if (Comments == null)
                Comments = new List<string>();
            Comments.Add(text);
        }
        public List<string> ToCS() {
            List<string> ris = new List<string>();
            if (Comments != null && Comments.Count > 0) {
                ris.Add("/// <summary>");
                foreach (string item in Comments)
                    ris.Add("/// " + item);
                ris.Add("/// </summary>");
            }
            ris.Add($"public {Type} {Name} = {Value};");
            return ris;
        }       
    }
    public class Comment : ICSharpable {
        public string Text { get; set; }
        public Comment(string text) {
            this.Text = text;
        }
        public List<string> ToCS() {
            return new List<string>() { "// " + Text };
        }
    }
}
