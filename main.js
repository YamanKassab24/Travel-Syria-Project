document.addEventListener("DOMContentLoaded", () => {
    // ===== عناصر الصفحة =====
    let searchBtn = document.querySelector('#search-btn');
    let searchBar = document.querySelector('.search-bar-container');
    const authModel = document.querySelector('.auth-model');
    const loginLink = document.querySelector('.login-link');
    const registerLink = document.querySelector('.register-link');
    const loginBtnIcon = document.querySelector('#login-btn');
    const loginClose = document.querySelector('#close');
    let menBar = document.querySelector('#mainbar');
    let navBar = document.querySelector('.navbar');

    // ===== أحداث التمرير =====
    window.onscroll = () => {
        searchBtn.classList.remove('fa-times');
        searchBar.classList.remove('active');
        menBar.classList.remove('fa-times');
        navBar.classList.remove("active");
    };

    // ===== قائمة الموبايل =====
    menBar.addEventListener('click', () => {
        menBar.classList.toggle('fa-times');
        navBar.classList.toggle("active");
    });

    // ===== شريط البحث =====
    searchBtn.addEventListener('click', () => {
        searchBtn.classList.toggle('fa-times');
        searchBar.classList.toggle("active");
    });

    // ===== التنقل بين login و register =====
    registerLink.addEventListener('click', () => {
        authModel.classList.add('active');
    });

    loginLink.addEventListener('click', () => {
        authModel.classList.remove('active');
    });

    loginBtnIcon.addEventListener('click', () => {
        authModel.classList.add('show');
    });

    loginClose.addEventListener('click', () => {
        authModel.classList.remove('show', 'active');
    });

    // ===== تسجيل الدخول =====
    const loginBt = document.getElementById("login-bt");

    loginBt.addEventListener("click", async (e) => {
        e.preventDefault();

        const emailOrPhone = document.getElementById("login-email").value.trim();
        const password = document.getElementById("login-password").value.trim();

        // التحقق من الحقول الفارغة
        if (!emailOrPhone && !password) {
            alert("⚠️ الرجاء إدخال الإيميل أو رقم الهاتف وكلمة المرور");
            return;
        }
        if (!emailOrPhone) {
            alert("⚠️ الرجاء إدخال الإيميل أو رقم الهاتف");
            return;
        }
        if (!password) {
            alert("⚠️ الرجاء إدخال كلمة المرور");
            return;
        }

        try {
            const resp = await fetch("http://localhost:5133/api/User/login", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ emailOrPhone, password }),
            });

            const data = await resp.json();

            if (!resp.ok) {
                const msg = data?.error || "حدث خطأ غير معروف";
                alert("❌ فشل تسجيل الدخول: " + msg);
                return;
            }

            // حفظ التوكن وبيانات المستخدم
            localStorage.setItem("token", data.token);
            localStorage.setItem("user", JSON.stringify(data.user));

            alert(`🎉 أهلاً ${data.user.person.firstName}! تم تسجيل الدخول بنجاح.`);
            authModel.classList.remove("active");

        } catch (error) {
            console.error("خطأ بالشبكة:", error);
            alert("⚠️ حدث خطأ في الاتصال بالخادم، حاول لاحقاً.");
        }
    });

    // ===== إنشاء حساب جديد =====
   const registerForm = document.getElementById("registerForm");

registerForm.addEventListener("submit", async (e) => {
    e.preventDefault();

    const email = document.getElementById("reg-email").value.trim();
    const password = document.getElementById("password").value.trim();
    const first_name = document.getElementById("first-name").value.trim();
    const last_name = document.getElementById("last-name").value.trim();
    const phone = document.getElementById("phone").value.trim();
    const country = document.getElementById("country").value.trim();
    const gender = document.getElementById("gender").value;

    // التحقق من الحقول الأساسية
    if (!email || !password) {
        alert("⚠️ الرجاء إدخال الإيميل وكلمة المرور");
        return;
    }

    try {
        // تجهيز البيانات حسب شكل الـ API
        const registerBody = {
            newUser: {
                userID: 0,
                role: "User",
                createAt: new Date().toISOString(),
                isActive: true,
                personID: 0,
                walletBalance: 0,
                person: {
                    personID: 0,
                    firstName: first_name,
                    lastName: last_name,
                    phone: phone,
                    email: email,
                    isMale: gender === "Male", // أو true/false حسب اختيار المستخدم
                    dateOfBirth: new Date().toISOString(),
                    image: " ",
                    countryID: 1, // ممكن نغيرها حسب الدولة المختارة
                    country: {
                        countryID: 0,
                        countryName: country || "string"
                    }
                }
            },
            password: password
        };

        const resp = await fetch("http://localhost:5133/api/User/AddNewUser", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(registerBody),
        });

        const data = await resp.json();
console.log("Server response:", data);
       if (!resp.ok || data.userID === -1) {
    alert("❌ فشل إنشاء الحساب: قد يكون البريد الإلكتروني أو رقم الهاتف مستخدم مسبقاً.");
    return;
}

alert("🎉 تم إنشاء الحساب بنجاح! يمكنك الآن تسجيل الدخول.");

        alert("🎉 تم إنشاء الحساب بنجاح! يمكنك الآن تسجيل الدخول.");
        authModel.classList.remove("active");

    } catch (error) {
        console.error("خطأ بالشبكة:", error);
        alert("⚠️ حدث خطأ في الاتصال بالخادم، حاول لاحقاً.");
    }
});

});