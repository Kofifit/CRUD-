using System.Collections.Generic;

namespace ToDo.Models.ViewModels
{
    public class TodoViewModel
    {
        public List<TodoItem> TodoList { get; set; }
        public TodoItem Todo { get; set; }

        public List<int> getIds()
        {
            List<int> ids = new();
            foreach (var i in TodoList) {
                ids.Add(i.Id);
            };
            return ids;
        }

        public int len()
        {
            int output = 0;
            foreach (var i in TodoList) {
                output++;
            };
            return output;
        }
    }
}