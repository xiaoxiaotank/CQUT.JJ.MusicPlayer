using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CQUT.JJ.MusicPlayer.Controls.Commands
{
    public class LostFocusCommand : ICommand
    {
        private Action<object> _executeDelegate;

        public Key GestureKey { get; set; }
        public ModifierKeys GestureModifier { get; set; }
        public MouseAction MouseGesture { get; set; }

        public event EventHandler CanExecuteChanged;

        public LostFocusCommand(Action<object> executeDelegate)
        {
            _executeDelegate = executeDelegate;
        }

        public void Execute(object parameter)
        {
            _executeDelegate(parameter);
        }

        public bool CanExecute(object parameter) { return true; }

    }
}
