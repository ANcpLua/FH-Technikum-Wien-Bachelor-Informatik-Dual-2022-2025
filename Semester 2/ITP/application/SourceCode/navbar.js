var prevScrollPos = window.pageYOffset;

window.onscroll = function() {
  var currentScrollPos = window.pageYOffset;

  if (prevScrollPos > currentScrollPos) {
    document.getElementById("navbar").classList.remove("navbar-scroll-hide");
    document.getElementById("navbar").classList.add("navbar-scroll-show");
  } else {
    document.getElementById("navbar").classList.remove("navbar-scroll-show");
    document.getElementById("navbar").classList.add("navbar-scroll-hide");
  }

  prevScrollPos = currentScrollPos;
};
