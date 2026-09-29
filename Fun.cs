using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLispToCS {
   public class Fun:ICSharpable {
        public List<string> Comments { get; set; }
        public string TypeReturn { get; set; } = "dynamic";
        public string Name { get; set; }
        //NO dict perchè è importante l'ordine
        public List<string> Params { get; set; }
        public List<string> ParamsType { get; set; }
        public string Body { get; set; }//alcune volte è una formula
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
            StringBuilder sb = new StringBuilder();
            if (Params != null && Params.Count > 0) { 
                for (int i = 0; i < Params.Count; i++) {
                    if(i>0)
                        sb.Append(", ");
                    if (ParamsType != null && ParamsType.Count > i) {
                        sb.Append(ParamsType[i]).Append(" ");
                    } else {
                        sb.Append("var ");
                    }
                    sb.Append(Params[i]);
                }
            }
            ris.Add($"public {TypeReturn} {Name} ({sb.ToString()})" +"{");
            ris.Add(Body);
            //return 0;
            ris.Add("}");
            return ris;
        }              
    }
}
