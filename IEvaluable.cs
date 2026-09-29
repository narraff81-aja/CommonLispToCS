using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLispToCS {
    interface IEvaluable {
        string Eval(string func, IEnumerable<IEvaluable> parameters);
    }
    class Value : IEvaluable {
        public string Eval(string func, IEnumerable<IEvaluable> parameters) {
            throw new NotImplementedException();
        }
    }
}
