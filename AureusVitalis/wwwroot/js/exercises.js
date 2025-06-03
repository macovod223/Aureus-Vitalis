document.addEventListener('DOMContentLoaded', function() {
    // Анимация появления карточек с упражнениями
    const exerciseCards = document.querySelectorAll('.exercise-card');
    exerciseCards.forEach((card, index) => {
        card.style.opacity = '0';
        card.style.transform = 'translateY(20px)';
        setTimeout(() => {
            card.style.transition = 'all 0.3s ease';
            card.style.opacity = '1';
            card.style.transform = 'translateY(0)';
        }, index * 100);
    });

    // Валидация формы на стороне клиента
    const form = document.querySelector('.exercises-form');
    const ageInput = document.querySelector('input[name="Age"]');
    const weightInput = document.querySelector('input[name="Weight"]');

    form.addEventListener('submit', function(e) {
        let isValid = true;

        // Проверка выбора пола
        const gender = document.querySelector('input[name="Gender"]:checked');
        if (!gender) {
            isValid = false;
            showError('Gender', 'Please select your gender');
        }

        // Проверка возраста
        if (!ageInput.value || ageInput.value < 14 || ageInput.value > 100) {
            isValid = false;
            showError('Age', 'Age must be between 14 and 100');
        }

        // Проверка веса
        if (!weightInput.value || weightInput.value < 30 || weightInput.value > 200) {
            isValid = false;
            showError('Weight', 'Weight must be between 30 and 200 kg');
        }

        if (!isValid) {
            e.preventDefault();
        }
    });

    function showError(fieldName, message) {
        const errorSpan = document.querySelector(`[data-valmsg-for="${fieldName}"]`);
        if (errorSpan) {
            errorSpan.textContent = message;
            errorSpan.style.display = 'block';
        }
    }
});