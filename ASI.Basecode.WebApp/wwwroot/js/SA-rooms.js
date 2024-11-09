const deleteBtns = document.querySelectorAll('.delete-btn');
const editBtns = document.querySelectorAll('.edit-btn');

// Delete functionality
deleteBtns.forEach(btn => {
    btn.addEventListener('click', function () {
        const row = this.closest('tr');
        const roomName = row.cells[0].textContent;
        document.getElementById('itemToDelete').textContent = roomName;
        new bootstrap.Modal(document.getElementById('deleteConfirmationModal')).show();
    });
});

document.getElementById('confirmDeleteBtn').addEventListener('click', function () {
    const row = document.querySelector('.delete-btn:focus').closest('tr');
    row.remove(); // Remove the row from the table
    bootstrap.Modal.getInstance(document.getElementById('deleteConfirmationModal')).hide();
});

// Edit functionality
editBtns.forEach(btn => {
    btn.addEventListener('click', function () {
        const roomName = this.getAttribute('data-bs-whatever');
        const capacity = this.getAttribute('data-capacity');
        const location = this.getAttribute('data-location');

        document.getElementById('roomName').value = roomName;
        document.getElementById('roomCapacity').value = capacity;
        document.getElementById('roomLocation').value = location;
    });
});

document.getElementById('saveRoomChanges').addEventListener('click', function () {
    // Update logic for saving changes can go here
    const roomName = document.getElementById('roomName').value;
    const capacity = document.getElementById('roomCapacity').value;
    const location = document.getElementById('roomLocation').value;

    bootstrap.Offcanvas.getInstance(document.getElementById('editRoom')).hide();
});
// Handle Delete button click
document.querySelectorAll('.delete-btn').forEach(button => {
    button.addEventListener('click', function () {
        const row = button.closest('tr');
        const username = row.cells[0].innerText;
        document.getElementById('itemToDelete').innerText = username;
    });
});
// Handle Confirm Delete button click (you can add functionality here)
document.getElementById('confirmDeleteBtn').addEventListener('click', function () {
    // Logic to delete the account goes here
    alert('Account deleted!');
    // Close the modal
    var modal = bootstrap.Modal.getInstance(document.getElementById('deleteConfirmationModal'));
    modal.hide();
});