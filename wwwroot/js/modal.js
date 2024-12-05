document.addEventListener("DOMContentLoaded", function () {
    // Modal de agregar
    const addModal = document.getElementById("add-modal");
    const addBtn = document.getElementById("add-btn");
    const closeAddBtn = document.getElementById("close-add-modal");

    // Modal de edición
    const editModal = document.getElementById("edit-subscription-modal");
    const closeEditBtn = document.getElementById("close-edit-modal");
    const saveChangesBtn = document.getElementById("save-changes-btn");

    // formularios y los botones de cambio
    const payUnitForm = document.getElementById("payUnitForm");
    const payReqForm = document.getElementById("payReqForm");
    const switchToRecurrent = document.getElementById("switch-to-recurrent");
    const switchToUnit = document.getElementById("switch-to-unit");

    // Modal de configuración
    const configModal = document.getElementById("config-modal");
    const configBtn = document.getElementById("config-btn");
    const closeConfigBtn = document.getElementById("close-config-modal");

    const cancelButtons = document.querySelectorAll(".delete-btn");

    


    let currentPaymentId = null;

    
    const showModal = (modal) => {
        modal.style.display = "flex"; 
    };

    
    const closeModal = (modal) => {
        modal.style.display = "none"; 
    };

   
    configBtn.addEventListener("click", function () {
        showModal(configModal);
    });

    closeConfigBtn.addEventListener("click", function () {
        closeModal(configModal);
    });

    
    addBtn.addEventListener("click", function () {
        showModal(addModal);
        payUnitForm.style.display = "block"; 
        payReqForm.style.display = "none"; 
    });

    
    closeAddBtn.addEventListener("click", function () {
        closeModal(addModal);
    });

   
    switchToRecurrent.addEventListener("click", function () {
        payUnitForm.style.display = "none"; 
        payReqForm.style.display = "block"; 
    });

    
    switchToUnit.addEventListener("click", function () {
        payReqForm.style.display = "none";  
        payUnitForm.style.display = "block"; 

    });

    
    window.addEventListener("click", function (event) {
        if (event.target === addModal) {
            closeModal(addModal);
        }
        if (event.target === editModal) {
            closeModal(editModal);
        }
    });

    

    cancelButtons.forEach(button => {
        button.addEventListener("click", function () {
            const paymentId = this.getAttribute("data-id");

            const confirmCancel = confirm("¿Estás seguro de que quieres cancelar este pago?");
            if (!confirmCancel) {
                return; 
            } 

            fetch("/api/payments/cancel", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(paymentId),
            })
                .then(response => {
                    if (response.ok) {
                        return response.json();
                    } else {
                        throw new Error("No se pudo cancelar el pago.");
                    }
                })
                .then(data => {
                    alert(data.message);
                    location.reload(); 
                })
                .catch(error => {
                    console.error(error);
                    alert("Error al cancelar el pago.");
                });
        });
    });

    
    document.querySelectorAll(".edit-btn").forEach((btn) => {
        btn.addEventListener("click", function () {

            currentPaymentId = this.getAttribute("data-id");
            console.log(currentPaymentId);
            
            fetch(`/api/payments/${currentPaymentId}`)
                .then(response => response.json())
                .then(data => {
                    
                    document.getElementById("subscription-name").value = data.nombre;
                    document.getElementById("subscription-date").value = data.fechaVencimiento.split("T")[0];
                    document.getElementById("subscription-temporality").value = data.temporalidad;
                    document.getElementById("subscription-type").value = data.tipo;
                    document.getElementById("subscription-amount").value = data.cantidad;

                    
                    showModal(editModal);
                })
                .catch(error => console.error("Error al cargar los datos del pago:", error));
        });
    });

    
    closeEditBtn.addEventListener("click", function () {
        closeModal(editModal);
    });

    
    saveChangesBtn.addEventListener("click", function () {
        const updatedData = {
            id: currentPaymentId,
            concepto: document.getElementById("subscription-name").value,
            fechaVencimiento: document.getElementById("subscription-date").value,
            frecuencia: document.getElementById("subscription-temporality").value,
            tipo: document.getElementById("subscription-type").value,
            monto: document.getElementById("subscription-amount").value,
        };
        console.log(currentPaymentId);
        fetch(`/api/payments/update`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(updatedData)
        })
            .then(response => response.json())
            .then(data => {
                console.log("Pago actualizado correctamente:", data);
                closeModal(editModal);

    
                location.reload();
            })
            .catch(error => console.error("Error al guardar los cambios del pago:", error));
    });
});

