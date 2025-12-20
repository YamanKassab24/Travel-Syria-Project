export function initBooking() {
    document.querySelectorAll("#btn-book").forEach(btn => {
        btn.addEventListener("click", async () => {
            const id = parseInt(btn.dataset.id);
            await bookdetails(id);
        })
    });
    document.getElementById("closebook").addEventListener("click", () => {
        document.getElementById("bookModal").classList.add("hidden");
        document.getElementById("bookModal").classList.remove("modal");
    });
    async function bookdetails(id, oldReservation = null) {
        const trip = window.allTrips.find(t => t.tripID === id);
        if (!trip) return;

        let currentOldReservation = oldReservation;


        const bookModal = document.getElementById("bookModal");
        document.getElementById("tripModal").classList.remove("modal")
        document.getElementById("tripModal").classList.add("hidden");
        bookModal.classList.remove("hidden");
        bookModal.classList.add("modal");


        document.getElementById("imgbook").src = trip.imageUrl;
        document.getElementById("num-seats").innerText = trip.seat;

        const seatInput = document.getElementById("seatCount");
        const seatError = document.getElementById("seatError");
        seatInput.max = trip.seat;
        seatInput.min = 1;
        seatInput.value = 1;

        const servicesbook = document.getElementById("servicecontent");
        servicesbook.innerHTML = "";

        if (trip.services?.length > 0) {
            const title = document.createElement("h4");
            title.innerText = "Services";
            servicesbook.appendChild(title);

            trip.services.forEach(s => {
                const wrap = document.createElement("div");
                wrap.classList.add("service");

                const checkbox = document.createElement("input");
                checkbox.type = "checkbox";
                checkbox.id = `srv-${trip.tripID}-${s.serviceID}`;
                checkbox.dataset.price = s.price;


                checkbox.checked = false;
                updateTotalPrice()


                if (s.price === 0) {
                    checkbox.checked = true;
                    checkbox.disabled = true
                }

                const label = document.createElement("label");
                label.classList.add("check");
                label.htmlFor = checkbox.id;
                label.innerText = `${s.serviceName} (${s.price === 0 ? "Free" : "$" + s.price}) ➝ ${s.description}`;

                wrap.appendChild(checkbox);
                wrap.appendChild(label);
                servicesbook.appendChild(wrap);
            });
            document.querySelectorAll("#servicecontent input[type='checkbox']").forEach(cb => {
                cb.addEventListener("change", updateTotalPrice);
            });

        } else {
            const noServices = document.createElement("p");
            noServices.innerText = "No services available for this trip.";
            servicesbook.appendChild(noServices);
        }


        function updateTotalPrice() {
            const seats = Number(seatInput.value);
            const maxSeats = trip.seat;
            if (seats < 1 || seats > maxSeats) {
                seatError.style.display = "block";
                seatError.innerText = `Available seats: ${maxSeats} only`;
            } else {
                seatError.style.display = "none";
            }
            let servicesTotal = 0;
            const checkedServices = document.querySelectorAll("#servicecontent input[type='checkbox']:checked");
            checkedServices.forEach(cb => {
                servicesTotal += Number(cb.dataset.price);
            });
            let total = (trip.price + servicesTotal) * seats;
            document.getElementById("pricebook").innerText = total.toFixed(2) + "$";
        }

        updateTotalPrice();
        seatInput.addEventListener("input", updateTotalPrice);


        document.getElementById("bookNow").onclick = async () => {
            const user = JSON.parse(localStorage.getItem("user"));
            if (!user) return alert(" يجب تسجبل دخول قبل الحجز");

            const seats = Number(seatInput.value);
            if (seats < 1) return alert(" أدخل عدد مقاعد صالح");
            updateTotalPrice();
            const total = Number(document.getElementById("pricebook").innerText.replace("$", ""));
            const checkedServices = Array.from(document.querySelectorAll("#servicecontent input[type='checkbox']:checked"))
                .map(cb => ({
                    serviceID: Number(cb.id.split("-").pop()),
                    price: Number(cb.dataset.price)
                }));

            if (oldReservation) {

                const updatedBooking = {
                    ...oldReservation,
                    seatsCount: seats,
                    totalPrice: total
                };

                await fetch("http://yamankassab-001-site1.mtempurl.com/api/Reservation/UpdateReservation", {
                    method: "PUT",
                    headers: authHeaders(),
                    body: JSON.stringify(updatedBooking)
                });


                for (let s of trip.services) {
                    const res = await fetch(`http://yamankassab-001-site1.mtempurl.com/api/ReservationService/CheckReservationService?ReservationID=${currentOldReservation.reservationID}&ServiceID=${s.serviceID}`, {
                        method: "GET",
                        headers: authHeaders()
                    });
                    const exists = await res.json();

                    const checkbox = document.getElementById(`srv-${trip.tripID}-${s.serviceID}`);
                    const isChecked = checkbox.checked;

                    if (isChecked && !exists) {

                        await fetch("http://yamankassab-001-site1.mtempurl.com/api/ReservationService/AddNewReservationService", {
                            method: "POST",
                            headers: authHeaders(),
                            body: JSON.stringify({
                                reservationServiceID: 0,
                                reservationID: currentOldReservation.reservationID,
                                serviceID: s.serviceID,
                                price: s.price
                            })
                        });
                    } else if (!isChecked && exists) {

                        await fetch(`http://yamankassab-001-site1.mtempurl.com/api/ReservationService/DeleteReservationServiceByReservationIDAndServiceID?ReservationID=${currentOldReservation.reservationID}&ServiceID=${s.serviceID}`, {
                            method: "DELETE",
                            headers: authHeaders()
                        });
                    }
                }

                alert(" تم تعديل الحجز بنجاح");
                location.reload()
            } else {

                const reservationBody = {
                    reservationID: 0,
                    userID: user.userID,
                    tripID: trip.tripID,
                    seatsCount: seats,
                    reservationDate: new Date().toISOString(),
                    totalPrice: total,
                    status: 1
                };

                const res = await fetch("http://yamankassab-001-site1.mtempurl.com/api/Reservation/AddNewReservation", {
                    method: "POST",
                    headers: authHeaders(),
                    body: JSON.stringify(reservationBody)
                });
                const reservationResult = await res.json();
                const reservationID = reservationResult.reservationID;

                for (let s of checkedServices) {
                    await fetch("http://yamankassab-001-site1.mtempurl.com/api/ReservationService/AddNewReservationService", {
                        method: "POST",
                        headers: authHeaders(),
                        body: JSON.stringify({
                            reservationServiceID: 0,
                            reservationID: reservationID,
                            serviceID: s.serviceID,
                            price: s.price
                        })
                    });
                }

                alert(" تم الحجز بنجاح");
                location.reload()
            }
            bookModal.classList.add("hidden");
            bookModal.classList.remove("modal");
        };
    }


    document.getElementById("mybook").addEventListener("click", async () => {
        const user = JSON.parse(localStorage.getItem("user"));
        if (!user) {
            alert("يجب تسجيل الدخول");

            return;
        }

        try {
            const res = await fetch(`http://yamankassab-001-site1.mtempurl.com/api/Reservation/GetAllReservationsByUserID?UserID=${user.userID}`,
                {
                    method: "GET",
                    headers: authHeaders()
                });
            if (!res.ok) {
                alert("لايوجد حجوزات")
                return
            }

            const bookings = await res.json();
            const section = document.getElementById("mybooksec");
            section.style.backgroundImage = "none";
            section.style.height = "fit-content"
            section.classList.add("color");
            section.innerHTML = `<h1 class="heading">
            <span>M</span>
            <span>y</span>
            <span>b</span>
            <span>o</span>
            <span>o</span>
            <span>k</span>
        </h1>`;
            const boxMybook = document.createElement("div");
            boxMybook.classList.add("mybook-box");

            for (let b of bookings) {
                const card = document.createElement("div");
                card.classList.add("cardmybook");

                let servicesHTML = "";
                try {
                    const servicesRes = await fetch(`http://yamankassab-001-site1.mtempurl.com/api/ReservationService/GetAllReservationServiceByReservationID?ReservationID=${b.reservationID}`,
                        {
                            method: "GET",
                            headers: authHeaders()
                        });
                    if (servicesRes.ok) {
                        const services = await servicesRes.json();
                        if (services.length > 0) {
                            servicesHTML = `service:
                            <ul class='services-list'>`;
                            services.forEach(s => {
                                servicesHTML += `<li>${s.service.serviceName} (${s.price === 0 ? "Free" : "$" + s.price})</li>`;
                            });
                            servicesHTML += `</ul>`;
                        }
                    } else if (!servicesRes.ok) {

                        servicesHTML = `<p>No services selected</p>`;
                    }

                } catch (err) {
                    servicesHTML = `<p>No services selected</p>`
                }

                card.innerHTML = `
                <p class="headingmybook">${b.trip.departureCity.cityName} to ${b.trip.arrivalCity.cityName}</p>
                <p>${b.trip.company.companyName}</p>
                <p>Seats: ${b.seatsCount}</p>
                <p>Total: $${b.totalPrice}</p>
                <div class="reservation-services">${servicesHTML}
                </div>
                <div class=edit-btn-book>
                <button class="edit-book">Edit</button>
                <button id="delete-book" class="delete-book">delete</button>
                </div>
                </section>`
                    ;

                card.querySelector(".edit-book").addEventListener("click", () => {
                    bookdetails(b.trip.tripID, b)
                });
                card.querySelector(".delete-book").addEventListener("click", () => {
                    deleteReservation(b.reservationID);
                });

                boxMybook.appendChild(card);
            }

            section.appendChild(boxMybook);
        } catch (err) {
            console.log(err)
            alert(" خطأ بجلب الحجوزات");
        }
    });




    async function deleteReservation(reservationID) {
        const confirmDelete = confirm(" هل أنت متأكد من حذف هذا الحجز؟");
        if (!confirmDelete) return;

        try {

            await fetch(`http://yamankassab-001-site1.mtempurl.com/api/ReservationService/DeleteReservationServiceByReservationID?ReservationID=${reservationID}`, {
                method: "DELETE",
                headers: authHeaders()
            });


            const res = await fetch(`http://yamankassab-001-site1.mtempurl.com/api/Reservation/DeleteReservationByReservationID?ReservationID=${reservationID}`, {
                method: "DELETE",
                headers: authHeaders()
            });

            if (!res.ok) throw new Error("Delete failed");

            alert(" تم حذف الحجز بنجاح");

            location.reload();

        } catch (err) {
            alert(" خطأ أثناء حذف الحجز");
        }

    }
}
