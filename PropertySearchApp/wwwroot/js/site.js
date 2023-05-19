// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var password = document.getElementById("inputPassword")
    , confirm_password = document.getElementById("repeatPassword");

function validatePassword() {
    if (password.value != confirm_password.value) {
        confirm_password.setCustomValidity("Passwords Don't Match");
    } else {
        confirm_password.setCustomValidity('');
    }
}

password.onchange = validatePassword;
confirm_password.onkeyup = validatePassword;

$(this).removeAttr('b-x9zjbty19p');
document.getElementById('nav').removeAttribute('b-x9zjbty19p')
document.getElementById('wrapper').removeAttribute('b-x9zjbty19p')
document.getElementById('main').removeAttribute('b-x9zjbty19p')
document.getElementById('container').removeAttribute('b-x9zjbty19p')
document.getElementById('footer').removeAttribute('b-x9zjbty19p')
document.getElementById('footer-menu').removeAttribute('b-x9zjbty19p')