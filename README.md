# 🧠 Quiz Master (WPF Application)

![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![WPF](https://img.shields.io/badge/WPF-MVVM-blue?style=for-the-badge)
![XAML](https://img.shields.io/badge/XAML-UI-orange?style=for-the-badge)

**Quiz Master** is a feature-rich desktop assessment tool built with **WPF** and the **MVVM pattern**. It allows users to create, edit, and play custom quizzes with dynamic category selection.

## 🏗 Architecture & Design Patterns

The project structure strictly follows the **Model-View-ViewModel (MVVM)** pattern to ensure testability and separation of concerns.

### 1. 📂 DataModels (The "M" in MVVM)
located in `/DataModels`
- **Question.cs & Quiz.cs:** POCO classes representing the core domain entities.
- **Separation:** Keeping models separate ensures the data layer is decoupled from the UI.

### 2. 🎮 ViewModels (The "VM" in MVVM)
Handles the business logic and state, communicating with the UI via Data Binding and Commands.
- **PlayQuizViewModel:** Manages the game loop, score tracking, and question navigation.
- **Create/EditQuizViewModel:** Handles the CRUD logic for managing the question bank.
- **CategorySelectionViewModel:** Logic for filtering and selecting quiz topics.

### 3. 🖥️ Views (The "V" in MVVM)
Pure XAML files responsible for the layout.
- **CreateQuizView / EditQuizView:** Forms for data entry.
- **PlayQuizView:** The interactive game interface.
- **Value Converters:** Uses `NullOrEmptyToVisibilityConverter` to dynamically control UI element visibility based on data state.

## ✨ Key Features

- **Full CRUD System:**
  - **Create:** Add new questions and quizzes.
  - **Read:** Load existing quizzes from storage.
  - **Update:** Edit typos or options in existing questions (`EditQuizView`).
  - **Delete:** Remove outdated content.
- **Dynamic UI:**
  - Real-time feedback during gameplay.
  - Smart visibility toggling using custom **IValueConverter**.

## 🚀 How to Run

1.  **Clone the repository**
    ```bash
    git clone https://github.com/QQQQQQQQQian/QuizGame.git
    ```
2.  **Build Solution**
    - Open `Labb3-NET22.sln` in Visual Studio.
    - Build the solution to restore dependencies.
3.  **Run**
    - Press `F5`.
    - Select a category to start playing, or enter "Edit Mode" to manage questions.

---
*Developed as a demonstration of advanced WPF Data Binding and MVVM structure.*
