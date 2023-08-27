class CustomObserver {
    constructor(callback = () => { }) {
        this.callback = callback;
    }

    observe(element, className, reversable) {
        const observer = new IntersectionObserver((entries) => {
            entries.forEach((entry) => {
                if (entry.isIntersecting) {
                    entry.target.classList.add(className);
                } else if (reversable) {
                    entry.target.classList.remove(className);
                }
            });
        });

        observer.observe(element);
    }
}

const observer = new CustomObserver();

const rightSlideElements = document.querySelectorAll('.right-slide-hidden');
const leftSlideElements = document.querySelectorAll('.left-slide-hidden');
const leftRotateElements = document.querySelectorAll('.left-rotate-hidden');
const rotateYElements = document.querySelectorAll('.rotateY-hidden');
const rotateXElements = document.querySelectorAll('.rotateX-hidden');
const fadeInElements = document.querySelectorAll('.fade-in-hidden');

rightSlideElements.forEach((el) => observer.observe(el, 'right-slide-show', true));
leftSlideElements.forEach((el) => observer.observe(el, 'left-slide-show', true));
leftRotateElements.forEach((el) => observer.observe(el, 'left-rotate-show', true));
rotateYElements.forEach((el) => observer.observe(el, 'rotateY-show', false));
rotateXElements.forEach((el) => observer.observe(el, 'rotateX-show', false));
fadeInElements.forEach((el) => observer.observe(el, 'fade-in-show', false));
