using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLispToCS {
    public interface IValueTkn : IToken {
    }

    public class ValueTkn : IValueTkn {
        public int Ident { get; set; }
        public string Text { get; set; }
        public string Comment { get; set; }

        public ValueTkn() {

        }
        public ValueTkn(int ident, string text) {
            Ident = ident;
            Text = text;
            if (Text.Contains("<List>"))
                throw new Exception("da rivedere");// Console.WriteLine(Text);//
        }

        public override string ToString() {
            return "{value}";
        }
    }
    public class OperationTkn : IValueTkn {
        public int Ident { get; set; }
        public string Text { get; set; }
        public string Comment { get; set; }
        public OperationTkn() {

        }
        public OperationTkn(ListToken list) {
            Ident = list.Ident;
            StringBuilder s = new StringBuilder(list.Open);
            if (!String.IsNullOrWhiteSpace(list.Comment))
                s.Append(" /* ").Append(list.Comment).AppendLine(" */");
            // Comment può essere multiline

            switch (list.Items.Count) {
                case 0:
                case 1:
                    throw new Exception("non è un OperationTkn!");
                case 2:
                    // Ammesso ognuno a modo suo  (+ x), (- x), (* x) e (/ x)
                    switch (list.Items[0].Text) {
                        case "+":// (x)
                            if (!String.IsNullOrWhiteSpace(list.Items[0].Comment))
                                s.Append(" // ").AppendLine(list.Items[0].Comment);
                            s.Append(list.Items[1].Text);
                            if (!String.IsNullOrWhiteSpace(list.Items[1].Comment))
                                s.Append(" // ").AppendLine(list.Items[1].Comment);
                            break;
                        case "-":
                            if (!String.IsNullOrWhiteSpace(list.Items[0].Comment))
                                s.Append(" // ").AppendLine(list.Items[0].Comment);
                            s.Append("-1 * ").Append(list.Items[1].Text);
                            if (!String.IsNullOrWhiteSpace(list.Items[1].Comment))
                                s.Append(" // ").AppendLine(list.Items[1].Comment);
                            break;
                        case "*":// (x)
                            if (!String.IsNullOrWhiteSpace(list.Items[0].Comment))
                                s.Append(" // ").AppendLine(list.Items[0].Comment);
                            s.Append(list.Items[1].Text);
                            if (!String.IsNullOrWhiteSpace(list.Items[1].Comment))
                                s.Append(" // ").AppendLine(list.Items[1].Comment);
                            break;
                        case "/":
                            if (!String.IsNullOrWhiteSpace(list.Items[0].Comment))
                                s.Append(" // ").AppendLine(list.Items[0].Comment);
                            s.Append("1 / ").Append(list.Items[1].Text);
                            if (!String.IsNullOrWhiteSpace(list.Items[1].Comment))
                                s.Append(" // ").AppendLine(list.Items[1].Comment);
                            break;
                        case "1+":// (1+ x) -> (++x)
                            if (!String.IsNullOrWhiteSpace(list.Items[0].Comment))
                                s.Append(" // ").AppendLine(list.Items[0].Comment);
                            s.Append("++").Append(list.Items[1].Text);
                            if (!String.IsNullOrWhiteSpace(list.Items[1].Comment))
                                s.Append(" // ").AppendLine(list.Items[1].Comment);
                            break;
                        case "1-":// (1- x) -> (--x)
                            if (!String.IsNullOrWhiteSpace(list.Items[0].Comment))
                                s.Append(" // ").AppendLine(list.Items[0].Comment);
                            s.Append("--").Append(list.Items[1].Text);
                            if (!String.IsNullOrWhiteSpace(list.Items[1].Comment))
                                s.Append(" // ").AppendLine(list.Items[1].Comment);
                            break;
                        default:
                            throw new Exception("non è un OperationTkn!");
                    }
                    break;
                default:
                    string op;
                    switch (list.Items[0].Text) {
                        case "+":
                        case "-":
                        case "*":
                        case "/":
                            op = list.Items[0].Text;
                            break;
                        default:
                            throw new Exception("non è un OperationTkn!");
                    }
                    if (!String.IsNullOrWhiteSpace(list.Items[0].Comment))
                        s.Append(" // ").AppendLine(list.Items[0].Comment);
                    for (int i = 1; i < list.Items.Count; i++) {
                        s.Append(list.Items[i].Text);
                        if (!String.IsNullOrWhiteSpace(list.Items[i].Comment))
                            s.Append(" // ").AppendLine(list.Items[i].Comment);
                        if (i < list.Items.Count - 1)
                            s.Append(" ").Append(op).Append(" ");
                    }
                    break;
            }
            s.Append(")");
            Text = s.ToString();
            if (Text.Contains("<List>"))
                Console.WriteLine("xxx " + Text);// throw new Exception("da rivedere");
        }
        public override string ToString() {
            return "{* + - /}";
        }
    }
    public class FunctionTkn : IValueTkn {
        public int Ident { get; set; }
        public string Text { get; set; }
        public string Comment { get; set; }
        public FunctionTkn() {

        }
        public FunctionTkn(ListToken list, string newName = "") {
            Ident = list.Ident;
            StringBuilder s = new StringBuilder(String.IsNullOrWhiteSpace(newName) ? list.Items[0].Text : newName);
            s.Append(" ").Append(list.Open);
            if (!String.IsNullOrWhiteSpace(list.Comment))
                s.Append(" /* ").Append(list.Comment).AppendLine(" */");
            // Comment può essere multiline
            for (int i = 1; i < list.Items.Count; i++) {
                s.Append(list.Items[i].Text);
                if (!String.IsNullOrWhiteSpace(list.Items[i].Comment))
                    s.Append(" // ").AppendLine(list.Items[i].Comment);
                if (i < list.Items.Count - 1)
                    s.Append(", ");
            }
            s.Append(")");
            Text = s.ToString();
            if (Text.Contains("<List>"))
                Console.WriteLine("xxx " + Text);// throw new Exception("da rivedere");
        }
        public override string ToString() {
            return "{ Func() }";
        }
    }
    public class ConditionTkn : IValueTkn {
        public int Ident { get; set; }
        public string Text { get; set; }
        public string Comment { get; set; }
        public ConditionTkn() {

        }
        public ConditionTkn(ListToken list) {
            Ident = list.Ident;
            StringBuilder s = new StringBuilder(list.Open);
            if (!String.IsNullOrWhiteSpace(list.Comment))
                s.Append(" /* ").Append(list.Comment).AppendLine(" */");
            // Comment può essere multiline
            switch (list.Items[0].Text) {
                case "=":
                case "/=":
                case "<":
                case ">":
                case "<=":
                case ">=":
                    string op = list.Items[0].Text;
                    if (op == "=")
                        op = "==";
                    if (op == "/=")
                        op = "!=";

                    if (list.Items.Count < 3) {
                        throw new Exception("non è un ConditionTkn!");
                        // NON Ammessi  (= x), (/= x), (< x), (> x), (<= x), (>= x)
                    } else if (list.Items.Count == 3) {
                        // (op a b)
                        if (!String.IsNullOrWhiteSpace(list.Items[0].Comment))
                            s.Append(" // ").AppendLine(list.Items[0].Comment);
                        // (a
                        s.Append(list.Items[1].Text);
                        if (!String.IsNullOrWhiteSpace(list.Items[1].Comment))
                            s.Append(" // ").AppendLine(list.Items[1].Comment);
                        // (a op
                        s.Append(" ").Append(op).Append(" ");
                        // (a op b
                        s.Append(list.Items[2].Text);
                        if (!String.IsNullOrWhiteSpace(list.Items[2].Comment))
                            s.Append(" // ").AppendLine(list.Items[2].Comment);
                    } else {
                        // (op a b c) => ((a op b) && (b op c))
                        // (op a b c d) => ((a op b) && (b op c) && (c op d))
                        if (!String.IsNullOrWhiteSpace(list.Items[0].Comment))
                            s.Append(" // ").AppendLine(list.Items[0].Comment);
                        //(
                        for (int i = 1; i < list.Items.Count - 1; i++) {//1,2,3
                            StringBuilder sb2 = new StringBuilder("(");
                            // (a  |  (b  |  (c
                            sb2.Append(list.Items[i].Text);
                            if (!String.IsNullOrWhiteSpace(list.Items[i].Comment))
                                sb2.Append(" // ").AppendLine(list.Items[i].Comment);
                            // (a op |  (b op |  (c op
                            sb2.Append(" ").Append(op).Append(" ");
                            // (a op b   |  (b op c  |  (c op d
                            sb2.Append(list.Items[i + 1].Text);
                            if ((i == list.Items.Count - 2) && (!String.IsNullOrWhiteSpace(list.Items[i + 1].Comment)))
                                sb2.Append(" // ").AppendLine(list.Items[i + 1].Comment);
                            // (a op b)   |  (b op c)  |  (c op d)
                            sb2.Append(")");
                            // "(a op b) && " |  "(b op c) && "  |  "(c op d)"
                            if (i < list.Items.Count - 2)//1,2
                                sb2.Append(" && ");
                            s.Append(sb2.ToString());
                        }
                    }
                    break;
                case "equal":
                    if (list.Items.Count != 3)
                        throw new Exception("non è un ConditionTkn!");
                    // NON ammessi (and x) (or x)
                    // (op a b c d) => (a op b op c op d)
                    if (!String.IsNullOrWhiteSpace(list.Items[0].Comment))
                        s.Append(" // ").AppendLine(list.Items[0].Comment);
                    s.Append(list.Items[1].Text);
                    if (!String.IsNullOrWhiteSpace(list.Items[1].Comment))
                        s.Append(" // ").AppendLine(list.Items[1].Comment);
                    s.Append(" == ").Append(list.Items[2].Text);
                    if (!String.IsNullOrWhiteSpace(list.Items[2].Comment))
                        s.Append(" // ").AppendLine(list.Items[2].Comment);
                    break;
                case "and":
                case "or":
                    if (list.Items.Count < 3)
                        throw new Exception("non è un ConditionTkn!");
                    // NON ammessi (and x) (or x)
                    // (op a b c d) => (a op b op c op d)
                    if (!String.IsNullOrWhiteSpace(list.Items[0].Comment))
                        s.Append(" // ").AppendLine(list.Items[0].Comment);
                    for (int i = 1; i < list.Items.Count; i++) {
                        s.Append(list.Items[i].Text);
                        if (!String.IsNullOrWhiteSpace(list.Items[i].Comment))
                            s.Append(" // ").AppendLine(list.Items[i].Comment);
                        if (i < list.Items.Count - 1)
                            s.Append(list.Items[0].Text == "and" ? " && " : " || ");
                    }
                    break;
                case "not":
                    if (list.Items.Count != 2)
                        throw new Exception("non è un ConditionTkn!");
                    // Ammesso solo (not x) -> (!x)
                    s.Append("!").Append(list.Items[1].Text);
                    if (!String.IsNullOrWhiteSpace(list.Items[0].Comment))
                        s.Append(" // ").AppendLine(list.Items[0].Comment);
                    if (!String.IsNullOrWhiteSpace(list.Items[1].Comment))
                        s.Append(" // ").AppendLine(list.Items[1].Comment);
                    break;
                default:
                    throw new Exception("non è un ConditionTkn!");
            }
            s.Append(")");
            Text = s.ToString();
            if (Text.Contains("<List>"))
                Console.WriteLine("xxx " + Text);//  throw new Exception("da rivedere");
        }
        public override string ToString() {
            return "{ bool }";
        }
    }
    public class ArrayTkn : IValueTkn {
        public int Ident { get; set; }
        public string Text { get; set; }
        public string Comment { get; set; }
        public ArrayTkn() {

        }
        public ArrayTkn(ListToken list) {
            Ident = list.Ident;
            StringBuilder s;
            switch (list.Items[0].Text) {
                case "list":
                    // (list a b c d e) => (new dynamic[] {a, b, c, d, e})
                    s = new StringBuilder(list.Open);
                    if (!String.IsNullOrWhiteSpace(list.Comment))
                        s.Append(" /* ").Append(list.Comment).AppendLine(" */");
                    // Comment può essere multiline
                    // "(new dynamic[] {"
                    s.Append("new dynamic[] {");
                    if (!String.IsNullOrWhiteSpace(list.Items[0].Comment))
                        s.Append(" // ").AppendLine(list.Items[0].Comment);
                    for (int i = 1; i < list.Items.Count; i++) {
                        s.Append(list.Items[i].Text);
                        if (!String.IsNullOrWhiteSpace(list.Items[i].Comment))
                            s.Append(" // ").AppendLine(list.Items[i].Comment);
                        if (i < list.Items.Count - 1)
                            s.Append(", ");
                    }
                    s.Append("})");
                    Text = s.ToString();
                    if (Text.Contains("<List>"))
                        Console.WriteLine("xxx " + Text);//  throw new Exception("da rivedere");
                    break;
                case "nth":
                    if (list.Items.Count != 3)
                        throw new Exception("non è un ArrayTkn!");
                    // (nth index list) => (list[index])
                    s = new StringBuilder(list.Open);
                    if (!String.IsNullOrWhiteSpace(list.Comment))
                        s.Append(" /* ").Append(list.Comment).AppendLine(" */");
                    if (!String.IsNullOrWhiteSpace(list.Items[0].Comment))
                        s.Append(" // ").AppendLine(list.Items[0].Comment);
                    // "(list["
                    s.Append(list.Items[2].Text).Append("[");
                    if (!String.IsNullOrWhiteSpace(list.Items[2].Comment))
                        s.Append(" // ").AppendLine(list.Items[2].Comment);
                    // "(list[index"
                    s.Append(list.Items[1].Text);
                    if (!String.IsNullOrWhiteSpace(list.Items[1].Comment))
                        s.Append(" // ").AppendLine(list.Items[1].Comment);
                    // "(list[index]"
                    s.Append("])");
                    Text = s.ToString();
                    if (Text.Contains("<List>"))
                        Console.WriteLine("xxx " + Text);// throw new Exception("da rivedere");
                    break;
                case "first":
                case "second":
                case "third":
                case "fourth":
                case "fifth":
                case "sixth":
                case "seventh":
                case "eighth":
                case "ninth":
                case "tenth":
                    if (list.Items.Count != 2)
                        throw new Exception("non è un ArrayTkn!");
                    string[] opers = { "first","second","third","fourth","fifth",
                        "sixth","seventh","eighth","ninth","tenth"};
                    // (oper list) => (list[index])
                    s = new StringBuilder(list.Open);
                    if (!String.IsNullOrWhiteSpace(list.Comment))
                        s.Append(" /* ").Append(list.Comment).AppendLine(" */");
                    if (!String.IsNullOrWhiteSpace(list.Items[0].Comment))
                        s.Append(" // ").AppendLine(list.Items[0].Comment);
                    // "(list["
                    s.Append(list.Items[1].Text).Append("[");
                    if (!String.IsNullOrWhiteSpace(list.Items[1].Comment))
                        s.Append(" // ").AppendLine(list.Items[1].Comment);
                    // "(list[index"
                    s.Append(Array.IndexOf(opers, list.Items[0].Text));
                    // "(list[index]"
                    s.Append("])");
                    Text = s.ToString();
                    if (Text.Contains("<List>"))
                        Console.WriteLine("xxx " + Text);// throw new Exception("da rivedere");
                    break;
                default:
                    throw new Exception("non è un ArrayTkn!");
            }
        }
        public override string ToString() {
            return "{ array }";
        }
    }
    public class BlockTkn : IValueTkn {
        public int Ident { get; set; }
        public string Text { get; set; }
        public string Comment { get; set; }
        public BlockTkn() {

        }
        public BlockTkn(ListToken list) {
            Ident = list.Ident;
            StringBuilder s = new StringBuilder();
            s.AppendLine(list.Open.Replace("(", "{"));

            if (!String.IsNullOrWhiteSpace(list.Comment))
                s.Append(" /* ").Append(list.Comment).AppendLine(" */");
            // Comment può essere multiline
            switch (list.Items[0].Text) {
                case "if":
                    if (list.Items.Count < 3 || list.Items.Count > 4)
                        throw new Exception("non è un BlockTkn!");
                    if (!String.IsNullOrWhiteSpace(list.Items[0].Comment))
                        s.Append(" // ").AppendLine(list.Items[0].Comment);
                    s.Append("if ");
                    if (list.Items[1] is ConditionTkn) {
                        //cioè ha le parentesi
                        s.Append(list.Items[1].Text);
                    } else {
                        if (list.Items[1] is ElementToken || list.Items[1] is FunctionTkn) {
                            //|| list.Items[1] is ValueTkn
                            //magari è una variabile bool|| funzione booleana || un valore ??? 
                            s.Append("(").Append(list.Items[1].Text);
                            if (!String.IsNullOrWhiteSpace(list.Items[1].Comment))
                                s.Append(" // ").AppendLine(list.Items[1].Comment);
                            s.Append(")");
                        } else {
                            throw new Exception("non è un BlockTkn!");//Console.WriteLine(Text);//
                        }
                    }
                    // if(cond){
                    s.AppendLine("{");
                    s.Append(list.Items[2].Text);
                    if (!String.IsNullOrWhiteSpace(list.Items[2].Comment))
                        s.Append(" // ").AppendLine(list.Items[2].Comment);
                    else
                        s.AppendLine();
                    // if(cond){then}
                    s.AppendLine("}");
                    if (list.Items.Count == 4) {
                        // ha else
                        s.AppendLine("else {");
                        s.Append(list.Items[3].Text);
                        if (!String.IsNullOrWhiteSpace(list.Items[3].Comment))
                            s.Append(" // ").AppendLine(list.Items[3].Comment);
                        else
                            s.AppendLine();
                        s.AppendLine("}");
                    }
                    // (if test then else) -> (cond (test then) (t else))
                    // -> if(test){then} else if(t){else}
                    break;
                case "cond":
                    // corrisponde ad una serie di: if, elseif
                    // es "(cond (cond1 istruz1) (cond2 istruz2) (cond3 istruz3) (t istruz4))"
                    /*
                     {
                    if(cond1) {istruz1} 
                    else if(cond2) {istruz2}
                    else if(cond3) {istruz3}
                    else if(t) {istruz4}
                    }
                     */
                    for (int i = 1; i < list.Items.Count; i++) {
                        //ogni list.Items[i] e un ListToken composto da (condizione istruzione) o (t istruzione)
                        if (!(list.Items[i] is ListToken))
                            throw new Exception("non è un BlockTkn!");
                        ListToken list1 = list.Items[i] as ListToken;
                        if (list1.Items.Count != 2)
                            throw new Exception("non è un BlockTkn!");
                        if (!String.IsNullOrWhiteSpace(list1.Comment))
                            s.Append(" /* ").AppendLine(list1.Comment).Append(" */ ");//può essere multiline
                        if (i > 1)
                            s.Append("else ");
                        if (list1.Items[0] is ElementToken && list1.Items[0].Text == "t") {
                            // sostituisce gli "else if (t)" in "else"
                            if (!String.IsNullOrWhiteSpace(list1.Items[0].Comment))
                                s.Append(" // ").AppendLine(list1.Items[0].Comment);
                        } else {
                            s.Append("if ");
                            // "if " | "else if "
                            if (list1.Items[0] is ConditionTkn) {
                                //cioè ha le parentesi
                                s.Append(list1.Items[0].Text);
                            } else {
                                if (list1.Items[0] is ElementToken || list1.Items[0] is FunctionTkn) {
                                    //|| list.Items[1] is ValueTkn
                                    //magari è una variabile bool|| funzione booleana || un valore ??? 
                                    s.Append("(").Append(list1.Items[0].Text);
                                    if (!String.IsNullOrWhiteSpace(list1.Items[0].Comment))
                                        s.Append(" // ").AppendLine(list1.Items[0].Comment);
                                    s.Append(")");
                                } else {
                                    throw new Exception("non è un BlockTkn!");//Console.WriteLine(Text);//
                                }
                            }
                        }
                        // "if(cond){" | "else if(cond){" | "else {"
                        s.AppendLine("{");
                        s.Append(list1.Items[1].Text);
                        if (!String.IsNullOrWhiteSpace(list1.Items[1].Comment))
                            s.Append(" // ").AppendLine(list1.Items[1].Comment);
                        else
                            s.AppendLine();
                        // "if(cond){then}" | "else if(cond){then}"
                        s.AppendLine("}");
                    }
                    break;
                case "let":
                case "let*":
                    // è composto da (let[*] ((var0 value0) (var1 value1) ... (varN valueN)) (body-che-usa-var))
                    /* e deve diventare:
                     * {
                     * var0 = value0;
                     * var1 = value1;
                     * ... varN = valueN;
                     * body-che-usa-var
                     * }
                     */
                    if (list.Items.Count != 3)
                        throw new Exception("non è un BlockTkn!");
                    //altrimenti o sarebbe 0 body o >1 body
                    if (!String.IsNullOrWhiteSpace(list.Items[0].Comment))
                        s.Append(" // ").AppendLine(list.Items[0].Comment);
                    if (!(list.Items[1] is ListToken))
                        throw new Exception("non è un BlockTkn!");
                    ListToken listVars = list.Items[1] as ListToken;
                    if (!String.IsNullOrWhiteSpace(listVars.Comment))
                        s.Append(" /* ").Append(listVars.Comment).AppendLine(" */ ");
                    foreach (IToken tkn in listVars.Items) {
                        // tkn = "(varN valueN)"
                        if (tkn is ListToken) {
                            ListToken listVar = tkn as ListToken;
                            if (!String.IsNullOrWhiteSpace(listVar.Comment))
                                s.Append(" /* ").Append(listVar.Comment).AppendLine(" */ ");
                            s.Append("var ").Append(listVar.Items[0].Text);
                            if (!String.IsNullOrWhiteSpace(listVar.Items[0].Comment))
                                s.Append(" // ").AppendLine(listVar.Items[0].Comment);
                            s.Append(" = ").Append(listVar.Items[1].Text).Append(";");
                            if (!String.IsNullOrWhiteSpace(listVar.Items[1].Comment))
                                s.Append(" // ").AppendLine(listVar.Items[1].Comment);
                            else
                                s.AppendLine();
                        } else if (tkn is FunctionTkn ) {
                            //mal interpretazione della funzione con la variabile
                            if (tkn.Text.StartsWith("hour")) {
                                // "hour (Math.Floor ((time * 24)))"
                                s.Append(tkn.Text.Replace("hour", "var hour =")).AppendLine(";");
                                if (!String.IsNullOrWhiteSpace(tkn.Comment))
                                    s.Append(" /* ").Append(tkn.Comment).AppendLine(" */ ");
                            } else if (tkn.Text.StartsWith("minute")) {
                                // "minute (Math.Floor (mod ((time * 24 * 60), 60)))"
                                s.Append(tkn.Text.Replace("minute", "var minute =")).AppendLine(";");
                                if (!String.IsNullOrWhiteSpace(tkn.Comment))
                                    s.Append(" /* ").Append(tkn.Comment).AppendLine(" */ ");
                            } else if (tkn.Text.StartsWith("start")) {
                                //"start ((gregorian_epoch + (365 * approx) + quotient (approx, 4) + (-1 * quotient (approx, 100)) + quotient (approx, 400)))"
                                s.Append(tkn.Text.Replace("start", "var start =")).AppendLine(";");
                                if (!String.IsNullOrWhiteSpace(tkn.Comment))
                                    s.Append(" /* ").Append(tkn.Comment).AppendLine(" */ ");
                            } else if (tkn.Text.StartsWith("solar_anomaly")) {
                                // "solar_anomaly (poly (c, deg ((new dynamic[] {2.5534E0, (1236.85E0 * 29.10535669E0), -0.0000014E0, -0.00000011E0}))))"
                                s.Append(tkn.Text.Replace("solar_anomaly", "var solar_anomaly =")).AppendLine(";");
                                if (!String.IsNullOrWhiteSpace(tkn.Comment))
                                    s.Append(" /* ").Append(tkn.Comment).AppendLine(" */ ");
                            } else if (tkn.Text.StartsWith("lunar_anomaly")) {
                                // "lunar_anomaly (poly (c, deg ((new dynamic[] {201.5643E0, (385.81693528E0 * 1236.85E0), 0.0107582E0, 0.00001238E0, -0.000000058E0}))))"
                                s.Append(tkn.Text.Replace("lunar_anomaly", "var lunar_anomaly =")).AppendLine(";");
                                if (!String.IsNullOrWhiteSpace(tkn.Comment))
                                    s.Append(" /* ").Append(tkn.Comment).AppendLine(" */ ");
                            } else if (tkn.Text.StartsWith("precession")) {
                                // "precession ((deg (27) - Math.Abs ((deg (54) - mod ((deg (27) + (deg (108) * new Rational(600,1577917828) * days)), 108)))))"
                                s.Append(tkn.Text.Replace("precession", "var precession =")).AppendLine(";");
                                if (!String.IsNullOrWhiteSpace(tkn.Comment))
                                    s.Append(" /* ").Append(tkn.Comment).AppendLine(" */ ");
                            } else {
                                throw new Exception("non è un BlockTkn!");
                            }
                        } else if (tkn is ArrayTkn && tkn.Text.EndsWith("[1])")) {
                            // "(mod ((time * 24 * 60 * 60), 60)[1])"
                            s.Append("var second = ").AppendLine(tkn.Text.Replace("[1])", ");"));
                            if (!String.IsNullOrWhiteSpace(tkn.Comment))
                                s.Append(" /* ").Append(tkn.Comment).AppendLine(" */ ");
                        } else {
                            throw new Exception("non è un BlockTkn!");
                        }
                    }
                    s.AppendLine(list.Items[2].Text);// non conosco il formato di body
                    if (!String.IsNullOrWhiteSpace(list.Items[2].Comment))
                        s.Append(" // ").AppendLine(list.Items[2].Comment);
                    break;
                default:
                    throw new Exception("non è un BlockTkn!");
            }
            s.Append("}");
            Text = s.ToString();
            if (Text.Contains("<List>"))
                Console.WriteLine("xxx " + Text);// throw new Exception("da rivedere");
        }
        public override string ToString() {
            return "{ block }";
        }
    }

    public class SigmaBlockTkn : BlockTkn {
        public SigmaBlockTkn() {

        }
        public SigmaBlockTkn(ListToken list) {
            Ident = list.Ident;
            StringBuilder s = new StringBuilder();
            if (list.Items[0].Text != "sigma")
                throw new Exception("non è un SigmaBlockTkn!");
            /* es (sigma ((x (list 1 2 3 4)) (y (list 5 6 7 8))) (* x y))
            *  = 70 
            *  (sigma ((x (list 1 2 3 4)) (y (list 5 6 7 8)) (z (list 2 4 6 8))) (- (* x y) z))
            *  = 50    */
            /*  (sigma 
            *   ((i add-const) (j add-coeff) (l add-factor))
            *   (* l (sin-degrees (+ i (* j k))))
            *  )
            *  ->
            *  double sigma = 0;
            *  for (int ii = 0; ii < add-const.Length; ii++) {
            *     double i = add-const[ii];
            *     double j = add-coeff[ii]; 
            *     double l = add-factor[ii];
            *     sigma += (l * sin-degrees (i + j * k));
            *  }    */
            s.AppendLine("double sigma = 0;");
            if (!String.IsNullOrWhiteSpace(list.Comment))
                s.Append(" /* ").Append(list.Comment).AppendLine(" */");
            if (!String.IsNullOrWhiteSpace(list.Items[0].Comment))
                s.Append(" // ").AppendLine(list.Items[0].Comment);
            if (!(list.Items[1] is ListToken))// ((i add-const) (j add-coeff) (l add-factor))
                throw new Exception("non è un SigmaBlockTkn!");
            ListToken listArray = list.Items[1] as ListToken;
            s.AppendLine("for (int ii = 0; ii < §§§.Length; ii++) {");
            if (!String.IsNullOrWhiteSpace(listArray.Comment))
                s.Append(" /* ").Append(listArray.Comment).AppendLine(" */");
            for (int i = 0; i < listArray.Items.Count; i++) {
                if (!(listArray.Items[i] is ListToken))// (i add-const) | (j add-coeff) | (l add-factor)
                    throw new Exception("non è un SigmaBlockTkn!");
                ListToken arrNomeValue = listArray.Items[i] as ListToken;
                if (!String.IsNullOrWhiteSpace(arrNomeValue.Comment))
                    s.Append(" /* ").Append(arrNomeValue.Comment).AppendLine(" */");
                if (i == 0)
                    s = s.Replace("§§§", arrNomeValue.Items[1].Text);
                s.Append("double ").Append(arrNomeValue.Items[0].Text);
                if (!String.IsNullOrWhiteSpace(arrNomeValue.Items[0].Comment))
                    s.Append(" // ").AppendLine(arrNomeValue.Items[0].Comment);
                s.Append(" = ").Append(arrNomeValue.Items[1].Text).Append("[ii];");
                if (!String.IsNullOrWhiteSpace(arrNomeValue.Items[1].Comment))
                    s.Append(" // ").AppendLine(arrNomeValue.Items[1].Comment);
                else
                    s.AppendLine();
            }
            // (* l (sin-degrees (+ i (* j k))))  ->   (l * sin-degrees (i + j * k))
            if (!String.IsNullOrWhiteSpace(list.Items[2].Comment))
                s.Append(" /* ").Append(list.Items[2].Comment).AppendLine(" */");
            s.Append("sigma += ").Append(list.Items[2].Text).AppendLine(";");
            s.AppendLine("}");
            Text = s.ToString();
            if (Text.Contains("<List>"))
                Console.WriteLine("xxx " + Text);// throw new Exception("da rivedere");
        }
        public override string ToString() {
            return "{ sigma() }";
        }
    }
}
