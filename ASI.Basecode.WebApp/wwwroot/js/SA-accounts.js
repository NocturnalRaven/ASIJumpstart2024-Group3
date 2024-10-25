// Handle Edit button click
document.querySelectorAll('.edit-btn').forEach(button => {
    button.addEventListener('click', function () {
        const row = button.closest('tr');
        const username = row.cells[0].innerText;
        const email = row.cells[1].innerText;
        const role = row.cells[2].innerText;

        document.getElementById('editUsername').value = username;
        document.getElementById('editEmail').value = email;
        document.getElementById('editRole').value = role;
    });
});

// Handle Delete button click
document.querySelectorAll('.delete-btn').forEach(button => {
    button.addEventListener('click', function () {
        const row = button.closest('tr');
        const username = row.cells[0].innerText;
        document.getElementById('itemToDelete').innerText = username;
    });
});

// Handle Save Changes button click (you can add functionality here)
document.getElementById('saveChangesBtn').addEventListener('click', function () {
    // Logic to save changes goes here
    alert('Changes saved!');
    // Close the offcanvas
    var offcanvas = bootstrap.Offcanvas.getInstance(document.getElementById('editOffcanvas'));
    offcanvas.hide();
});

// Handle Confirm Delete button click (you can add functionality here)
document.getElementById('confirmDeleteBtn').addEventListener('click', function () {
    // Logic to delete the account goes here
    alert('Account deleted!');
    // Close the modal
    var modal = bootstrap.Modal.getInstance(document.getElementById('deleteConfirmationModal'));
    modal.hide();
});