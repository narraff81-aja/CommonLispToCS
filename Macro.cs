using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLispToCS {
    [Obsolete("L'ho fatto a mano",true)]
    class Macro : ICSharpable {
        public List<string> Comments { get; set; }
        public string TypeReturn { get; set; } = "var";//o "dynamic" ???
        public string Name { get; set; }

        

        public string Body { get; set; }//alcune volte è una formula
        public void AddComment(string text) {
            if (Comments == null)
                Comments = new List<string>();
            Comments.Add(text);
        }
        //NO dict perchè è importante l'ordine
        public List<string> Params { get; set; }
        //public List<string> ParamsType { get; set; }

        public List<string> ToCS() {
            List<string> ris = new List<string>();
            if (Comments != null && Comments.Count > 0) {
                ris.Add("/// <summary>");
                foreach (string item in Comments)
                    ris.Add("/// " + item);
                ris.Add("/// </summary>");
            }
            StringBuilder sb = new StringBuilder();
            if (Params != null && Params.Count > 0) {
                for (int i = 0; i < Params.Count; i++) {
                    if (i > 0)
                        sb.Append(", ");
                    sb.Append("var ");//meglio lasciarlo così
                    sb.Append(Params[i]);
                }
            }
            ris.Add($"public {TypeReturn} {Name} ({sb.ToString()})" + "{");
            ris.Add(Body);
            //return 0;
            ris.Add("}");
            return ris;
        }
    }
}
