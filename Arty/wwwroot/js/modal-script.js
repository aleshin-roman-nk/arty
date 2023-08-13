const openModalBtn = document.getElementById("openModalBtn");
const modal = document.getElementById("myModal");
const closeButton = document.querySelector(".close");

openModalBtn.addEventListener("click", () => {
    modal.style.display = "block";
});

closeButton.addEventListener("click", () => {
    modal.style.display = "none";
});

window.addEventListener("click", (event) => {
    if (event.target === modal) {
        modal.style.display = "none";
    }
});