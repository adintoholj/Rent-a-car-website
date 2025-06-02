document.addEventListener('DOMContentLoaded', function () {
    // selektovanje vaznih elemenata
    const loginForm = document.getElementById('login-form');
    const registerForm = document.getElementById('register-form');
    const profileSection = document.getElementById('profile-section');
    const displayName = document.getElementById('display-name');
    const logoutBtn = document.getElementById('logout-btn');
    const showRegister = document.getElementById('show-register');
    const showLogin = document.getElementById('show-login');

    // Funkcija za prikaz poruka (grešaka ili uspjeha)
    function showMessage(form, message, type = 'error') {
        // Brišemo prethodne poruke ako postoje
        const existingMessage = form.querySelector('.form-message');
        if (existingMessage) existingMessage.remove();

        // paragraf sa porukom
        const msg = document.createElement('p');
        msg.className = 'form-message';
        msg.style.color = type === 'error' ? 'red' : 'green';
        msg.innerText = message;

        form.appendChild(msg);
    }

    // Prikazivanje forme za registraciju
    showRegister.addEventListener('click', function (e) {
        e.preventDefault();
        loginForm.style.display = 'none';
        registerForm.style.display = 'block';
    });

    // Prikazivanje forme za login
    showLogin.addEventListener('click', function (e) {
        e.preventDefault();
        registerForm.style.display = 'none';
        loginForm.style.display = 'block';
    });

    // EVENT: Registracija korisnika
    registerForm.addEventListener('submit', async function (e) {
        e.preventDefault();

        // Čitamo unesene podatke iz forme
        const username = document.getElementById('reg-username').value.trim();
        const phone = document.getElementById('reg-phone').value.trim();
        const email = document.getElementById('reg-email').value.trim();
        const password = document.getElementById('reg-password').value;

        // Provjera da li su sva polja popunjena
        if (!username || !phone || !email || !password) {
            showMessage(registerForm, 'Molimo popunite sva polja.');
            return;
        }

        try {
            // Slanje zahtjeva na backend
            const response = await fetch('https://localhost:7215/api/register', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    username: username,                         // ispravljeno: "user" umjesto "username"
                    phone: phone,             // ispravljeno: pretvoreno u broj
                    email: email,
                    passwordHash: password
                })
            });

            if (response.ok) {
                showMessage(registerForm, 'Registracija uspješna! Možete se prijaviti.', 'success');

                // Automatski prebacujemo na login formu
                setTimeout(() => {
                    registerForm.style.display = 'none';
                    loginForm.style.display = 'block';
                }, 1500);
            } else {
                const error = await response.text();
                showMessage(registerForm, `Greška: ${error}`);
            }
        } catch (err) {
            showMessage(registerForm, 'Greška pri povezivanju sa serverom.');
        }
    });

    // EVENT: Login korisnika
    loginForm.addEventListener('submit', async function (e) {
        e.preventDefault();

        // Čitanje unosa iz forme
        const username = document.getElementById('username').value.trim();
        const password = document.getElementById('password').value;

        if (!username || !password) {
            showMessage(loginForm, 'Unesite korisničko ime i lozinku.');
            return;
        }

        try {
            // Slanje zahtjeva za login
            const response = await fetch('https://localhost:7215/api/login', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ username, passwordHash: password })
            });

            if (response.ok) {
                const user = await response.json();
                localStorage.setItem('user', JSON.stringify(user));

                // sakrivanje forme i prikazivanje profila
                loginForm.style.display = 'none';
                profileSection.style.display = 'block';
                displayName.innerText = user.username;
                document.querySelector('.emailtext').innerText = 'Email: ' + user.email;
            } else {
                const error = await response.text();
                showMessage(loginForm, `Greška: ${error}`);
            }
        } catch (err) {
            showMessage(loginForm, 'Greška pri povezivanju sa serverom.');
        }
    });

    // EVENT: Odjava korisnika
    logoutBtn.addEventListener('click', function () {
        profileSection.style.display = 'none';
        loginForm.style.display = 'block';
        localStorage.removeItem('user');
    });

    const savedUser = localStorage.getItem('user');
    if (savedUser) {
        const user = JSON.parse(savedUser);
        loginForm.style.display = 'none';
        registerForm.style.display = 'none';
        profileSection.style.display = 'block';
        displayName.innerText = user.username;
        document.querySelector('.emailtext').innerText = 'Email: ' + user.email;
    }
});
