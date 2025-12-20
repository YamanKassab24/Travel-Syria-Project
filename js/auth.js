// auth.js

export function initAuth({ authModel, loginLink, registerLink, loginBtnIcon, loginClose }) {

    const loginForm = document.querySelector(".form-container.login");
    const registerForm = document.querySelector(".form-container.register");
    const editdata = document.getElementById("editProfileModal");
    const profileContainer = document.getElementById("profile-container");
    loadCountries();

    function showProfile(user) {
        authModel.classList.add("show");
        loginForm.classList.add("hidden");
        registerForm.classList.add("hidden");
        editdata.classList.add("hidden");
        profileContainer.classList.remove("hidden");

        document.getElementById("profile-name").textContent = `${user.person.firstName} ${user.person.lastName}`;
        document.getElementById("profile-email").textContent = user.person.email;
        document.getElementById("profile-phone").textContent = user.person.phone;
        document.getElementById("profile-country").textContent = user.person.country.countryName;
        document.getElementById("logout-btn").textContent = "logout";

        document.getElementById("profile-gender").textContent = user.person.isMale ? "male" : "female";

        document.getElementById("logout-btn").onclick = () => {
            localStorage.removeItem("token");
            localStorage.removeItem("user");
            profileContainer.classList.add("hidden");
            loginForm.classList.remove("hidden");
            registerForm.classList.remove("hidden");
            alert("تم تسجيل الخروج بنجاح");
        };
    }

    loginBtnIcon.addEventListener('click', () => {
        const user = JSON.parse(localStorage.getItem("user"));
        if (user) {
            showProfile(user);
        } else {
            authModel.classList.add('show');
        }
    });

    loginClose.addEventListener('click', () => {
        authModel.classList.remove('show', 'active');
    });


    registerLink.addEventListener('click', () => authModel.classList.add('active'));
    loginLink.addEventListener('click', () => authModel.classList.remove('active'));

    //login
    const loginBt = document.getElementById("login-bt");
    loginBt.addEventListener("click", async (e) => {
        e.preventDefault();
        const emailOrPhone = document.getElementById("login-email").value.trim();
        const password = document.getElementById("login-password").value.trim();

        if (!emailOrPhone || !password) {
            alert(" الرجاء إدخال الإيميل أو رقم الهاتف وكلمة المرور");
            return;
        }

        try {
            const resp = await fetch("http://yamankassab-001-site1.mtempurl.com/api/User/login", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ emailOrPhone, password }),
            });
            const data = await resp.json();

            if (!resp.ok) {
                alert(" فشل تسجيل الدخول: " + (data?.error || "حدث خطأ غير معروف"));
                return;
            }

            localStorage.setItem("token", data.token);
            localStorage.setItem("user", JSON.stringify(data.user));

            authModel.classList.remove("active", "show");
            alert(` أهلاً ${data.user.person.firstName}! تم تسجيل الدخول بنجاح`);
        } catch (error) {
            console.error(error);
            alert(" حدث خطأ في الاتصال بالخادم، حاول لاحقاً");
        }
    });

    //register
    const registerFormEl = document.getElementById("registerForm");
    registerFormEl.addEventListener("submit", async (e) => {
        e.preventDefault();
        const email = document.getElementById("reg-email").value.trim();
        const password = document.getElementById("password").value.trim();
        const first_name = document.getElementById("first-name").value.trim();
        const last_name = document.getElementById("last-name").value.trim();
        const phone = document.getElementById("phone").value.trim();
        const country = document.getElementById("country").value.trim();
        const gender = document.getElementById("gender").value;

        if (!email || !password) {
            alert("الرجاء إدخال الإيميل وكلمة المرور");
            return;
        }

        try {
            const registerBody = {
                userID: 0,
                firstName: first_name,
                lastName: last_name,
                phone,
                email,
                isMale: gender === "Male",
                dateOfBirth: new Date().toISOString(),
                image: " ",
                countryID: country,
                createAt: new Date().toISOString(),
                role: "User",
                password
            };

            const resp = await fetch("http://yamankassab-001-site1.mtempurl.com/api/User/AddNewUser", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(registerBody),
            });

            const data = await resp.json();
            if (!resp.ok || data.userID === -1) {
                alert("فشل إنشاء الحساب: البريد الإلكتروني أو رقم الهاتف مستخدم مسبقاً");
                return;
            }

            alert(" تم إنشاء الحساب بنجاح! يمكنك الآن تسجيل الدخول");
            authModel.classList.remove("active");

        } catch (error) {
            console.error(error);
            alert("حدث خطأ في الاتصال بالخادم")
        }
    });

    // update
    document.getElementById("editProfileBtn").addEventListener("click", () => {
        const user = JSON.parse(localStorage.getItem("user"));
        profileContainer.classList.add("hidden");
        editdata.classList.remove("hidden");
        authModel.classList.add("active");

        document.getElementById("edit-first").value = user.person.firstName;
        document.getElementById("edit-last").value = user.person.lastName;
        document.getElementById("edit-email").value = user.person.email;
        document.getElementById("edit-phone").value = user.person.phone;
        document.getElementById("gender-edit").value = user.person.isMale ? "male" : "female";
        document.getElementById("edit-country").value = user.person.country.countryID;
    });

    document.getElementById("closeEdit").addEventListener("click", () => {
        editdata.classList.add("hidden");
        authModel.classList.remove("active")
        const user = JSON.parse(localStorage.getItem("user"));
        showProfile(user);
    });

    document.getElementById("editProfileForm").addEventListener("submit", async (e) => {
        e.preventDefault();
        const user = JSON.parse(localStorage.getItem("user"));
        const updatedData = {
            userID: user.userID,
            firstName: document.getElementById("edit-first").value,
            lastName: document.getElementById("edit-last").value,
            phone: document.getElementById("edit-phone").value,
            email: document.getElementById("edit-email").value,
            isMale: (document.getElementById("gender-edit")?.value === "male"),
            dateOfBirth: new Date().toISOString(),
            image: " ",
            countryID: Number(document.getElementById("edit-country").value)
        };

        try {
            const resp = await fetch("http://yamankassab-001-site1.mtempurl.com/api/User/UpdateUser", {
                method: "PUT",
                headers: authHeaders(),
                body: JSON.stringify(updatedData),
            }); const data = await resp.json();
            if (!resp.ok) {
                alert("فشل التعديل");
                return;
            }

            localStorage.setItem("user", JSON.stringify(data));
            alert(" تم تعديل البيانات بنجاح!");
        } catch (err) {
            console.error(err);
            alert(" خطأ في الاتصال.");
        }
    });

    //delete
    const deleteAccountBtn = document.getElementById("deleteAccount");
    deleteAccountBtn.addEventListener("click", async () => {
        const user = JSON.parse(localStorage.getItem("user"));
        if (!user) return alert("يجب تسجيل الدخول أولاً");

        if (!confirm("هل أنت متأكد من حذف الحساب؟")) return;

        try {
            const res = await fetch(`http://yamankassab-001-site1.mtempurl.com/api/User/DeleteUserByUserID?UserID=${user.userID}`, {
                method: "DELETE",
                headers: authHeaders()
            });

            if (!res.ok) throw new Error("Delete failed");

            localStorage.removeItem("user");
            localStorage.removeItem("token")

            authModel.classList.remove("show")


            alert("تم حذف الحساب بنجاح");
        } catch (err) {
            console.error(err);
            alert(" حدث خطأ أثناء حذف الحساب");
        }
    });
}
async function loadCountries() {
    try {
        const res = await fetch("http://yamankassab-001-site1.mtempurl.com/api/Country/GetAllCountries");
        const countries = await res.json();

        const countrySelect = document.getElementById("country");
        const editCountrySelect = document.getElementById("edit-country");

        countrySelect.innerHTML = `<option value="">Select country</option>`;
        editCountrySelect.innerHTML = `<option value="">Select country</option>`;

        countries.forEach(country => {
            const option = document.createElement("option");
            option.value = country.countryID;
            option.textContent = country.countryName;

            countrySelect.appendChild(option.cloneNode(true));
            editCountrySelect.appendChild(option);
        });


        const savedUser = JSON.parse(localStorage.getItem("user"));
        if (savedUser?.person?.country?.countryID) {
            editCountrySelect.value = savedUser.person.country.countryID.countryName;
        }

    } catch (err) {
        console.error("Failed to load countries", err);
    }
}
