using System;
using WpfApp1.Infrastucture;

namespace WpfApp1.Infrastucture
{
    internal class LambdaCommandAsync : Base.Command
    {
        private readonly ActionAsync<object> _Execute;
        private readonly Func<object, bool> _CanExecute;

        public LambdaCommandAsync(ActionAsync Execute, Func<bool> CanExecute = null)
            : this(async p => await Execute(), CanExecute is null ? (Func<object, bool>)null : p => CanExecute())
        {

        }

        public LambdaCommandAsync(ActionAsync<object> Execute, Func<object, bool> CanExecute = null)
        {
            _Execute = Execute;
            _CanExecute = CanExecute;
        }

        public override bool CanExecute(object p) => _CanExecute?.Invoke(p) ?? true;

        public override void Execute(object p) => _Execute(p);
    }
}