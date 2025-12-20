export function initTrips() {
    async function fetchTrips() {
        try {
            const tripsRes = await fetch("http://yamankassab-001-site1.mtempurl.com/api/Trip/GetAllTrips");
            if (!tripsRes.ok) throw new Error("Failed to fetch trips");
            const tripsData = await tripsRes.json();
            const trips = await Promise.all(tripsData.map(async trip => {

                const imgRes = await fetch(`http://yamankassab-001-site1.mtempurl.com/api/TripImage/GetAllTripImages/${trip.tripID}`);
                const images = await imgRes.ok ? await imgRes.json() : [];
                const firstImage = images.length > 0 ? images[0].imageUrl : "images/pexels-pixabay-50594.jpg";


                let services = [];
                try {
                    const servicesRes = await fetch(`http://yamankassab-001-site1.mtempurl.com/api/TripService/GetTripServicesByTripID?TripID=${trip.tripID}`);
                    if (servicesRes.ok) {
                        const data = await servicesRes.json();
                        if (Array.isArray(data)) {
                            services = data.map(s => ({
                                serviceID: s.service.serviceID,
                                serviceName: s.service.serviceName,
                                price: s.price,
                                description: s.service.description
                            }));
                        }
                    }
                } catch (err) { services = []; }
                return {
                    tripID: trip.tripID,
                    imageUrl: firstImage,
                    departureTime: trip.departureTime,
                    arrivalTime: trip.arrivalTime,
                    address: trip.company.address,
                    company: trip.company.companyName,
                    departure: trip.departureCity.cityName,
                    arrival: trip.arrivalCity.cityName,
                    price: trip.price,
                    status: trip.status,
                    phone: trip.company.phone,
                    rating: trip.rating,
                    details: trip.details,
                    seat: trip.seatsAvailable,
                    services: services
                };
            }));

            window.allTrips = trips;
            displayTrips(trips);

        } catch (err) {
            console.error("fetch err :" + err);
        }
    }

    function displayTrips(trips) {
        const container = document.getElementById("tripsContainer");
        container.innerHTML = "";

        trips.forEach(t => {
            const div = document.createElement("div");
            div.classList.add("trip-card");
            div.innerHTML = `
            <img src="${t.imageUrl}" class="trip-img" />
            <h3>${t.departure} ➝ ${t.arrival}</h3>
            <p>Company: ${t.company}</p>
            <p>Price: $${t.price}</p>
            <p>${getStars(t.rating)}</p>
            <button class="btn-details" data-id="${t.tripID}">Details</button>
            <button class="btn-addcart" data-id="${t.tripID}">Add to Cart</button>`
                ;
            container.appendChild(div);
        });
        document.getElementById("cartBtn").addEventListener("click", () => {
            document.getElementById("cartModal").classList.add("auth-model");
            document.getElementById("cartModal").classList.add("show")
        });

        document.querySelectorAll(".btn-details").forEach(btn => {
            btn.addEventListener("click", () => {
                const id = parseInt(btn.dataset.id);
                showDetails(id);
            });
        });

        document.querySelectorAll(".btn-addcart").forEach(btn => {
            btn.addEventListener("click", async () => {
                const id = parseInt(btn.dataset.id);
                await addToCart(id);
            });
        });


    }
    fetchTrips();

    function showDetails(id) {
        const trip = window.allTrips.find(t => t.tripID === id);
        if (!trip) return;
        const user = JSON.parse(localStorage.getItem("user"))
        const bookbtn = document.getElementById("btn-book")
        if (trip.status != 1) {
            bookbtn.style.display = "none";
        }
        else if (!user) {
            bookbtn.style.display = "none";
            const msg = document.createElement("p")
            msg.classList.add("msg");
            msg.innerText = "you can't book please log in first "
            bookbtn.parentElement.appendChild(msg)
        }
        else {
            bookbtn.style.display = "block";
        }

        const dep = formatDateTime(trip.departureTime);
        const arr = formatDateTime(trip.arrivalTime);

        const modal = document.getElementById("tripModal");
        modal.classList.remove("hidden");
        modal.classList.add("modal");
        document.getElementById("modalImage").src = trip.imageUrl;
        document.getElementById("modalRoute").innerText = ` ${trip.departure} ➝ ${trip.arrival}`;
        document.getElementById("modalCompany").innerText = `Company: ${trip.company} | Address: ${trip.address}`;
        document.getElementById("modalTime").innerHTML = `Departure time:
         <div>
         <i class="fa-regular fa-calendar"></i> ${dep.date}
          <i class="fa-regular fa-clock"></i> ${dep.time}</div>
              Arrival time:<div>
          <i class="fa-regular fa-calendar"></i> ${arr.date}
          <i class="fa-regular fa-clock"></i> ${arr.time}</div>`
            ;
        document.getElementById("modalStatus").innerHTML = statustrip(trip.status)
        document.getElementById("modalRating").innerHTML = getStars(trip.rating);
        document.getElementById("modalDetails").innerText = trip.details;
        document.getElementById("btn-book").dataset.id = trip.tripID;


        const servicesBox = document.getElementById("servicesContainer");
        servicesBox.innerHTML = "";
        if (trip.services.length > 0) {
            const title = document.createElement("h4");
            title.innerText = "Services";
            servicesBox.appendChild(title);
            trip.services.forEach(s => {
                const div = document.createElement("div");
                div.classList.add("service-item");
                div.innerText = `${s.serviceName} (${s.price === 0 ? "Free" : "$" + s.price}) ➝ ${s.description}`;
                servicesBox.appendChild(div);
            });
        }

    }
    // السلة

    async function addToCart(tripID) {
        const user = JSON.parse(localStorage.getItem("user"));
        if (!user) return alert(" يجب تسجيل الدخول أولاً");

        const trip = window.allTrips.find(t => t.tripID === tripID);
        if (!trip) return;


        let cart = JSON.parse(localStorage.getItem(`cart_${user.userID}`)) || [];

        if (!cart.some(t => t.tripID === trip.tripID)) {
            cart.push({
                tripID: trip.tripID,
                imageUrl: trip.imageUrl,
                departure: trip.departure,
                arrival: trip.arrival,
                price: trip.price
            });
        }

        localStorage.setItem(`cart_${user.userID}`, JSON.stringify(cart));
        alert("تمت إضافة الرحلة للسلة");
    }


    const cartBtn = document.getElementById("cartBtn");
    const cartModal = document.getElementById("cartModal");
    const cartContainer = document.getElementById("cartContainer");
    const closeCart = document.getElementById("closeCart");

    cartBtn.addEventListener("click", () => {
        const user = JSON.parse(localStorage.getItem("user"));
        if (!user) {
            cartModal.style.display = "none"
            return alert(" يجب تسجيل الدخول لعرض السلة");
        }
        let cart = JSON.parse(localStorage.getItem(`cart_${user.userID}`)) || [];
        const updatedCart = cart.map(item => {
            const trip = window.allTrips.find(t => t.tripID === item.tripID);
            if (trip) {
                return { ...item, price: trip.price, imageUrl: trip.imageUrl };
            }
            return null;
        }).filter(Boolean);

        localStorage.setItem(`cart_${user.userID}`, JSON.stringify(updatedCart));
        cart = updatedCart;
        cartContainer.innerHTML = "";
        cart.forEach(trip => {
            const div = document.createElement("div");
            div.classList.add("cart-item");
            div.innerHTML = `
            <img src="${trip.imageUrl}" width="50px">
            <span class="cart-trip-name" data-id="${trip.tripID}">${trip.departure} ➝ ${trip.arrival} | ${trip.price}</span>
            <button class="btn-remove-cart" data-id="${trip.tripID}">Remove</button>`
                ;
            cartContainer.appendChild(div);
        });

        document.querySelectorAll(".cart-trip-name").forEach(el => {
            el.addEventListener("click", async () => {
                const tripID = parseInt(el.dataset.id);
                await showDetails(tripID);
            });
        });


        document.querySelectorAll(".btn-remove-cart").forEach(btn => {
            btn.addEventListener("click", () => {
                const id = parseInt(btn.dataset.id);
                cart = cart.filter(t => t.tripID !== id);
                localStorage.setItem(`cart_${user.userID}`, JSON.stringify(cart));
                btn.parentElement.remove();
            });
        });

        cartModal.style.display = "block";
    });

    closeCart.addEventListener("click", () => {
        cartModal.style.display = "none";
    });




    document.getElementById("closedet").addEventListener("click", () => {
        document.getElementById("tripModal").classList.add("hidden");
        document.getElementById("tripModal").classList.remove("modal");
    });
}
