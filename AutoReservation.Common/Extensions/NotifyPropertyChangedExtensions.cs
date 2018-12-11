using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AutoReservation.Common.Extensions {
    public static class NotifyPropertyChangedExtensions {
        public static void OnPropertyChanged<T, TProperty>(this T obj, Expression<Func<T, TProperty>> expr)
            where T : IExtendedNotifyPropertyChanged {

            if (obj == null) {
                return;
            }
            if (expr == null) {
                throw new ArgumentException("Expression is null!");
            }
            var memberExrpession = expr.Body as MemberExpression;
            if (memberExrpession == null) {
                throw new ArgumentException("Expression invalid");
            }
            obj.OnPropertyChanged(memberExrpession.Member.Name);
        }
    }
}
