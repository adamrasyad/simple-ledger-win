using CSV_Accounting.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace CSV_Accounting.Services
{
    public class UndoRedoService
    {
        private List<Ledger> _history = new List<Ledger>();
        private int _currentIndex = -1;
        private const int MaxHistory = 10;

        public bool CanUndo => _currentIndex > 0;
        public bool CanRedo => _currentIndex < _history.Count - 1;

        public event Action OnStateChanged;

        public void AddSnapshot(Ledger ledger)
        {
            // 1. If we are in the middle of the stack and do a new action, 
            // delete everything "ahead" of us (the Redo branch).
            if (_currentIndex < _history.Count - 1)
                _history.RemoveRange(_currentIndex + 1, _history.Count - (_currentIndex + 1));

            // 2. Clone the ledger (Deep Copy)
            _history.Add(Clone(ledger));

            // 3. Keep it at 10 (FIFO)
            if (_history.Count > MaxHistory)
            {
                _history.RemoveAt(0);
            }
            else
            {
                _currentIndex++;
            }

            _currentIndex = _history.Count - 1; // Redo is gone, now put index to last

            OnStateChanged?.Invoke();
        }
        public void ClearHistory()
        {
            _history.Clear();
            _currentIndex = -1;
        }

        public Ledger Undo()
        {
            if (_currentIndex > 0)
            {
                _currentIndex--;
                return Clone(_history[_currentIndex]);
            }
            return null;
        }

        public Ledger Redo()
        {
            if (_currentIndex < _history.Count - 1)
            {
                _currentIndex++;
                return Clone(_history[_currentIndex]);
            }
            return null;
        }

        // Deep Clone (Requires Newtonsoft.Json or System.Text.Json)
        private Ledger Clone(Ledger source)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(source);
            return System.Text.Json.JsonSerializer.Deserialize<Ledger>(json);
        }
    }

}
