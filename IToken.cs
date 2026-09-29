using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLispToCS {
   public interface IToken {
        int Ident { get; set; }

        string Text { get; set; }
        //string ToText();
        string Comment { get; set; }
   }
    public class OpenToken : IToken {
        public int Ident { get ; set ; }
        public string Text { get; set; } = "(";
        public string Comment { get; set; }
        public OpenToken() {

        }
        public OpenToken(int ident, string text) {
            Ident = ident;
            Text = text;
        }
        //public string ToText() {
        //    throw new NotImplementedException();
        //}
        public override string ToString() {
            return "Open " + Text;
        }
    }
    public class CloseToken : IToken {
        public int Ident { get; set; }
        public string Text { get; set; } = ")";
        public string Comment { get; set; }
        public CloseToken() {

        }
        public CloseToken(int ident,string text) {
            Ident = ident;
            Text = text;
        }
        public override string ToString() {
            return "Close " + Text;
        }
    }
    public class ElementToken : IToken {
        public int Ident { get; set; }
        public string Text { get; set; }
        public string Comment { get; set; }
        public ElementToken() {

        }
        public ElementToken(int ident,string text) {
            Ident = ident;
            Text = text;
        }
        public override string ToString() {
            return "Elem " + Text;
        }
    }
    public class ListToken : IToken {
        public int Ident { get; set; }
        public string Open { get; set; }
        public List<IToken> Items { get; set; } = new List<IToken>();
        public string Text { get; set; } = "<List>";
        public string Comment { get; set; }
        public ListToken() {

        }
        public ListToken(int ident) {
            Ident = ident;            
        }
        public override string ToString() {
            return "List ";
        }
    }

    
}
