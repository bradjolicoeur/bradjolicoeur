// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

document.addEventListener('DOMContentLoaded', function () {
    var hamburger = document.querySelector('.nav-hamburger');
    var navMenu = document.getElementById('nav-menu');

    if (hamburger && navMenu) {
        hamburger.addEventListener('click', function () {
            var isOpen = navMenu.classList.toggle('nav-open');
            hamburger.setAttribute('aria-expanded', isOpen);
        });

        // Close menu when a nav link is clicked
        navMenu.querySelectorAll('a').forEach(function (link) {
            link.addEventListener('click', function () {
                navMenu.classList.remove('nav-open');
                hamburger.setAttribute('aria-expanded', 'false');
            });
        });
    }
});
