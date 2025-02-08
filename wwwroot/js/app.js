document.addEventListener("DOMContentLoaded", () => {
  loadTodos();
});

function handleKeyPress(event) {
  // Check if the pressed key is "Enter" (key code 13)
  if (event.key === "Enter") {
    addTodo();
  }
}

function addTodo() {
  const input = document.getElementById("todoInput");
  const text = input.value.trim();

  if (text === "") {
    alert("Please enter a todo");
    return;
  }

  const todos = JSON.parse(localStorage.getItem("todos") || "[]");
  todos.push({ text, id: Date.now() });
  localStorage.setItem("todos", JSON.stringify(todos));

  input.value = "";
  renderTodos(todos);
}

function deleteTodo(id) {
  const todos = JSON.parse(localStorage.getItem("todos") || "[]");
  const filteredTodos = todos.filter((todo) => todo.id !== id);
  localStorage.setItem("todos", JSON.stringify(filteredTodos));
  renderTodos(filteredTodos);
}

function renderTodos(todos) {
  const todoList = document.getElementById("todoList");
  todoList.innerHTML = "";

  todos.forEach((todo) => {
    const li = document.createElement("li");
    li.innerHTML = `
            <span>${todo.text}</span>
            <button class="delete-btn" onclick="deleteTodo(${todo.id})">Delete</button>
        `;
    todoList.appendChild(li);
  });
}

function loadTodos() {
  const todos = JSON.parse(localStorage.getItem("todos") || "[]");
  renderTodos(todos);
}
