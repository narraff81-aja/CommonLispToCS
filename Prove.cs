using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLispToCS {
  public  class Prove {
        public static void Prova() {
            //Rational rational = new Rational(2345, 5675);
            //Console.WriteLine(rational.Numerator + " / " + rational.Denumerator);

            /*
            Random rnd = new Random();
            //string[] opers = { "if (> 5 3) 544 T)", "if (> 5 3) \"bau\")", "if cond 544 T)" };
            //string[] opers ={ "cond (cond1 istruz1) (cond2 istruz2) (cond3 istruz3) (t istruz4))",
            //     "cond (cond1 istruz1) (cond2 istruz2) (cond3 istruz3) (t istruz4))"
            //};
            //string[] opers ={ "let ((var0 value0) (var1 value1) (varN valueN)) (+ x y))",
            //     "let* ((var0 value0) (var1 value1) (varN valueN)) (+ x y))"
            //};
            string[] opers ={ "sigma ((x list1234) (y list5678)) (* x y))",
                "sigma ((x list1234) (y list5678)) (* x y))"
                 //"sigma ((x list1234) (y list5678) (z list2468)) (- (* x y) z))"
            };
            for (int i = 0; i < opers.Length; i++) {
                string tmp = opers[i];
                List<IToken> tkns = new List<IToken>();
                tkns.Add(new OpenToken(0, "("));
                TakeToken(tkns, tmp);
                if (i == 1) {
                    for (int j = 0; j < tkns.Count; j++) {
                        int cas = rnd.Next(100);
                        //if (cas < 10)
                        tkns[j].Comment = "Comment " + j;
                    }
                    tkns[0].Comment = "Comment op";
                }
                AnalizeTokens(tkns);
                if (tkns[0] is BlockTkn)
                    Console.WriteLine(tkns[0].Text);
                else {
                   SigmaBlockTkn operation = new SigmaBlockTkn(tkns[0] as ListToken);
                    Console.WriteLine(operation.Text);
                }
                Console.WriteLine();
            }*/

            // "(cond (cond1 istruz1) (cond2 istruz2) (cond3 istruz3) (t istruz4))"
            /* List<IToken> tkns = new List<IToken>
             {
                 new OpenToken(0, "("),
                 new ElementToken(1, "cond"),
                 new OpenToken(1, "("),
                 new ConditionTkn() { Ident = 2, Text = "(cond1)" },
                 new ElementToken(2,"istruz1"),
                 new CloseToken(1, ")"),
                 new OpenToken(1, "("),
                 new ConditionTkn() { Ident = 2, Text = "(cond2)" },
                 new ElementToken(2,"istruz2"),
                 new CloseToken(1, ")"),
                 new OpenToken(1, "("),
                 new ConditionTkn() { Ident = 2, Text = "(cond3)" },
                 new ElementToken(2,"istruz3"),
                 new CloseToken(1, ")"),
                 new OpenToken(1, "("),
                 new ElementToken(  2, "t" ),
                 new ElementToken(2,"istruz4"),
                 new CloseToken(1, ")"),
                 new CloseToken(0, ")")
             };
             Console.WriteLine(TakeToken(tkns, ""));
             AnalizeTokens(tkns);
             BlockTkn operation = new BlockTkn(tkns[0] as ListToken);
             Console.WriteLine(operation.Text);*/

            /*
            Random rnd = new Random();
            //string[] opers = { "if (> 5 3) 544 T)", "if (> 5 3) \"bau\")", "if cond 544 T)" };
            string[] opers ={ "cond (cond1 istruz1) (cond2 istruz2) (cond3 istruz3) (t istruz4))",
                "cond (cond1 istruz1) (cond2 istruz2) (cond3 istruz3) (t istruz4))"
            };
            for (int i = 0; i < opers.Length; i++) {
                string tmp = opers[i];
                List<IToken> tkns = new List<IToken>();
                tkns.Add(new OpenToken(0, "("));
                TakeToken(tkns, tmp);
                if (i == 1) {
                    for (int j = 0; j < tkns.Count; j++) {
                        int cas = rnd.Next(100);
                        //if (cas < 10)
                        tkns[j].Comment = "Comment " + j;
                    }
                    tkns[0].Comment = "Comment op";
                }
                AnalizeTokens(tkns);
                if (tkns[0] is BlockTkn)
                    Console.WriteLine(tkns[0].Text);//solo se c'è già in ChangeToken
                else {
                    BlockTkn operation = new BlockTkn(tkns[0] as ListToken);
                    Console.WriteLine(operation.Text);
                }
                Console.WriteLine();
            }*/

            // "(cond (cond1 istruz1) (cond2 istruz2) (cond3 istruz3) (t istruz4))"
            /* List<IToken> tkns = new List<IToken>
             {
                 new OpenToken(0, "("),
                 new ElementToken(1, "cond"),
                 new OpenToken(1, "("),
                 new ConditionTkn() { Ident = 2, Text = "(cond1)" },
                 new ElementToken(2,"istruz1"),
                 new CloseToken(1, ")"),
                 new OpenToken(1, "("),
                 new ConditionTkn() { Ident = 2, Text = "(cond2)" },
                 new ElementToken(2,"istruz2"),
                 new CloseToken(1, ")"),
                 new OpenToken(1, "("),
                 new ConditionTkn() { Ident = 2, Text = "(cond3)" },
                 new ElementToken(2,"istruz3"),
                 new CloseToken(1, ")"),
                 new OpenToken(1, "("),
                 new ElementToken(  2, "t" ),
                 new ElementToken(2,"istruz4"),
                 new CloseToken(1, ")"),
                 new CloseToken(0, ")")
             };
             Console.WriteLine(TakeToken(tkns, ""));
             AnalizeTokens(tkns);
             BlockTkn operation = new BlockTkn(tkns[0] as ListToken);
             Console.WriteLine(operation.Text);*/


            //Random rnd = new Random();
            //string[] opers = { "not a)", "and a b)", "or a b)", "and 1 2 5 y)", "or x y z t o i)" };
            //for (int i = 0; i < opers.Length; i++) {
            //    string tmp = opers[i]; 
            //    List<IToken> tkns = new List<IToken>();
            //    tkns.Add(new OpenToken(0, "("));
            //    TakeToken(tkns, tmp);
            //    for (int j = 0; j < tkns.Count; j++) {
            //        int cas = rnd.Next(100);
            //        //if (cas < 10)
            //        tkns[j].Comment = "Comment " + j;
            //    }
            //    tkns[0].Comment = "Comment op";
            //    AnalizeTokens2(tkns);
            //    ConditionTkn operation = new ConditionTkn(tkns[0] as ListToken);
            //    Console.WriteLine(operation.Text);
            //    Console.WriteLine();
            //}


            //Random rnd = new Random();
            //string[] opers = { "first","second","third","fourth","fifth",
            //            "sixth","seventh","eighth","ninth","tenth"};
            //for (int i = 0; i < opers.Length; i++) {
            //    string tmp = $"{opers[i]} myList)";
            //    List<IToken> tkns = new List<IToken>();
            //    tkns.Add(new OpenToken(0, "("));
            //    TakeToken(tkns, tmp);
            //    for (int j = 0; j < tkns.Count; j++) {
            //        int cas = rnd.Next(100);
            //        //if (cas < 10)
            //        tkns[j].Comment = "Comment " + j;
            //    }
            //    tkns[0].Comment = "Comment op";
            //    AnalizeTokens2(tkns);
            //    ArrayTkn operation = new ArrayTkn(tkns[0] as ListToken);
            //    Console.WriteLine(operation.Text);
            //    Console.WriteLine();
            //}

            //Random rnd = new Random();
            //string tmp = "list true \"pippo\" 45.66)";// 7 8 5 3 .13
            //List<IToken> tkns = new List<IToken>();
            //tkns.Add(new OpenToken(0, "("));
            //TakeToken(tkns, tmp);
            //for (int j = 0; j < tkns.Count; j++) {
            //    int cas = rnd.Next(100);
            //    if (cas < 10)
            //    tkns[j].Comment = "Comment " + j;
            //}
            //tkns[0].Comment = "Comment op";
            //AnalizeTokens2(tkns);
            //ArrayTkn operation = new ArrayTkn(tkns[0] as ListToken);
            //Console.WriteLine(operation.Text);
            //Console.WriteLine();

            //Random rnd = new Random();
            //string[] op = { "=","<",">", "/=", "<=", ">=" };
            //for (int i = 0; i < op.Length; i++) {
            //    string tmp = $"{op[i]} 7 8 5 3 .13)";//3 5 
            //    List<IToken> tkns = new List<IToken>();
            //    tkns.Add(new OpenToken(0, "("));
            //    TakeToken(tkns, tmp);
            //    for (int j = 0; j < tkns.Count; j++) {
            //        int cas = rnd.Next(100);
            //        //if (cas < 10)
            //            tkns[j].Comment = "Comment " + j;
            //    }
            //    tkns[0].Comment = "Comment op";
            //    AnalizeTokens2(tkns);
            //    ConditionTkn operation = new ConditionTkn(tkns[0] as ListToken);
            //    Console.WriteLine(operation.Text);
            //    Console.WriteLine();
            //}

            //Random rnd = new Random();
            //string op = "+-*/";
            //for (int i = 0; i < 4; i++) {
            //    string tmp = $"{op[i]} 7 8)";//3 5 
            //    List<IToken> tkns = new List<IToken>();
            //    tkns.Add(new OpenToken(0, "("));
            //    TakeToken(tkns, tmp);
            //    for (int j = 0; j < tkns.Count; j++) {
            //        int cas = rnd.Next(100);
            //        if (cas < 10)
            //            tkns[j].Comment = "Comment " + j;
            //    }
            //    tkns[0].Comment = "Comment op";
            //    AnalizeTokens2(tkns);
            //    OperationTkn operation = new OperationTkn(tkns[0] as ListToken);
            //    Console.WriteLine(operation.Text);
            //    Console.WriteLine();
            //}

            //Random rnd = new Random();
            //string tmp = "MyFunc 3 5 7 8)";// 
            //List<IToken> tkns = new List<IToken>();
            //tkns.Add(new OpenToken(0, "("));
            //TakeToken(tkns, tmp);
            //for (int j = 0; j < tkns.Count; j++) {
            //    int cas = rnd.Next(100);
            //    if (cas < 10)
            //        tkns[j].Comment = "Comment " + j;
            //}
            //tkns[0].Comment = "Comment fun";
            //AnalizeTokens2(tkns);
            //FunctionToken function = new FunctionToken(tkns[0] as ListToken);
            //Console.WriteLine(function.Text);


            //double id=  Array.Find(new dynamic[] { 5, 6, 7, 8 }, i => i==7);



            //List<IToken> tokens = new List<IToken>();
            //tokens.Add(new OpenToken(0, "("));
            /* string s = "(fixed-from-julian (julian-date (ce 622) july 16)))";
             s = "(- mayan-epoch (mayan-haab-ordinal (mayan-haab-date 18 8))))";
             s = "(- aztec-correlation (aztec-tonalpohualli-ordinal (aztec-tonalpohualli-date 1 5))))";
             s = "\"bogus\")";

             if (TakeToken(tokens, s)) {
                 Console.WriteLine($"trovati {tokens.Count} tokens");
                 for (int i = 0; i < tokens.Count; i++) {
                     for (int l = 0; l < tokens[i].Ident; l++) {
                         Console.Write("    ");
                     }
                     Console.WriteLine(tokens[i].Text);
                 }
             }*/
            /* List<  string> ss =new List<string>{"(universal-from-standard",
                   "(midnight (+ date 1) paris) ; Midnight to Paris",
                   "paris))"};
            List<string> ss = new List<string>{  "(let* ((left (gensym)))",
"`(do* ((,x false (/ (+ ,h ,l) 2))",
"(,left false ,test)",
"(,l ,lo (if ,left ,l ,x))",
"(,h ,hi (if ,left ,x ,h)))",
"(,end (/ (+ ,h ,l) 2)))))"};
            for (int k = 0; k < 6; k++) {
                bool ris = TakeToken(tokens, ss[k]);
                if (ris) {
                    Console.WriteLine($"trovati {tokens.Count} tokens, fine? " + ris);
                    for (int i = 0; i < tokens.Count; i++) {
                        for (int l = 0; l < tokens[i].Ident; l++) {
                            Console.Write("    ");
                        }
                        Console.WriteLine(tokens[i].Text);
                    }
                    AnalizeTokens(tokens);
                }
            }*/

            /*  if (!TakeToken(tokens, ss[0])) {
                  Console.WriteLine($"trovati {tokens.Count} tokens, ma non la fine");
                  for (int i = 0; i < tokens.Count; i++) {
                      for (int l = 0; l < tokens[i].Ident; l++) {
                          Console.Write("    ");
                      }
                      Console.WriteLine(tokens[i].Text);
                  }
                  if (!TakeToken(tokens, ss[1])) {
                      Console.WriteLine($"trovati {tokens.Count} tokens, ma non la fine");
                      for (int i = 0; i < tokens.Count; i++) {
                          for (int l = 0; l < tokens[i].Ident; l++) {
                              Console.Write("    ");
                          }
                          Console.WriteLine(tokens[i].Text);
                      }
                      if (TakeToken(tokens, ss[2])) {
                          Console.WriteLine($"trovati {tokens.Count} tokens e la fine");
                          for (int i = 0; i < tokens.Count; i++) {
                              for (int l = 0; l < tokens[i].Ident; l++) {
                                  Console.Write("    ");
                              }
                              Console.WriteLine(tokens[i].Text);
                          }
                      }
                  }
              }*/


            Console.ReadLine();
        }
    }
}
