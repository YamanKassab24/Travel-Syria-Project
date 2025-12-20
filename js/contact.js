export function initcontact() {
    const contactForm = document.querySelector(".contact-form");
    const nameInput = document.getElementById("contact-name");
    const emailInput = document.getElementById("contact-email");
    const phoneInput = document.getElementById("contact-phone");
    const messageInput = document.getElementById("contact-message");
    contactForm.addEventListener("submit", async (e) => {
        e.preventDefault();
        const contactdata = {
            name: nameInput.value,
            email: emailInput.value,
            phone: phoneInput.value,
            message: messageInput.value
        };

        try {
            const response = await fetch(`http://yamankassab-001-site1.mtempurl.com/api/ContactMessage/AddNewContactMessage`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(contactdata)
            });
            if (!response.ok) throw new Error("Failed to send");
            const result = await response.json();
            alert("تم إرسال الرسالة بنجاح");
            contactForm.reset();

        } catch (err) {
            console.error(err);
            alert("حدث خطأ أثناء إرسال ")
        }
    });
}
