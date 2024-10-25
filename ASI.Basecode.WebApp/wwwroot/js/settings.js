document.addEventListener('DOMContentLoaded', () => {
    const editProfileModalElement = document.getElementById('editProfileModal');
    const editProfileBtn = document.getElementById('editProfileBtn');
    const profileDropdown = document.getElementById('profileDropdown');
    const profileIcon = document.getElementById('profileIcon');
    const settingsModalElement = document.getElementById('settingsModal');

    // Create Bootstrap Modal Instances
    const editProfileModal = new bootstrap.Modal(editProfileModalElement);
    const settingsModal = new bootstrap.Modal(settingsModalElement);

    // Toggle Dropdown and Show Modal
    profileIcon.addEventListener('click', () => {
        profileDropdown.style.display = profileDropdown.style.display === 'none' ? 'block' : 'none';
    });

    // Open Modal on Button Click
    editProfileBtn.addEventListener('click', () => {
        editProfileModal.show();
    });

    // Open Settings Modal
    document.querySelector('.settings-icon').addEventListener('click', () => {
        settingsModal.show();
    });

    // Remove Backdrop When Modals are Hidden
    editProfileModalElement.addEventListener('hidden.bs.modal', () => {
        const backdrop = document.querySelector('.modal-backdrop');
        if (backdrop) {
            backdrop.remove();  // Remove the backdrop if it exists
        }
    });

    settingsModalElement.addEventListener('hidden.bs.modal', () => {
        const backdrop = document.querySelector('.modal-backdrop');
        if (backdrop) {
            backdrop.remove();  // Remove the backdrop if it exists
        }
    });
});
