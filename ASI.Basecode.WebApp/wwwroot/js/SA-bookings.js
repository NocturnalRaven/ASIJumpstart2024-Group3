document.querySelectorAll('.edit-btn').forEach(button => {
    button.addEventListener('click', function () {
        const row = button.closest('tr');
        document.getElementById('editUser').value = row.cells[0].innerText;
        document.getElementById('editRoom').value = row.cells[1].innerText;
        document.getElementById('editDate').value = row.cells[2].innerText;
        document.getElementById('editPeople').value = row.cells[3].innerText;
    });
});

const deleteButtons = document.querySelectorAll('.delete-btn');
deleteButtons.forEach(button => {
    button.addEventListener('click', function () {
        const row = button.closest('tr');
        document.getElementById('itemToDelete').innerText = row.cells[0].innerText;

        document.getElementById('confirmDeleteBtn').onclick = function () {
            row.remove();
            const modal = bootstrap.Modal.getInstance(document.getElementById('deleteConfirmationModal'));
            modal.hide();
        };
    });
});

document.getElementById('saveChangesBtn').addEventListener('click', function () {
    const user = document.getElementById('editUser').value;
    const room = document.getElementById('editRoom').value;
    const date = document.getElementById('editDate').value;
    const people = document.getElementById('editPeople').value;

    const row = document.querySelector('.edit-btn:focus').closest('tr');
    row.cells[0].innerText = user;
    row.cells[1].innerText = room;
    row.cells[2].innerText = date;
    row.cells[3].innerText = people;

    const offcanvas = bootstrap.Offcanvas.getInstance(document.getElementById('editOffcanvas'));
    offcanvas.hide();
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