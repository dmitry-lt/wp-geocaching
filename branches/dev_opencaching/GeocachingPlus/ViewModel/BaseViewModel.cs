using System;
using System.ComponentModel;
using System.Linq.Expressions;
using GeocachingPlus.Model.Utils;
using System.Windows.Threading;

namespace GeocachingPlus.ViewModel
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                var body = propertyExpression.Body as MemberExpression;
                if (body == null)
                    throw new ArgumentException("'propertyExpression' should be a member expression");

                var expression = body.Expression as ConstantExpression;
                if (expression == null)
                    throw new ArgumentException("'propertyExpression' body should be a constant expression");

                object target = Expression.Lambda(expression).Compile().DynamicInvoke();

                var e = new PropertyChangedEventArgs(body.Member.Name);
                handler(target, e);
            }
        }

        protected void RaisePropertyChanged<T>(params Expression<Func<T>>[] propertyExpressions)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            foreach (var propertyExpression in propertyExpressions)
            {
                RaisePropertyChanged<T>(propertyExpression);
            }
        }
    }
}
