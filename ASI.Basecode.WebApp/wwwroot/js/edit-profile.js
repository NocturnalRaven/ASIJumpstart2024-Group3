// Profile dropdown logic
const profileIcon = document.getElementById('profileIcon');
const profileDropdown = document.getElementById('profileDropdown');
const editProfileBtn = document.getElementById('editProfileBtn');

// Toggle profile dropdown
profileIcon.addEventListener('click', () => {
    profileDropdown.classList.toggle('show');
});

// Show Edit Profile modal when button is clicked
editProfileBtn.addEventListener('click', () => {
    const editProfileModal = new bootstrap.Modal(document.getElementById('editProfileModal'));
    editProfileModal.show();
});

// Close the dropdown if clicking outside
document.addEventListener('click', (e) => {
    if (!profileIcon.contains(e.target) && !profileDropdown.contains(e.target)) {
        profileDropdown.classList.remove('show');
    }
});
