function resetFieldsetChecked(event) {
    const button = event.target;
    const fieldset = button.closest('fieldset');
    const inputs = fieldset.querySelectorAll('input[type="checkbox"]');
    inputs.forEach(input => input.checked = false);
}

function resetForm(event) {
    const button = event.target;
    const form = button.closest('form');
    const inputs = form.querySelectorAll('input');
    inputs.forEach(input => {
        if (input.type === 'checkbox' || input.type === 'radio') {
            input.checked = false;
        } else {
            input.value = '';
        }
    });
}