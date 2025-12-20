
document.addEventListener("DOMContentLoaded", () => {

    const authModel = document.querySelector('.auth-model');
    const loginLink = document.querySelector('.login-link');
    const registerLink = document.querySelector('.register-link');
    const loginBtnIcon = document.querySelector('#login-btn');
    const loginClose = document.querySelector('#close');
    const menBar = document.querySelector('#mainbar');
    const navBar = document.querySelector('.navbar');

    menBar.addEventListener('click', () => {
        menBar.classList.toggle('fa-times');
        navBar.classList.toggle("active");
    });

    window.onscroll = () => {
        menBar.classList.remove('fa-times');
        navBar.classList.remove("active");
    };

    import('./auth.js').then(module => {
        module.initAuth({ authModel, loginLink, registerLink, loginBtnIcon, loginClose });
    });
    import('./trips.js').then(module => {
        module.initTrips();
    });

    import('./booking.js').then(module => {
        module.initBooking();
    });
    import('./contact.js').then(module => {
        module.initcontact();
    });
    //review
    var swiper = new Swiper(".review-slider", {

        spaceBetween: 20,

        loop: true,

        breakpoints: {
            640: {
                slidesPerView: 1
            },
            768: {
                slidesPerView: 2
            },
            1024: {
                slidesPerView: 3
            },
        },
    });
    //faq
    document.querySelectorAll(".faq-question").forEach(q => {
        q.addEventListener("click", () => {
            const item = q.parentElement;


            document.querySelectorAll(".faq-item").forEach(i => {
                if (i !== item) i.classList.remove("active");
            });


            item.classList.toggle("active");
        });
    });

});
function getStars(rating) {
    let stars = "";
    let full = Math.floor(rating);

    for (let i = 0; i < full; i++) {
        stars += '<i class="fa-solid fa-star" style="color: gold;font-size:18px"></i>';
    }

    let emptyCount = 5 - full;
    for (let i = 0; i < emptyCount; i++) {
        stars += '<i class="fa-regular fa-star" style="color: gold;font-size:18px"></i>';
    }
    return stars;
}
function formatDateTime(dateTimeString) {
    const [date, time] = dateTimeString.split("T");
    const [year, month, day] = date.split("-");
    const [hour, minute] = time.split(":");

    return {
        date: `${year}/${month}/${day}`,
        time: `${hour}:${minute}`
    };
}
function statustrip(status) {
    if (status === 1) {
        return `<p style="color:blue">inprogerss</p>`
    }
    else if (status === 2)
        return `<p style="color:green">completed</p>`
    else

        return `<p style="color:red">inprogerss</p>`
}
function authHeaders() {
    const token = localStorage.getItem("token");
    return {
        "Content-Type": "application/json",
        "Authorization": `Bearer ${token}`
    };
}

