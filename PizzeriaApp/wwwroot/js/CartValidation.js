document.addEventListener('DOMContentLoaded', function () {
    document.querySelector("form").addEventListener("submit", function (e) {
        e.preventDefault();
        let isValid = true;

        // Vārds uz kredītkartes
        let nameOnCardInput = document.querySelector("#nameOnCardInput");
        let nameOnCard = nameOnCardInput.value;
        let nameOnCardLabel = document.querySelector("#nameOnCardLabel");
        console.log(nameOnCardLabel, nameOnCard);
        if (!nameOnCard.match(/^([A-Z]{1}[A-Z]+\s[A-Z]{1}[A-Z]+)$/)) {
            nameOnCardLabel.classList.add("error");
            nameOnCardInput.classList.add("is-invalid");
            nameOnCardLabel.innerHTML = "Kļūda: Vārds uz kredītkartes ir kļūdains (Vārds Uzvārds)";
            isValid = false;
        } else {
            nameOnCardLabel.classList.remove("error");
            nameOnCardInput.classList.remove("is-invalid");
            nameOnCardLabel.innerHTML = "Vārds uz kredītkartes";
        }

        // Kredītkartes numurs
        let creditCardNumberInput = document.querySelector("#creditCardNumberInput");
        let creditCardNumber = creditCardNumberInput.value;
        let creditCardNumberLabel = document.querySelector("#creditCardNumberLabel");
        if (!creditCardNumber.match(/^(?:4[0-9]{12}(?:[0-9]{3})?|5[1-5][0-9]{14}|(222[1-9]|22[3-9][0-9]|2[3-6][0-9]{2}|27[01][0-9]|2720)[0-9]{12}|6(?:011|5[0-9][0-9])[0-9]{12}|3[47][0-9]{13}|3(?:0[0-5]|[68][0-9])[0-9]{11}|(?:2131|1800|35\d{3})\d{11})$/)) {
            creditCardNumberLabel.classList.add("error");
            creditCardNumberInput.classList.add("is-invalid");
            creditCardNumberLabel.innerHTML = "Kļūda: Kredītkartes numurs ir kļūdains";
            isValid = false;
        } else {
            creditCardNumberLabel.classList.remove("error");
            creditCardNumberInput.classList.remove("is-invalid");
            creditCardNumberLabel.innerHTML = "Kredītkartes numurs";
        }
        // Derīguma termiņš
        let validUntilInput = document.querySelector("#validUntilInput");
        let validUntil = validUntilInput.value;
        let validUntilLabel = document.querySelector("#validUntilLabel");
        let currentDate = new Date();
        let currentMonth = currentDate.getMonth() + 1;
        let currentYear = currentDate.getFullYear().toString().substr(-2);
        if (!validUntil.match(/^(0?[1-9]|1[012])\/(\d{2})$/)) {
            validUntilLabel.classList.add("error");
            validUntilInput.classList.add("is-invalid");
            validUntilLabel.innerHTML = "Kļūda: Derīguma termiņš ir kļūdains (MM/GG)";
            isValid = false;
        } else {
            let month = validUntil.split("/")[0];
            let year = validUntil.split("/")[1];
            if (year < currentYear || (year == currentYear && month < currentMonth)) {
                validUntilLabel.classList.add("error");
                validUntilLabel.innerHTML = "Kļūda: Derīguma termiņš ir pagājis";
                validUntilInput.classList.add("is-invalid");
                isValid = false;
            } else {
                validUntilLabel.classList.remove("error");
                validUntilInput.classList.remove("is-invalid");
                validUntilLabel.innerHTML = "Derīguma termiņš";
            }
        }
        // CVV2
        let cvv2Input = document.querySelector("#cvv2Input");
        let cvv2 = cvv2Input.value;
        let cvv2Label = document.querySelector("#cvv2Label");
        if (!cvv2.match(/^[0-9]{3}$/)) {
            cvv2Label.classList.add("error");
            cvv2Input.classList.add("is-invalid");
            cvv2Label.innerHTML = "Kļūda: CVV2 ir kļūdains (3 cipari)";
            isValid = false;
        } else {
            cvv2Label.classList.remove("error");
            cvv2Input.classList.remove("is-invalid");
            cvv2Label.innerHTML = "CVV2";
        }

        // Saņēmēja vārds
        let contactNameInput = document.querySelector("#contactNameInput");
        let contactName = contactNameInput.value;
        let contactNameLabel = document.querySelector("#contactNameLabel");
        if (!contactName.match(/^([A-ZĀČĒĢĪĶĻŅŠŪŽ]{1}[a-zāčēģīķļņšūž]+\s[A-ZĀČĒĢĪĶĻŅŠŪŽ]{1}[a-zāčēģīķļņšūž]+)$/)) {
            contactNameLabel.classList.add("error");
            contactNameInput.classList.add("is-invalid");
            contactNameLabel.innerHTML = "Kļūda: Saņēmēja vārds ir kļūdains (Vārds Uzvārds)";
            isValid = false;
        } else {
            contactNameLabel.classList.remove("error");
            contactNameInput.classList.remove("is-invalid");
            contactNameLabel.innerHTML = "Norādiet saņēmēja vārdu";
        }
        // Telefona numurs
        let contactPhoneInput = document.querySelector("#contactPhoneInput");
        let contactPhone = contactPhoneInput.value;
        let contactPhoneLabel = document.querySelector("#contactPhoneLabel");
        if (!contactPhone.match(/^[0-9]{8}$/)) {
            contactPhoneLabel.classList.add("error");
            contactPhoneInput.classList.add("is-invalid");
            contactPhoneLabel.innerHTML = "Kļūda: Telefona numurs ir kļūdains (8 cipari)";
            isValid = false;
        } else {
            contactPhoneLabel.classList.remove("error");
            contactPhoneInput.classList.remove("is-invalid");
            contactPhoneLabel.innerHTML = "Norādiet telefona numuru";
        }
        deliveryType.addEventListener("change", function () {
            const selectedOption = deliveryType.options[deliveryType.selectedIndex];
            const selectedValue = deliveryType.value;
            if (selectedValue === "2") {
                // Adrese
                let addressInput = document.querySelector("#addressInput");
                let address = addressInput.value;
                let addressLabel = document.querySelector("#addressLabel");
                if (!address) {
                    addressLabel.classList.add("error");
                    addressInput.classList.add("is-invalid");
                    addressLabel.innerHTML = "Kļūda: Adrese ir obligāti jāaizpilda";
                    isValid = false;
                } else {
                    addressLabel.classList.remove("error");
                    addressInput.classList.remove("is-invalid");
                    addressLabel.innerHTML = "Norādiet savu adresi";
                }
            }   
        });
        if (isValid) {
            //submit the form
            document.querySelector("form").submit();
        } else {
            //prevent the form from submitting
            e.preventDefault();
        }
    });
});