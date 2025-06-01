document.addEventListener('DOMContentLoaded', function() {
    const form = document.querySelector('.auth-form');
    const inputs = form.querySelectorAll('input');
    const submitButton = form.querySelector('.auth-button');

    inputs.forEach(input => {
        input.addEventListener('focus', function() {
            this.parentElement.classList.add('focused');
        });

        input.addEventListener('blur', function() {
            if (!this.value) {
                this.parentElement.classList.remove('focused');
            }
        });

        if (input.type === 'email') {
            input.addEventListener('input', function() {
                const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
                if (!emailRegex.test(this.value)) {
                    this.classList.add('invalid');
                    showError(this, 'Пожалуйста, введите корректный email');
                } else {
                    this.classList.remove('invalid');
                    hideError(this);
                }
            });
        }

        if (input.type === 'password') {
            input.addEventListener('input', function() {
                if (this.value.length < 6) {
                    this.classList.add('invalid');
                    showError(this, 'Пароль должен содержать минимум 6 символов');
                } else {
                    this.classList.remove('invalid');
                    hideError(this);
                }
            });
        }
    });

    const passwordInput = form.querySelector('input[type="password"]');
    if (passwordInput) {
        const togglePassword = document.createElement('span');
        togglePassword.className = 'toggle-password';
        togglePassword.innerHTML = '<svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z"></path><circle cx="12" cy="12" r="3"></circle></svg>';
        passwordInput.parentElement.style.position = 'relative';
        passwordInput.parentElement.appendChild(togglePassword);

        togglePassword.addEventListener('click', function() {
            const type = passwordInput.getAttribute('type') === 'password' ? 'text' : 'password';
            passwordInput.setAttribute('type', type);

            if (type === 'password') {
                this.innerHTML = '<svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z"></path><circle cx="12" cy="12" r="3"></circle></svg>';
            } else {
                this.innerHTML = '<svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M17.94 17.94A10.07 10.07 0 0 1 12 20c-7 0-11-8-11-8a18.45 18.45 0 0 1 5.06-5.94M9.9 4.24A9.12 9.12 0 0 1 12 4c7 0 11 8 11 8a18.5 18.5 0 0 1-2.16 3.19m-6.72-1.07a3 3 0 1 1-4.24-4.24"></path><line x1="1" y1="1" x2="23" y2="23"></line></svg>';
            }
        });
    }

    form.addEventListener('submit', function(e) {
        e.preventDefault();

        if (validateForm()) {
            submitButton.classList.add('loading');

            setTimeout(() => {
                submitButton.classList.remove('loading');
                submitButton.classList.add('success');
                submitButton.textContent = 'Успешно!';

                setTimeout(() => {
                    this.submit();
                }, 1000);
            }, 1500);
        }
    });

    function showError(input, message) {
        const errorDiv = input.parentElement.querySelector('.error-message') ||
            createErrorElement(input);
        errorDiv.textContent = message;
    }

    function hideError(input) {
        const errorDiv = input.parentElement.querySelector('.error-message');
        if (errorDiv) {
            errorDiv.textContent = '';
        }
    }

    function createErrorElement(input) {
        const errorDiv = document.createElement('div');
        errorDiv.className = 'error-message';
        input.parentElement.appendChild(errorDiv);
        return errorDiv;
    }

    function validateForm() {
        let isValid = true;
        inputs.forEach(input => {
            if (input.required && !input.value) {
                isValid = false;
                input.classList.add('invalid');
                showError(input, 'Это поле обязательно для заполнения');
            }
        });
        return isValid;
    }
});