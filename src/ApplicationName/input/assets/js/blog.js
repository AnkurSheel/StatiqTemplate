const mobileButton = document.querySelector("button.mobile-menu-button");
const mobileMenu = document.querySelector(".mobile-menu");

mobileButton.addEventListener("click", () => {
    mobileMenu.classList.toggle("hidden");
});
